using System;
using UnityEngine;

public class ViewportTrigger : MonoBehaviour {
	[NonSerialized] public Viewport viewport;

	void OnTriggerEnter(Collider other) {
		Page current = Page.current;
		current.ViewStoryboard(viewport.storyboard, current.init);
		current.init = false;
	}
}
