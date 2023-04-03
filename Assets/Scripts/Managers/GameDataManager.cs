using UnityEngine;
using System.Collections;
using System;

public class GameDataManager : MonoBehaviour {

	private static GameDataManager instance;
	private static GameObject container;

	public Player player = new Player();

	private float currentBossHp;
	private Action BossHpChange;
	public event Action OnBossHpChange{
		add{BossHpChange+=value;}
		remove{BossHpChange-=value;}
	}

	private Action BossDied;
	public event Action OnBossDied{
		add{BossDied+=value;}
		remove{BossDied-=value;}
	}

	private bool isShowBossHp;
	private Action ShowBossHp;
	public event Action OnShowBossHp{
		add{ShowBossHp+=value;}
		remove{ShowBossHp-=value;}
	}


	private Action GameRestart;
	public event Action OnGameRestart{
		add{ GameRestart+=value;}
		remove{GameRestart-=value;}
	}

	private bool isLevelComplete=false;
	private Action LevelComplete;
	public event Action OnLevelComplete{
		add{LevelComplete+=value;}
		remove{LevelComplete-=value;}
	}

	private bool isLevelStart=false;
	private Action LevelStart;
	public event Action OnLevelStart{
		add{LevelStart+=value;}
		remove{LevelStart-=value;}
	}

	private Action GameOver;
	public event Action OnGameOver{
		add{GameOver+=value;}
		remove{GameOver-=value;}
	}

	private bool hasInitPlayerData =false;
	public bool hasSkipLevel =false;

	public static GameDataManager GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name ="GameDataManager";
			instance = container.AddComponent(typeof(GameDataManager)) as GameDataManager;			
			DontDestroyOnLoad(instance);
		}
		return instance;
	}

	private void Awake(){
		if(!hasInitPlayerData){
			hasInitPlayerData = true;
			InitPlayerData();
		}
	}

	// Use this for initialization
	void Start () {
		AddListener();
	}

	private void InitPlayerData(){
		Score = 0;
		Level = 0;
		Life = 3;
		Coin = 0;
		HiScore = 0;
		IsInvulnerable = false;
		//Debug.Log("init player data...");
	}

	private void AddListener(){
		player.OnPlayerRevive+=PlayerRevive;
		player.OnPlayerInvulnerableChange+=OnPlayerInvulnerableChange;
		player.OnGotFireball+=OnGotFireball;
	}

	private void RemoveListener(){
		if(player!=null){
			player.OnPlayerRevive-=PlayerRevive;
			player.OnPlayerInvulnerableChange-=OnPlayerInvulnerableChange;
			player.OnGotFireball-=OnGotFireball;
		}
	}

	private void OnDestroy(){
		RemoveListener();
	}

	private void OnGotFireball(bool val){
		if(val){
			//Invoke("FireballTimer",45f);
			Invoke(Task.FireballTimer.ToString(),45f);
		}
	}

	private void FireballTimer(){
		player.IsGotFireball = false;
	}

	private void PlayerRevive(){
		if(null!= GameRestart){
			GameRestart();
		}
	}

	private void OnPlayerInvulnerableChange(bool val){
		if(val){
			//Invoke("InvulnerableTimer",15f);
			Invoke(Task.InvulnerableTimer.ToString(),15f);
		}
	}

	private void InvulnerableTimer(){
		player.IsInvulnerable = false;
	}

	public bool IsInvulnerable{
		set{ player.IsInvulnerable = value;}
		get{ return player.IsInvulnerable;}
	}

	public int HP{
		set{ player.HP = value;}
		get{ return player.HP;}
	}

	public int Coin{
		set{player.Coin=value;}
		get{ return player.Coin;}
	}

	public void UpdateLife( int val ){
		player.Life+=val;
	}

	public int Life{
		set{ player.Life = value;
			if(player.Life<0){
				if(null!=GameOver){
					GameOver();
				}
			}
		}
		get{return player.Life;}
	}

	public int UpdateLevel(){
		//Debug.Log("b4 level " + player.Level);
		player.Level++;
		return player.Level;
		//Debug.Log("after level " + player.Level);
	}

	public int Level{
		set{ player.Level = value;}
		get{ return player.Level;}
	}

	public void UpdateScore(int val){
		player.Score+=val;
		if(player.Score > player.HiScore){
			player.HiScore = player.Score;
			//Debug.Log("update hiscore...");
		}
	}

	public int Score{
		set{
			player.Score = value;
			//Debug.Log("check score: " + player.Score + " hiScore: " + player.HiScore );
			if(player.Score > player.HiScore){
				player.HiScore = player.Score;
				//Debug.Log("update hiscore...");
			}
		}
		get{ return player.Score;}
	}

	public int HiScore{
		set{player.HiScore = value;}
		get{return player.HiScore;}
	}

	public void UpdateHiScore(int val){
		player.HiScore+=val;
	}

	public bool IsLevelComplete{
		set{ isLevelComplete=value;
			if(null!=LevelComplete){
				LevelComplete();
			}
		}
		get{return isLevelComplete;}
	}

	public bool IsLevelStart{
		set{ isLevelStart=value;
			if(null!=LevelStart){
				LevelStart();
				//Debug.Log("dispatch level start event");
			}
		}		
		get{return isLevelStart;}
	}

	public float CurrentBossHP{
		get{ return currentBossHp;}
		set{ currentBossHp=value;
			if(null!=BossHpChange){
				BossHpChange();
			}

			if(currentBossHp<=0){
				if(null!=BossDied){
					BossDied();
				}
			}
		}
	}

	public bool IsShowBossHP{
		set{ 
			isShowBossHp =value;
			//Debug.Log("show boss hp data manager 1 isShowBossHp  " + isShowBossHp );
			if(isShowBossHp){
				//Debug.Log("show boss hp data manager 2 isShowBossHp  " + isShowBossHp );
				if(null!=ShowBossHp){
					ShowBossHp();
					//Debug.Log("show boss hp data manager 3");
				}
			}
		}
		get{return isShowBossHp;}
	}


	//reset level data
	public void ResetLevelData(){
		Score = 0;
		HP = 3;
		isLevelStart =false;
		isLevelComplete = false;
		IsInvulnerable = false;
		player.IsGotFireball = false;
		IsShowBossHP = false;
		if(player.IsDead){
			player.IsDead = false;
		}
		CancelInvoke(Task.InvulnerableTimer.ToString());
		CancelInvoke(Task.FireballTimer.ToString());
	}

	public void ResetGameData(){
		ResetLevelData();
		Score = 0;
		Level = 0;
		Life = 3;
		Coin = 0;
		IsInvulnerable = false;
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
