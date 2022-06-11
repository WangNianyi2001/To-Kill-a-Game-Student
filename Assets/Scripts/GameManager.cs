using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;

public static class GameManager {
	public static T GetMember<T>(object target, string name) {
		return (T)target.GetType()
			.GetField(name, BindingFlags.Instance
				| BindingFlags.Public | BindingFlags.NonPublic
			).GetValue(target);
	}

	public static void SetMember<T>(object target, string name, T value) {
		target.GetType()
			.GetField(name, BindingFlags.Instance
				| BindingFlags.Public | BindingFlags.NonPublic
			).SetValue(target, value);
	}

	public static T DeepClone<T>(this T obj) {
		using(var ms = new MemoryStream()) {
			var formatter = new BinaryFormatter();
			formatter.Serialize(ms, obj);
			ms.Position = 0;

			return (T)formatter.Deserialize(ms);
		}
	}

	public static int CreateViewportRenderer(int stencilID) {
		var pipeline = Object.Instantiate(GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset);
		var rendererDataList = GetMember<ScriptableRendererData[]>(pipeline, "m_RendererDataList").ToList();
		int index = rendererDataList.Count;
		var rendererData = Object.Instantiate(Resources.Load<UniversalRendererData>("Viewport Renderer"));
		var features = rendererData.rendererFeatures.Cast<RenderObjects>()
			.Select((RenderObjects target) => {
				var copy = ScriptableObject.CreateInstance<RenderObjects>();
				copy.settings = typeof(object)
					.GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic)
					.Invoke(target.settings, new object[] { }) as RenderObjects.RenderObjectsSettings;
				copy.settings.stencilSettings = target.settings.stencilSettings.DeepClone();
				return copy;
			}).ToList();
		var opaqueFeature = features[1];
		opaqueFeature.settings.stencilSettings.stencilReference = stencilID;
		var transparentFeature = features[2];
		transparentFeature.settings.stencilSettings.stencilReference = stencilID;
		SetMember(rendererData, "m_RendererFeatures", features.ToList<ScriptableRendererFeature>());
		rendererDataList.Add(rendererData);
		SetMember(pipeline, "m_RendererDataList", rendererDataList.ToArray());
		GraphicsSettings.renderPipelineAsset = pipeline;
		return index;
	}
}