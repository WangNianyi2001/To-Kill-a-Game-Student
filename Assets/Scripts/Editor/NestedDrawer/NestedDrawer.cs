using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class NestedDrawer : DrawerBase {
	public delegate bool PropertyFilter(PropertyAccessor accessor);

	protected PropertyFilter propertyFilter = declaredProperties;

	const BindingFlags instanceBindingFlags = BindingFlags.Instance |
		BindingFlags.Public | BindingFlags.NonPublic;
	const BindingFlags declaredBindingFlags = instanceBindingFlags | BindingFlags.DeclaredOnly;

	public static PropertyFilter declaredProperties = (PropertyAccessor accessor) => {
		if(accessor == null)
			return false;
		string name = (accessor as PropertyAccessor.Field).name;
		return accessor.type.GetField(name, declaredBindingFlags) != null;
	};

	public static PropertyFilter isPropertyOf(Type type) => (PropertyAccessor accessor) => {
		if(accessor == null)
			return false;
		string name = (accessor as PropertyAccessor.Field).name;
		FieldInfo fi = accessor.root.type.GetField(name, instanceBindingFlags);
		return type.IsAssignableFrom(fi?.DeclaringType);
	};

	protected Dictionary<string, PropertyDrawer> drawerCache = new Dictionary<string, PropertyDrawer>();

	protected PropertyDrawer GetDrawer(string key, Type type) {
		if(drawerCache.ContainsKey(key))
			return drawerCache[key];
		PropertyDrawer drawer = Activator.CreateInstance(type) as PropertyDrawer;
		drawerCache[key] = drawer;
		return drawer;
	}

	public void DrawProperty(PropertyAccessor accessor, GUIContent label) {
		SerializedProperty property = accessor?.MakeProperty();
		if(property == null)
			return;
		var key = accessor.ToString();
		Type drawerType = TypeFinder.ClosestDrawerType(accessor.type);
		EditorGUI.BeginChangeCheck();
		if(GetType().Equals(drawerType)) {
			if(accessor.value == null)
				NullGUI(accessor, label);
			else
				InstanceGUI(accessor, label);
		}
		else if(drawerType != null || accessor.isArray) {
			PropertyDrawer drawer = GetDrawer(key, accessor.isArray ? typeof(ListDrawer) : drawerType);
			if(draw)
				drawer.OnGUI(TempArea(), property, label);
			position.height += drawer.GetPropertyHeight(property, label);
		}
		else
			Property(property, label);
		MakeArea(EditorGUIUtility.standardVerticalSpacing);
		if(EditorGUI.EndChangeCheck())
			property.serializedObject.ApplyModifiedProperties();
	}

	public void InitPos(Rect position) {
		this.position = position;
		this.position.height = 0;
	}

	public virtual void InstanceGUI(PropertyAccessor accessor, GUIContent label) {
		if(label != null && !label.Equals(GUIContent.none))
			Label(label);
		var target = accessor.value as Object;
		if(target == null)
			return;
		++EditorGUI.indentLevel;
		var child = new SerializedObject(target).GetIterator();
		for(bool end = child.Next(true); end; end = child.NextVisible(false)) {
			PropertyAccessor childAccessor = PropertyAccessor.FromProperty(child);
			if(childAccessor == null)
				continue;
			if(!propertyFilter(childAccessor))
				continue;
			var childLabel = new GUIContent(child.displayName);
			DrawProperty(childAccessor, childLabel);
		}
		--EditorGUI.indentLevel;
	}

	public virtual void NullGUI(PropertyAccessor accessor, GUIContent label) {
		Label(label);
		Label(new GUIContent("Object is null"));
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		draw = false;
		OnGUI(position, property, label);
		draw = true;
		return position.height;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		InitPos(position);
		DrawProperty(PropertyAccessor.FromProperty(property), label);
	}
}