Shader "MediaPipe/TransparentMask" {
  Properties {
    _MainTex ("CameraTex", 2D) = "white" {}
    _MaskTex ("MaskTex", 2D) = "white" {}
    _FadeAmount ("Background Fade", Range(0,1)) = 1
  }
  SubShader {
    Tags { "Queue"="Overlay" "RenderType"="Transparent" }
    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #include "UnityCG.cginc"

      struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
      struct v2f {
        float4 vertex : SV_POSITION;
        float2 uvCam  : TEXCOORD0;
        float2 uvMask : TEXCOORD1;
      };

      sampler2D _MainTex;
      float4   _MainTex_ST;
      sampler2D _MaskTex;
      float4   _MaskTex_ST;
      float    _FadeAmount;

      v2f vert (appdata v) {
        v2f o;
        o.vertex  = UnityObjectToClipPos(v.vertex);
        o.uvCam   = TRANSFORM_TEX(v.uv, _MainTex);
        o.uvMask  = TRANSFORM_TEX(v.uv, _MaskTex);
        return o;
      }

      fixed4 frag (v2f i) : SV_Target {
        fixed4 camColor  = tex2D(_MainTex, i.uvCam);
        float maskValue  = tex2D(_MaskTex, i.uvMask).r;
        float fade = lerp(_FadeAmount, 1.0, maskValue);
        camColor.rgb *= fade;
        return camColor;
      }
      ENDCG
    }
  }
}