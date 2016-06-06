Shader "Unlit/TrailBillBoard"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		LOD 100

		Pass
		{
			Cull back
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite on


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
				float4 uv2: TEXCOORD1;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 normal : Normal;
				float2 uv : TEXCOORD0;
				float4 uv2 : TEXCOORD1;
				fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;

//				float4x4 obj2WorldInverse = _World2Object * 1.0; 
// 				obj2WorldInverse[3][3] = 1.0;

				///////
// 				float3 cameraDirection = mul(obj2WorldInverse,_WorldSpaceCameraPos)-v.vertex;
 				float3 cameraDirection = mul(_World2Object,_WorldSpaceCameraPos)-v.vertex;
// 				cameraDirection = float3(0,0,-1);
 				v.normal.xyz = cross(cameraDirection,v.tangent.xyz);
// 				v.normal = abs(v.normal);

 				v.normal = normalize(v.normal);

 				float normalizedLife = clamp((_Time.y-v.uv2.z)/v.uv2.y,0,1);
 				v.vertex.xyz+=v.normal.xyz*(v.uv.x*2-1)*v.uv2.x * (1-normalizedLife);
//				v.vertex.xyz+=v.normal.xyz*(v.uv.x*2-1);
 				//////////

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = float4(1-normalizedLife,0,0,0);
 				o.normal = v.tangent;
 				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

//				return fixed4(i.normal.xyz,0.5);
				return i.color;
//				return fixed4(i.uv2.x,i.uv2.y,0,i.uv2.x);
			}
			ENDCG
		}
	}
}
