Shader "HoopAWolf/Dissolve"
{
	Properties
	{
		_MainTexture("Main Texture", 2D) = "white" {}
		_DissolveTexture("Dissolve Texture", 2D) = "white" {}

		_Color("Color", Color) = (1, 1, 1, 1)
		_DissolveAmount("Dissolve Amount", Range(0, 1)) = 1

		_DissolveColorStart("Dissolve Color Start", Color) = (1, 1, 1, 1)
		_DissolveColorEnd("Dissolve Color End", Color) = (1, 1, 1, 1)
		_DissolveSize("Dissolve Size", Range(0, 0.1)) = 0
	}

		SubShader
		{
				
			Tags 
			{ 
				"Queue" = "Transparent" 
				"RenderType" = "Transparent" 
			}

			Pass
			{
				Blend SrcAlpha OneMinusSrcAlpha
				Cull Off
				ZWrite Off

				CGPROGRAM

				#pragma vertex vertexFunc
				#pragma fragment fragmentFunc

				#include "UnityCG.cginc"


				//Vertex
				//Build the object
				// Vert, Normal, Color, UV
				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float4 position : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				sampler2D 
					_MainTexture,
					_DissolveTexture;

				float4 
					_Color, 
					_DissolveColorStart,
					_DissolveColorEnd,
					
					_MainTexture_ST;

				float 
					_DissolveAmount, 
					_DissolveSize;

				//Build Object
				v2f vertexFunc(appdata _input)
				{
					v2f output;

					output.position = UnityObjectToClipPos(_input.vertex);
					output.uv = TRANSFORM_TEX(_input.uv, _MainTexture);
					
					return output;
				}

				//Fragment
				//Color it in
				fixed4 fragmentFunc(v2f _input) : SV_Target
				{
					float4 texColor = tex2D(_MainTexture, _input.uv) * _Color;
					float disColor = tex2D(_DissolveTexture, _input.uv).r;

					if (disColor < _DissolveAmount)
						discard;

					if(disColor < texColor.a && disColor < _DissolveAmount + _DissolveSize)
						texColor = lerp(_DissolveColorStart, _DissolveColorEnd, (disColor - _DissolveAmount) / _DissolveSize);

					return texColor;
				}	

				ENDCG
			}
		}
}