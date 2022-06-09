using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

public static class TypeFinder {
	static Assembly editorAssembly = Assembly.GetAssembly(typeof(Editor));
	static object scriptAttributeUtility = editorAssembly.CreateInstance("UnityEditor.ScriptAttributeUtility");
	static MethodInfo getDrawerTypeForType = scriptAttributeUtility.GetType().GetMethod("GetDrawerTypeForType",
			BindingFlags.Instance | BindingFlags.Static |
			BindingFlags.Public | BindingFlags.NonPublic);

	public static Type DirectDrawerType(Type type) =>
		(Type)getDrawerTypeForType?.Invoke(scriptAttributeUtility, new object[] { type });

	public static Type ClosestDrawerType(Type type) {
		for(; type != null; type = type.BaseType) {
			Type result = DirectDrawerType(type);
			if(result != null)
				return result;
		}
		return null;
	}

	public static IEnumerable<Type> SolidSubTypes(Type type) =>
		from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
		from assemblyType in domainAssembly.GetExportedTypes()
		where
			type.IsAssignableFrom(assemblyType) &&
			!assemblyType.IsAbstract &&
			!assemblyType.IsGenericType
		select assemblyType;
}
