using UnityEngine;
using System.Collections;

public class OptionRetryButton : MonoBased {
	
	private ScenePreloader scenePreloader;
	private GameControllerManager gameControllerManager;
	private GameDataManager gameDataManager;
	public OptionPopupController optionPopupController;
	
	private bool hasClick;
	
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
	}
	
	private void OnClick(){
		if(!hasClick){
			hasClick = true;
			optionPopupController.SkipAnimation();
			ReloadLevel();
		}
	}
	
	private void ReloadLevel(){
		gameDataManager.Coin = 0;
		scenePreloader.LoadScene(ScenePreloader.Scenes.Game);
	}
}
