using System;
using UnityEngine;

public class ViewportCamera : CameraImitator {
	[NonSerialized] public RenderTexture renderTexture = null;

	public void OnPreRender() {
		if(renderTexture != null)
			RenderTexture.ReleaseTemporary(renderTexture);
		renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height);
		GetComponent<Camera>().targetTexture = renderTexture;
	}
}