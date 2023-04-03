using UnityEngine;
using System.Collections;

public class MushroomController : HeroController,IEventListener{

	private GameDataManager gameDataManager;
	private SoundManager soundManager;
	private ParticleManager particleManager;
	private bool isCollected =false;
	public bool isOneup = false;

	//new
	private Vector3 targetPosition;
	private Vector3 originalPosition;
	private float smooth =0.90f;
	private float targetValue =1.75f;
	private bool isReachUpTarget = false;

	private Transform mushroomTransform;

	public override void Start(){
		base.Start();
		gameDataManager = GameDataManager.GetInstance();
		soundManager = SoundManager.GetInstance();

		mushroomTransform = this.gameObject.transform;

		isDestroyBrick =false;
		originalPosition = this.gameObject.transform.position;
		originalPosition.y-=1f;
		this.gameObject.transform.position = originalPosition;

		targetPosition = new Vector3(originalPosition.x,originalPosition.y + targetValue,originalPosition.z);

		AddEventListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	public void AddEventListener(){
		this.OnHeroHit += OnMushroomHit;
		this.OnHeroHitLevelObject+=OnMushroomHitLevelObject;
	}

	public void RemoveEventListener(){
		this.OnHeroHit -= OnMushroomHit;
		this.OnHeroHitLevelObject-=OnMushroomHitLevelObject;
	}

	public override void Update ()
	{
		base.Update ();
		AnimateMushroom();
		if(mushroomTransform.position.y <= 0){
			Destroy(this.gameObject);
		}
	}

	private void AnimateMushroom(){
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

	private void OnMushroomHitLevelObject(LevelObjectTagger levelObjectTagger){
		if(levelObjectTagger!=null){
			//Debug.Log("Mushroom hit " + levelObjectTagger.levelTag );
			if(levelObjectTagger.levelTag == LevelTag.Hero 
			   ||	levelObjectTagger.levelTag == LevelTag.Mario 
			   ||	levelObjectTagger.levelTag == LevelTag.HeroFeet 
			   ){
				//Debug.Log("Mushroom hit player");
				CollectMushroom();
			}

			if(levelObjectTagger.levelTag == LevelTag.Ground){
				if(isHitLeftSide || isHitRightSide){
					SwitchDirection();
				}
			}else if(levelObjectTagger.levelTag == LevelTag.Brick || levelObjectTagger.levelTag == LevelTag.UnBreakableBrick){
				if(isHitLeftSide || isHitRightSide){
					SwitchDirection();
				}
			}else{
				if(isHitLeftSide || isHitRightSide){
					SwitchDirection();
				}
			}
		}
	}

	public void CollectMushroom(){
		if(!isCollected){
			isCollected =true;
			if(isOneup){
				soundManager.PlaySfx(SFX.OneUp);
				gameDataManager.player.Life++;
				gameDataManager.UpdateScore(ScoreValue.MUSHROOM_LIFE);
			}else{
				soundManager.PlaySfx(SFX.ExtraLife);
				gameDataManager.player.HP++;
				gameDataManager.UpdateScore(ScoreValue.MUSHROOM_HEART);
			}
			Destroy(this.gameObject);
		}
	}
	
	public override void LimitSpeed ()
	{
		base.LimitSpeed ();
	}
}