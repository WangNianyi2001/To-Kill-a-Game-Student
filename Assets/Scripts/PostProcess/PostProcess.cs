using System.Collections.Generic;
using UnityEngine;

public class PostProcess : MonoBehaviour {
	[System.NonSerialized] public new Camera camera;
	PostProcessCamera ppc;
	public List<Material> materials = new List<Material>();

	protected virtual void Start() {
		if(camera == null)
			camera = GetComponent<Camera>();
		ppc = PostProcessCamera.CreateOn(this);
	}

	protected ImageBuffer buffer = new ImageBuffer();
	public RenderTexture bufferTex => buffer.rt;

	void OnPreRender() {
		if(buffer.Update(camera?.targetTexture))
			ppc.UpdateDest(buffer);
		ppc.camera.Render();
	}

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination) {
		foreach(var mat in materials)
			Graphics.Blit(buffer.rt, buffer.rt, mat);
		Graphics.Blit(buffer.rt, destination);
	}

	void OnDestroy() {
		buffer?.Dispose();
	}
}
