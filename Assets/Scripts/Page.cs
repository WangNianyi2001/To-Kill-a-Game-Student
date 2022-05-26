using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Page : MonoBehaviour {
	public static Page current = null;

	IEnumerable<Storyboard> storyboards;

	Cinemachine.CinemachineBrain brain;
	public new Camera camera => brain.OutputCamera;
	public Storyboard currentStoryboard;

	public float blendTime = 1.0f;

	public void ViewBoard(Storyboard board, float blendTime) {
		brain.m_DefaultBlend.m_Time = blendTime;
		if(currentStoryboard != null)
			currentStoryboard.state = Storyboard.State.Visible;
		currentStoryboard = board;
		currentStoryboard.state = Storyboard.State.Active;
	}

	public void ViewBoard(Storyboard board) => ViewBoard(board, blendTime);

	void CheckMouseClick() {
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask.GetMask("Storyboard"));
		Storyboard board = hit.collider?.GetComponent<Storyboard>();
		if(board == null)
			return;
	}

	void Start() {
		brain = GetComponentInChildren<Cinemachine.CinemachineBrain>();
		current = this;
		storyboards = FindObjectsOfType<Storyboard>();
		var urp = camera.GetComponent<UniversalAdditionalCameraData>();
		foreach(var storyboard in storyboards)
			urp.cameraStack.Add(storyboard.viewport.camera);
		ViewBoard(currentStoryboard, 0);
	}

	void Update() {
		foreach(var storyboard in storyboards)
			storyboard.viewport.UpdateCamera(storyboard.transform, brain.transform);
		if(Input.GetMouseButtonDown(0))
			CheckMouseClick();
	}
}
