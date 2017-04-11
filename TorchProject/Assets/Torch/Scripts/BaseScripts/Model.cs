using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*Keeps track of controller and view objects*/

public interface IModelObserver {
}

public class Model : MonoBehaviour {
	MVC capsule;

	public void initiate(MVC mvcCapsule) {
		capsule = mvcCapsule;
	}

	public void Update() {
		StateManager.Update();
	}
}
