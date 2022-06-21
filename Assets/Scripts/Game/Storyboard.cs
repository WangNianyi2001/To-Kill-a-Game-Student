using System;
using UnityEngine;
using UnityEngine.UI;

public class Storyboard : MonoBehaviour {
	[NonSerialized] public Page page;

	public enum Type {
		Plain, Viewport
	}
	public Type type;

	public Viewport viewport;
	public Camera soulCamera;
	[NonSerialized] public Transform @base;

	public void Init(Page page) {
		this.page = page;
		switch(type) {
			case Type.Plain:
				@base = transform;
				break;
			case Type.Viewport:
				viewport.Init(this);
				soulCamera = viewport.camera;
				@base = viewport.transform;
				var material = new Material(Shader.Find("StencilWrite"));
				material.SetInteger("_StencilID", viewport.stencilID);
				GetComponent<RawImage>().material = material;
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
		var viewportCtrl = viewport.camCtrl;
		viewportCtrl.enabled = true;
		if(state == State.Active) {
			viewportCtrl.transformCtrl.sourceBasis = viewport.mask;
			viewportCtrl.target = viewport.soulCamera;
		}
		else {
			viewportCtrl.target = page.camera;
			viewportCtrl.transformCtrl.sourceBasis = transform;
		}
	}
}
