using UnityEngine;

public class Level : MonoBehaviour {
	public Page page;
	public Storyboard initialStoryboard;

	void Start() {
		page.ViewBoard(initialStoryboard);
	}
}
