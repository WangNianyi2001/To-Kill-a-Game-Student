using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Page : MonoBehaviour {
	public static Page current = null;
	[NonSerialized] public new Camera camera;
	[NonSerialized] public UniversalAdditionalCameraData urp;
	[NonSerialized] CameraImitator imitator;

	public Storyboard currentStoryboard;

	public float blendTime = 1.0f;

	public void ViewBoard(Storyboard board) {
		if(currentStoryboard != null)
			currentStoryboard.state = Storyboard.State.Visible;
		if((currentStoryboard = board) != null) {
			currentStoryboard.state = Storyboard.State.Active;
			imitator.enabled = true;
			imitator.target = board.viewport.camera;
			imitator.destinationBasis = board.transform;
		}
		else {
			imitator.enabled = false;
			imitator.target = null;
			imitator.destinationBasis = transform;
		}
	}

	void Start() {
		current = this;
		camera = GetComponentInChildren<Camera>();
		urp = camera.GetComponent<UniversalAdditionalCameraData>();
		imitator = camera.GetComponent<CameraImitator>();
	}
}
