using UnityEngine;
using System.Collections;

public class GameUIManager : MonoBehaviour {

	private GameDataManager gameDataManager;

	public GameObject levelHUDPanel;
	public GameObject levelCompletePopup;
	public GameObject retryLevelPopup;
	public GameObject optionPopup;
	public GameObject gameOverPopup;
	public GameObject mobileControllerGUI;

	private SoundManager soundManager;

	// Use this for initialization
	void Start () {
		gameDataManager = GameDataManager.GetInstance();
		soundManager = SoundManager.GetInstance();
		//PlayBGM();
		AddEventListener();
	}

	private void PlayBGM(){
		if(soundManager.isReady){
			if(gameDataManager.Level >= 11 && gameDataManager.Level <22){
				soundManager.PlayBGM(BGM.IceBGM);
			}else if(gameDataManager.Level >= 22){
				soundManager.PlayBGM(BGM.LavaBGM);
			}else{
				soundManager.PlayBGM(BGM.GrassBGM);
			}			
		}
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEventListener(){
		//if(gameDataManager!=null){
		gameDataManager.OnLevelStart+=OnLevelStart;
		gameDataManager.OnGameRestart+=OnGameRestart;
		gameDataManager.OnLevelComplete+= OnLevelComplete;
		gameDataManager.player.OnPlayerDead+=OnPlayerDead;
		gameDataManager.player.OnMaxCoin+=OnMaxCoin;
		soundManager.OnSoundManagerReady += OnSoundManagerReady;
		//Debug.Log("GameUIManager added Listener!");
		//}
	}

	private void RemoveEventListener(){
		if(gameDataManager!=null){
			gameDataManager.OnLevelStart-=OnLevelStart;
			gameDataManager.OnGameRestart-=OnGameRestart;
			gameDataManager.OnLevelComplete-= OnLevelComplete;
			gameDataManager.player.OnPlayerDead-=OnPlayerDead;
			gameDataManager.player.OnMaxCoin-=OnMaxCoin;
			soundManager.OnSoundManagerReady -= OnSoundManagerReady;
		}
	}

	private void OnMaxCoin(){
		soundManager.PlaySfx(SFX.MaxCoin,0.90f);
	}

	private void OnSoundManagerReady(){
		PlayBGM();
	}
	
	public void ShowHideOption(bool val){
		optionPopup.SetActive(val);
	}

	public void ShowHideLevelHUDPanel(bool val){
		levelHUDPanel.SetActive(val);
	}

	public void ShowHideLevelCompletePopup(bool val){
		levelCompletePopup.SetActive(val);
	}

	public void ShowHideMobileControllerGUI(bool val){
		mobileControllerGUI.SetActive(val);
	}

	public void ShowHideGameOverPopup(bool val){
		gameOverPopup.SetActive(val);
	}

	public void ShowHideRetryLevelPopup(bool val){
		retryLevelPopup.SetActive(val);
	}

	private void OnLevelStart(){
		//Debug.Log("GameUIManager level start!");
		PlayBGM();
		ShowHideLevelHUDPanel(true);
	}

	private void OnGameRestart(){
		PlayBGM();
		ShowHideLevelHUDPanel(true);
	}

	private void OnLevelComplete(){
		ShowHideLevelCompletePopup(true);
		soundManager.PlayBGM(BGM.ReachGoal,1f,false);
	}

	private void OnPlayerDead(){
		//if(gameDataManager.Life <= 0){
		if(gameDataManager.Life <=0){
			ShowHideMobileControllerGUI(false);
			ShowHideGameOverPopup(true);
		}else{
			ShowHideRetryLevelPopup(true);
		}

		soundManager.PlayBGM(BGM.Died,1f,false);
	}
}
