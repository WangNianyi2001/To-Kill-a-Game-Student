using UnityEngine;
using Cinemachine;

public class Storyboard : MonoBehaviour {
	CinemachineVirtualCamera vCam;

	public Viewport viewport;

	void Start() {
		// Set viewport Stencil mask
		var sprite = GetComponent<SpriteRenderer>().sprite;
		Vector3 scale = transform.localScale / sprite.pixelsPerUnit;
		viewport.mask.localScale = scale;
		Shader stencilShader = Shader.Find("Custom/Stencil");
		var material = new Material(stencilShader);
		material.SetInteger("_StencilID", viewport.StencilID);
		viewport.mask.GetComponent<SpriteRenderer>().material = material;

		// Self initializations
		vCam = transform.parent.GetComponentInChildren<CinemachineVirtualCamera>();
		state = State.Disabled;
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
			vCam.enabled = _state == State.Active;
		}
	}
}
