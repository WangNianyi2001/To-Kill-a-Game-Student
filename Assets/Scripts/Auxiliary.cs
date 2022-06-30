using UnityEngine;
using UnityEngine.SceneManagement;

public static class Auxiliary {
	public static void SwitchScene(string name) {
		SceneManager.LoadScene(name);
	}

	public static void Quit() {
		Application.Quit();
	}
}