Shader "Custom/Stencil" {
    Properties {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _StencilID ("Stencil ID", Integer) = 0
    }
    SubShader {
        Tags {
            "RenderType" = "Opaque"
            "Queue" = "Geometry-1"
            "RenderPipeline" = "UniversalPipeline"
        }
        ColorMask 0
        Lighting Off
        Pass {
            ZWrite Off
            Stencil {
                Ref[_StencilID]
                Comp Always
                Pass Replace
            }
        }
    }
}
