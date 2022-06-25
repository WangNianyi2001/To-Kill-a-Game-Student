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
	public float cameraDistance = 5;
	[NonSerialized] public Camera soulCamera;
	[NonSerialized] public Transform @base;

	public void Start() {
		var collider = gameObject.AddComponent<BoxCollider2D>();
		collider.size = (transform as RectTransform).sizeDelta;
	}

	public void Init(Page page) {
		this.page = page;
		switch(type) {
			case Type.Plain:
				var soulCamObj = new GameObject("Camera");
				var t = soulCamObj.transform;
				t.parent = transform;
				t.localPosition = new Vector3(0, 0, -cameraDistance);
				t.localRotation = Quaternion.identity;
				soulCamera = soulCamObj.AddComponent<Camera>();
				soulCamera.enabled = false;
				@base = transform;
				break;
			case Type.Viewport:
				viewport.Init(this);
				soulCamera = viewport.camera;
				@base = viewport.transform;
				var texture = GetComponent<RawImage>().texture;
				var material = new Material(Shader.Find("Custom/StencilWrite"));
				material.SetTexture("_MainTex", texture);
				material.SetInteger("_Resolution", page.stencilResolution);
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
	private State _state;
	public State state {
		get => _state;
		set {
			_state = value;
			gameObject.SetActive(value != State.Disabled);
			switch(type) {
				case Type.Plain:
					break;
				case Type.Viewport:
					var viewportCtrl = viewport.camCtrl;
					viewportCtrl.enabled = true;
					if(value == State.Active) {
						viewportCtrl.transformCtrl.sourceBasis = viewport.transform;
						viewportCtrl.Target = viewport.soulCamera;
					}
					else {
						viewportCtrl.Target = page.camera;
						viewportCtrl.transformCtrl.sourceBasis = transform;
					}
					break;
			}
		}
	}
}
