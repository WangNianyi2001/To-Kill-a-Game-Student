using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class ListDrawer : NestedDrawer {
	protected SerializedProperty property;
	protected PropertyAccessor accessor;
	protected IList source;
	protected ReorderableList list;
	protected GUIContent label;

	protected void Init(SerializedProperty property) {
		if(accessor != null && SerializedProperty.DataEquals(property, this.property))
			return;
		this.property = property;
		accessor = PropertyAccessor.FromProperty(property);
		source = accessor.value as IList;
		if(source == null) {
			list = null;
			return;
		}
		list = new ReorderableList(source, accessor.type) {
			drawHeaderCallback = (Rect rect) => EditorGUI.LabelField(rect, label),
			onAddCallback = (ReorderableList list) => source.Add(null),
			drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
				var child = accessor.GetElement(index);
				var key = child.ToString();
				PropertyDrawer drawer = drawerCache.ContainsKey(key)
					? drawerCache[key]
					: (drawerCache[key] = new NestedDrawer());
				SerializedProperty property = child.MakeProperty();
				if(property != null)
					drawer.OnGUI(rect, property, GUIContent.none);
			},
			elementHeightCallback = (int index) => {
				var child = accessor.GetElement(index);
				var key = child.ToString();
				PropertyDrawer drawer = drawerCache.ContainsKey(key)
					? drawerCache[key]
					: (drawerCache[key] = new NestedDrawer());
				SerializedProperty property = child.MakeProperty();
				if(property != null)
					return drawer.GetPropertyHeight(property, GUIContent.none);
				else
					return 0;
			},
		};
	}

	public override void InstanceGUI(PropertyAccessor accessor, GUIContent label) {
		this.label = label;
		float height = list.GetHeight();
		Rect area = EditorGUI.IndentedRect(MakeArea(height));
		if(draw)
			list.DoList(area);
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		draw = false;
		OnGUI(position, property, label);
		draw = true;
		return position.height;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		InitPos(position);
		Init(property);
		if(accessor.value == null)
			NullGUI(accessor, label);
		else
			InstanceGUI(accessor, label);
	}
}
