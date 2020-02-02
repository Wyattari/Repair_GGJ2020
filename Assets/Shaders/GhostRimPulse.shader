Shader "-smn-/GhostRimPulse" {
     Properties {
        _PulseTint("PulseTint", Color) = (1,0,0,1)
        _ColorTint("ColorTint", Color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump" {}
        _RimColorOuter("Rim Color Outer", Color) = (1,1,1,1)
        _RimColorInner("Rim Color Inner", Color) = (1,1,1,1)
        _RimPowerOuter("Rim Power Outer", Range(0.0, 7.0)) = 3.0
        _RimPowerInner("Rim Power Inner", Range(0.0, 20.0)) = 3.0
     }
     SubShader {
        Tags { "Queue"="Transparent" "RenderType" = "Opaque" }
  
        Blend One One // Additive
        
        CGPROGRAM
        #pragma surface surf Lambert alpha
  
        struct Input {
          float4 color : COLOR;
          float2 uv_MainTex;
          float2 uv_BumpMap;
          float3 viewDir;
          float3 worldPos;
        };
  
        float4 _PulseTint;
        float4 _ColorTint;
        sampler2D _MainTex;
        sampler2D _BumpMap;
        float4 _RimColorOuter;
        float4 _RimColorInner;
        float _RimPowerOuter;
        float _RimPowerInner;

        float hash(float n) { return frac(sin(n) * 1e4); }
        float hash(float2 p) { return frac(1e4 * sin(17.0 * p.x + p.y * 0.1) * (0.1 + abs(sin(p.y * 13.0 + p.x)))); }

        float noise(float x) {
            float i = floor(x);
            float f = frac(x);
            float u = f * f * (3.0 - 2.0 * f);
            return lerp(hash(i), hash(i + 1.0), u);
        }
  
        void surf (Input IN, inout SurfaceOutput o) {
          IN.color = _ColorTint;
          o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * IN.color;
          o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
          o.Alpha = _ColorTint.a; // For example. Could also be the alpha channel on the interpolated vertex color (IN.color.a), or the one from the texture.
         
          half rimOuter = 1.0 -saturate(dot(normalize(IN.viewDir), o.Normal));
          half rimInner = saturate(dot(normalize(IN.viewDir), o.Normal));
          o.Emission = (_RimColorOuter.rgb * pow(rimOuter, _RimPowerOuter)) + (_RimColorInner.rgb * pow(rimInner, _RimPowerInner));
          float base_offset = 1e-1*(IN.worldPos.z + 5e-1 * noise(sin(_Time.z)*IN.worldPos.x + _Time.x)) - _Time.y;
          float frac_value = frac(base_offset);
          float integer_value = base_offset - frac_value;
          o.Emission += _PulseTint * (0.5 - 0.5*cos(6.28*saturate((5 + 5 * noise(integer_value)) * frac_value)));
        }
        ENDCG
     } 
     FallBack "Diffuse"
 }