using System;
using UnityEngine;

public class TransformController : Controller<Transform> {
	public Transform sourceBasis = null;
	public Transform destinationBasis = null;

	[Range(0, 100)]
	public float positionDamping = 0;
	[Range(0, 100)]
	public float rotationDamping = 0;

	public void Jump() {
		if(target == null)
			return;
		transform.position = target.position;
		transform.rotation = target.rotation;
	}

	public override void Step() {
		if(target == null)
			return;
		var localPosition = sourceBasis == null ? target.position : sourceBasis.worldToLocalMatrix.MultiplyPoint(target.position);
		var position = destinationBasis == null ? localPosition : destinationBasis.localToWorldMatrix.MultiplyPoint(localPosition);
		var localRotation = sourceBasis == null ? target.rotation : Quaternion.Inverse(sourceBasis.rotation) * target.rotation;
		var rotation = destinationBasis == null ? localRotation : destinationBasis.rotation * localRotation;
		//
		transform.position = Vector3.Lerp(transform.position, position, 1 / (1 + positionDamping));
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1 / (1 + rotationDamping));
	}
}
