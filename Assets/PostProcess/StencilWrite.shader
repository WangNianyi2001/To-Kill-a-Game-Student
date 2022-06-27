Shader "Custom/StencilWrite" {
	Properties {
		_MainTex("Texture", 2D) = "white" {}
		_StencilID ("Stencil ID", Integer) = 0
		_Resolution("Resolution", Integer) = 16
	}

	SubShader {
		Tags {
			"RenderType" = "Transparent"
		}

		Lighting Off
		ColorMaterial Emission
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct vertexInput {
				float4 vertex: POSITION;
				float2 texcoord: TEXCOORD0;
			};

			struct vertexOutput {
				float2 texcoord: TEXCOORD0;
				float4 position: SV_POSITION;
			};

			vertexOutput vert(vertexInput i) {
				vertexOutput o;
				o.position = UnityObjectToClipPos(i.vertex);
				o.texcoord = i.texcoord;
				return o;
			}

			sampler2D _MainTex;
			int _StencilID;
			int _Resolution;

			fixed4 frag(vertexOutput i) : SV_Target{
				float4 baseColor = tex2D(_MainTex, i.texcoord);
				float g = (float)_StencilID / _Resolution;
				float a = step(.5f, baseColor[3]);
				float4 color = { 0, g, 0, a };
				return color;
			}
			ENDHLSL
		}
	}
}
