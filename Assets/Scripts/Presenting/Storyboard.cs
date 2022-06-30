using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem.Wrappers;

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

	public bool clickNext = false;
	public Storyboard next;

	public void Start() {
		var collider = gameObject.AddComponent<BoxCollider2D>();
		collider.size = (transform as RectTransform).sizeDelta;
		if(clickNext) {
			var usable = gameObject.AddComponent<Usable>();
			usable.overrideName = "ä¯ÀÀ";
			usable.maxUseDistance = 1000;
			usable.events = new Usable.UsableEvents();
			usable.events.onUse.AddListener(() => page.ViewStoryboard(next));
		}
	}

	public void Init(Page page) {
		this.page = page;
		transform.localPosition += Vector3.back * .01f;
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
				page.anchor.Register(viewport.camCtrl);
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
						viewportCtrl.Target = page.pc.camera;
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
		page.protagonist.grantControl = false;
	}

	public void Focus() {
		State = StoryboardState.Active;
		var pcct = page.anchor.controller.transformCtrl;
		switch(type) {
			case Type.Plain:
				page.anchor.SetMaster(soulCamera);
				pcct.sourceBasis = null;
				pcct.destinationBasis = null;
				break;
			case Type.Viewport:
				page.anchor.SetMaster(viewport.camera);
				viewport.camCtrl.Target = viewport.soulCamera;
				pcct.sourceBasis = viewport.transform;
				pcct.destinationBasis = transform;
				if(viewport.trigger != null)
					page.protagonist.controlBase = viewport.trigger.transform;
				page.protagonist.grantControl = true;
				break;
		}
	}

	[SerializeField] public UnityEvent onEscape;

	public void OnEscape() {
		onEscape?.Invoke();
	}
}
