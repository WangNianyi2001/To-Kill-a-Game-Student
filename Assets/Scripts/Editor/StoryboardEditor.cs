using UnityEditor;

[CustomEditor(typeof(Storyboard))]
public class StoryboardEditor : Editor {
	public override void OnInspectorGUI() {
		var so = serializedObject;
		Storyboard target = so.targetObject as Storyboard;
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(so.FindProperty("type"));
		switch(target.type) {
			case Storyboard.Type.Plain:
				EditorGUILayout.Slider(so.FindProperty("cameraDistance"), 1, 20);
				break;
			case Storyboard.Type.Viewport:
				EditorGUILayout.ObjectField(so.FindProperty("viewport"), typeof(Viewport));
				break;
		}
		EditorGUILayout.PropertyField(so.FindProperty("onEscape"));
		if(EditorGUI.EndChangeCheck()) {
			so.ApplyModifiedProperties();
			EditorUtility.SetDirty(target);
		}
	}
}
