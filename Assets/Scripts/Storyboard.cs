using System;
using UnityEngine;

public class Storyboard : MonoBehaviour {
	public enum Type {
		Plain, Viewport
	}
	public Type type;

	public Viewport viewport;
	public Camera soulCamera;
	[NonSerialized] public Transform @base;

	void Start() {
		switch(type) {
			case Type.Plain:
				@base = transform;
				break;
			case Type.Viewport:
				viewport.storyboard = this;
				soulCamera = viewport.camera;
				@base = viewport.transform;
				break;
		}
	}

	public enum State {
		Disabled,
		Active,
		Visible,
	}
	public void SetState(State state) {
		viewport.Visible = state != State.Disabled;
		var vpIm = viewport.imitator;
		vpIm.enabled = true;
		if(state == State.Active) {
			vpIm.sourceBasis = viewport.mask;
			vpIm.target = viewport.soulCamera;
		}
		else {
			vpIm.target = Page.current.camera;
			vpIm.sourceBasis = transform;
		}
	}
}
