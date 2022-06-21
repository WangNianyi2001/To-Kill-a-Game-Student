using System;
using UnityEngine;

public class Viewport : MonoBehaviour {
	public static byte nextStencilID = 1;

	[NonSerialized] public Page page;
	[NonSerialized] public Storyboard storyboard;
	public Camera soulCamera;
	public ViewportTrigger trigger = null;

	public byte stencilID;

	[NonSerialized] public new Camera camera;
	[NonSerialized] public CameraController camCtrl;
	[NonSerialized] public Transform mask;
	[NonSerialized] public Material maskMaterial;

	public bool Visible {
		get => camera.enabled;
		set => camera.enabled = value;
	}

	public void UpdateCamera(Transform root, Transform target) {
		var camPos = root.worldToLocalMatrix.MultiplyPoint(target.position);
		camera.transform.position = mask.localToWorldMatrix.MultiplyPoint(camPos);
		camera.transform.rotation = mask.rotation * Quaternion.Inverse(root.rotation) * target.rotation;
	}

	void Awake() {
		nextStencilID = 1;
	}

	void Start() {
		stencilID = nextStencilID++;

		if(trigger != null)
			trigger.viewport = this;
		Destroy(GetComponent<SpriteRenderer>());

		// Create camera
		var camObj = new GameObject("Camera");
		camObj.transform.SetParent(transform);
		camObj.transform.localScale = Vector3.one;
		camera = camObj.AddComponent<Camera>();
		camera.depth = 1;
		camera.clearFlags = CameraClearFlags.Nothing;
	}

	public void Init(Storyboard storyboard) {
		this.storyboard = storyboard;
		page = storyboard.page;
		camera.enabled = true;

		// Create viewport mask
		maskMaterial = new Material(Shader.Find("ViewportMask"));
		maskMaterial.SetInteger("_StencilID", stencilID);

		// Camera controller
		camCtrl = CameraController.CreateOn(camera.gameObject);
		camCtrl.Target = page.camera;
		camCtrl.transformCtrl.destinationBasis = mask;
		camCtrl.transformCtrl.sourceBasis = storyboard.transform;
		camCtrl.transformCtrl.positionDamping = page.camCtrl.transformCtrl.positionDamping;
		camCtrl.transformCtrl.rotationDamping = page.camCtrl.transformCtrl.rotationDamping;
	}
}
