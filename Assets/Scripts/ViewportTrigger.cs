using System;
using UnityEngine;

public class ViewportTrigger : MonoBehaviour {
	[NonSerialized] public Viewport viewport;

	void OnTriggerEnter(Collider other) {
		Page page = viewport.page;
		page.ViewStoryboard(viewport.storyboard);
	}
}
