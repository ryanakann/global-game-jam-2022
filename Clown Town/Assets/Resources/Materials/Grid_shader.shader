Shader "Unlit/Grid_shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Color1", Color) = (1, 1, 1, 1)
        _Color2 ("Color2", Color) = (1, 1, 1, 1)
        _CellCountX ("Cell Count X", float) = 10.0
        _CellCountY ("Cell Count Y", float) = 10.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color1;
            float4 _Color2;
            float _CellCountX;
            float _CellCountY;

            float4 checkerboard(float2 uv) {
                float parityX = uint(uv.x * _CellCountX) % 2;
                float parityY = uint(uv.y * _CellCountY) % 2;
                float parity = (parityX + parityY) % 2;
                return (1 - parity) * _Color1 + parity * _Color2;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture

                fixed4 col = checkerboard(i.uv);
                //fixed4 col = _Color1;
                // apply fog
                return col;
            }

            
            ENDCG
        }
    }
}
