Shader "Unlit/RadialGredient"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)

		 
		_Strength("Strength",Range (0,1)) = 1

		_Offset("Offset", Range (0,1)) = 0
	}
	SubShader
	{
		Tags { "Queue" = "Geometry" "RenderType" = "Opaque" "LightMode" = "Always" "LightMode" = "ForwardBase" }

		Pass
		{
		Blend One One
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma fragmentoption ARB_precision_hint_fastest
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				LIGHTING_COORDS(1, 2)
			};

			float4 _Color;
			float _Strength;
			float _Offset;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = float4( v.uv.xy, 0, 0 );
				TRANSFER_VERTEX_TO_FRAGMENT(o)
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed atten = LIGHT_ATTENUATION(i);

				//distance
				float d = sqrt((0.5-i.uv.x)*(0.5-i.uv.x)+(0.5-i.uv.y)*(0.5-i.uv.y));
				d-=(_Offset-1);
				float b=(1-d)*_Strength;
				fixed4 col = fixed4(b,b,b,1);

				//tint
				col.rgb*=_Color.rgb;

				//shadow
				col.rgb*=atten;
				return col;
			}
			ENDCG
		}
	}
}
