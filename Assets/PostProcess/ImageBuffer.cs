using System;
using UnityEngine;

public class ImageBuffer : IDisposable {
	public RenderTexture rt;

	public bool Update(Texture reference = null) {
		var refSize = reference == null
			? new Vector2(Screen.width, Screen.height)
			: new Vector2(reference.width, reference.height);
		if(!(rt == null || new Vector2(rt.width, rt.height) != refSize))
			return false;
		Dispose();
		rt = RenderTexture.GetTemporary((int)refSize.x, (int)refSize.y, 24);
		return true;
	}

	public void Dispose() {
		if(rt != null)
			RenderTexture.ReleaseTemporary(rt);
	}
}
