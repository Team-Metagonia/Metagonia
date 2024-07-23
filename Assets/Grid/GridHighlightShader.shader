Shader "Custom/GridHighlightShader"
{
    Properties
    {
        _Color ("Base Color", Color) = (1,1,1,1)
        _HighlightColor ("Highlight Color", Color) = (0,0,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"RenderType"="Opaque"}
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            sampler2D _MainTex;
            float4 _Color;
            float4 _HighlightColor;
            float4 _HighlightUVMin;
            float4 _HighlightUVMax;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                if (i.uv.x >= _HighlightUVMin.x && i.uv.x <= _HighlightUVMax.x &&
                    i.uv.y >= _HighlightUVMin.y && i.uv.y <= _HighlightUVMax.y)
                {
                    return _HighlightColor;
                }
                else
                {
                    return texColor * _Color;
                }
            }
            ENDCG
        }
    }
}
