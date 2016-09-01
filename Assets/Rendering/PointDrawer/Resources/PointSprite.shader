Shader "Hidden/PointSprite"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_PointSize("PointSize", float) = 10
		_CameraParam("CameraParam",float) = 0
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

			Blend SrcAlpha OneMinusSrcAlpha
//			Blend OneMinusDstColor One
			ZWrite Off
			ZTest Always
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
				float _CameraParam;

				v2f vert(
					appdata v,
					out float4 outpos : SV_POSITION // clip space position output
				)
				{
					v2f o;
					outpos = mul(UNITY_MATRIX_MVP, v.vertex);

					float d =distance(_WorldSpaceCameraPos,v.vertex);
					float multipler = _CameraParam/d;

					o.psize = _PointSize*multipler;
					o.size = _PointSize*multipler;
					o.clipPos = outpos;
					o.color = v.color;
					return o;
				}

				fixed4 frag(v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
				{
					return i.color;

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

					return tex2D(_MainTex, uv)*i.color;
				}
				ENDCG
			}
		}
}