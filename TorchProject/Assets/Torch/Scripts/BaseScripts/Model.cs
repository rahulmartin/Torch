using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*Keeps track of controller and view objects*/

public interface IModelObserver {
}

public class Model : MonoBehaviour {
	MVC capsule;
	Dictionary<string, object> globalPropertyList = new Dictionary<string, object>();

	public void initiate(MVC mvcCapsule) {
		capsule = mvcCapsule;
	}

	public void Update() {
		StateManager.Update();
	}

	public void SetGlobalProperty(string propertyID,object propertyValue) {
		if(globalPropertyList.ContainsKey(propertyID))
			globalPropertyList[propertyID] = propertyValue;
		else
			globalPropertyList.Add(propertyID, propertyValue);
	}

	public T GetGlobalProperty<T>(string propertyID) {
		if(globalPropertyList.ContainsKey(propertyID))
			return (T)globalPropertyList[propertyID];
		else {
			Debug.LogWarning(propertyID + " is not a valid global property please check the ID");
			return (T) (object) 0;
		}
	}

	public void RemoveGlobalProperty(string propertyID) {
		if(globalPropertyList.ContainsKey(propertyID))
			globalPropertyList.Remove(propertyID);
		else
			Debug.LogWarning("Property was not removed , it was already removed or was never present int the dictionary");
	}
}
