Shader "Custom/DotGlowShader"
{
    Properties
    {
        _MainTex ("Texture (Black and White Dot)", 2D) = "white" {}
        _ScrollSpeed ("Scroll Speed", Float) = 1.5
        _TilingX ("Dot Tiling", Float) = 10.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

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
                fixed4 color : COLOR; // Line Renderer의 그라데이션 컬러를 받아옴
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float _ScrollSpeed;
            float _TilingX;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // Line Renderer의 세로 UV는 유지하고, 가로 UV만 Tiling 처리
                o.uv = float2(v.uv.x * _TilingX, v.uv.y);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 시간을 더해 UV를 가로로 스크롤 (앞으로 이동하는 애니메이션)
                float2 scrolldUV = i.uv;
                scrolldUV.x -= _Time.y * _ScrollSpeed;

                // 점선 텍스처에서 알파값 추출
                fixed4 texColor = tex2D(_MainTex, scrolldUV);
                
                // Line Renderer 자체의 그라데이션 색상에 점선 알파 마스크 적용
                fixed4 finalColor = i.color;
                finalColor.a *= texColor.a; 

                return finalColor;
            }
            ENDCG
        }
    }
}
