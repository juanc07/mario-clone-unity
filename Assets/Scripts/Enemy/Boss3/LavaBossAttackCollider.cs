using UnityEngine;
using System.Collections;

public class LavaBossAttackCollider : MonoBehaviour {
	
	public LavaBossAIController lavaBossAIController;	
	private LevelObjectTagger levelObjectTagger;
	// Use this for initialization
	void Start (){
	}
	
	private void OnDestroy(){
		RemoveEventListener();
	}
	
	private void AddEventListener(){
		if(lavaBossAIController!=null && lavaBossAIController.aiHeroController!=null){
			lavaBossAIController.aiHeroController.OnMidAttackComplete+=OnMidAttackComplete;
			lavaBossAIController.aiHeroController.OnAttackComplete+=OnAttackComplete;
		}
	}
	
	private void RemoveEventListener(){
		if(lavaBossAIController!=null && lavaBossAIController.aiHeroController!=null){
			lavaBossAIController.aiHeroController.OnMidAttackComplete-=OnMidAttackComplete;
			lavaBossAIController.aiHeroController.OnAttackComplete-=OnAttackComplete;
		}
	}
	
	private void OnMidAttackComplete(AttackType type){

	}
	
	private void OnAttackComplete(AttackType type){

	}
	
	private void OnTriggerEnter(Collider collider){
		levelObjectTagger = collider.gameObject.GetComponent<LevelObjectTagger>();
		if(levelObjectTagger==null){
			levelObjectTagger = collider.gameObject.transform.GetComponentInChildren<LevelObjectTagger>();
		}
		
		if(levelObjectTagger!=null){
			if(levelObjectTagger.levelTag == LevelTag.Hero || levelObjectTagger.levelTag == LevelTag.Mario){
				//lavaBossAIController.Smash();
			}else if(levelObjectTagger.levelTag == LevelTag.Boss){
				EnemyController enemyController = levelObjectTagger.gameObject.GetComponent<EnemyController>();
				if(enemyController!=null){
					if(enemyController.enemyType == EnemyType.BigMushroom){
						AIController aiController = levelObjectTagger.gameObject.GetComponent<AIController>();
						if(aiController!=null){
							//lavaBossAIController.Smash();
						}
					}
				}
			}
		}
	}
	
	private void OnTriggerStay(Collider collider){
		levelObjectTagger = collider.gameObject.GetComponent<LevelObjectTagger>();
		if(levelObjectTagger==null){
			levelObjectTagger = collider.gameObject.transform.GetComponentInChildren<LevelObjectTagger>();
		}
		
		if(levelObjectTagger!=null){
			if(levelObjectTagger.levelTag == LevelTag.Hero || levelObjectTagger.levelTag == LevelTag.Mario){
				MarioController marioController = levelObjectTagger.gameObject.GetComponent<MarioController>();
				if(marioController!=null && lavaBossAIController.activateDamage){
					lavaBossAIController.activateDamage = false;
					marioController.TakeDamage();
				}
				
				//lavaBossAIController.Smash();
			}else if(levelObjectTagger.levelTag == LevelTag.Boss){
				EnemyController enemyController = levelObjectTagger.gameObject.GetComponent<EnemyController>();
				if(enemyController!=null && lavaBossAIController.activateDamage ){
					if(enemyController.enemyType == EnemyType.BigMushroom){
						AIController aiController = levelObjectTagger.gameObject.GetComponent<AIController>();
						if(aiController!=null){
							lavaBossAIController.activateDamage = false;
							aiController.TakeDamage();
						}
					}
				}
			}else if(levelObjectTagger.levelTag == LevelTag.Enemy || levelObjectTagger.levelTag == LevelTag.CannonBullet){
				AIController aiController = levelObjectTagger.gameObject.GetComponent<AIController>();
				if(aiController!=null && lavaBossAIController.activateDamage){
					lavaBossAIController.activateDamage = false;
					aiController.InstantDeath();
				}
			}
		}
	}
}
