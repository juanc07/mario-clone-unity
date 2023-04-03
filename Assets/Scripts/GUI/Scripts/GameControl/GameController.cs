using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
	private HeroController heroController;
	private GameDataManager gameDataManager;
	private GameControllerManager gameControllerManager;

	public LevelManager levelManager;

	// Use this for initialization
	private void Start (){
		gameDataManager = GameDataManager.GetInstance();
		gameControllerManager = GameControllerManager.GetInstance();
		AddEventListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEventListener(){
		gameControllerManager.OnDownInput+=OnDownInput;
		gameControllerManager.OnUpInput+=OnUpInput;
		gameControllerManager.OnHeldInput+=OnHeldInput;

		gameDataManager.OnLevelStart+=OnLevelStart;
		gameDataManager.OnGameRestart+=OnGameRestart;
	}

	private void RemoveEventListener(){
		gameControllerManager.OnDownInput-=OnDownInput;
		gameControllerManager.OnUpInput-=OnUpInput;
		gameControllerManager.OnHeldInput-=OnHeldInput;

		gameDataManager.OnLevelStart-=OnLevelStart;
		gameDataManager.OnGameRestart-=OnGameRestart;
	}

	private void OnLevelStart(){
		heroController = levelManager.heroInstance.gameObject.GetComponent<HeroController>();
		//Debug.Log("OnLevelStart GameController");
	}

	private void OnGameRestart(){
		heroController = levelManager.heroInstance.gameObject.GetComponent<HeroController>();
		//Debug.Log("OnGameRestart GameController");
	}

	private void OnDownInput(ButtonInput  buttonInput){
		if(gameDataManager.IsLevelComplete) return;
		if(heroController==null)return;
		//Debug.Log( "Button Press:  " + buttonInput.ToString() );
		if(buttonInput == ButtonInput.B || buttonInput == ButtonInput.Grab){
			if(!heroController.IsDead){
				if(heroController.isHoldingSomething){
					if(!heroController.isThrow){
						heroController.isThrow =true;
					}
				}else{
					if(gameDataManager.player.IsGotFireball){
						heroController.FireWeapon();
					}
				}
			}
		}

		if(buttonInput == ButtonInput.Down){
			heroController.IsDownBtnPress = true;
			//Debug.Log("down down!");
		}
	}

	private void OnUpInput(ButtonInput  buttonInput){
		if(gameDataManager.IsLevelComplete) return;
		if(heroController==null)return;
		//Debug.Log( "Button up:  " + buttonInput.ToString() );
		if(buttonInput == ButtonInput.B || buttonInput == ButtonInput.Grab){
			heroController.isHoldingAction=false;
		}

		if(buttonInput == ButtonInput.A || buttonInput == ButtonInput.Y || buttonInput ==  ButtonInput.Jump ){
			heroController.isJumping =false;
		}

		if(buttonInput == ButtonInput.Left || buttonInput == ButtonInput.Right){
			heroController.isLeftBtnPress =false;
			heroController.isRightBtnPress =false;
		}

		if(buttonInput == ButtonInput.Down){
			heroController.IsDownBtnPress = false;
			//Debug.Log("down up!");
		}
	}

	private void OnHeldInput(ButtonInput  buttonInput){
		if(gameDataManager.IsLevelComplete) return;
		if(heroController==null)return;
		//Debug.Log( "Button held:  " + buttonInput.ToString() );
		if(buttonInput == ButtonInput.Left){
			heroController.isLeftBtnPress =true;
			heroController.isRightBtnPress =false;
		}else if(buttonInput == ButtonInput.Right){
			heroController.isLeftBtnPress =false;
			heroController.isRightBtnPress =true;
		}else{
			heroController.isLeftBtnPress =false;
			heroController.isRightBtnPress =false;
		}

		if(buttonInput == ButtonInput.Down){
			heroController.isLookingUp =false;
			heroController.isLookingDown =true;
		}else if( buttonInput == ButtonInput.Up ){
			heroController.isLookingUp =true;
			heroController.isLookingDown =false;
		}else{
			heroController.isLookingUp =false;
			heroController.isLookingDown =false;
		}

		if(buttonInput == ButtonInput.B || buttonInput == ButtonInput.Grab ){
			if(!heroController.IsDead){
				if(gameDataManager.player.IsGotFireball){
					heroController.FireWeapon();
				}else{
					heroController.isHoldingAction=true;
				}
			}
		}

		if(buttonInput ==  ButtonInput.A || buttonInput ==  ButtonInput.Y || buttonInput ==  ButtonInput.Jump){
			if(!heroController.IsDead){
				heroController.Jump();
			}
		}

		if(buttonInput ==  ButtonInput.FireWeapon){
			if(!heroController.IsDead){
				if(gameDataManager.player.IsGotFireball){
					heroController.FireWeapon();
				}
			}
		}
	}


	// Update is called once per frame
	void Update (){
		if(heroController==null)return;

		if(gameDataManager.IsLevelComplete){
			heroController.isLeftBtnPress =false;
			heroController.isRightBtnPress =false;

			heroController.isLookingUp =false;
			heroController.isLookingDown =false;

			heroController.isRunning =false;
			heroController.isWalking =false;
			return;
		}
	}
	#endif
}
