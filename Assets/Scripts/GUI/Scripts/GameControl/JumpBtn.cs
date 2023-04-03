using UnityEngine;
using System.Collections;

public class JumpBtn : MonoBehaviour {

	private HeroController heroController;
	public LevelManager levelManager;
	private bool isPress=false;
	private GameDataManager gameDataManager;

	// Use this for initialization
	void Start () {
		gameDataManager = GameDataManager.GetInstance();
		AddEventListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEventListener(){
		gameDataManager.OnLevelStart+=OnLevelStart;
		gameDataManager.OnGameRestart+=OnGameRestart;
	}

	private void RemoveEventListener(){
		if(gameDataManager!=null){
			gameDataManager.OnLevelStart-=OnLevelStart;
			gameDataManager.OnGameRestart-=OnGameRestart;
		}
	}

	private void OnLevelStart(){
		heroController = levelManager.heroInstance.gameObject.GetComponent<HeroController>();
	}

	private void OnGameRestart(){
		heroController = levelManager.heroInstance.gameObject.GetComponent<HeroController>();
	}

	private void Update(){
		if(heroController==null)return;

		if(gameDataManager.IsLevelComplete){
			heroController.isJumping =false;
			return;
		}

		if(isPress){
			heroController.Jump();
		}else if(!isPress){
			heroController.isJumping =false;
		}
	}
	
	private void OnClick(){
		//heroController.Jump();
		//Debug.Log("jump");
	}


	private void OnDoubleClick (){
		//heroController.Jump();
		//Debug.Log("double jump");
	}


	private void OnPress(bool isDown){
		isPress = isDown;
	}
}
