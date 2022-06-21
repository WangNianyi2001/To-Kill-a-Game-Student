using System;
using UnityEngine;

public class CameraController : Controller<Camera> {
	[NonSerialized] public new Camera camera = null;
	[NonSerialized] public TransformController transformCtrl;

	public override Camera Target {
		get => base.Target;
		set {
			base.Target = value;
			transformCtrl.Target = value.transform;
		}
	}

	public static CameraController CreateOn(GameObject gameObject) {
		var ctrl = gameObject.AddComponent<CameraController>();
		ctrl.transformCtrl = gameObject.AddComponent<TransformController>();
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
	}
}