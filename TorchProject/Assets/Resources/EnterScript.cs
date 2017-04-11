using UnityEngine;
using System.Collections;

public static class EnterScript  {

	public static void GameEnter() {
		Debug.Log ("We have Entered the game");
		string[] scriptChain = { "ExampleChain" };
		for(int i = 0; i < 100; i++) {
			string uid = Torch.CreateObject(null, scriptChain);
			Torch.SetPrefab(uid, "Cube");
			Torch.SetTransformPosition(uid, new Vector3(Random.value*10,Random.value*10,Random.value*10));
		}

		string textUID = Torch.CreateObject(null);
		Torch.SetPrefab(textUID, "Text", true);
		Torch.SetUIPosition(textUID, new Vector2(Screen.width/2, Screen.height/2));



		Torch.SwitchSceneAsych("ExampleScene", SceneLoadCompleteCallback);
	}

	public static void SceneLoadCompleteCallback(float progress, bool isDone) {
		if(progress < 1)
			Debug.Log("loading scnee");
		else
			Debug.Log("load completed");
		
	}

}
