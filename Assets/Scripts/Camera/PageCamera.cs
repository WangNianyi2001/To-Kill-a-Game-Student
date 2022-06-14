using UnityEngine;

public class PageCamera : CameraImitator {
	public void OnRenderImage(RenderTexture source, RenderTexture destination) {
		RenderTexture buffer = RenderTexture.GetTemporary(source.width, source.height, source.depth);
		Graphics.Blit(source, buffer);
		foreach(Storyboard board in FindObjectsOfType<Storyboard>()) {
			if(board.type != Storyboard.Type.Viewport)
				continue;
			var vp = board.viewport;
			Graphics.Blit(vp.camCtrl.renderTexture, buffer, vp.maskMaterial);
		}
		Graphics.Blit(buffer, destination);
		RenderTexture.ReleaseTemporary(buffer);
	}
}