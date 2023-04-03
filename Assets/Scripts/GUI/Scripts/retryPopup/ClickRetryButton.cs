using UnityEngine;
using System.Collections;

public class ClickRetryButton :EventListener {

	public GameObject retryPopupPanel;
	private GameDataManager gameDataManager;
	private ScenePreloader scenePreloader;
	private GameControllerManager gameControllerManager;

	// Use this for initialization
	public override void Start () {
		scenePreloader  = GameObject.FindObjectOfType<ScenePreloader>();
		gameDataManager = GameDataManager.GetInstance();
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
			//ReloadGame();
			RestartGame();
		}else if(buttonInput == ButtonInput.Back){
			scenePreloader.LoadScene(ScenePreloader.Scenes.Title);
		}
	}

	
	private void OnClick(){
		//ReloadGame();
		RestartGame();
	}

	private void RestartGame(){
		if(gameDataManager.player.IsDead && gameDataManager.player.Life >=0){
			gameDataManager.Coin = 0;
			retryPopupPanel.SetActive(false);
			gameDataManager.ResetLevelData();			
		}
	}

	private void ReloadGame(){
		RestartGame();
		scenePreloader.LoadScene(ScenePreloader.Scenes.Game);
	}
}
