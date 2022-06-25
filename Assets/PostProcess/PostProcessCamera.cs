using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessCamera : MonoBehaviour {
	[NonSerialized] public PostProcess pp;
	public new Camera camera;
	ImageBuffer buffer = new ImageBuffer();
	CommandBuffer cb;
	const CameraEvent cbEv = CameraEvent.AfterForwardAlpha;

	public static PostProcessCamera CreateOn(PostProcess pp) {
		var obj = new GameObject("Post Process Camera");
		obj.transform.parent = pp.transform;
		var ppc = obj.AddComponent<PostProcessCamera>();
		ppc.camera = obj.AddComponent<Camera>();
		ppc.camera.enabled = false;
		CameraController.CreateOn(ppc.camera).Target = pp.camera;
		return ppc;
	}

	void Start() {
		cb = new CommandBuffer();
		camera.AddCommandBuffer(cbEv, cb);
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
