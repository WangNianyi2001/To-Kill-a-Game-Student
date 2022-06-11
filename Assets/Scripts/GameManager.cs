using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;

public static class GameManager {
	public static T GetMember<T>(object target, string name) {
		return (T)target.GetType()
			.GetField(name, System.Reflection.BindingFlags.Instance
				| System.Reflection.BindingFlags.Public
				| System.Reflection.BindingFlags.NonPublic
			).GetValue(target);
	}

	public static void SetMember<T>(object target, string name, T value) {
		target.GetType()
			.GetField(name, System.Reflection.BindingFlags.Instance
				| System.Reflection.BindingFlags.Public
				| System.Reflection.BindingFlags.NonPublic
			).SetValue(target, value);
	}

	public static int CreateViewportRenderer(int stencilID) {
		var pipeline = Object.Instantiate(GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset);
		var rendererData = GetMember<ScriptableRendererData[]>(pipeline, "m_RendererDataList").ToList();
		int index = rendererData.Count;
		var rendererDatum = Object.Instantiate(Resources.Load<UniversalRendererData>("Viewport Renderer"));
		var opaqueFeature = rendererDatum.rendererFeatures[1] as RenderObjects;
		opaqueFeature.settings.stencilSettings.stencilReference = stencilID;
		var transparentFeature = rendererDatum.rendererFeatures[2] as RenderObjects;
		transparentFeature.settings.stencilSettings.stencilReference = stencilID;
		rendererData.Add(rendererDatum);
		SetMember(pipeline, "m_RendererDataList", rendererData.ToArray());
		GraphicsSettings.renderPipelineAsset = pipeline;
		return index;
	}
}