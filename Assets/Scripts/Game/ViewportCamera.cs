using UnityEngine;

public class ViewportCamera : PostProcess {
	public static ViewportCamera CreateOn(GameObject gameObject) {
		var vpCam = gameObject.AddComponent<ViewportCamera>();
		vpCam.camera = gameObject.GetComponent<Camera>();
		if(vpCam.camera == null)
			vpCam.camera = gameObject.AddComponent<Camera>();
		return vpCam;
	}
}
