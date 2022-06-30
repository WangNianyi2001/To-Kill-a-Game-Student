using System;
using UnityEngine;

public class Viewport : MonoBehaviour {
	public static byte nextStencilID = 1;

	[NonSerialized] public Page page;
	[NonSerialized] public Storyboard storyboard;
	public Camera soulCamera;
	public ViewportTrigger trigger = null;

	[NonSerialized] public byte stencilID;

	[NonSerialized] public ViewportCamera vpCam;
	[NonSerialized] public CameraController camCtrl;
	[NonSerialized] public new Camera camera;

	public void UpdateCamera(Transform root, Transform target) {
		var camPos = root.worldToLocalMatrix.MultiplyPoint(target.position);
		camera.transform.position = transform.localToWorldMatrix.MultiplyPoint(camPos);
		camera.transform.rotation = transform.rotation * Quaternion.Inverse(root.rotation) * target.rotation;
	}

	void Awake() {
		nextStencilID = 1;
	}

	void Start() {
		stencilID = nextStencilID++;
		Destroy(GetComponent<SpriteRenderer>());
		if(trigger != null)
			trigger.viewport = this;

		// Create camera
		var camObj = new GameObject("Camera");
		camObj.transform.SetParent(transform);
		camera = camObj.AddComponent<Camera>();
		camera.depth = 1;
		camera.enabled = false;
	}

	public void Init(Storyboard storyboard) {
		this.storyboard = storyboard;
		page = storyboard.page;

		// Camera controller
		vpCam = ViewportCamera.CreateOn(this);
		camCtrl = camera.gameObject.AddComponent<CameraController>();
		camCtrl.SendMessage("Start");
		camCtrl.Target = page.pc.camera;
		camCtrl.transformCtrl.destinationBasis = transform;
		camCtrl.transformCtrl.sourceBasis = storyboard.transform;
	}

	void OnDrawGizmos() {
		Gizmos.DrawRay(transform.position, transform.forward);
	}
}
