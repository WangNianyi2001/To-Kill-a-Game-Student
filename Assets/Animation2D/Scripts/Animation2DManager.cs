using System;
using UnityEngine;

[AddComponentMenu("Animation2D/Manager")]
public class Animation2DManager : MonoBehaviour {
	private Animation2D[] animations => GetComponents<Animation2D>();
	[NonSerialized] public Animation2D current = null;

	public bool isPlaying => current != null && current.isPlaying;

	public void Play(string name) {
		Stop();
		foreach(Animation2D anim in animations) {
			if(anim.animationName == name) {
				anim.Play(true);
				current = anim;
				return;
			}
		}
		Debug.LogError($"No animation ${name} found", gameObject);
	}

	public void Stop() {
		current?.Pause();
		current = null;
	}

	public void Pause() {
		current?.Pause();
	}

	public void Resume() {
		current?.Play();
	}
}
