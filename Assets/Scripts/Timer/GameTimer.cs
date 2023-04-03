using UnityEngine;
using System.Collections;
using System;
using System.Text;


public class GameTimer : MonoBehaviour {

	public int min;
	public int sec;
	public float totalSeconds;
	private float cacheTotalSeconds;
	public string timerDisplay{set;get;}

	private GameDataManager gameDataManager;
	private SoundManager soundManager;
	private bool hasPlayTimesupNear =false;

	private bool hasStarted = false;
	private bool isStop = false;
	//private bool isLevelComplete =false;

	private bool isTimeOut=false;

	private Action TimeOut;
	public event Action OnTimeOut{
		add{TimeOut+=value;}
		remove{TimeOut-=value;}
	}

	private Action TimeTick;
	public event Action OnTimeTick{
		add{TimeTick+=value;}
		remove{TimeTick-=value;}
	}

	// Use this for initialization
	void Start () {
		gameDataManager = GameDataManager.GetInstance();
		soundManager = SoundManager.GetInstance();
		totalSeconds = ConvertMinToSec(min) + sec;
		cacheTotalSeconds = totalSeconds;
		AddEventListener();
		//Invoke("LevelStart",0.1f);
		//Debug.Log("time started!");
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEventListener(){
		if(gameDataManager!=null){
			gameDataManager.OnLevelStart+=OnLevelStart;
			gameDataManager.OnGameRestart+=OnGameRestart;
			gameDataManager.player.OnPlayerDead+=OnPlayerDead;
			gameDataManager.OnLevelComplete += OnLevelComplete;
		}
	}

	private void RemoveEventListener(){
		if(gameDataManager!=null){
			gameDataManager.OnLevelStart-=OnLevelStart;
			gameDataManager.OnGameRestart-=OnGameRestart;
			gameDataManager.player.OnPlayerDead-=OnPlayerDead;
			gameDataManager.OnLevelComplete -= OnLevelComplete;
		}
	}

	/*private void LevelStart(){
		if(!gameDataManager.IsLevelStart){
			gameDataManager.IsLevelStart = true;
		}
	}*/

	public void Reset(){
		totalSeconds = cacheTotalSeconds;
	}

	private void OnLevelComplete(){
		isStop = true;
	}

	private void OnPlayerDead(){
		isStop = true;
	}

	private void OnGameRestart(){
		isStop = false;
		Reset();
		hasStarted = true;
	}

	private void OnLevelStart(){
		isStop = false;
		Reset();
		hasStarted = true;
	}
	
	// Update is called once per frame
	void Update (){
		if(!hasStarted || isStop) return;
		if(totalSeconds>0){
			totalSeconds-=Time.deltaTime;
			timerDisplay = ConvertSecToMin(totalSeconds);
			if(totalSeconds <30f && !hasPlayTimesupNear){
				hasPlayTimesupNear = true;
				soundManager.PlaySfx(SFX.TimesupNear);
			}
			//Debug.Log( "time ==>" + timerDisplay);
		}else{
			if(!isTimeOut){
				isTimeOut =true;
				if(null != TimeOut){
					TimeOut();
				}
			}
		}

		if(null!=TimeTick){
			TimeTick();
		}
	}

	private int ConvertMinToSec(int minute){
		return minute * 60;
	}

	public string ConvertSecToMin(float second){
		float remainder = (second % 60);
		float total = second / 60;
		return ((int)total).ToString("00") + ":" + ((int)remainder).ToString("00");
	}
}
