using System.Collections.Generic;
using UnityEngine;

public class PostProcess : MonoBehaviour {
	Camera targetCam;
	Camera renderingCam;

	void Start() {
		targetCam = GetComponent<Camera>();
		var renderingCamObj = new GameObject("Rendering Camera");
		renderingCamObj.transform.parent = transform;
		renderingCam = renderingCamObj.AddComponent<Camera>();
		CameraController.CreateOn(renderingCamObj).Target = targetCam;
	}

	static RenderTexture CreateRenderTexture(RenderTexture reference) {
		if(reference != null)
			return new RenderTexture(reference);
		return RenderTexture.GetTemporary(Screen.width, Screen.height, 24);
	}

	ImageBuffer buffer = new ImageBuffer();
	public List<Material> materials;

	void OnPreRender() {
		buffer.Update(targetCam.targetTexture);
		renderingCam.targetTexture = buffer.rt;
		renderingCam.Render();
	}
	
	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		foreach(var mat in materials)
			Graphics.Blit(buffer.rt, buffer.rt, mat);
		Graphics.Blit(buffer.rt, destination);
	}

	void OnDestroy() {
		buffer.Dispose();
	}
}
