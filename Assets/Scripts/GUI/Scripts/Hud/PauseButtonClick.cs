using UnityEngine;
using System.Collections;

public class PauseButtonClick : MonoBehaviour {

	private ScenePreloader scenePreloader;
	public GameUIManager gameUIManager;
	private GameDataManager gameDataManager;

	void Start () {
		gameDataManager = GameDataManager.GetInstance();
		scenePreloader  = GameObject.FindObjectOfType<ScenePreloader>();
	}
	
	private void OnClick(){
		if(!gameDataManager.IsLevelComplete){
			gameUIManager.ShowHideOption(true);
		}
	}
}
