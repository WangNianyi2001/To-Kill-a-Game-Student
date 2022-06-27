using System;
using UnityEngine;
using UnityEngine.InputSystem;
using PixelCrushers;

public class Page : MonoBehaviour {
	public new Camera camera;
	PageCamera pc;
	[NonSerialized] public Protagonist protagonist;
	public Storyboard storyboard;
	public GameObject sceneStaticRoot;

	public int stencilResolution = 64;
	[NonSerialized] public Material stencilPassMat;
	public Material comicMat;

	public InputAction interactionInput;

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
		InputDeviceManager.RegisterInputAction("Interact", interactionInput);
		pc = PageCamera.CreateOn(this);
		protagonist = FindObjectOfType<Protagonist>();
		stencilPassMat = new Material(Shader.Find("Custom/StencilPass"));

		foreach(Storyboard storyboard in FindObjectsOfType<Storyboard>()) {
			storyboard.Init(this);
			storyboard.state = Storyboard.State.Disabled;
		}

		foreach(Camera camera in sceneStaticRoot.GetComponentsInChildren<Camera>()) {
			camera.enabled = false;
			camera.clearFlags = CameraClearFlags.Color;
			camera.backgroundColor = Color.black;
		}

		ViewStoryboard(storyboard);
	}
}
