using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManipulator : MonoBehaviour {
	public enum Type {
		RawImage, Image, SpriteRenderer
	}
	static Dictionary<Type, System.Type> typeDict = new Dictionary<Type, System.Type> {
		{ Type.RawImage, typeof(RawImage) },
		{ Type.Image, typeof(Image) },
		{ Type.SpriteRenderer, typeof(SpriteRenderer) }
	};

	public Type type;
	public Component target;

	static Texture GetTexture(Object image) {
		if(image is Texture)
			return image as Texture;
		if(image is Sprite)
			return (image as Sprite).texture;
		return null;
	}

	public void SetImage(Object image) {
		switch(type) {
			case Type.RawImage:
				(target as RawImage).texture = GetTexture(image);
				break;
			case Type.Image:
				(target as Image).sprite = image as Sprite;
				break;
			case Type.SpriteRenderer:
				(target as SpriteRenderer).sprite = image as Sprite;
				break;
		}
	}

	void InitTarget() {
		target = GetComponent(typeDict[type]);
		if(target == null) {
			foreach(var candidateType in typeDict.Keys) {
				var candidate = GetComponent(typeDict[candidateType]);
				if(candidate != null) {
					type = candidateType;
					target = candidate;
					Debug.LogWarning($"Incompatible image renderer type, detected and replaced with {typeDict[candidateType].Name}", gameObject);
					return;
				}
			}
			Debug.LogWarning("No renderer found", gameObject);
		}
	}

	protected void Start() {
		InitTarget();
	}
}
