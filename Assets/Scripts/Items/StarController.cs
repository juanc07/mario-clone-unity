using UnityEngine;
using System.Collections;

public class StarController : HeroController,IEventListener{
	
	private GameDataManager gameDataManager;
	private SoundManager soundManager;
	private ParticleManager particleManager;
	private bool isCollected =false;

	private Vector3 targetPosition;
	private Vector3 originalPosition;
	private float smooth =0.90f;
	private float targetValue =1.75f;
	private bool isReachUpTarget = false;
	
	public override void Start(){
		base.Start();
		gameDataManager = GameDataManager.GetInstance();
		soundManager = SoundManager.GetInstance();

		isDestroyBrick =false;

		originalPosition = this.gameObject.transform.position;
		originalPosition.y-=0.5f;
		this.gameObject.transform.position = originalPosition;
		targetPosition = new Vector3(originalPosition.x,originalPosition.y + targetValue,originalPosition.z);
		AddEventListener();
	}
	
	private void OnDestroy(){
		RemoveEventListener();
	}
	
	public void AddEventListener(){
		this.OnHeroHit += OnMushroomHit;
		this.OnHeroHitLevelObject+=OnStarHitLevelObject;
	}
	
	public void RemoveEventListener(){
		this.OnHeroHit -= OnMushroomHit;
		this.OnHeroHitLevelObject-=OnStarHitLevelObject;
	}
	
	public override void Update ()
	{
		base.Update ();
		AnimateStar();
		//Bounce(1f);
		//Jump();
	}

	private void AnimateStar(){
		if(!isReachUpTarget){
			this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, targetPosition,smooth * Time.deltaTime );
			if(this.gameObject.transform.position.y >=  (originalPosition.y + targetValue) - 0.5f){
				isReachUpTarget = true;
				ApplyGravity = true;
				Bounce2(5f);
				InitDirection();
			}
		}
	}
	
	private void InitDirection(){
		float random = Random.Range(0,1f);
		//Debug.Log("random "+ random);
		if(random > 0.5f){
			TurnRight();
		}else{
			TurnLeft();
		}
	}
	
	public void TurnLeft(){		
		speed = 0;
		isRightBtnPress =false;
		isLeftBtnPress =true;	
		isHitLeftSide = false;
		isHitRightSide = false;
	}
	
	public void TurnRight(){
		speed = 0;
		isRightBtnPress =true;
		isLeftBtnPress =false;
		isHitLeftSide =false;
		isHitRightSide =false;
	}
	
	private void SwitchDirection(){
		if(isFacingLeft){
			TurnRight();
		}else{
			TurnLeft();
		}
	}
	
	private void OnMushroomHit(){
		Debug.Log("");
	}
	
	private void OnStarHitLevelObject(LevelObjectTagger levelObjectTagger){
		if(levelObjectTagger!=null){
			if(levelObjectTagger.levelTag == LevelTag.Hero 
			   ||	levelObjectTagger.levelTag == LevelTag.Mario 
			   ||	levelObjectTagger.levelTag == LevelTag.HeroFeet 
			   ){
				//Debug.Log("Mushroom hit player");
				CollectStar();
			}else{
				Bounce2(40f);
				if(isHitLeftSide || isHitRightSide){
					SwitchDirection();
				}
			}
		}
	}
	
	public void CollectStar(){
		if(!isCollected){
			isCollected =true;
			soundManager.PlaySfx(SFX.CollectPowerup);
			gameDataManager.player.IsInvulnerable = true;
			gameDataManager.UpdateScore(ScoreValue.STAR);
			Destroy(this.gameObject);
		}
	}
	
	public override void LimitSpeed ()
	{
		base.LimitSpeed ();
	}
	

}