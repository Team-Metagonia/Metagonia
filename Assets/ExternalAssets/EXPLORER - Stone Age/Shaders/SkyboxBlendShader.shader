Shader "Custom/SkyboxBlend"
{
    Properties
    {
        _FrontTex1 ("Front (Day)", 2D) = "white" {}
        _BackTex1 ("Back (Day)", 2D) = "white" {}
        _LeftTex1 ("Left (Day)", 2D) = "white" {}
        _RightTex1 ("Right (Day)", 2D) = "white" {}
        _UpTex1 ("Up (Day)", 2D) = "white" {}
        _DownTex1 ("Down (Day)", 2D) = "white" {}

        _FrontTex2 ("Front (Night)", 2D) = "white" {}
        _BackTex2 ("Back (Night)", 2D) = "white" {}
        _LeftTex2 ("Left (Night)", 2D) = "white" {}
        _RightTex2 ("Right (Night)", 2D) = "white" {}
        _UpTex2 ("Up (Night)", 2D) = "white" {}
        _DownTex2 ("Down (Night)", 2D) = "white" {}

        _BlendFactor ("Blend Factor", Range(0, 1)) = 0.0
        _Rotation ("Rotation", Float) = 0.0
    }
    SubShader
    {
        Tags { "Queue" = "Background" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _FrontTex1, _BackTex1, _LeftTex1, _RightTex1, _UpTex1, _DownTex1;
            sampler2D _FrontTex2, _BackTex2, _LeftTex2, _RightTex2, _UpTex2, _DownTex2;
            float _BlendFactor;
            float _Rotation;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float3 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            half4 BlendTextures(sampler2D tex1, sampler2D tex2, float3 texcoord)
            {
                half4 color1 = tex2D(tex1, texcoord.xy);
                half4 color2 = tex2D(tex2, texcoord.xy);
                return lerp(color1, color2, _BlendFactor);
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 front = BlendTextures(_FrontTex1, _FrontTex2, i.texcoord);
                half4 back = BlendTextures(_BackTex1, _BackTex2, i.texcoord);
                half4 left = BlendTextures(_LeftTex1, _LeftTex2, i.texcoord);
                half4 right = BlendTextures(_RightTex1, _RightTex2, i.texcoord);
                half4 up = BlendTextures(_UpTex1, _UpTex2, i.texcoord);
                half4 down = BlendTextures(_DownTex1, _DownTex2, i.texcoord);

                if (i.texcoord.z > 0.5) return up;
                else if (i.texcoord.z < -0.5) return down;
                else if (i.texcoord.x > 0.5) return right;
                else if (i.texcoord.x < -0.5) return left;
                else if (i.texcoord.y > 0.5) return front;
                else return back;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
