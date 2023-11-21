Shader "Custom/Outline"
{//https://www.santoshnalla.com/post/outline-shader - взял и доработал у этого типа
	Properties
	{
		[PerRendererData] _MainTex("MainTex ", 2D) = "white" {}

	  _Width("Width", Range(0, 1)) = 0.1
	  _Color("Color", Color) = (1,1,1,1)
	  _OutlineColor("OutlineColor", Color) = (1,1,1,1)
	}


		Subshader
	  {
		  Tags{"Queue"="Transparent+1"}
		  Blend SrcAlpha OneMinusSrcAlpha
		 Cull Off ZWrite On ZTest Always
			  

			Pass
			  {
			  CGPROGRAM
				   #pragma vertex vert
				   #pragma fragment frag

						 #include "UnityCG.cginc"

				 half4 _OutlineColor;
				 half _Width;

			/*	fixed4 Normalize(fixed4 value) {
					return value / sqrt(value.x * value.x + value.y * value.y + value.z * value.z + value.a * value.a); //желательно юзать, но много будет хавать
				}*/

				 struct mesh
				  {
					  half4 vertex: POSITION;
				  };

				  struct pixel
				  {
					  half4 pos : SV_POSITION;
				  };

				  pixel vert(mesh v)
				  {
					  pixel o;
					  v.vertex += v.vertex * _Width;
					  o.pos = UnityObjectToClipPos(v.vertex);
					  return o;
				  }

				  half4 frag(pixel i) : COLOR
				  {
					  return _OutlineColor.rgba;
				  }
			  ENDCG
			  }

			Pass{
					  CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag

						 #include "UnityCG.cginc"

				 half4 _Color;

					  struct mesh {
						  float4 vertex : POSITION;
						float2 uv : TEXCOORD0;
						};

				  struct pixel {
					  float4 position : POSITION;
					  float2 uv : TEXCOORD0;
				  };

				  pixel vert(mesh i) {
					  pixel o;
					  o.position = UnityObjectToClipPos(i.vertex);
					  o.uv = i.uv;
					  return o;
				  }

				  sampler2D _MainTex;

				  half4 frag(pixel i) : SV_TARGET{
					  return tex2D(_MainTex, i.uv) * _Color;
				  }
					  ENDCG
}

	  }
}