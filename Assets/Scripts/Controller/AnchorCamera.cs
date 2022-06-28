using System.Collections.Generic;
using UnityEngine;
using CC = CameraController;

[RequireComponent(typeof(CC))]
public class AnchorCamera : MonoBehaviour {
	public HashSet<CC> registry = new HashSet<CC>();
	CC self;
	public CC controller => self;

	public static void Link(CC slave, CC master) {
		if(master != null)
			master.Target = null;
		if(slave != null)
			slave.Target = master?.camera;
	}

	void Start() {
		self = GetComponent<CC>();
	}

	public void Register(CC cc) {
		registry.Add(cc);
		Link(self, cc);
	}

	public void SetMaster(CC cc) {
		if(cc == null || cc == self) {
			foreach(CC slave in registry)
				Link(slave, self);
		}
		else {
			if(!registry.Contains(cc))
				Register(cc);
			SetMaster(self);
			Link(self, cc);
		}
	}

	public void SetMaster(Camera camera) {
		var cc = camera.GetComponent<CC>();
		if(cc != null)
			SetMaster(cc);
		else {
			SetMaster(self);
			self.Target = camera;
		}
	}
}
