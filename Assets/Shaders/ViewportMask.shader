Shader "ViewportMask" {
	Properties {
		_MainTex("Source", 2D) = "white" {}
		_StencilID("Stencil ID", Integer) = 0
	}
	SubShader{
		Cull Off ZWrite Off ZTest Always

		Pass {
			Stencil {
				Ref [_StencilID]
				Comp Equal
				Pass Keep
			}

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
				o.position = mul(UNITY_MATRIX_MVP, i.vertex);
				o.texcoord = i.texcoord;
				return o;
			}

			sampler2D _MainTex;
			float4 _MainTex_ST;

			fixed4 frag(vertexOutput i) : SV_Target {
				float4 color = tex2D(
					_MainTex,
					UnityStereoScreenSpaceUVAdjust(
						i.texcoord, _MainTex_ST
					)
				);
				fixed4 c = { 1, .5f, 0, .5 };
				return lerp(color, c, .5);
			}
			ENDHLSL
		}
	}
}
