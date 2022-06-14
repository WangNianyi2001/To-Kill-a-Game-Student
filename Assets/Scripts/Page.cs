using System;
using UnityEngine;

public class Page : MonoBehaviour {
	public static Page current = null;
	[NonSerialized] public new Camera camera;
	[NonSerialized] public PageCamera camCtrl;
	public Protagonist protagonist;

	[NonSerialized] public bool init = true;

	public Storyboard storyboard;

	public void ViewStoryboard(Storyboard target, bool jump = false) {
		if(storyboard != null)
			storyboard.SetState(Storyboard.State.Visible);
			target?.SetState(Storyboard.State.Active);
		camCtrl.enabled = target != null;
		camCtrl.target = target?.soulCamera;
		camCtrl.sourceBasis = target?.@base;
		camCtrl.destinationBasis = target?.transform;
		protagonist.controlBase = target?.viewport.trigger?.transform;
		if(jump) {
			foreach(var viewport in FindObjectsOfType<Viewport>())
				viewport.camCtrl.Jump();
			camCtrl.Jump();
		}
	}

	void Start() {
		current = this;
		camera = GetComponentInChildren<Camera>();
		camCtrl = camera.gameObject.AddComponent<PageCamera>();

		// Disable all camera in scene
		foreach(Camera camera in FindObjectsOfType<Camera>())
			camera.enabled = false;
		camera.enabled = true;

		foreach(Storyboard storyboard in FindObjectsOfType<Storyboard>())
			storyboard.Init(this);

		ViewStoryboard(storyboard);
	}
}
