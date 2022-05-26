using System;
using UnityEngine;

public class CameraImitator : Controller {
	[NonSerialized] public new Camera camera = null;
	public Camera target = null;
	public Transform sourceBasis = null;
	public Transform destinationBasis = null;

	void Start() {
		camera = GetComponent<Camera>();
	}

	protected override void FixedUpdate() {
		if(camera == null || target == null)
			return;
		if(sourceBasis == null)
			sourceBasis = target.transform.parent;
		if(destinationBasis == null)
			destinationBasis = transform.parent;
		var sourceScale = sourceBasis.localScale;
		sourceBasis.localScale = Vector3.one;
		var localPosition = sourceBasis.worldToLocalMatrix.MultiplyPoint(target.transform.position);
		sourceBasis.localScale = sourceScale;
		var destinationScale = destinationBasis.localScale;
		destinationBasis.localScale = Vector3.one;
		position = destinationBasis.localToWorldMatrix.MultiplyPoint(localPosition);
		destinationBasis.localScale = destinationScale;
		rotation = destinationBasis.rotation * Quaternion.Inverse(sourceBasis.rotation) * target.transform.rotation;
		base.FixedUpdate();
	}
}