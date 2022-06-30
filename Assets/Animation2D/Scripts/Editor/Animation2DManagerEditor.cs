using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Animation2DManager))]
public class Animation2DManagerEditor : Editor {
	public override void OnInspectorGUI() {
		Animation2DManager script = (Animation2DManager)target;

		Animation2D[] animations;
		animations = script.gameObject.GetComponents<Animation2D>();

		if(animations.Length > 0) {
			GUILayout.Label("Currently attached animations list", EditorStyles.boldLabel);
			for(int i = 0; i < animations.Length; ++i)
				GUILayout.Label("ID: " + i + " Name: " + animations[i].animationName);
		}
		else
			EditorGUILayout.HelpBox("There are no animations attached to gameobject.", MessageType.Warning);

		for(int i = 0; i < animations.Length; ++i) {
			for(int j = i + 1; j < animations.Length; ++j) {
				if(animations[i].animationName == animations[j].animationName) {
					EditorGUILayout.HelpBox(
						$"Duplication found, both will be played at the same call, animations' name: {animations[i].animationName}",
						MessageType.Warning
					);
					return;
				}
			}
		}
	}
}