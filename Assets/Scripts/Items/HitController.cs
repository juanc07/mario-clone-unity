using UnityEngine;
using System.Collections;
using System;

public abstract class HitController : MonoBehaviour
{
	
	public GameObject obj;	
	public HeroController marioController{ set; get; }
	//public GameObject currentHitObject;
	public HeroController aiHeroController{ set; get; }
	private LevelManager levelManager;

	public LevelObjectTagger levelObjectTagger{ set; get; }
	public SoundManager soundManager{ set; get; }
	public ParticleManager particleManager{ set; get; }
	public GameDataManager gameDataManager{ set; get; }

	// Use this for initialization
	public virtual void Start(){
		gameDataManager = GameDataManager.GetInstance ();
		soundManager = SoundManager.GetInstance ();
		particleManager = ParticleManager.GetInstance ();
		levelManager = GameObject.FindObjectOfType (typeof(LevelManager))as LevelManager;
		aiHeroController = this.gameObject.transform.parent.gameObject.GetComponent<HeroController> ();
		//Debug.Log("check hitController for mario Controller " + marioController);
		AddEventListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEventListener (){
		gameDataManager.OnGameRestart+=OnGameRestart;
		gameDataManager.OnLevelStart+=OnLevelStart;

		if(aiHeroController!=null){
			aiHeroController.OnHeroDied += OnHeroDied;
		}
	}

	private void RemoveEventListener (){
		if(aiHeroController!=null){
			aiHeroController.OnHeroDied -= OnHeroDied;
			gameDataManager.OnGameRestart-=OnGameRestart;
			gameDataManager.OnLevelStart-=OnLevelStart;
		}
	}

	private void OnLevelStart(){
		marioController = levelManager.heroInstance.gameObject.GetComponent<HeroController> ();
		levelObjectTagger = null;
	}

	private void OnGameRestart(){
		marioController = levelManager.heroInstance.gameObject.GetComponent<HeroController> ();
		levelObjectTagger = null;
	}

	private void OnHeroDied(){
		levelObjectTagger = null;
	}	

	private void GetLevelTag (GameObject hitObject){
		if (hitObject.GetComponent<LevelObjectTagger> () != null){
			levelObjectTagger = hitObject.GetComponent<LevelObjectTagger>();
			Debug.Log("hit controller got levelObjectTagger: "+ levelObjectTagger.levelTag.ToString());
		}
	}

	/*public virtual void OnTriggerEnter (Collider col){
		GetHitTagger (col.gameObject);
	}*/

	public virtual void OnCollisionEnter(Collision collision){
		GetHitTagger (collision.gameObject);
	}

	private void GetHitTagger (GameObject hitObject){
		if (aiHeroController == null)return;
		if (aiHeroController.IsDead)return;

		//Debug.Log("HitController OnTriggerEnter");		
		if (hitObject.GetComponent<HitController> () != null){
			HeroController collidedHeroController = hitObject.GetComponent<HitController> ().obj.gameObject.GetComponent<HeroController> () as HeroController;
			if (collidedHeroController != null){
				if (aiHeroController.id.Equals (collidedHeroController.id, StringComparison.Ordinal)){
					Debug.Log("this object hit himself ");
				}else{
					GetLevelTag (hitObject);
				}
			}
		}else{
			GetLevelTag (hitObject);
		}
	}

	/*public virtual void OnTriggerExit (Collider col){
		//levelObjectTagger = null;
	}*/

	/*public virtual void OnTriggerStay (Collider col){
		levelObjectTagger = col.gameObject.GetComponent<LevelObjectTagger>();
		if(levelObjectTagger!=null){
			if(levelObjectTagger.levelTag == LevelTag.Mario || levelObjectTagger.levelTag == LevelTag.Hero){
				GetHitTagger (col.gameObject);
			}
		}
	}*/
}
