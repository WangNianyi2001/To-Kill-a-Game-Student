using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum StoryboardState {
	Disabled,
	Active,
	Visible,
}

public class Storyboard : MonoBehaviour {
	[NonSerialized] public Page page;
	[NonSerialized] public Camera soulCamera;
	[NonSerialized] public Transform @base;

	public enum Type {
		None, Plain, Viewport
	}
	public Type type;

	public float cameraDistance = 5;

	public Viewport viewport;
	public bool grantControl = true;

	public void Start() {
		var collider = gameObject.AddComponent<BoxCollider2D>();
		collider.size = (transform as RectTransform).sizeDelta;
	}

	public void Init(Page page) {
		this.page = page;
		transform.localPosition += Vector3.forward * -.01f;
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

	private StoryboardState state;
	public StoryboardState State {
		get => state;
		set {
			state = value;
			gameObject.SetActive(value != StoryboardState.Disabled);
			switch(type) {
				case Type.Plain:
					break;
				case Type.Viewport:
					var viewportCtrl = viewport.camCtrl;
					viewportCtrl.enabled = true;
					if(value == StoryboardState.Active) {
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
	public bool visibility {
		set => State = value ? StoryboardState.Visible : StoryboardState.Disabled;
	}

	public void Blur() {
		if(State == StoryboardState.Active)
			State = StoryboardState.Visible;
		var camCtrl = page.camera.GetComponent<CameraController>();
		camCtrl.enabled = false;
		camCtrl.Target = null;
		camCtrl.transformCtrl.sourceBasis = null;
		camCtrl.transformCtrl.destinationBasis = null;
	}

	public void Focus() {
		State = StoryboardState.Active;
		var camCtrl = page.camera.GetComponent<CameraController>();
		camCtrl.enabled = true;
		camCtrl.Target = soulCamera;
		camCtrl.transformCtrl.sourceBasis = @base;
		camCtrl.transformCtrl.destinationBasis = transform;
		page.protagonist.controlBase = viewport?.trigger.transform;
	}

	[SerializeField] public UnityEvent onEscape;

	public void OnEscape() {
		onEscape?.Invoke();
	}
}
