using UnityEngine;

public class ViewportTrigger : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		Viewport viewport = transform.parent.GetComponent<Viewport>();
		Page page = viewport.page;
		page.ViewStoryboard(viewport.storyboard);
	}
}
