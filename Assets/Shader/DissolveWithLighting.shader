// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "HoopAWolf/Dissolve/With Lighting" 
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_Roughness("Roughness Texture", 2D) = "white" {}
		_BumpMap("Normal Texture", 2D) = "bump" {}
		_BumpDepth("Bump Depth", Range(-2.0, 2.0)) = 1
		_Shininess("Shininess", float) = 10
		_RimPower("Rim Power", Range(0.1, 10.0)) = 3.0

		_DissolveTexture("Dissolve Texture", 2D) = "white" {}

		_Color("Color", Color) = (1, 1, 1, 1)
		_DissolveAmount("Dissolve Amount", Range(0, 1)) = 1

		_DissolveColorStart("Dissolve Color Start", Color) = (1, 1, 1, 1)
		_DissolveColorEnd("Dissolve Color End", Color) = (1, 1, 1, 1)
		_DissolveSize("Dissolve Size", Range(0, 0.1)) = 0
	}
		SubShader
		{
			Pass
			{
				Tags
				{
					"Queue" = "Transparent"
					"RenderType" = "Transparent"
					"LightMode" = "ForwardBase"
				}

				Blend SrcAlpha OneMinusSrcAlpha
				Cull Off

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma exclude_renderers flash

			// User Defined Variables
			uniform sampler2D
				_MainTex,
				_DissolveTexture,
				_Roughness,
				_BumpMap;

			uniform float4
				_Color, 
				_DissolveColorStart, 
				_DissolveColorEnd,

				_MainTex_ST,
				_Roughness_ST,
				_BumpMap_ST, 
				_LightColor0;

			uniform float
				_BumpDepth,
				_Shininess,
				_RimPower, 
				_DissolveAmount, 
				_DissolveSize;
			

			// Base Input Structs
			struct vertexInput 
			{
				float4 vertex: POSITION;
				float3 normal: NORMAL;
				float4 texcoord: TEXCOORD0;
				float4 tangent: TANGENT;
			};
			struct vertexOutput 
			{
				float4 pos: SV_POSITION;
				float4 tex: TEXCOORD0;
				float4 posWorld: TEXCOORD1;
				float3 normalWorld: TEXCOORD2;
				float3 tangentWorld: TEXCOORD3;
				float3 binormalWorld: TEXCOORD4;
			};

			// Vertex Function
			vertexOutput vert(vertexInput v) 
			{
				vertexOutput o;

				o.normalWorld = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
				o.tangentWorld = normalize(mul(unity_ObjectToWorld, v.tangent).xyz);
				o.binormalWorld = normalize(cross(o.normalWorld, o.tangentWorld));

				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.tex = v.texcoord;

				return o;
			}

			// Fragment Function
			float4 frag(vertexOutput i) : COLOR 
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
				float4 texM = tex2D(_MainTex, i.tex);
				float4 texR = tex2D(_Roughness, i.tex.xy * _Roughness_ST.xy + _Roughness_ST.zw);
				float4 texN = tex2D(_BumpMap, i.tex.xy * _BumpMap_ST.xy + _BumpMap_ST.zw);

				float disColor = tex2D(_DissolveTexture, i.tex).r;

				// unpackNormal Function
				float3 localCoords = float3(2.0 * texN.ag - float2(1.0,1.0), 0.0);
				
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
				

				if (disColor < _DissolveAmount)
					discard;

				if (disColor < texM.a && disColor < _DissolveAmount + _DissolveSize)
					texM = lerp(_DissolveColorStart, _DissolveColorEnd, (disColor - _DissolveAmount) / _DissolveSize);

				return float4(texM.xyz * lightFinal * _Color.xyz, 1.0);
			}

			ENDCG
		}

		////Point light / Spot light pass
		//Pass
		//{
		//	Tags
		//	{
		//		"Queue" = "Transparent"
		//		"RenderType" = "Transparent"
		//		"LightMode" = "ForwardAdd"
		//	}

		//	Blend SrcAlpha OneMinusSrcAlpha
		//	Cull Off
		//	Blend One One

		//	CGPROGRAM
		//	#pragma vertex vert
		//	#pragma fragment frag
		//	#pragma exclude_renderers flash

		//	// User Defined Variables
		//	uniform sampler2D
		//		_MainTex,
		//		_DissolveTexture,
		//		_Roughness,
		//		_BumpMap;

		//	uniform float4
		//		_Color,
		//		_DissolveColorStart,
		//		_DissolveColorEnd,

		//		_MainTex_ST,
		//		_Roughness_ST,
		//		_BumpMap_ST,
		//		_LightColor0;

		//	uniform float
		//		_BumpDepth,
		//		_Shininess,
		//		_RimPower,
		//		_DissolveAmount,
		//		_DissolveSize;


		//	// Base Input Structs
		//	struct vertexInput
		//	{
		//		float4 vertex: POSITION;
		//		float3 normal: NORMAL;
		//		float4 texcoord: TEXCOORD0;
		//		float4 tangent: TANGENT;
		//	};
		//	struct vertexOutput
		//	{
		//		float4 pos: SV_POSITION;
		//		float4 tex: TEXCOORD0;
		//		float4 posWorld: TEXCOORD1;
		//		float3 normalWorld: TEXCOORD2;
		//		float3 tangentWorld: TEXCOORD3;
		//		float3 binormalWorld: TEXCOORD4;
		//	};

		//	// Vertex Function
		//	vertexOutput vert(vertexInput v)
		//	{
		//		vertexOutput o;

		//		o.normalWorld = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
		//		o.tangentWorld = normalize(mul(unity_ObjectToWorld, v.tangent).xyz);
		//		o.binormalWorld = normalize(cross(o.normalWorld, o.tangentWorld));

		//		o.posWorld = mul(unity_ObjectToWorld, v.vertex);
		//		o.pos = UnityObjectToClipPos(v.vertex);
		//		o.tex = v.texcoord;

		//		return o;
		//	}

		//	// Fragment Function
		//	float4 frag(vertexOutput i) : COLOR
		//	{
		//		float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
		//		float3 lightDirection;
		//		float atten;

		//		if (_WorldSpaceLightPos0.w == 0.0)
		//		{ // Directional Light
		//			atten = 1.0;
		//			lightDirection = normalize(_WorldSpaceLightPos0.xyz);
		//		}
		//		else
		//		{	// Point/Spot Light
		//			float3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
		//			float distance = length(fragmentToLightSource);
		//			atten = 1 / distance;
		//			lightDirection = normalize(fragmentToLightSource);
		//		}

		//		// Texture Maps
		//		float4 texM = tex2D(_MainTex, i.tex);
		//		float4 texR = tex2D(_Roughness, i.tex.xy * _Roughness_ST.xy + _Roughness_ST.zw);
		//		float4 texN = tex2D(_BumpMap, i.tex.xy * _BumpMap_ST.xy + _BumpMap_ST.zw);

		//		float disColor = tex2D(_DissolveTexture, i.tex).r;

		//		// unpackNormal Function
		//		float3 localCoords = float3(2.0 * texN.ag - float2(1.0,1.0), 0.0);

		//		localCoords.z = _BumpDepth;

		//		// Normal Transpose Matrix
		//		float3x3 local2WorldTranspose = float3x3
		//			(
		//				i.tangentWorld,
		//				i.binormalWorld,
		//				i.normalWorld
		//			);

		//		// Calculate Normal Direction
		//		float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose));

		//		// Lighting
		//		float3 diffuseReflection = atten * _LightColor0.rgb * saturate(dot(normalDirection, lightDirection));
		//		float3 specularReflection = diffuseReflection * pow(saturate(dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);

		//		// Rim Lighting
		//		float rim = 1 - saturate(dot(viewDirection, normalDirection));
		//		float3 rimLighting = saturate(pow(rim, _RimPower) * diffuseReflection);
		//		float3 lightFinal = diffuseReflection + (specularReflection * texR.a) + rimLighting + UNITY_LIGHTMODEL_AMBIENT.rgb;


		//		if (disColor < _DissolveAmount)
		//			discard;

		//		if (disColor < texM.a && disColor < _DissolveAmount + _DissolveSize)
		//			texM = lerp(_DissolveColorStart, _DissolveColorEnd, (disColor - _DissolveAmount) / _DissolveSize);

		//		return float4(texM.xyz * lightFinal * _Color.xyz, 1.0);
		//	}

		//	ENDCG
		//}
	}
			//FallBack "Specular"
}