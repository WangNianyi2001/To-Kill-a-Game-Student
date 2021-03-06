public class ViewportCamera : PostProcess {
	public Viewport viewport;

	public static ViewportCamera CreateOn(Viewport viewport) {
		var vpc = viewport.camera.gameObject.AddComponent<ViewportCamera>();
		vpc.materials.Add(viewport.page.comicMat);
		return vpc;
	}

	new void Start() {
		base.Start();
		transform.localScale = UnityEngine.Vector3.one;
	}
}
