Shader "-smn-/GhostRim" {
     Properties {
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
        };
  
        float4 _ColorTint;
        sampler2D _MainTex;
        sampler2D _BumpMap;
        float4 _RimColorOuter;
        float4 _RimColorInner;
        float _RimPowerOuter;
        float _RimPowerInner;
  
        void surf (Input IN, inout SurfaceOutput o) {
          IN.color = _ColorTint;
          o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * IN.color;
          o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
          o.Alpha = _ColorTint.a; // For example. Could also be the alpha channel on the interpolated vertex color (IN.color.a), or the one from the texture.
         
          half rimOuter = 1.0 -saturate(dot(normalize(IN.viewDir), o.Normal));
          half rimInner = saturate(dot(normalize(IN.viewDir), o.Normal));
          o.Emission = (_RimColorOuter.rgb * pow(rimOuter, _RimPowerOuter)) + (_RimColorInner.rgb * pow(rimInner, _RimPowerInner)) ;
        }
        ENDCG
     } 
     FallBack "Diffuse"
 }