using UnityEngine;
using System.Collections;

public class PressAnyWhere : MonoBehaviour {

	private bool isTouched = false;
	private ScenePreloader scenePreloader;
	private GameDataManager gameDataManager;

	private GameControllerManager gameControllerManager;

	// Use this for initialization
	void Start () {
		gameDataManager = GameDataManager.GetInstance();
		scenePreloader  = GameObject.FindObjectOfType<ScenePreloader>();
		gameControllerManager = GameControllerManager.GetInstance();
		AddEventListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}
	
	private void AddEventListener(){
		gameControllerManager.OnUpInput+=OnUpInput;
	}
	
	private void RemoveEventListener(){
		gameControllerManager.OnUpInput-=OnUpInput;
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_ANDROID || UNITY_IPHONE
    
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !isTouched){
			isTouched = true;
			LoadMainMenu();
		}
		#endif

		#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_STANDALONE_LINUX
		if(Input.GetMouseButtonUp(0) && !isTouched){
			isTouched = true;
			LoadMainMenu();
		}
		#endif
	}

	private void LoadMainMenu(){
		gameDataManager.ResetGameData();
		scenePreloader.LoadScene(ScenePreloader.Scenes.Title);
	}

	private void OnUpInput(ButtonInput  buttonInput){
		if(buttonInput == ButtonInput.B || buttonInput == ButtonInput.Grab){
			if(!isTouched){
				isTouched = true;
				LoadMainMenu();
			}
		}
		
		if(buttonInput == ButtonInput.A || buttonInput == ButtonInput.Y || buttonInput ==  ButtonInput.Jump ){
			if(!isTouched){
				isTouched = true;
				LoadMainMenu();
			}
		}
		
		if(buttonInput == ButtonInput.Down){
			if(!isTouched){
				isTouched = true;
				LoadMainMenu();
			}
		}
	}
}
