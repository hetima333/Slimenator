// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "HoopAWolf/ShowBehind"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", color) = (1, 1, 1, 1)
		_PlayerPosition("Player Position", vector) = (0, 0, 0, 0)
		_VisibleDistance("Visibility Distance", Range(0, 8)) = 0
		_OutlineWidth("Outline Width", Range(0, 0.1)) = 0
		_OutlineColor("Outline Color", color) = (1, 1, 1, 1)

		_Roughness("Roughness Texture", 2D) = "white" {}
		_BumpMap("Normal Texture", 2D) = "bump" {}
		_BumpDepth("Bump Depth", Range(-2.0, 2.0)) = 1
		_Shininess("Shininess", float) = 10
		_RimPower("Rim Power", Range(0.1, 10.0)) = 3.0

		_Translucency("Translucent of object when something is behind", Range(0, 1)) = 0

	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"RenderType" = "Transparent"
			}

			//Base Texture
			Pass
			{
			
				 Tags {"LightMode" = "ForwardBase"}

				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fwdbase
			
				#include "AutoLight.cginc"
				#include "UnityCG.cginc"

				uniform sampler2D
					_MainTex,
					_Roughness,
					_BumpMap;

				uniform float4
					_PlayerPosition;

				uniform float
					_VisibleDistance,
					_OutlineWidth, 
					_Translucency, 

					_BumpDepth,
					_Shininess,
					_RimPower;

				uniform fixed4
					_OutlineColor,
					_Color, 

					_MainTex_ST,
					_Roughness_ST,
					_BumpMap_ST,
					_LightColor0;

				//defines what information we are getting from each vertex on the mesh
				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					float3 normal : NORMAL;		
					float4 tangent: TANGENT;
				};

				//defines what information we are passing into the fragment func
				struct v2f
				{
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;
					float4 posWorld : TEXCOORD1;
					float4 color : COLOR0;
					float4 tangent: TANGENT;
					float3 normalWorld: TEXCOORD2;
					float3 tangentWorld: TEXCOORD3;
					float3 binormalWorld: TEXCOORD4;

					// FOLLOWING LINE IS NEW
					SHADOW_COORDS(1)
				};

				//it takes appdata struct as para and returns a v2f
				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.posWorld = mul(unity_ObjectToWorld, v.vertex);
					o.uv = v.uv;
					// first off, we need the normal to be in world space
					float3 normalDirection = mul(unity_ObjectToWorld, v.normal);

					// we will only be dealing with a single directional light
					float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
					float ndotl = dot(normalDirection, lightDirection);
					float3 diffuse = _LightColor0.xyz * max(0.0, ndotl);

					o.color = half4(diffuse, 1.0);

					o.normalWorld = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
					o.tangentWorld = normalize(mul(unity_ObjectToWorld, v.tangent).xyz);
					o.binormalWorld = normalize(cross(o.normalWorld, o.tangentWorld));

					o.posWorld = mul(unity_ObjectToWorld, v.vertex);

					// FOLLOWING LINE IS NEW
					TRANSFER_VERTEX_TO_FRAGMENT(o);
					return o;
				}

				//it takes a v2f and returns a color of the float
				float4 frag(v2f i) : SV_Target
				{


					float dist = distance(i.posWorld, _PlayerPosition);

					if (dist > _VisibleDistance)
					{
						float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
						float3 lightDirection;
						float atten;

						if (_WorldSpaceLightPos0.w == 0.0)
						{ // Directional Light
							atten = 1.0;
							lightDirection = normalize(_WorldSpaceLightPos0.xyz);
						}
						else
						{	// Point/Spot Light
							float3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
							float distance = length(fragmentToLightSource);
							atten = 1 / distance;
							lightDirection = normalize(fragmentToLightSource);
						}

						// Texture Maps
						float4 texM = tex2D(_MainTex, i.uv);
						float4 texR = tex2D(_Roughness, i.uv.xy * _Roughness_ST.xy + _Roughness_ST.zw);
						float4 texN = tex2D(_BumpMap, i.uv.xy * _BumpMap_ST.xy + _BumpMap_ST.zw);

						// unpackNormal Function
						float3 localCoords = float3(2.0 * texN.ag - float2(1.0, 1.0), 0.0);

						localCoords.z = _BumpDepth;

						// Normal Transpose Matrix
						float3x3 local2WorldTranspose = float3x3
							(
								i.tangentWorld,
								i.binormalWorld,
								i.normalWorld
								);

						// Calculate Normal Direction
						float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose));

						// Lighting
						float3 diffuseReflection = atten * _LightColor0.rgb * saturate(dot(normalDirection, lightDirection));
						float3 specularReflection = diffuseReflection * pow(saturate(dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);

						// Rim Lighting
						float rim = 1 - saturate(dot(viewDirection, normalDirection));
						float3 rimLighting = saturate(pow(rim, _RimPower) * diffuseReflection);
						float3 lightFinal = diffuseReflection + (specularReflection * texR.a) + rimLighting + UNITY_LIGHTMODEL_AMBIENT.rgb;


						return float4(texM.xyz * lightFinal * _Color.xyz, 1.0) * SHADOW_ATTENUATION(i);
					}
					else if (dist > _VisibleDistance - _OutlineWidth)
					{
						return _OutlineColor;
					}
					else
					{
						float4 tex = tex2D(_MainTex, i.uv);
						tex.a = _Translucency;
						return tex;
					}
				}
				ENDCG
					}
		}

			Fallback "Diffuse"
}