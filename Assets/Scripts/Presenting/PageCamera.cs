using System;
using UnityEngine;

[RequireComponent(typeof(CameraController))]
public class PageCamera : PostProcess {
	public Page page;
	[NonSerialized] public CameraController controller;

	protected override void Start() {
		base.Start();
		controller = GetComponent<CameraController>();
	}

	protected override void OnRenderImage(RenderTexture source, RenderTexture destination) {
		var tempBuffer = new ImageBuffer();
		tempBuffer.Update(buffer.rt);
		page.stencilPassMat.SetInteger("_Resolution", page.stencilResolution);
		foreach(var storyboard in FindObjectsOfType<Storyboard>()) {
			if(storyboard.State == StoryboardState.Disabled)
				continue;
			if(storyboard.type != Storyboard.Type.Viewport)
				continue;
			storyboard.viewport.camera.Render();
			page.stencilPassMat.SetTexture("_Replacement", storyboard.viewport.vpCam.bufferTex);
			page.stencilPassMat.SetInteger("_StencilID", storyboard.viewport.stencilID);
			Graphics.Blit(buffer.rt, tempBuffer.rt, page.stencilPassMat);
			Graphics.Blit(tempBuffer.rt, buffer.rt);
		}
		tempBuffer.Dispose();
		base.OnRenderImage(source, destination);
	}
}
