Shader "examples/week 8/water"
{
    Properties 
    {
        _albedo ("albedo", 2D) = "white" {}
        [NoScaleOffset] _normalMap ("normal map", 2D) = "bump" {}
        [NoScaleOffset] _displacementMap ("displacement map", 2D) = "white" {}
        _gloss ("gloss", Range(0,1)) = 1
        _normalIntensity ("normal intensity", Range(0, 1)) = 1
        _displacementIntensity ("displacement intensity", Range(0,1)) = 0.5
        _refractionIntensity ("refraction intensity", Range(0, 0.5)) = 0.1
        _pan("pan", Vector) = (0, 0, 0, 0)
        _opacity ("opacity", Range(0,1)) = 0.9
        _MainTex ("maintex", 2D) = "white" {}
    }
    SubShader
    {
        // this tag is required to use _LightColor0
        // this shader won't actually use transparency, but we want it to render with the transparent objects
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "LightMode"="ForwardBase" }

        GrabPass {
            "_BackgroundTex"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc" // might be UnityLightingCommon.cginc for later versions of unity

            #define MAX_SPECULAR_POWER 256

            sampler2D _albedo; float4 _albedo_ST;
            sampler2D _normalMap;
            sampler2D _displacementMap;
            sampler2D _BackgroundTex;
            float4 _pan; // When you say "Vector" in Properties{}, it'll be a float4.
            float _gloss;
            uniform float _normalIntensity;
            uniform float _displacementIntensity;
            float _refractionIntensity;
            float _opacity;

            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 tangent : TEXCOORD2;
                float3 bitangent : TEXCOORD3;
                float3 posWorld : TEXCOORD4;
                
                // create a variable to hold two float2 direction vectors that we'll use to pan our textures
                float4 uvPan : TEXCOORD5;
                float4 screenUV : TEXCOORD6;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.uv = TRANSFORM_TEX(v.uv, _albedo);
                
                // panning
                o.uvPan = _pan * _Time.x;
                // We're not normalizing _pan, so as you scale _pan you're 
                // changing how time affects those values. 

                // Original pan line
                //o.uvPan = float4(float2(0.9, 0.2) * _Time.x, float2(0.5, -0.2) * _Time.x);

                // add our panning to our displacement texture sample
                float height = tex2Dlod(_displacementMap, float4(o.uv + o.uvPan.xy, 0, 0)).r;
                v.vertex.xyz += v.normal * height * _displacementIntensity;

                o.normal = UnityObjectToWorldNormal(v.normal);
                o.tangent = UnityObjectToWorldNormal(v.tangent);
                o.bitangent = cross(o.normal, o.tangent) * v.tangent.w;

                o.vertex = UnityObjectToClipPos(v.vertex);
                
                o.screenUV = ComputeGrabScreenPos(o.vertex);
                
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                
                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                float2 uv = i.uv;
                float2 screenUV = i.screenUV.xy / i.screenUV.w;


                float3 tangentSpaceNormal = UnpackNormal(tex2D(_normalMap, uv + i.uvPan.xy));
                // uvPan is in vert()––it's a float4 that contains two hard-coded
                // 2D vectors multiplied by time. 
                // We're just offsetting our current UV by the pan amount and 
                // sampling the normal map at that spot. 
                // Now we need to declare the second one. 
                float3 tangentSpaceDetailNormal = UnpackNormal(tex2D(_normalMap, (uv * 5) + i.uvPan.zw));
                // The only thing that changes is we want to scale our UV space––
                // if we multiply it by 5, it'll tile the normal 5 times in each 
                // direction, which will have the effect of being smaller. Then
                // we add the second vector. 

                // There's a particular algorithm that is useful for blending
                // normals––WHITEOUT BLENDING. Unity has built it in as a helpful
                // function:
                tangentSpaceNormal = BlendNormals(tangentSpaceNormal, tangentSpaceDetailNormal);
                // Now we have our first map at one scale moving in one direction
                // at one speed, and we have our second map at a different scale, 
                // moving in a different direction at a different speed. 



                tangentSpaceNormal = normalize(lerp(float3(0, 0, 1), tangentSpaceNormal, _normalIntensity));
                
                float2 refractionUV = screenUV.xy + (tangentSpaceNormal.xy * _refractionIntensity);
                float3 background = tex2D(_BackgroundTex, refractionUV);

                float3x3 tangentToWorld = float3x3 
                (
                    i.tangent.x, i.bitangent.x, i.normal.x,
                    i.tangent.y, i.bitangent.y, i.normal.y,
                    i.tangent.z, i.bitangent.z, i.normal.z
                );

                float3 normal = mul(tangentToWorld, tangentSpaceNormal);


                // blinn phong
                float3 surfaceColor = tex2D(_albedo, uv + i.uvPan.xy).rgb;

                float3 lightDirection = _WorldSpaceLightPos0;
                float3 lightColor = _LightColor0; // includes intensity

                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld);
                float3 halfDirection = normalize(viewDirection + lightDirection);

                float diffuseFalloff = max(0, dot(normal, lightDirection));
                float specularFalloff = max(0, dot(normal, halfDirection));

                float3 specular = pow(specularFalloff, _gloss * MAX_SPECULAR_POWER + 0.0001) * _gloss * lightColor;
                float3 diffuse = diffuseFalloff * surfaceColor * lightColor;

                float3 color = (diffuse * _opacity) + (background * (1 - _opacity)) + specular;
                return float4(color, 1);
            }
            ENDCG
        }
    }
}
