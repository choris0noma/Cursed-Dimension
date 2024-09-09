Shader "Custom/TV_Noise"
{
    Properties
    {
        _BaseMap("Base Map", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

            Pass
            {
                HLSLPROGRAM

                #pragma vertex vert
                #pragma fragment frag

                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                struct Attributes
                {
                    float4 positionOS   : POSITION;
                    float2 uv: TEXCOORD0;
                };

                struct Varyings
                {
                    float4 positionCS  : SV_POSITION;
                    float2 uv: TEXCOORD0;
                };

                TEXTURE2D(_BaseMap);
                SAMPLER(sampler_BaseMap);

                CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _BaseMap_ST;
                CBUFFER_END

                Varyings vert(Attributes IN)
                {
                    Varyings OUT;

                    OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                    OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);

                    return OUT;
                }

                half4 frag(Varyings IN) : SV_Target
                {
                    float4 texel = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                    float v = frac(sin((IN.uv.x * 100. + IN.uv.y * 6000.) * _Time) * 5000.);
                    _Color = float4(v, v, v, v);
                    return texel * _Color;
                }
                ENDHLSL
            }
        }
}