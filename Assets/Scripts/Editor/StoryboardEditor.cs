using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Storyboard))]
public class StoryboardEditor : Editor {
	public override void OnInspectorGUI() {
		var so = serializedObject;
		Storyboard target = so.targetObject as Storyboard;
		EditorGUI.BeginChangeCheck();
		target.type = (Storyboard.Type)
			EditorGUILayout.EnumPopup("Type", target.type);
		switch(target.type) {
			case Storyboard.Type.Viewport:
				target.viewport = EditorGUILayout.ObjectField("Viewport", target.viewport, typeof(Viewport)) as Viewport;
				break;
			case Storyboard.Type.Plain:
				target.soulCamera = EditorGUILayout.ObjectField("Soul camera", target.soulCamera, typeof(Camera)) as Camera;
				break;
		}
		if(EditorGUI.EndChangeCheck())
			EditorUtility.SetDirty(target);
	}
}
