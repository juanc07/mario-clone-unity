using UnityEngine;
using System.Collections;

public class LevelCompleteNextLevelBtn : EventListener {
	
	private ScenePreloader scenePreloader;
	private GameDataManager gameDataManager;
	private bool hasClicked =false;
	private GameControllerManager gameControllerManager;
	public LevelCompletePopupController levelCompletePopupController;

	public override void Start () {
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
		if(buttonInput == ButtonInput.Start || buttonInput == ButtonInput.A){
			LoadNextLevel();
		}
	}
	
	private void OnClick(){
		if(!hasClicked){
			hasClicked =true;
			if(levelCompletePopupController!=null){
				if(!levelCompletePopupController.isDoneScoreAnimation){
					levelCompletePopupController.SkipTotalScore();
					Invoke("LoadNextLevel",1.75f);
				}else{
					LoadNextLevel();
				}				
			}
		}
	}

	private void LoadNextLevel(){
		 int level = gameDataManager.UpdateLevel();
		if(level <= 32){
			scenePreloader.LoadScene(ScenePreloader.Scenes.Game);
		}else{
			//load ending scene here
			scenePreloader.LoadScene(ScenePreloader.Scenes.Ending);
		}
	}


}
