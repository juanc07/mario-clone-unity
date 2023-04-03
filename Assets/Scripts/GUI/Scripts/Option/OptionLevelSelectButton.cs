using UnityEngine;
using System.Collections;

public class OptionLevelSelectButton : MonoBehaviour {
	private bool hasClicked = false;

	private ScenePreloader scenePreloader;
	private GameControllerManager gameControllerManager;
	private GameDataManager gameDataManager;
	public OptionPopupController optionPopupController;
	// Use this for initialization
	void Start () {
		hasClicked = false;
		gameDataManager = GameDataManager.GetInstance();
		scenePreloader  = GameObject.FindObjectOfType<ScenePreloader>();
		gameControllerManager = GameControllerManager.GetInstance();
	}
	
	private void OnClick(){
		if(!hasClicked){
			hasClicked = true;
			//go to level select
			optionPopupController.SkipAnimation();
			loadLevelSelectScreen();
			Debug.Log("click level select");
		}
	}

	private void loadLevelSelectScreen(){
		gameDataManager.Coin = 0;
		scenePreloader.LoadScene(ScenePreloader.Scenes.Title);
	}
}
