using System;
using UnityEngine;

public class Storyboard : MonoBehaviour {
	[NonSerialized] public Viewport viewport;

	void Start() {
		state = State.Visible;
	}

	public enum State {
		Disabled,
		Active,
		Visible,
	}
	State _state;
	public State state {
		get => _state;
		set {
			_state = value;
			viewport.Visible = _state != State.Disabled;
			var vpIm = viewport.imitator;
			vpIm.enabled = true;
			if(_state == State.Active) {
				vpIm.sourceBasis = viewport.mask;
				vpIm.target = viewport.soul;
			}
			else {
				vpIm.target = Page.current.camera;
				vpIm.sourceBasis = transform;
			}
		}
	}
}
