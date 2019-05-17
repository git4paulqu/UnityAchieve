Shader "Scene/BlendLightmap"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LightMap ("LightMap", 2D) = "white" {}
		_Diffuse ("Diffuse", Color) = (1, 1, 1, 1)
		_Factor ("Factor", Range(0, 1)) = 0.5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Tags { "LightMode"="ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD2;
				float3 worldNormal : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _LightMap;
			float4 _LightMap_ST;

			fixed4 _Diffuse;
			float _Factor;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv0 = TRANSFORM_TEX(v.uv0, _MainTex);
				o.uv1 = v.uv1 * unity_LightmapST.xy + unity_LightmapST.zw;
				o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col, tex;
				// Fetch color texture
				tex = tex2D(_MainTex, i.uv0.xy);

				// Fetch lightmap
				half4 bakedColorTexL = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv1.xy);
				half4 bakedColorTexR = tex2D(_LightMap, i.uv1.xy);

				half3 bakedColorL = DecodeLightmap(bakedColorTexL);
				half3 bakedColorR = DecodeLightmap(bakedColorTexR);
				half3 result = lerp(bakedColorL, bakedColorR, _Factor);
			
				col.rgb = tex.rgb * result.rgb;

				//fixed3 worldNormal = normalize(i.worldNormal);
				//fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);

				//fixed halfLambert = dot(worldNormal, worldLightDir) * 0.5 + 0.5;
				//fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * halfLambert;
				//col.rgb *= diffuse;

				return col;
			}
			ENDCG
		}
	}
}
