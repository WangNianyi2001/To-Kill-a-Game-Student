using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Viewport : MonoBehaviour {
	static byte nextStencilID = 1;

	[NonSerialized] public Storyboard storyboard;
	public Camera soulCamera;
	public ViewportTrigger trigger = null;

	public readonly byte stencilID;

	public Viewport() {
		stencilID = ++nextStencilID;
	}

	[NonSerialized] public new Camera camera;
	[NonSerialized] public CameraImitator imitator;
	[NonSerialized] public Transform mask;

	public bool Visible {
		get => camera.enabled;
		set => camera.enabled = value;
	}

	public void UpdateCamera(Transform root, Transform target) {
		var camPos = root.worldToLocalMatrix.MultiplyPoint(target.position);
		camera.transform.position = mask.localToWorldMatrix.MultiplyPoint(camPos);
		camera.transform.rotation = mask.rotation * Quaternion.Inverse(root.rotation) * target.rotation;
	}

	void Start() {
		if(trigger != null)
			trigger.viewport = this;
		var dummySprite = GetComponent<SpriteRenderer>();
		if(dummySprite != null)
			Destroy(dummySprite);

		// Create Stencil mask
		var maskObj = new GameObject("Stencil Mask");
		mask = maskObj.transform;
		maskObj.layer = LayerMask.NameToLayer("Viewport");
		mask.SetParent(transform);
		mask.localPosition = Vector3.zero;
		mask.localRotation = Quaternion.identity;
		var maskRenderer = maskObj.AddComponent<SpriteRenderer>();
		maskRenderer.sprite = Resources.Load<Sprite>("Sprite/White");

		// Set Stencil mask
		mask.localScale = (storyboard.transform as RectTransform).rect.size;
		Shader stencilShader = Shader.Find("Custom/Stencil");
		var material = new Material(stencilShader);
		material.SetInteger("_StencilID", stencilID);
		mask.GetComponent<SpriteRenderer>().material = material;

		// Camera settings
		var camObj = new GameObject("Camera");
		camObj.transform.SetParent(transform);
		camObj.transform.localScale = Vector3.one;
		camera = camObj.AddComponent<Camera>();
		UniversalAdditionalCameraData urpCamera = camObj.AddComponent<UniversalAdditionalCameraData>();
		urpCamera.renderType = CameraRenderType.Overlay;
		urpCamera.SetRenderer(stencilID + 1);
		Page.current.urp.cameraStack.Add(camera);
		imitator = camObj.AddComponent<CameraImitator>();
		imitator.target = Page.current.camera;
		imitator.destinationBasis = mask;
		imitator.sourceBasis = storyboard.transform;
		imitator.Jump();
		imitator.positionDamping = Page.current.imitator.positionDamping;
		imitator.rotationDamping = Page.current.imitator.rotationDamping;
	}
}
