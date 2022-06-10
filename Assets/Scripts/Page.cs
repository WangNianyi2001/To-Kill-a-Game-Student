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

	public void ViewStoryboard(Storyboard target, bool jump = false) {
		if(storyboard != null)
			storyboard.SetState(Storyboard.State.Visible);
			target?.SetState(Storyboard.State.Active);
		imitator.enabled = target != null;
		imitator.target = target.soulCamera;
		imitator.sourceBasis = target.@base;
		imitator.destinationBasis = target?.transform;
		if(target.viewport.trigger != null)
			protagonist.controlBase = target.viewport.trigger.transform;
		if(jump) {
			foreach(var viewport in FindObjectsOfType<Viewport>())
				viewport.imitator.Jump();
			imitator.Jump();
		}
	}

	void Awake() {
		current = this;
		camera = GetComponentInChildren<Camera>();
		urp = camera.GetComponent<UniversalAdditionalCameraData>();
		imitator = camera.GetComponent<CameraImitator>();
	}

	void Start() {
		// Disable all camera in scene
		foreach(Camera camera in FindObjectsOfType<Camera>())
			camera.enabled = false;
		camera.enabled = true;

		ViewStoryboard(storyboard);
	}
}
