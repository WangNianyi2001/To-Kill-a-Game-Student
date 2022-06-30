using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(TransformController))]
public class CameraController : Controller<Camera> {
	[NonSerialized] public new Camera camera = null;
	[NonSerialized] public TransformController transformCtrl;

	public override Camera Target {
		get => base.Target;
		set {
			base.Target = value;
			if(transformCtrl != null)
				transformCtrl.Target = value?.transform;
		}
	}

	protected void Start() {
		camera = GetComponent<Camera>();
		transformCtrl = GetComponent<TransformController>();
	}

	void FixedUpdate() {
		if(camera == null || Target == null)
			return;
		camera.fieldOfView = Target.fieldOfView;
	}
}