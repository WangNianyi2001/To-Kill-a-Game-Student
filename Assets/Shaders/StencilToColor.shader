Shader "Hidden/StencilToColor" {
	Properties {
		_MainTex("Texture", 2D) = "white" {}
		_RefValue("Ref Value", Integer) = 0
	}

	SubShader {
		Pass {
			Stencil {
				Ref[_RefValue]
				Comp Equal
				Pass Keep
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			int _RefValue;

			v2f vert(appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target{
				fixed4 color = { _RefValue, 0, 0, 1 };
				fixed4 c = { 1, 0, 0, 1 };
				return c;
			}
			ENDCG
		}
	}
}