using UnityEngine;
using UnityEditor;

public static class Auxiliary {
	[MenuItem("Auxiliary/LogAllCameras")]
	public static void LogAllCameras() {
		foreach(Camera cam in Camera.allCameras)
			Debug.Log(cam.name);
	}
}