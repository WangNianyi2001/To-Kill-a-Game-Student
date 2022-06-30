using System;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera), typeof(CameraController))]
public class PostProcessCamera : MonoBehaviour {
	[NonSerialized] public PostProcess pp;
	public new Camera camera;
	ImageBuffer buffer = new ImageBuffer();
	CommandBuffer cb;
	const CameraEvent cbEv = CameraEvent.AfterForwardAlpha;

	void Start() {
		camera = GetComponent<Camera>();
		camera.enabled = false;
		cb = new CommandBuffer();
		camera.AddCommandBuffer(cbEv, cb);
		GetComponent<CameraController>().Target = pp.camera;
		camera.clearFlags = pp.camera.clearFlags;
		camera.backgroundColor = pp.camera.backgroundColor;
	}

	public void UpdateDest(ImageBuffer dest) {
		buffer.Update(dest.rt);
		camera.targetTexture = buffer.rt;
		cb.Clear();
		cb.Blit(buffer.rt, dest.rt);
	}

	void OnDestroy() {
		cb.Release();
		buffer?.Dispose();
		Destroy(gameObject);
	}
}
