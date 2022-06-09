using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Page : MonoBehaviour {
	public static Page current = null;
	[NonSerialized] public new Camera camera;
	[NonSerialized] public UniversalAdditionalCameraData urp;
	[NonSerialized] public CameraImitator imitator;
	public Protagonist protagonist;

	[NonSerialized] public bool init = true;

	public Storyboard storyboard;

	public float blendTime = 1.0f;

	public void ViewBoard(Storyboard target, bool jump = false) {
		if(storyboard != null)
			storyboard.SetState(Storyboard.State.Visible);
		if((storyboard = target) != null) {
			storyboard.SetState(Storyboard.State.Active);
			imitator.enabled = true;
			imitator.target = target.viewport.camera;
			imitator.sourceBasis = target.viewport.transform;
			imitator.destinationBasis = target.transform;
		}
		else {
			imitator.enabled = false;
			imitator.target = null;
			imitator.sourceBasis = null;
			imitator.destinationBasis = transform;
		}
		if(target.viewport.trigger != null)
			protagonist.controlBase = target.viewport.trigger.transform;
		if(jump) {
			foreach(var viewport in FindObjectsOfType<Viewport>())
				viewport.imitator.Jump();
			imitator.Jump();
		}
	}

	void Start() {
		current = this;
		camera = GetComponentInChildren<Camera>();
		urp = camera.GetComponent<UniversalAdditionalCameraData>();
		imitator = camera.GetComponent<CameraImitator>();

		// Disable all camera in scene
		foreach(Camera cam in FindObjectsOfType<Camera>())
			cam.enabled = false;
		camera.enabled = true;
	}
}
