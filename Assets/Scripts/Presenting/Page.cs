using System;
using UnityEngine;

public class Page : MonoBehaviour {
	[NonSerialized] public new Camera camera;
	[NonSerialized] public Protagonist protagonist;
	public Storyboard storyboard;

	public int stencilResolution = 64;
	[NonSerialized] public Material stencilPassMat;
	public Material comicMat;

	public void ViewStoryboard(Storyboard target) {
		if(storyboard != null)
			storyboard.state = Storyboard.State.Visible;
		storyboard = target;
		if(target != null)
			target.state = Storyboard.State.Active;
		var camCtrl = camera.GetComponent<CameraController>();
		camCtrl.enabled = target != null;
		camCtrl.Target = target?.soulCamera;
		camCtrl.transformCtrl.sourceBasis = target?.@base;
		camCtrl.transformCtrl.destinationBasis = target?.transform;
		protagonist.controlBase = target?.viewport?.trigger.transform;
	}

	void Start() {
		camera = GetComponentInChildren<Camera>();
		PageCamera.CreateOn(this);
		CameraController.CreateOn(camera);
		protagonist = FindObjectOfType<Protagonist>();
		stencilPassMat = new Material(Shader.Find("Custom/StencilPass"));
		stencilPassMat.SetInteger("_Resolution", stencilResolution);

		foreach(Storyboard storyboard in FindObjectsOfType<Storyboard>()) {
			storyboard.Init(this);
			storyboard.state = Storyboard.State.Disabled;
		}

		foreach(Camera camera in FindObjectsOfType<Camera>())
			camera.enabled = false;
		camera.enabled = true;

		ViewStoryboard(storyboard);
	}
}
