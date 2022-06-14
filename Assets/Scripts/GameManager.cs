using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;

public static class GameManager {
	public static T GetMember<T>(object target, string name) {
		return (T)target.GetType()
			.GetField(name, BindingFlags.Instance
				| BindingFlags.Public | BindingFlags.NonPublic
			).GetValue(target);
	}

	public static void SetMember<T>(object target, string name, T value) {
		target.GetType()
			.GetField(name, BindingFlags.Instance
				| BindingFlags.Public | BindingFlags.NonPublic
			).SetValue(target, value);
	}

	public static T DeepClone<T>(this T obj) {
		using(var ms = new MemoryStream()) {
			var formatter = new BinaryFormatter();
			formatter.Serialize(ms, obj);
			ms.Position = 0;

			return (T)formatter.Deserialize(ms);
		}
	}
}