// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

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
				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

			    uniform sampler2D 
					_MainTex;

				uniform float4 
					_PlayerPosition;

				uniform float 
					_VisibleDistance, 
					_OutlineWidth;

				uniform fixed4 
					_OutlineColor, 
					_Color;

			//defines what information we are getting from each vertex on the mesh
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

		//defines what information we are passing into the fragment func
		struct v2f
		{
			float4 vertex : SV_POSITION;
			float4 world_space_position : TEXCOORD0;
			float2 uv : TEXCOORD1;
		};

		//it takes appdata struct as para and returns a v2f
		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.world_space_position = mul(unity_ObjectToWorld, v.vertex);
			o.uv = v.uv;
			return o;
		}

		//it takes a v2f and returns a color of the float
		float4 frag(v2f i) : SV_Target
		{
		    float dist = distance(i.world_space_position, _PlayerPosition);

			if (dist > _VisibleDistance) 
			{
				return tex2D(_MainTex, i.uv) * _Color;
			}
			else if (dist > _VisibleDistance - _OutlineWidth) 
			{
				return _OutlineColor;
			}
			else 
			{
				float4 tex = tex2D(_MainTex, i.uv);
				tex.a = 0;
				return tex;
			}
		}
		ENDCG
			}
		}
}