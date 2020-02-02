Shader "Custom/FX_HeightFog"
{
    Properties{
        _Blend("Blend", Range(0, 1)) = 0.5
        _MainTex("Texture 1", 2D) = ""
        _Texture2("Texture 2", 2D) = ""
    }

    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        BlendOp Add

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D_float _CameraDepthTexture;

            #include "UnityCG.cginc"

            uniform float4x4 ViewProjectionInverse;

#define FogColor HeightFogParameters.xyz
#define FogHeight HeightFogParameters.w
            uniform float4 HeightFogParameters;
#define FogTransparency HeightFogParameters2.x
            uniform float4 HeightFogParameters2;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 view : VIEW;
            };

            v2f vert (uint vid : SV_VertexID)
            {
                float x = -1.0 + float((vid & 1) << 2);
                float y = -1.0 + float((vid & 2) << 1);

                v2f o;
                o.vertex = float4(x, y, 0, 1);
                o.uv = float2(x * 0.5 + 0.5, 0.5 - y * 0.5);

                float4 view = mul(ViewProjectionInverse, float4(o.vertex.x, -o.vertex.y, 1, 1));
                o.view = view.xyz - _WorldSpaceCameraPos * view.w;

                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                float3 view = normalize(i.view);

                float nl_depth = tex2D(_CameraDepthTexture, i.uv);
                float depth = LinearEyeDepth(nl_depth);
                float3 wpos = _WorldSpaceCameraPos + depth * i.view;

                return fixed4(FogColor, saturate(FogTransparency*(FogHeight - wpos.y)));
            }
            ENDCG
        }
    }
}
