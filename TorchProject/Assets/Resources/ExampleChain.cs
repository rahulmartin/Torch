using UnityEngine;
using System.Collections;

public static class ExampleChain  {

	// Use this for initialization
	public static void Start (string uid) {
	}
	
	// Update is called once per frame
	public static void Update (string uid) {
		if(Input.GetKey("d")) {
			Torch.DestroyObject(uid);
		}

		Transform thisTransform = Torch.GetTransform(uid);
		thisTransform.Rotate(new Vector3(Random.value*10,Random.value*10,Random.value*10));
	}
}
