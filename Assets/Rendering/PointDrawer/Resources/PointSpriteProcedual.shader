Shader "Hidden/PointSpriteProcedual"
{
	Properties
	{
		_PointSize("PointSize", float) = 10
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0

				#include "UnityCG.cginc"

				struct appdata {
					float4 vertex : POSITION;
					float4 texcoord : TEXCOORD0;
					fixed4 color : COLOR;
				};

				struct v2f
				{
					half psize:PSIZE;
					float4 clipPos : TEXCOORD0;
					float size : TEXCOORD1;
					fixed4 color : COLOR;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				half _PointSize;

				v2f vert(
					appdata v,
					out float4 outpos : SV_POSITION // clip space position output
				)
				{
					v2f o;
					outpos = mul(UNITY_MATRIX_MVP, v.vertex);

					o.psize = _PointSize;
					o.size = _PointSize;
					o.clipPos = outpos;
					o.color = v.color;
					return o;
				}

				fixed4 frag(v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
				{
					//pixel
					float2 pixPos = screenPos.xy / _ScreenParams.xy;

					//vertex
					float4 vertpos = i.clipPos / i.clipPos.w;
					vertpos.xyz = vertpos.xyz * 0.5 + 0.5;

					//Calculate uv
					float2 uv = (pixPos - vertpos.xy) / (i.size / _ScreenParams.y);

					float aspectRatio = _ScreenParams.x / _ScreenParams.y;
					uv.x *= aspectRatio;

					uv += 0.5;
					uv.y += _ScreenParams.y*2 / i.size * vertpos.y;
					uv.y -= fmod(_ScreenParams.y,i.size)/i.size;

					//distance
					float d = sqrt((0.5-uv.x)*(0.5-uv.x)+(0.5-uv.y)*(0.5-uv.y));
					//d-=(_Size-1);
					float b=(1-d);
					fixed4 col = fixed4(b,b,b,1);

					//tint
					col.rgb*=i.color.rgb;
					return col;
				}
				ENDCG
			}
		}
}