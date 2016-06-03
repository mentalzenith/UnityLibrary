Shader "Unlit/TrailBillBoard"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : Normal;
				float4 tangent : TANGENT;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 tangent : TANGENT;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;

				float3 world = mul(_Object2World,v.vertex);
				float3 cameraDirection = _WorldSpaceCameraPos-world;
				cameraDirection = normalize(cameraDirection);
//				float3 normal = v.tangent;
				v.normal.xyz = cross(v.tangent,cameraDirection);
				v.normal.xyz = cross(cameraDirection,v.tangent);
				v.normal = normalize(v.normal);
				v.vertex.xyz += v.normal.xyz*(v.uv.x*2-1);

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.tangent = v.tangent;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				return fixed4(i.uv.x,i.uv.y,1,1);
			}
			ENDCG
		}
	}
}
