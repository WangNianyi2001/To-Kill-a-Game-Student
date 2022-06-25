using UnityEngine;

public class PageCamera : PostProcess {
	public Page page;

	public static PageCamera CreateOn(Page page) {
		var pc = page.mainCamera.gameObject.AddComponent<PageCamera>();
		pc.page = page;
		return pc;
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
			page.stencilPassMat.SetTexture("_Replacement", storyboard.viewport.vpCam.bufferTex);
			page.stencilPassMat.SetInteger("_StencilID", storyboard.viewport.stencilID);
			Graphics.Blit(buffer.rt, tempBuffer.rt, page.stencilPassMat);
			Graphics.Blit(tempBuffer.rt, buffer.rt);
		}
		tempBuffer.Dispose();
		base.OnRenderImage(source, destination);
	}
}
