Shader "StencilWrite" {
	Properties {
		_MainTex("Texture", 2D) = "white" {}
		_StencilID ("Stencil ID", Integer) = 0
	}

	SubShader {
		Tags {
			"RenderType" = "Opaque"
		}
		//ColorMask 0
		//Lighting Off
		Pass {
			//ZWrite Off
			Stencil {
				Ref[_StencilID]
				Comp Always
				Pass Replace
			}
		}
	}
}
