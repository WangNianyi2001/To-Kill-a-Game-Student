Shader "Custom/StencilPass" {
	Properties {
		_MainTex("Base", 2D) = "white" {}
		_Replacement("Replacement", 2D) = "white" {}
		_StencilID("Stencil ID", Integer) = 0
		_Resolution("Resolution", Integer) = 16
	}
	SubShader{
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#define EPS .002f

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

			float CloseEnough(float a, float b) {
				return 1 - step(EPS, abs(a - b));
			}

			sampler2D _MainTex;
			sampler2D _Replacement;
			int _StencilID;
			int _Resolution;

			fixed4 frag(vertexOutput i) : SV_Target {
				float4 base = tex2D(_MainTex, i.texcoord);
				float rb = CloseEnough(base[0] + base[2], 0);
				float stencil = CloseEnough(round(base[1] * _Resolution), _StencilID);
				return lerp(base, tex2D(_Replacement, i.texcoord), rb * stencil);
			}
			ENDHLSL
		}
	}
}
