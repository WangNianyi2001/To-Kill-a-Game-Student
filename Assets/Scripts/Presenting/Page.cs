using System;
using UnityEngine;
using UnityEngine.InputSystem;
using PixelCrushers;

public class Page : MonoBehaviour {
	public PageCamera pc;
	public AnchorCamera anchor;
	[NonSerialized] public Protagonist protagonist;
	public Storyboard storyboard;
	public GameObject sceneStaticRoot;

	public int stencilResolution = 64;
	[NonSerialized] public Material stencilPassMat;
	public Material comicMat;

	public InputAction interactionInput;

	public void ViewStoryboard(Storyboard target) {
		storyboard?.Blur();
		storyboard = target;
		storyboard?.Focus();
	}

	void Start() {
		anchor.Register(pc.controller);
		InputDeviceManager.RegisterInputAction("Interact", interactionInput);
		protagonist = FindObjectOfType<Protagonist>();
		stencilPassMat = new Material(Shader.Find("Custom/StencilPass"));

		foreach(Storyboard storyboard in FindObjectsOfType<Storyboard>()) {
			storyboard.Init(this);
			storyboard.State = StoryboardState.Disabled;
		}

		foreach(Camera camera in sceneStaticRoot.GetComponentsInChildren<Camera>()) {
			camera.enabled = false;
			camera.clearFlags = CameraClearFlags.Color;
			camera.backgroundColor = Color.black;
		}

		ViewStoryboard(storyboard);
	}

	public void OnEscape(InputValue _) {
		storyboard?.SendMessage("OnEscape");
	}
}
