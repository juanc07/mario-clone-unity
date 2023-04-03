using UnityEngine;
using System.Collections;

public class TitlePlayButton : EventListener {

	private ScenePreloader scenePreloader;
	private GameControllerManager gameControllerManager;

	public override void Start () {
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
			StartGame();
		}
	}


	
	private void OnClick(){
		StartGame();
	}

	private void StartGame(){
		scenePreloader.LoadScene(ScenePreloader.Scenes.Game);
	}

}
