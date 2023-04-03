using UnityEngine;
using System.Collections;

public class LevelCompletePopupController : MonoBehaviour {

	public GameObject levelPopup;

	public UILabel coinLabel;
	public UILabel lifeLabel;
	public UILabel scoreLabel;
	public UILabel hiScoreLabel;
	public UILabel timeleftLabel;
	public UILabel hpLabel;

	private GameDataManager gameDataManager;
	private GameTimer gameTimer;

	private int endScore;
	private int timeScore;
	private int coinScore;
	private int lifeScore;
	private int hpScore;

	private MyScore myScore;
	private GoTweenChain chain;
	public bool isDoneScoreAnimation{set;get;}

	// Use this for initialization
	void Start () {
		isDoneScoreAnimation = false;
		gameDataManager = GameDataManager.GetInstance();
		gameTimer = GameObject.FindObjectOfType(typeof(GameTimer)) as GameTimer;

		//init size of level complete popup 
		Vector3 tempLocalScale = new Vector3(0,0,0);
		levelPopup.gameObject.transform.localScale = tempLocalScale;

		//init total score data
		myScore = new MyScore();
		myScore.score = 0;
		myScore.coin = gameDataManager.player.Coin;
		myScore.timerLeft = (int)gameTimer.totalSeconds;
		myScore.life = gameDataManager.Life;
		myScore.hp = gameDataManager.HP;

		//compute total scores
		endScore = gameDataManager.Score;
		lifeScore = (gameDataManager.Life * 200) + endScore;
		coinScore = (gameDataManager.player.Coin * 20) + lifeScore;
		hpScore = (gameDataManager.HP * 100) + coinScore;
		timeScore = ((int)gameTimer.totalSeconds * 10) + hpScore;

		UpdateLevelCompleteInfo();
	}
	
	private void UpdateLevelCompleteInfo(){
		coinLabel.text = "x " + gameDataManager.player.Coin.ToString("000");
		lifeLabel.text = "x " + gameDataManager.Life.ToString("000");
		hpLabel.text = "x " + gameDataManager.HP.ToString("000");
		scoreLabel.text = "Score\n0000";
		hiScoreLabel.text = "HiScore\n" + gameDataManager.HiScore.ToString("0000");
		timeleftLabel.text = "Time Left\n " + gameTimer.timerDisplay;

		ScaleTweenProperty levelPopupScaleProperty = new ScaleTweenProperty(new Vector3(1f,1f,1f));
		GoTween levelPopupTween = new GoTween( levelPopup.gameObject.transform,1f, new GoTweenConfig().addTweenProperty(levelPopupScaleProperty).setEaseType(GoEaseType.ElasticOut));

		GoTween scoreTween = new GoTween(myScore,2f, new GoTweenConfig().intProp( "score",endScore));

		GoTween lifeTween = new GoTween(myScore,0.3f, new GoTweenConfig().intProp( "life",0));
		GoTween lifeScoreTween = new GoTween(myScore,0.5f, new GoTweenConfig().intProp( "score",lifeScore));

		GoTween coinTween = new GoTween(myScore,0.5f, new GoTweenConfig().intProp( "coin",0));
		GoTween coinScoreTween = new GoTween(myScore,0.5f, new GoTweenConfig().intProp( "score",coinScore));

		GoTween hpTween = new GoTween(myScore,0.3f, new GoTweenConfig().intProp( "hp",0));
		GoTween hpScoreTween = new GoTween(myScore,0.5f, new GoTweenConfig().intProp( "score",hpScore));

		GoTween timeTween = new GoTween(myScore,1.5f, new GoTweenConfig().intProp( "timerLeft",0));
		GoTween timeScoreTween = new GoTween(myScore,0.5f, new GoTweenConfig().intProp( "score",timeScore),ScoreAnimationComplete);



		chain = new GoTweenChain();
		chain.append(levelPopupTween)
				.appendDelay(0.5f).append(scoreTween)
				.appendDelay(0.5f).append(lifeTween).append(lifeScoreTween)
				.appendDelay(0.5f).append(coinTween).append(coinScoreTween)
				.appendDelay(0.5f).append(hpTween).append(hpScoreTween)
				.appendDelay(0.5f).append(timeTween).append(timeScoreTween);
		chain.play();
	}

	public void SkipTotalScore(){
		chain.complete();
	}

	private void ScoreAnimationComplete(AbstractGoTween abstractGoTween){
		if(timeScore > gameDataManager.HiScore){
			gameDataManager.HiScore = timeScore;
			if(hiScoreLabel!=null){
				hiScoreLabel.text = "New HiScore\n" + gameDataManager.HiScore.ToString("0000");
			}
		}else{
			hiScoreLabel.text = "HiScore\n" + gameDataManager.HiScore.ToString("0000");
		}

		isDoneScoreAnimation = true;
		chain.complete();
		chain.destroy();
		chain =null;
	}

	private void Update(){
		scoreLabel.text = string.Format("Score\n{0}",myScore.score.ToString("0000"));
		lifeLabel.text = string.Format("x {0}",myScore.life.ToString("000"));
		hpLabel.text = string.Format("x {0}",myScore.hp.ToString("000"));
		coinLabel.text = string.Format("x {0}",myScore.coin.ToString("000"));			
		timeleftLabel.text = string.Format("Time Left\n{0}",gameTimer.ConvertSecToMin(myScore.timerLeft));
	}

	public class MyScore{
		public int score{set;get;}
		public int coin{set;get;}
		public int life{set;get;}
		public int hp{set;get;}
		public int timerLeft{set;get;}
		public int hiScore{set;get;}
	}
}
