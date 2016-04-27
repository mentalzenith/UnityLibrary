Shader "Unlit/TransparentZWrite"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color",Color) = (1,1,1,1)
		_RimStrength("Rim Strength",Range(0,1))=0.5
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "RenderType" = "Transparent"}
		LOD 100

		Pass
		{
			//Cull Back
			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha
			//Blend One OneMinusSrcAlpha
			//Blend DstColor Zero
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal: NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
				float3 normal:TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _RimStrength;
			
			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				//offset the vertex inside a bit to avoid ZFighting
				float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				float2 offset = TransformViewToProjection(norm.xy); 
				o.vertex.xy += offset * o.vertex.z * -0.0005;


				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = v.normal;
				// compute world space position of the vertex
                float3 worldPos = mul(_Object2World, v.vertex).xyz;
                // compute world space view direction
                o.viewDir = normalize(UnityWorldSpaceViewDir(worldPos));

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;
				half rim = 1.0 - saturate(dot (normalize(i.viewDir), i.normal));
				col+=rim*_RimStrength;
				return col;
			}
			ENDCG
		}
	}
}
