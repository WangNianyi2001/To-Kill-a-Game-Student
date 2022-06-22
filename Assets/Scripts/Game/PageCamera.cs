using UnityEngine;

public class PageCamera : PostProcess {
	public Page page;
	Material stencilPassMat;

	protected override void Start() {
		base.Start();
		if(page == null)
			page = transform.GetComponent<Page>();
		if(page == null)
			page = FindObjectOfType<Page>();
		stencilPassMat = new Material(Shader.Find("Custom/StencilPass"));
		stencilPassMat.SetInteger("_Resolution", page.stencilResolution);
	}

	protected override void OnRenderImage(RenderTexture source, RenderTexture destination) {
		var tempBuffer = new ImageBuffer();
		tempBuffer.Update(buffer.rt);
		foreach(var storyboard in FindObjectsOfType<Storyboard>()) {
			if(storyboard.state == Storyboard.State.Disabled)
				continue;
			if(storyboard.type != Storyboard.Type.Viewport)
				continue;
			storyboard.viewport.camera.Render();
			stencilPassMat.SetTexture("_Replacement", storyboard.viewport.vpCam.bufferTex);
			stencilPassMat.SetInteger("_StencilID", storyboard.viewport.stencilID);
			Graphics.Blit(buffer.rt, tempBuffer.rt, stencilPassMat);
			Graphics.Blit(tempBuffer.rt, buffer.rt);
		}
		tempBuffer.Dispose();
		base.OnRenderImage(source, destination);
	}
}
