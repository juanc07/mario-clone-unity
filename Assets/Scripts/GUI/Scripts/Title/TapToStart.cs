using UnityEngine;
using System.Collections;

public class TapToStart : MonoBehaviour {

	private ScenePreloader scenePreloader;

	// Use this for initialization
	void Start () {
		scenePreloader  = GameObject.FindObjectOfType<ScenePreloader>();
	}

	void Update() {
		if (Input.GetMouseButtonDown(0))
			scenePreloader.LoadScene(ScenePreloader.Scenes.Game);

		#if UNITY_ANDROID || UNITY_IPHONE
			foreach (Touch touch in Input.touches) {
				if (touch.phase == TouchPhase.Began){
					scenePreloader.LoadScene(ScenePreloader.Scenes.Game);
				}			
			}
		#endif
	}
}
