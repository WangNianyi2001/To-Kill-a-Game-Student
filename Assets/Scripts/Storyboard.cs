using UnityEngine;

public class Storyboard : MonoBehaviour {
	public enum Type {
		Plain, Viewport
	}
	public Type type;

	public Viewport viewport;
	public Camera soulCamera;

	void Start() {
		if(type == Type.Viewport) {
			viewport.storyboard = this;
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
