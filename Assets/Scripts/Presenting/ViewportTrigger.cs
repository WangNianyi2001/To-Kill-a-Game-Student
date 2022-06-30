using System;
using UnityEngine;

public class ViewportTrigger : MonoBehaviour {
	[NonSerialized] public Viewport viewport;

	void OnTriggerEnter(Collider other) {
		viewport.page.ViewStoryboard(viewport.storyboard);
	}
}
