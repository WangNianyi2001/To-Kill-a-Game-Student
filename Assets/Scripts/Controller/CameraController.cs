using System;
using UnityEngine;

[RequireComponent(typeof(TransformController))]
public class CameraController : Controller<Camera> {
	[NonSerialized] public new Camera camera = null;
	[NonSerialized] public TransformController transformCtrl;

	public override Camera Target {
		get => base.Target;
		set {
			base.Target = value;
			transformCtrl.Target = value?.transform;
		}
	}

	public static CameraController CreateOn(Camera camera) {
		var ctrl = camera.gameObject.AddComponent<CameraController>();
		ctrl.target = camera;
		ctrl.transformCtrl = camera.gameObject.GetComponent<TransformController>();
		ctrl.transformCtrl.target = camera.transform;
		return ctrl;
	}

	protected void Start() {
		if(camera == null)
			camera = GetComponent<Camera>();
		transformCtrl = GetComponent<TransformController>() ?? gameObject.AddComponent<TransformController>();
		transformCtrl.Target = target.transform;
	}

	public override void Step() {
		if(camera == null || target == null)
			return;
		camera.fieldOfView = target.fieldOfView;
		camera.clearFlags = target.clearFlags;
		camera.backgroundColor = target.backgroundColor;
	}
}