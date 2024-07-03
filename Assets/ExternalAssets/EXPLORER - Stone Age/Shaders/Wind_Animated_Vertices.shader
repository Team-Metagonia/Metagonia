Shader "EXPLORER: Stone Age/Wind Animated Vertices"
{
 
    Properties {
        _MainTex ("Diffuse Texture", 2D) = "white" {}
        _Tint ("Tint", Color) = (1,1,1,1)
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5

        _wind_dir ("Wind Direction", Vector) = (0.5,0.05,0.5,0)
        _wind_size ("Wind Wave Size", range(5,50)) = 12
        _sway_stutter_influence("Sway Stutter Influence", range(0,1)) = 0.2
        _sway_stutter ("Sway Stutter", range(0,10)) = 1.5
        _sway_speed ("Sway Speed", range(0,10)) = 1
        _sway_disp ("Sway Displacement", range(0,1)) = 0.3
        _wiggle_disp ("Wiggle Displacement", range(0,1)) = 0.07
        _wiggle_speed ("Wiggle Speed", range(0,25)) = 0.01
        _b_influence ("Wiggle Influence", range(0,1)) = 1

    }
 
    SubShader {
        
        CGPROGRAM
        #pragma target 3.0
        #pragma surface surf Lambert vertex:vert alphatest:_Cutoff addshadow

            
            float4 _wind_dir;
            float _wind_size;
            float _sway_speed;
            float _sway_disp;
            float _wiggle_disp;
            float _wiggle_speed;
            float _vertical_disp;
            float _sway_stutter;
            float _sway_stutter_influence;
            float _r_influence;
            float _b_influence;

            sampler2D _MainTex;
            fixed4 _Tint;

                struct Input {
                    float2 uv_MainTex;
                };

                void vert (inout appdata_full i) {

                    
                    float3 worldPos = mul (unity_ObjectToWorld, i.vertex).xyz;


                    i.vertex.x += (cos(_Time.z * _sway_speed + (worldPos.x/_wind_size) + (sin(_Time.z * _sway_stutter * _sway_speed + (worldPos.x/_wind_size)) * _sway_stutter_influence) ) + 1)/2 * _sway_disp * _wind_dir.x * (i.vertex.y / 10) + 
                    cos(_Time.w * i.vertex.x * _wiggle_speed + (worldPos.x/_wind_size)) * _wiggle_disp * _wind_dir.x * i.color.b * _b_influence;

                    i.vertex.z += (cos(_Time.z * _sway_speed + (worldPos.z/_wind_size) + (sin(_Time.z * _sway_stutter * _sway_speed + (worldPos.z/_wind_size)) * _sway_stutter_influence) ) + 1)/2 * _sway_disp * _wind_dir.z * (i.vertex.y / 10) + 
                    cos(_Time.w * i.vertex.z * _wiggle_speed + (worldPos.x/_wind_size)) * _wiggle_disp * _wind_dir.z * i.color.b * _b_influence;

                    i.vertex.y += cos(_Time.z * _sway_speed + (worldPos.z/_wind_size)) * _sway_disp * _wind_dir.y * (i.vertex.y / 10);


                    i.vertex.y += sin(_Time.w * _sway_speed + _wind_dir.x + (worldPos.z/_wind_size)) * _vertical_disp  * i.color.r * _r_influence;

                }


                void surf (Input IN, inout SurfaceOutput o) {
                    fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Tint;
                    o.Albedo = c.rgb;
                    o.Alpha = c.a;
                }

        ENDCG
        }
     
    Fallback "Diffuse"
} 