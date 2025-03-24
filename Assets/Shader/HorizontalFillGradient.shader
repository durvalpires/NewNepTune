Shader "Custom/HorizontalFillGradient" {
    Properties {
        _LeftColor ("Left Color", Color) = (1,0,0,1)
        _RightColor ("Right Color", Color) = (0,0,1,1)
        _FillAmount ("Fill Amount", Range(0,1)) = 0
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _LeftColor;
            fixed4 _RightColor;
            float _FillAmount;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float factor = smoothstep(_FillAmount - 0.1, _FillAmount + 0.1, i.uv.x);
                return lerp(_LeftColor, _RightColor, factor);
            }
            ENDCG
        }
    }
}