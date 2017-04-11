using UnityEngine;
using System.Collections;

public static class Torch  {
	public static MVCUnwrapped capsule = new MVCUnwrapped();
	public delegate void LoadSceneCallback(float progress, bool isdone);
	public static void initTorch(MVC mvcCapsule) {
		capsule.model = mvcCapsule.model.GetComponent<Model>();
		capsule.view = mvcCapsule.view.GetComponent<View>();
		capsule.controller = mvcCapsule.controller.GetComponent<Controller>();
	}

	public static string CreateObject(string uid = null,string[] _scriptChain = null ) {
		return capsule.controller.CreateObject(uid, _scriptChain);
	}

	public static void GetPrefab(string prefabID) {
	}

	public static void SetPrefab(string uid, string prefabID, bool isUI = false) {
		capsule.view.SetPrefab(uid, prefabID, isUI);
	}

	public static void SetTransformPosition(string uid, Vector3 newPos) {
		capsule.view.SetPosition(uid, newPos);
	}

	public static void SetUIPosition(string uid, Vector2 newPos) {
		capsule.view.SetUIPosition(uid, newPos);
	}

	public static void DestroyObject(string uid) {
		capsule.controller.DestroyObject(uid);
	}

	public static void SwitchState(BaseState newState) {
		StateManager.SwitchState(newState);
	}

	public static BaseState GetState() {
		return StateManager.GetState();
	}

	public static string GetUid(Transform trans) {
		string foundUid = capsule.view.GetComponent<View>().GetTransUid(trans);
		return foundUid;
	}

	public static Transform GetTransform(string uid) {
		return capsule.view.GetComponent<View>().GetTransForm(uid);
	}

	public static bool HasTransform(string uid) {
		return capsule.view.GetComponent<View>().HasObject(uid);
	}

	public static void SetGlobalProperty<T>(string propertyID, T value) {
		capsule.model.GetComponent<Model>().SetGlobalProperty(propertyID, value as object);
	}

	public static T GetGlobalProperty<T>(string propertyID) {
		return capsule.model.GetComponent<Model>().GetGlobalProperty<T>(propertyID);
	}

	public static void RemoveGlobalProperty(string propertyID) {
		capsule.model.GetComponent<Model>().RemoveGlobalProperty(propertyID);
	}

	public static void SwitchSceneAsych(string sceneName, LoadSceneCallback callback) {
		capsule.model.GetComponent<Model>().SceneLoadAsynch(sceneName, callback);
	}
}
