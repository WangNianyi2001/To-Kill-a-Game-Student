using System;
using UnityEngine;

public class CameraController : Controller<Camera> {
	[NonSerialized] public new Camera camera = null;
	[NonSerialized] public TransformController transformCtrl;

	protected void Start() {
		if(camera == null)
			camera = GetComponent<Camera>();
		transformCtrl = GetComponent<TransformController>() ?? gameObject.AddComponent<TransformController>();
		transformCtrl.target = target.transform;
	}

	public override void Step() {
		if(camera == null || target == null)
			return;
	}
}