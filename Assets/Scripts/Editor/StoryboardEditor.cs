using UnityEditor;

[CustomEditor(typeof(Storyboard))]
public class StoryboardEditor : Editor {
	public override void OnInspectorGUI() {
		var so = serializedObject;
		Storyboard target = so.targetObject as Storyboard;
		EditorGUI.BeginChangeCheck();
		target.type = (Storyboard.Type)EditorGUILayout.EnumPopup("Type", target.type);
		switch(target.type) {
			case Storyboard.Type.Plain:
				target.cameraDistance = EditorGUILayout.Slider("Camera distance", target.cameraDistance, 1, 20);
				break;
			case Storyboard.Type.Viewport:
				target.viewport = EditorGUILayout.ObjectField("Viewport", target.viewport, typeof(Viewport), true) as Viewport;
				break;
		}
		if(EditorGUI.EndChangeCheck())
			EditorUtility.SetDirty(target);
	}
}
