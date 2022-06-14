using System;
using UnityEngine;

public class Viewport : MonoBehaviour {
	static byte nextStencilID = 1;

	[NonSerialized] public Page page;
	[NonSerialized] public Storyboard storyboard;
	public Camera soulCamera;
	public ViewportTrigger trigger = null;

	public byte stencilID;

	[NonSerialized] public new Camera camera;
	[NonSerialized] public ViewportCamera camCtrl;
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

		// Create Stencil mask
		//var maskObj = new GameObject("Stencil Mask", new Type[] {
		//	typeof(Canvas), typeof(CanvasRenderer)
		//});
		//mask = maskObj.transform;
		//maskObj.layer = LayerMask.NameToLayer("Viewport");
		//mask.SetParent(transform);
		//mask.localPosition = Vector3.zero;
		//mask.localRotation = Quaternion.identity;
		//var maskRenderer = maskObj.AddComponent<RawImage>();
		//maskRenderer.texture = storyboard.GetComponent<RawImage>().texture;

		// Set Stencil mask
		//(mask.transform as RectTransform).sizeDelta = (storyboard.transform as RectTransform).sizeDelta;
		//Shader stencilShader = Shader.Find("StencilWrite");
		//var material = new Material(stencilShader);
		//material.SetInteger("_StencilID", stencilID);
		//maskRenderer.material = material;

		// Create viewport mask
		maskMaterial = new Material(Shader.Find("ViewportMask"));
		maskMaterial.SetInteger("_StencilID", stencilID);

		// Camera controller
		camCtrl = camera.gameObject.AddComponent<ViewportCamera>();
		camCtrl.target = page.camera;
		camCtrl.destinationBasis = mask;
		camCtrl.sourceBasis = storyboard.transform;
		camCtrl.Jump();
		camCtrl.positionDamping = page.camCtrl.positionDamping;
		camCtrl.rotationDamping = page.camCtrl.rotationDamping;
	}
}
