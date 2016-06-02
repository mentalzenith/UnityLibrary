Shader "Unlit/CircleRenderer"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color",color ) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite On

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2: TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = v.uv2;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 xy = (i.uv-0.5)*2;
				float d = sqrt((0.5-i.uv.x)*(0.5-i.uv.x)+(0.5-i.uv.y)*(0.5-i.uv.y));
				float innerRadius = 0.5-i.uv2.x;
				float midPoint = lerp(0.5,innerRadius,0.5);

				fixed4 col = _Color;
				col.a *= smoothstep(1,0,(abs(d-midPoint))/(i.uv2.x*0.5));
				if(d<0.5 && d>innerRadius)
					return col;
//				return _Color*smoothstep(0,1,midPoint-d);
				return fixed4(i.uv.x,i.uv.y,0,0);
			}
			ENDCG
		}
	}
}
