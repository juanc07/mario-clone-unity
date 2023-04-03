using UnityEngine;
using System.Collections;

public class LevelCompleteRetryBtn : EventListener {

	private ScenePreloader scenePreloader;
	private GameControllerManager gameControllerManager;
	private GameDataManager gameDataManager;

	private bool hasClick;
	public LevelCompletePopupController levelCompletePopupController;

	public override void Start () {
		hasClick = false;
		gameDataManager = GameDataManager.GetInstance();
		scenePreloader  = GameObject.FindObjectOfType<ScenePreloader>();
		gameControllerManager = GameControllerManager.GetInstance();

		base.Start();
	}
	
	public override void OnDestroy(){
		base.OnDestroy();
	}
	
	public override void AddEventListner ()
	{
		base.AddEventListner ();
		if(gameControllerManager!=null){
			gameControllerManager.OnDownInput+=OnDownInput;
		}
	}
	
	public override void RemoveEventListner ()
	{
		base.RemoveEventListner ();
		if(gameControllerManager!=null){
			gameControllerManager.OnDownInput-=OnDownInput;
		}
	}
	
	private void OnDownInput(ButtonInput buttonInput){
		if(buttonInput == ButtonInput.Back){
			ReloadLevel();
		}

		Debug.Log("yeayh!");
	}
	
	private void OnClick(){
		if(!hasClick){
			hasClick = true;
			if(!levelCompletePopupController.isDoneScoreAnimation){
				levelCompletePopupController.SkipTotalScore();
				Invoke("ReloadLevel",1.75f);
			}else{
				ReloadLevel();
			}
		}
	}

	private void ShowHideLevelCompletePopupController(bool val){
		levelCompletePopupController.gameObject.SetActive(val);
	}

	private void ReloadLevel(){
		gameDataManager.Coin = 0;
		//gameDataManager.ResetLevelData();
		//ShowHideLevelCompletePopupController(false);
		scenePreloader.LoadScene(ScenePreloader.Scenes.Game);
	}
}
