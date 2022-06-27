using UnityEngine;

public class PageCamera : PostProcess {
	public Page page;
	public CameraController anchorCam;

	public static PageCamera CreateOn(Page page) {
		var camera = page.camera;
		var pc = camera.gameObject.AddComponent<PageCamera>();
		pc.page = page;
		CameraController.CreateOn(camera);
		var anchorCamObj = new GameObject();
		anchorCamObj.transform.SetParent(pc.transform.parent);
		anchorCamObj.AddComponent<Camera>().enabled = false;
		pc.anchorCam = anchorCamObj.AddComponent<CameraController>();
		return pc;
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
