using System;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using Object = UnityEngine.Object;

public abstract class SerializedWrapper {
	public abstract SerializedWrapper GetField(string name);
	public abstract SerializedWrapper GetElement(int index);

	public class Root : SerializedWrapper {
		public readonly SerializedObject so;
		public Root(SerializedObject so) => this.so = so;
		public override SerializedWrapper GetField(string name) => so.FindProperty(name);
		public override SerializedWrapper GetElement(int index) => throw new NotImplementedException();
	}

	public class Field : SerializedWrapper {
		public readonly SerializedProperty sp;
		public Field(SerializedProperty sp) => this.sp = sp.Copy();
		public override SerializedWrapper GetField(string name) {
			try {
				return sp.FindPropertyRelative(name);
			}
			catch(Exception) { return null; }
		}
		public override SerializedWrapper GetElement(int index) {
			try {
				if(index >= sp.arraySize)
					return null;
				return sp.GetArrayElementAtIndex(index);
			}
			catch(Exception) { return null; }
		}
	}

	public static implicit operator SerializedWrapper(SerializedObject so) => new Root(so);
	public static implicit operator SerializedWrapper(SerializedProperty sp) => new Field(sp);
}

public abstract class PropertyAccessor {
	public abstract Type type { get; }
	public bool isArray => typeof(IList).IsAssignableFrom(type);
	public abstract object value { get; set; }
	public abstract PropertyAccessor root { get; }
	public abstract string path { get; }

	public override abstract string ToString();
	public abstract SerializedWrapper Serialize();

	public class Root : PropertyAccessor {
		public readonly object obj;
		public override Type type => obj.GetType();
		public Root(object obj) => this.obj = obj;
		public override object value {
			get => obj;
			set => throw new NotImplementedException();
		}
		public override PropertyAccessor root => this;
		public override string path => "";
		public override string ToString() => obj.GetType().Name;
		public override SerializedWrapper Serialize() =>
			new SerializedWrapper.Root(new SerializedObject(obj as Object));
	}

	public abstract class Sub : PropertyAccessor {
		public readonly PropertyAccessor parent;
		public override PropertyAccessor root => parent.root;
		public override string path => parent.path + ToString();
		public Sub(PropertyAccessor parent) => this.parent = parent;
		public override abstract string ToString();
	}

	public class Field : Sub {
		const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
		public readonly FieldInfo fi;
		public string name => fi.Name;
		public override Type type => fi.FieldType;
		public Field(PropertyAccessor parent, string name) : base(parent) {
			fi = parent.type.GetField(name, bindingFlags);
			if(fi == null)
				throw new FieldAccessException();
		}
		public override object value {
			get => fi.GetValue(parent.value);
			set => fi.SetValue(parent.value, value);
		}
		public override string ToString() => $".{name}";
		public override SerializedWrapper Serialize() => parent.Serialize()?.GetField(name);
	}

	public class Element : Sub {
		public readonly int index;
		public Element(PropertyAccessor parent, int index) : base(parent) =>
			this.index = index;
		public override Type type => parent.type.IsGenericType ? parent.type.GenericTypeArguments[0] : typeof(object);
		public override object value {
			get => (parent.value as IList)[index];
			set => (parent.value as IList)[index] = value;
		}
		public override string ToString() => $"[{index}]";
		public override SerializedWrapper Serialize() {
			try { return parent.Serialize()?.GetElement(index); }
			catch(Exception) { return null; }
		}
	}

	public static PropertyAccessor FromProperty(SerializedProperty property) {
		try {
			PropertyAccessor accessor = new Root(property.serializedObject.targetObject);
			string path = '.' + property.propertyPath.Replace(".Array.data[", "[");
			while(path.Length != 0) {
				if(path[0] == '.') {
					Match match = new Regex(@"^\.([_a-zA-Z][_a-zA-Z\d]*)").Match(path);
					if(!match.Success)
						throw new FormatException();
					string name = match.Groups[1].Value;
					accessor = new Field(accessor, name);
					path = path.Substring(match.Groups[0].Value.Length);
					continue;
				}
				if(path[0] == '[') {
					Match match = new Regex(@"^\[([^]]+)\]").Match(path);
					if(!match.Success)
						throw new FormatException();
					int index = Convert.ToInt32(match.Groups[1].Value);
					accessor = new Element(accessor, index);
					path = path.Substring(match.Groups[0].Value.Length);
					continue;
				}
				return null;
			}
			return accessor;
		}
		catch(Exception) {
			return null;
		}
	}

	public SerializedProperty MakeProperty() => (Serialize() as SerializedWrapper.Field)?.sp;

	public PropertyAccessor GetField(string name) => new Field(this, name);
	public PropertyAccessor GetElement(int index) => new Element(this, index);
}
