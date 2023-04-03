using UnityEngine;
using System.Collections;

public class PreloaderChecker : MonoBehaviour {
	private ScenePreloader scenePreloader;
	public GameObject preloaderUIPrefab;
	// Use this for initialization
	void Start (){
		QualitySettings.SetQualityLevel(1);
		scenePreloader  = GameObject.FindObjectOfType<ScenePreloader>();
		if(scenePreloader==null){
			GameObject preloaderUI=  Instantiate(preloaderUIPrefab) as GameObject;
			preloaderUI.name = "PreloaderUI";
			scenePreloader = preloaderUI.GetComponent<ScenePreloader>();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
