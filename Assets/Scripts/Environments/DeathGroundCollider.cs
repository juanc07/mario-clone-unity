using UnityEngine;
using System.Collections;

public class DeathGroundCollider : MonoBehaviour {

	private GameDataManager gameDataManager;
	private LevelManager levelManager;
	private MarioController marioController;
	// Use this for initialization
	void Start () {
		gameDataManager = GameDataManager.GetInstance();
		levelManager = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
		AddEventListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEventListener(){
		gameDataManager.OnLevelStart+=OnLevelStart;
		gameDataManager.OnGameRestart+=OnGameRestart;
	}

	private void RemoveEventListener(){
		gameDataManager.OnLevelStart-=OnLevelStart;
		gameDataManager.OnGameRestart-=OnGameRestart;
	}

	private void OnLevelStart(){
		marioController = levelManager.heroInstance.gameObject.GetComponent<MarioController>();
	}

	private void OnGameRestart(){
		marioController = levelManager.heroInstance.gameObject.GetComponent<MarioController>();
	}


	
	private void OnTriggerEnter( Collider col ){
		if(marioController==null) return;
		//Debug.Log("Ground collider hit " + col.gameObject.tag);
		LevelObjectTagger levelObjTagger = col.gameObject.GetComponent<LevelObjectTagger>();

		if(levelObjTagger!=null){
			if(levelObjTagger.levelTag == LevelTag.Hero && !marioController.IsDead && !gameDataManager.player.IsDead){
				marioController.Kill();
			}else if(levelObjTagger.levelTag == LevelTag.HeroFeet && !marioController.IsDead && !gameDataManager.player.IsDead){
				marioController.Kill();
			}else if(levelObjTagger.levelTag == LevelTag.Enemy){
				marioController.Kill();
			}
		}

		if(col.gameObject.tag =="Mushroom"){
			Destroy(col.gameObject);
		}
	}
}
