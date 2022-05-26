using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Viewport : MonoBehaviour {
	static byte nextStencilID = 1;
	byte stencilID;
	public byte StencilID => stencilID;

	[NonSerialized] public new Camera camera;
	[NonSerialized] public Transform mask;

	public bool Visible {
		get => camera.enabled;
		set => camera.enabled = value;
	}

	public void UpdateCamera(Transform root, Transform target) {
		camera.transform.position = mask.localToWorldMatrix.MultiplyPoint
			(root.worldToLocalMatrix.MultiplyPoint(target.position));
		camera.transform.rotation = mask.rotation * Quaternion.Inverse(root.rotation) * target.rotation;
	}

	void Start() {
		stencilID = nextStencilID++;

		// Camera settings
		var camObj = new GameObject("Camera");
		camObj.transform.SetParent(transform);
		camObj.transform.position = Vector3.zero;
		camObj.transform.rotation = Quaternion.identity;
		camObj.transform.localScale = Vector3.one;
		camera = camObj.AddComponent<Camera>();
		UniversalAdditionalCameraData urpCamera = camObj.AddComponent<UniversalAdditionalCameraData>();
		urpCamera.renderType = CameraRenderType.Overlay;
		urpCamera.SetRenderer(stencilID + 1);

		// Create Stencil mask
		var maskObj = new GameObject("Stencil Mask");
		mask = maskObj.transform;
		maskObj.layer = LayerMask.NameToLayer("Viewport");
		mask.SetParent(transform);
		mask.localPosition = Vector3.zero;
		mask.localRotation = Quaternion.identity;
		var maskRenderer = maskObj.AddComponent<SpriteRenderer>();
		maskRenderer.sprite = Resources.Load<Sprite>("Sprite/White");
	}
}
