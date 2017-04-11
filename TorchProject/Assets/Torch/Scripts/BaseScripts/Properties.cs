using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public class Properties {
}

public class MVC {
	public GameObject model;
	public GameObject view;
	public GameObject controller;
}

public class MVCUnwrapped {
	public Model model;
	public View view;
	public Controller controller;
}

public class ObjectData {
	public string uid;
	public GameObject objectPrefab;
	public MethodInfo[] startMethod;
	public MethodInfo[] updateMethod;

	public ObjectData(string objectUid, GameObject prefab = null, MethodInfo[] _startMethod = null, MethodInfo[] _updateMethod = null) {
		uid = objectUid;
		objectPrefab = prefab;
		startMethod = _startMethod;
		updateMethod = _updateMethod;
	}
}

public class ObjectAttributes {
	//this will store different attributes for objects like speed , damage etc
}
