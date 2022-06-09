using UnityEditor;
using UnityEngine;

public class DrawerBase : PropertyDrawer {
	protected Rect position;
	protected bool draw = true;

	public Rect TempArea(float height = 0) =>
		new Rect(position.xMin, position.yMax, position.width, height);

	public Rect Indented(Rect area) => EditorGUI.IndentedRect(area);

	public Rect MakeArea(float height) {
		Rect rect = TempArea(height);
		position.yMax = rect.yMax;
		return rect;
	}

	public Rect MakeArea() => MakeArea(EditorGUIUtility.singleLineHeight);

	public void Property(SerializedProperty property, GUIContent label) {
		var area = MakeArea(EditorGUI.GetPropertyHeight(property, label));
		if(draw)
			EditorGUI.PropertyField(area, property, label, true);
	}

	public void Label(GUIContent label) {
		var area = MakeArea();
		if(draw && label != GUIContent.none)
			EditorGUI.LabelField(area, label);
	}

	public bool Button(GUIContent label) {
		var area = Indented(MakeArea());
		return draw && GUI.Button(area, label);
	}
}