using UnityEngine;

[AddComponentMenu("Animation2D/Animation")]
public class Animation2D : ImageManipulator {
	[Header("Settings Properties")]
	public string animationName;

	[Tooltip("Start animating in (in seconds).")]
	[Range(0f, 1000f)]
	public float startIn;

	[Tooltip("Speed of changing frames (in seconds).")]
	[Range(0.01f, 1f)]
	public float speed;

	[Tooltip("Start animation automatically on startup, no need calling from an external code.")]
	public bool autoStart;
	public bool loop = false;

	[Header("Frames & Resources")]
	[Tooltip("Frames to switch in specific time interval.")]
	public Object[] frames;

	const string stepCall = "Step";
	public bool isPlaying => IsInvoking(stepCall);

	private int index;

	new void Start() {
		base.Start();
		if(autoStart)
			Play();
	}

	public void Step() {
		++index;
		if(index == frames.Length) {
			if(loop)
				index = 0;
			else
				Pause();
		}
		SetImage(frames[index]);
	}

	public void Play(bool reset = false) {
		Pause();
		if(reset)
			index = 0;
		InvokeRepeating(stepCall, startIn, speed);
	}

	public void Pause() {
		if(isPlaying)
			CancelInvoke(stepCall);
	}
}
