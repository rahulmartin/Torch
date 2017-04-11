using UnityEngine;
using System.Collections;

public class Initiator : MonoBehaviour {
	public string GameName;
	public string rootClassName;
	public string rootClassMethod;

	public GameObject modelPrefab;
	public GameObject viewPrefab;
	public GameObject controllerPrefab;

	void Start() {
		DontDestroyOnLoad(this);

		MVC mvcCapsule = new MVC();
		mvcCapsule.model =(GameObject) GameObject.Instantiate (modelPrefab, this.transform);
		mvcCapsule.view =(GameObject) GameObject.Instantiate (viewPrefab, this.transform);
		mvcCapsule.controller =(GameObject) GameObject.Instantiate (controllerPrefab, this.transform);

		mvcCapsule.model.GetComponent<Model> ().initiate (mvcCapsule);
		mvcCapsule.view.GetComponent<View> ().initiate (mvcCapsule);
		mvcCapsule.controller.GetComponent<Controller> ().initiate (mvcCapsule);
		Torch.initTorch(mvcCapsule);
		mvcCapsule.controller.GetComponent<Controller> ().StartGame (rootClassName, rootClassMethod);
	}
}
