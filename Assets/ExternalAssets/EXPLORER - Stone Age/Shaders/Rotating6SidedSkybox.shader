Shader "Custom/Rotating6SidedSkybox"
{
    Properties
    {
        _Tint ("Tint Color", Color) = (1,1,1,1)
        _Exposure ("Exposure", Range(0, 8)) = 1.0
        _Rotation ("Rotation", Range(0, 360)) = 0
        _FrontTex ("Front (+Z)", 2D) = "white" {}
        _BackTex ("Back (-Z)", 2D) = "white" {}
        _LeftTex ("Left (+X)", 2D) = "white" {}
        _RightTex ("Right (-X)", 2D) = "white" {}
        _UpTex ("Up (+Y)", 2D) = "white" {}
        _DownTex ("Down (-Y)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue" = "Background" "RenderType" = "Background" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };

            sampler2D _FrontTex;
            sampler2D _BackTex;
            sampler2D _LeftTex;
            sampler2D _RightTex;
            sampler2D _UpTex;
            sampler2D _DownTex;
            float4 _Tint;
            float _Exposure;
            float _Rotation;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.vertex.xyz;
                return o;
            }

            fixed4 texcolor (sampler2D tex, float3 texcoord)
            {
                texcoord.xy /= texcoord.z;
                texcoord.xy = texcoord.xy * 0.5 + 0.5;
                return tex2D(tex, texcoord.xy);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 회전 매트릭스 적용
                float3 rotatedTexcoord = i.texcoord;
                float theta = radians(_Rotation);
                float c = cos(theta);
                float s = sin(theta);
                float3x3 rotationMatrix = float3x3(
                    c, 0, s,
                    0, 1, 0,
                    -s, 0, c
                );
                rotatedTexcoord = mul(rotationMatrix, rotatedTexcoord);

                fixed4 col = fixed4(0, 0, 0, 1);
                if (abs(rotatedTexcoord.z) >= abs(rotatedTexcoord.x) && abs(rotatedTexcoord.z) >= abs(rotatedTexcoord.y))
                {
                    if (rotatedTexcoord.z > 0)
                        col = texcolor(_FrontTex, rotatedTexcoord);
                    else
                        col = texcolor(_BackTex, rotatedTexcoord);
                }
                else if (abs(rotatedTexcoord.x) >= abs(rotatedTexcoord.y))
                {
                    if (rotatedTexcoord.x > 0)
                        col = texcolor(_LeftTex, rotatedTexcoord);
                    else
                        col = texcolor(_RightTex, rotatedTexcoord);
                }
                else
                {
                    if (rotatedTexcoord.y > 0)
                        col = texcolor(_UpTex, rotatedTexcoord);
                    else
                        col = texcolor(_DownTex, rotatedTexcoord);
                }

                col.rgb = col.rgb * _Tint.rgb * _Exposure;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}