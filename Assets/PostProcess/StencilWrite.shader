Shader "Custom/StencilWrite" {
	Properties {
		_MainTex("Texture", 2D) = "white" {}
		_StencilID ("Stencil ID", Integer) = 0
		_Resolution("Resolution", Integer) = 16
	}

	SubShader {
		Lighting Off
		ColorMaterial Emission

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
				float g = _StencilID;
				g /= _Resolution;
				float4 color = {0, g, 0, 1};
				return color;
			}
			ENDHLSL
		}
	}
}
