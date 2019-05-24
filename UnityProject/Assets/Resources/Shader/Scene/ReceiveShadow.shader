Shader "ShadowMap/ReceiveShadow"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Diffuse ("Diffuse", Color) = (1, 1, 1, 1)
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
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 worldNormal:TEXCOORD1;
				float4 shadowCoord:TEXCOORD2;
				UNITY_FOG_COORDS(3)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Diffuse;

			uniform float _ShadowIntensity;
			uniform fixed _ShadowMapTexmapScale;
			uniform sampler2D _LightSpaceDepthTexture;
			uniform float4x4 _World2LightSpace;
			uniform float4x4 _World2LightSpace2UV;

			float4 offset_lookup(sampler2D map, float4 uv, float2 offset)
			{
				return tex2D(map, uv.xy + offset * _ShadowMapTexmapScale);
			}

			float fragPCF4x4(v2f IN)
			{
		  		float sum = 0;
		  		float x,y;
		  		for (y = -1.5; y <= 1.5; y += 1.0)
					  for (x = -1.5; x <= 1.5; x += 1.0)
					  {
					  		float depth = DecodeFloatRGBA(offset_lookup(_LightSpaceDepthTexture, IN.shadowCoord, float2(x,y)));
		  					float shade = step(IN.shadowCoord.z, depth);
		  					sum += shade;
					  }
		  		sum = sum / 16.0;
				sum = max((1-sum), _ShadowIntensity);
		  	    return sum;
			}

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
				float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.shadowCoord = mul(_World2LightSpace2UV, worldPos);
				o.shadowCoord.z = mul(_World2LightSpace, worldPos).z;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);

				fixed halfLambert = dot(worldNormal, worldLightDir) * 0.5 + 0.5;
				fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * halfLambert;
				col.rgb *= diffuse;

				//float depth = DecodeFloatRGBA(tex2D(_LightSpaceDepthTexture, i.shadowCoord.xy));
		  		//float shade = step(i.shadowCoord.z, depth);
		  		//col *= max((1-shade), _ShadowIntensity);
				col *= fragPCF4x4(i);

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
