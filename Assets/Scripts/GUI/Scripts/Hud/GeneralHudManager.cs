using UnityEngine;
using System.Collections;

public class GeneralHudManager : MonoBehaviour {

	private GameDataManager gameDataManager;
	public UILabel coinLabel;
	public UILabel heartLabel;
	public UILabel levelLabel;
	public UILabel scoreLabel;
	public UILabel timeLabel;
	public UILabel timesupLabel;
	public UILabel lifeLabel;

	private GameTimer gameTimer;
	public GameObject bossHpBar;
	private UIProgressBar bossHpProgressBar;

	// Use this for initialization
	void Start (){
		gameDataManager = GameDataManager.GetInstance();
		gameTimer = GameObject.FindObjectOfType(typeof(GameTimer)) as GameTimer;
		bossHpProgressBar = bossHpBar.gameObject.GetComponent<UIProgressBar>();
		addListener();
		gameDataManager.ResetLevelData();
		//gameDataManager.Coin = gameDataManager.player.Coin;
		//gameDataManager.Score = 0;
		//Invoke("DelayInit", 0.1f);
		Invoke(Task.DelayInit.ToString(), 0.1f);
	}

	private void DelayInit(){
		UpdateAll();
		//ShowHideBossHPBar(false);
		//UpdateLife();
		//UpdateCoin();
		//UpdateLevel();
		//ShowHideTimesupLabel(0);
	}

	private void UpdateAll(){
		ShowHideBossHPBar(false);
		UpdateLife();
		UpdateCoin();
		UpdateLevel();
		UpdateScore();
		UpdateHp();
		UpdateLevel();
		ShowHideTimesupLabel(0);
	}

	private void OnDestroy(){
		removeListener();
	}

	private void addListener(){
		if(gameDataManager!=null){
			gameDataManager.OnGameRestart+=OnGameRestart;
			gameDataManager.OnLevelStart+=OnLevelStart;
			gameDataManager.player.OnCoinUpdate+=UpdateCoin;
			gameDataManager.player.OnHpUpdate+=OnHpUpdate;
			gameDataManager.player.OnPlayerRevive+=OnHpUpdate;
			gameDataManager.player.OnScoreUpdate+=OnScoreUpdate;
			gameDataManager.player.OnLevelUpdate+=OnLevelUpdate;
			gameTimer.OnTimeOut+=OnTimeOut;
			gameDataManager.OnShowBossHp+=OnShowBossHp;
			gameDataManager.OnBossHpChange+= OnBossHpChange;
			gameDataManager.OnBossDied+=OnBossDied;
			gameDataManager.player.OnLifeUpdate+=OnLifeUpdate;
			gameDataManager.OnGameOver+=OnGameOver;
			gameTimer.OnTimeTick+=OnTimeTick;
		}
	}

	private void removeListener(){
		if(gameDataManager!=null){
			if(gameDataManager.player!=null){
				gameDataManager.OnGameRestart-=OnGameRestart;
				gameDataManager.OnLevelStart-=OnLevelStart;
				gameDataManager.player.OnCoinUpdate-=UpdateCoin;
				gameDataManager.player.OnHpUpdate-=OnHpUpdate;
				gameDataManager.player.OnPlayerRevive-=OnHpUpdate;
				gameDataManager.player.OnScoreUpdate-=OnScoreUpdate;
				gameDataManager.player.OnLevelUpdate-=OnLevelUpdate;
				gameTimer.OnTimeOut-=OnTimeOut;
				gameDataManager.OnShowBossHp-=OnShowBossHp;
				gameDataManager.OnBossHpChange-=OnBossHpChange;
				gameDataManager.OnBossDied+=OnBossDied;
				gameDataManager.player.OnLifeUpdate-=OnLifeUpdate;
				gameDataManager.OnGameOver-=OnGameOver;
				gameTimer.OnTimeTick-=OnTimeTick;
			}
		}
	}

	private void OnGameRestart(){
		UpdateAll();
	}

	private void OnLevelStart(){
		UpdateAll();
	}

	private void OnBossHpChange(){
		UpdateBossHpProgressBar(gameDataManager.CurrentBossHP);
	}

	private void UpdateBossHpProgressBar(float val){
		bossHpProgressBar.value = val;
	}

	private void OnTimeOut(){
		ShowHideTimesupLabel(1f);
	}

	private void OnLevelUpdate(){
		UpdateLevel();
	}

	private void UpdateLevel(){
		levelLabel.text = ConvertLevelToWorld();
	}

	private void OnLifeUpdate(){
		UpdateLife();
		//Debug.Log(" check life " + gameDataManager.Life);
	}

	private string ConvertLevelToWorld(){
		int level = gameDataManager.Level;
		string worldLevel="";
		int baseLevel = 11;

		level++;

		if(level <=11){
			worldLevel = string.Format("world 1-{0}",level);
		}else if(level >=12 && level <23){
			level-=baseLevel;
			worldLevel = string.Format("world 2-{0}",level);
		}else if(level >=23 && level <34){
			level-=baseLevel*2;
			worldLevel = string.Format("world 3-{0}",level);
		}else{
			worldLevel = string.Format("world ?-{0}",level);
		}
		return worldLevel;
	}

	private void UpdateHp(){
		heartLabel.text = string.Format("x {0}",gameDataManager.player.HP.ToString("D2"));
	}

	private void OnHpUpdate(){
		UpdateHp();
	}

	private void UpdateCoin(){
		coinLabel.text = string.Format("x {0}",gameDataManager.player.Coin.ToString("D3"));
	}

	private void UpdateScore(){
		scoreLabel.text = string.Format("Score: {0}",gameDataManager.player.Score.ToString("D4"));
	}

	private void UpdateLife(){
		if(gameDataManager.player.Life >=0){
			lifeLabel.text = string.Format("x {0}",gameDataManager.player.Life.ToString("D2"));
		}else{
			lifeLabel.text = string.Format("x {0}","00");
		}
	}

	private void OnScoreUpdate(){
		UpdateScore();
	}

	private void ShowHideTimesupLabel(float val){
		timesupLabel.alpha = val;
	}

	private void OnShowBossHp(){
		//Debug.Log("on show boss hp");
		ShowHideBossHPBar(true);
	}

	private void OnBossDied(){
		ShowHideBossHPBar(false);
	}

	private void OnGameOver(){
		//Debug.Log("Game Over show game over screen here!");
	}

	private void ShowHideBossHPBar(bool val){
		if(bossHpBar!=null){
			bossHpBar.gameObject.SetActive(val);
		}
	}

	private void OnTimeTick(){
		if(timeLabel!=null){
			timeLabel.text = string.Format("Timer:{0}",gameTimer.timerDisplay);
		}
	}

	/*private void Update(){
		if(timeLabel!=null){
			timeLabel.text = string.Format("Timer:{0}",gameTimer.timerDisplay);
		}
	}*/
}
