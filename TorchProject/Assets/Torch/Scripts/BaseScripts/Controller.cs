using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

/* All the logic related to creation , controlling and destruction of game objects happens here*/
public interface IControllerObserver {
	void OnObjectCreated(string uid, ObjectData objectData);
	void OnObjectDestroyed(string uid);
}


public class Controller : MonoBehaviour {
	public MVC capsule;
	public Dictionary<string, ObjectData> newObjectsDic = new Dictionary<string, ObjectData>();
	public Dictionary<string,ObjectData> activeObjectsDic = new Dictionary<string, ObjectData>();
	public List<string> newUidList = new List<string>();
	public List<string> activeUidList = new List<string>();
	public List<string> inactiveUidList = new List<string>();
	public List<IControllerObserver> observerList = new List<IControllerObserver>();
	//top level methods

	public void Start() {
		DontDestroyOnLoad(this);
		Debug.Log("Controller Started");
	}

	public void AddObserver(IControllerObserver newObserver) {
		observerList.Add(newObserver);
	}

	public void RemoveObserver(IControllerObserver oldObserver) {
		observerList.Remove(oldObserver);
	}

	public void Update() {
		for(int i = newUidList.Count, j = 0; j <= i - 1; j++) {
			MethodInfo[] startMethods = newObjectsDic[newUidList[j]].startMethod;
			if(startMethods == null)
				continue;
			for(int k = startMethods.Length,l=0; l <= k-1; l++) {
				CallMethod(startMethods[l], newUidList[j]);
			}
			activeUidList.Add(newUidList[j]);
			activeObjectsDic.Add(newUidList[j], newObjectsDic[newUidList[j]]);
		}

		newUidList.Clear();
		newObjectsDic.Clear();

		for(int i = activeUidList.Count, j = 0; j <= i - 1; j++) {
			MethodInfo[] updateMethods = activeObjectsDic[activeUidList[j]].updateMethod;
			if(updateMethods == null)
				continue;
			for(int k = updateMethods.Length,l=0; l<=k-1; l++) {
				CallMethod(updateMethods[l], activeUidList[j]);
			}
		}

		//removing destroyed objects
		for(int i = inactiveUidList.Count, j = 0; j <= i - 1; j++) {
			GameObject prefabObject;
			if(activeObjectsDic[inactiveUidList[j]].objectPrefab != null)
				prefabObject = activeObjectsDic[inactiveUidList[j]].objectPrefab.gameObject;
			else
				prefabObject = null;
			
			if(prefabObject != null) {
				Destroy(prefabObject);
				NotifyObjectDestruction(inactiveUidList[j]);
			}
			activeUidList.Remove(inactiveUidList[j]);
			activeObjectsDic.Remove(inactiveUidList[j]);
		}

		inactiveUidList.Clear();
	}

	public ObjectData GetObjectData(string uid) {
		if(newUidList.Contains(uid))
			return newObjectsDic[uid];
		else if(activeUidList.Contains(uid))
			return activeObjectsDic[uid];
		else {
			Debug.LogWarning("Objectdata was not found in active/inactive directory for uid " + uid);
			return null;
		}
	}



	//Lower Level methods
	public void initiate(MVC mvcCapsule) {
		capsule = mvcCapsule;
	}

	public void CallMethod(MethodInfo method, string uid, object[] args = null) {
		object[] finalArgs = new object[] { uid };
		if(args != null)
			finalArgs = finalArgs.Concat(args).ToArray();

		method.Invoke(null, finalArgs);
	}

	public void StartGame(string startScript, string startMethod) {

		Type t = Type.GetType(startScript);

		if(t == null)
			throw new System.Exception("Game Root Class not found , check the initiator configuration!");

		MethodInfo startGameMethod = t.GetMethod(startMethod, BindingFlags.Static | BindingFlags.Public);
		startGameMethod.Invoke(null, null);
	}

	public string CreateObject(string _uid = null, string[] _scriptChain = null) {
		string uid;
		if(_uid == null)
			uid = System.Guid.NewGuid().ToString();
		else
			uid = _uid;

		ObjectData newObjectData = new ObjectData(uid);

		if(_scriptChain != null) {
			newObjectData.startMethod = GetStartMethods(_scriptChain);
			newObjectData.updateMethod = GetUpdateMethods(_scriptChain);
		}
		newObjectsDic.Add(uid, newObjectData);
		newUidList.Add(uid);
	
		//notify observers
		NotifyObjectCreation(uid, newObjectData);
	
		return uid;
	}

	public void NotifyObjectCreation(string uid, ObjectData data) {
		for(int i = observerList.Count, j = 0; j <= i - 1; j++) {
			observerList[j].OnObjectCreated(uid, data);
		}
	}

	public void NotifyObjectDestruction(string uid) {
		for(int i = observerList.Count, j = 0; j <= i - 1; j++) {
			observerList[j].OnObjectDestroyed(uid);
		}
	}

	public void DestroyObject(string _uid) {
		if(newUidList.Contains(_uid)) {
			Debug.LogWarning("Failed to destroy object uid " + _uid + " Cannot Destroy and object immediately after creation!");
		} else if(activeUidList.Contains(_uid)) {
			inactiveUidList.Add(_uid);
		} else
			Debug.LogWarning("Failed to destroy object uid " + _uid + " was not found");
	}

	MethodInfo[] GetStartMethods(string[] _scriptChain) {
		MethodInfo[] startMethods = new MethodInfo[_scriptChain.Length];
		for(int i = _scriptChain.Length, j = 0; j <= i - 1; j++) {
			Type t = Type.GetType(_scriptChain[j]);
			if(t == null)
				throw new System.Exception("ScriptChain " + _scriptChain[j] + " not found!");
			MethodInfo startMethod = t.GetMethod("Start", BindingFlags.Static | BindingFlags.Public);

			if(startMethod == null)
				throw new System.Exception("ScriptChain " + _scriptChain[j] + " does not have a start method!");

			startMethods[j] = startMethod;
		}

		return startMethods;
	}

	MethodInfo[] GetUpdateMethods(string[] _scriptChain) {
		MethodInfo[] updateMethods = new MethodInfo[_scriptChain.Length];
		for(int i = _scriptChain.Length, j = 0; j <= i - 1; j++) {
			Type t = Type.GetType(_scriptChain[j]);
			if(t == null)
				throw new System.Exception("ScriptChain " + _scriptChain[j] + " not found!");
			MethodInfo updateMethod = t.GetMethod("Update", BindingFlags.Static | BindingFlags.Public);

			if(updateMethod == null)
				throw new System.Exception("ScriptChain " + _scriptChain[j] + " does not have a update method!");

			updateMethods[j] = updateMethod;
		}

		return updateMethods;
	}
}
