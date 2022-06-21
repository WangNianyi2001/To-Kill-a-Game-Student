using System;
using UnityEngine;

public class ImageBuffer : IDisposable {
	public Texture reference = null;
	public RenderTexture rt;

	public ImageBuffer() { }
	public ImageBuffer(Texture reference) => this.reference = reference;

	public Vector2 RefSize => reference == null
		? new Vector2(Screen.width, Screen.height)
		: new Vector2(reference.width, reference.height);

	public bool NeedUpdate() => rt == null || new Vector2(rt.width, rt.height) != RefSize;

	public void New() {
		Dispose();
		var size = RefSize;
		rt = RenderTexture.GetTemporary((int)size.x, (int)size.y, 24);
	}

	public void Update() {
		if(!NeedUpdate())
			return;
		New();
	}

	public void Update(Texture reference) {
		this.reference = reference;
		Update();
	}

	public void Dispose() {
		rt?.Release();
	}
}
