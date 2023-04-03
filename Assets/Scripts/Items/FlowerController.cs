using UnityEngine;
using System.Collections;

public class FlowerController : HeroController,IEventListener{
	
	private GameDataManager gameDataManager;
	private SoundManager soundManager;
	private ParticleManager particleManager;
	private bool isCollected =false;

	private Vector3 targetPosition;
	private Vector3 originalPosition;
	private float smooth =0.90f;
	private float targetValue =1.5f;
	private bool isReachUpTarget = false;

	public override void Start(){
		base.Start();
		gameDataManager = GameDataManager.GetInstance();
		soundManager = SoundManager.GetInstance();
		
		isDestroyBrick =false;
		originalPosition = this.gameObject.transform.position;
		targetPosition = new Vector3(originalPosition.x,originalPosition.y + targetValue,originalPosition.z);
		AddEventListener();
	}
	
	private void OnDestroy(){
		RemoveEventListener();
	}
	
	public void AddEventListener(){
		this.OnHeroHitLevelObject+=OnFlowerHitLevelObject;
	}
	
	public void RemoveEventListener(){
		this.OnHeroHitLevelObject-=OnFlowerHitLevelObject;
	}
	
	public override void Update (){
		base.Update ();
		AnimateFlower();
		//Bounce(1f);
		//Jump();
	}

	private void AnimateFlower(){
		if(!isReachUpTarget){
			this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, targetPosition,smooth * Time.deltaTime );
			if(this.gameObject.transform.position.y >=  (originalPosition.y + targetValue) - 0.25){
				isReachUpTarget = true;
				ApplyGravity = true;
				//Debug.Log("end animation");
			}
		}
	}
	
	private void OnFlowerHitLevelObject(LevelObjectTagger levelObjectTagger){
		if(levelObjectTagger!=null){
			/*if(levelObjectTagger.levelTag != LevelTag.Ground ){
				Debug.Log( "flower hit " +  levelObjectTagger.levelTag );
			}*/

			if(levelObjectTagger.levelTag == LevelTag.Hero 
			   ||	levelObjectTagger.levelTag == LevelTag.Mario 
			   ||	levelObjectTagger.levelTag == LevelTag.HeroFeet 
			   ){
				//Debug.Log("Flower hit player");
				CollectFlower();
			}
		}
	}
	
	public void CollectFlower(){
		if(this!=null){
			if(gameDataManager!=null){
				if(gameDataManager.player!=null){
					if(!isCollected){
						isCollected =true;
						gameDataManager.player.IsGotFireball = true;
						soundManager.PlaySfx(SFX.CollectPowerup);
						gameDataManager.UpdateScore(ScoreValue.FLOWER);
						Destroy(this.gameObject);
					}
				}
			}
		}
	}
	
	public override void LimitSpeed (){
		base.LimitSpeed ();
	}	
}