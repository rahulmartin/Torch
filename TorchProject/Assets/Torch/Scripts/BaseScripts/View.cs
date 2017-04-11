using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* View Creates and manipulate objects viewed in the game , but only when called by the controller*/

public class View: MonoBehaviour, IControllerObserver {
	public MVC capsule;
	public Dictionary<string, ObjectData> viewObjectsDic = new Dictionary<string, ObjectData>();

	public void initiate(MVC mvcCapsule) {
		capsule = mvcCapsule;
		capsule.controller.GetComponent<Controller>().AddObserver(this);
	}

	public void SetPrefab(string uid, string prefabID) {
		Controller controller = capsule.controller.GetComponent<Controller>();
		GameObject loadedPrefab = Resources.Load<GameObject>(prefabID);
		controller.GetObjectData(uid).objectPrefab = Instantiate(loadedPrefab);
		GameObject instatiatedPrefab = controller.GetObjectData(uid).objectPrefab;
		instatiatedPrefab.transform.SetParent(capsule.view.transform);
		instatiatedPrefab.transform.name = prefabID + " (" + uid + ")";
	}

	public void SetPosition(string uid, Vector3 newPosition) {
		Controller controller = capsule.controller.GetComponent<Controller>();
		GameObject instatiatedPrefab = controller.GetObjectData(uid).objectPrefab;
		instatiatedPrefab.transform.position = newPosition;
	}

	public void OnObjectCreated(string uid, ObjectData objectData) {
		viewObjectsDic.Add(uid, objectData);
		Debug.Log("view object created");
	}

	public void OnObjectDestroyed(string uid) {
		viewObjectsDic.Remove(uid);
	}

	public string GetTransUid(Transform trans) {
		foreach(KeyValuePair<string, ObjectData> pair in viewObjectsDic) {
			if(pair.Value.objectPrefab == trans)
				return pair.Key;
		}
		Debug.LogWarning("required Uid was not in view list");
		return null;
	}

	public Transform GetTransForm(string uid) {
		if(viewObjectsDic.ContainsKey(uid))
			return viewObjectsDic[uid].objectPrefab.transform;
		else
			return null;
	}

	public bool HasObject(string uid) {
		if(!viewObjectsDic.ContainsKey(uid))
			return false;

		if(viewObjectsDic[uid].objectPrefab == null)
			return false;

		return true;
	}
}
