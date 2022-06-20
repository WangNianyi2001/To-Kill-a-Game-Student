using System;
using UnityEngine;

public class Page : MonoBehaviour {
	public static Page current = null;
	[NonSerialized] public new Camera camera;
	[NonSerialized] public CameraController camCtrl;
	public Protagonist protagonist;

	public Storyboard storyboard;

	public void ViewStoryboard(Storyboard target) {
		if(storyboard != null)
			storyboard.SetState(Storyboard.State.Visible);
			target?.SetState(Storyboard.State.Active);
		camCtrl.enabled = target != null;
		camCtrl.target = target?.soulCamera;
		camCtrl.transformCtrl.sourceBasis = target?.@base;
		camCtrl.transformCtrl.destinationBasis = target?.transform;
		protagonist.controlBase = target?.viewport.trigger?.transform;
	}

	void Start() {
		current = this;
		camera = GetComponentInChildren<Camera>();
		camCtrl = CameraController.CreateOn(camera.gameObject);

		// Disable all camera in scene
		foreach(Camera camera in FindObjectsOfType<Camera>())
			camera.enabled = false;
		camera.enabled = true;

		foreach(Storyboard storyboard in FindObjectsOfType<Storyboard>())
			storyboard.Init(this);

		ViewStoryboard(storyboard);
	}
}
