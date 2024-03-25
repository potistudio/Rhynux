Shader "Custom/GradientSkyboxShader" {
    Properties {
        _HorizonColor ("Horizon Color", Color) = (1.0, 1.0, 1.0, 0.0)
        _TopColor ("Top Color", Color) = (0.0, 0.0, 0.0, 0.0)
        _Offset ("Offset", Range(0.0, 1.0)) = 0.0
    }


    SubShader {
        Tags {
            "RenderType" = "Background"
            "Queue" = "Background"
            "PreviewType" = "Skybox"
        }


        Pass {
            ZWrite off
            Cull off


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            uniform fixed3 _TopColor;
            uniform fixed3 _HorizonColor;
            uniform float _Offset;


            struct appdata {
                float4 vertex : POSITION;
                float3 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : POSITION;
                float3 texcoord : TEXCOORD0;
            };


            float EaseOutCubic (float i) {
                return 1 - pow (1 - i, 3);
            }

            float EaseOutExpo (float i) {
                return i == 1 ? 1 : 1 - pow (2, -10 * i);
            }


            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos (v.vertex);
                o.texcoord = v.vertex.xyz;

                return o;
            }

            fixed4 frag (v2f i) : SV_TARGET {
                float y = i.texcoord.y + _Offset * 2 - 1;
                float interpolation = y >= 0 ? EaseOutCubic (y) : EaseOutExpo (-y);

                return fixed4 (lerp(_HorizonColor, _TopColor, interpolation), 1.0);
            }


            ENDCG
        }
    }
}
