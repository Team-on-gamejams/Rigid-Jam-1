using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallLoadScene : MonoBehaviour {
	public void CallSceneLoadWithUI(string name) {
		SceneLoader.instance.LoadScene(name, true, true);
	}

	public void CallSceneLoad(string name) {
		SceneLoader.instance.LoadScene(name, false, false);
	}
}
