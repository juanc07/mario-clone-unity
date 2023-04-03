using UnityEngine;
using System.Collections;

public class IceBossAttackCollider : MonoBehaviour {

	private IceBossAIController iceBossAIController;
	private CapsuleCollider attackCollider;
	private float originalRadius = 2.2f;
	private float attackRadius = 4f;

	// Use this for initialization
	void Start () {
		//iceBossAIController = gameObject.transform.parent.gameObject.GetComponent<IceBossAIController>();
		attackCollider = gameObject.GetComponent<CapsuleCollider>();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	public void InitIceBossAIController(IceBossAIController aiController){
		iceBossAIController = aiController;
		AddEventListener();
	}

	private void AddEventListener(){
		if(iceBossAIController!=null && iceBossAIController.aiHeroController!=null){
			iceBossAIController.aiHeroController.OnMidAttackComplete+=OnMidAttackComplete;
			iceBossAIController.aiHeroController.OnAttackComplete+=OnAttackComplete;
		}
	}

	private void RemoveEventListener(){
		if(iceBossAIController!=null && iceBossAIController.aiHeroController!=null){
			iceBossAIController.aiHeroController.OnMidAttackComplete-=OnMidAttackComplete;
			iceBossAIController.aiHeroController.OnAttackComplete-=OnAttackComplete;
		}
	}

	private void OnMidAttackComplete(AttackType type){
		if(type == AttackType.Attack1){
			attackCollider.radius = attackRadius;
		}
	}

	private void OnAttackComplete(AttackType type){
		if(type == AttackType.Attack1){
			attackCollider.radius = originalRadius;
		}
	}

	private void Update(){
		Vector3 tempCenter = attackCollider.center;
		if(iceBossAIController.aiHeroController.isFacingLeft){
			tempCenter.x = -2f;
		}else{
			tempCenter.x = 2f;
		}

		attackCollider.center = tempCenter;
	}
	
	private void OnTriggerEnter(Collider collider){
		LevelObjectTagger levelObjectTagger = collider.gameObject.GetComponent<LevelObjectTagger>();
		if(levelObjectTagger==null){
			levelObjectTagger = collider.gameObject.transform.GetComponentInChildren<LevelObjectTagger>();
		}

		if(levelObjectTagger!=null){
			if(levelObjectTagger.levelTag == LevelTag.Hero || levelObjectTagger.levelTag == LevelTag.Mario){
				iceBossAIController.Smash();
			}else if(levelObjectTagger.levelTag == LevelTag.Boss){
				EnemyController enemyController = levelObjectTagger.gameObject.GetComponent<EnemyController>();
				if(enemyController!=null){
					if(enemyController.enemyType == EnemyType.BigMushroom){
						AIController aiController = levelObjectTagger.gameObject.GetComponent<AIController>();
						if(aiController!=null){
							iceBossAIController.Smash();
						}
					}
				}
			}
		}
	}

	private void OnTriggerStay(Collider collider){
		LevelObjectTagger levelObjectTagger = collider.gameObject.GetComponent<LevelObjectTagger>();
		if(levelObjectTagger==null){
			levelObjectTagger = collider.gameObject.transform.GetComponentInChildren<LevelObjectTagger>();
		}
		
		if(levelObjectTagger!=null){
			if(levelObjectTagger.levelTag == LevelTag.Hero || levelObjectTagger.levelTag == LevelTag.Mario){
				MarioController marioController = levelObjectTagger.gameObject.GetComponent<MarioController>();
				if(marioController!=null && iceBossAIController.activateDamage){
					iceBossAIController.activateDamage = false;
					marioController.TakeDamage();
				}

				iceBossAIController.Smash();
			}else if(levelObjectTagger.levelTag == LevelTag.Boss){
				EnemyController enemyController = levelObjectTagger.gameObject.GetComponent<EnemyController>();
				if(enemyController!=null && iceBossAIController.activateDamage ){
					if(enemyController.enemyType == EnemyType.BigMushroom){
						AIController aiController = levelObjectTagger.gameObject.GetComponent<AIController>();
						if(aiController!=null){
							iceBossAIController.activateDamage = false;
							aiController.TakeDamage();
						}
					}
				}
			}else if(levelObjectTagger.levelTag == LevelTag.Enemy || levelObjectTagger.levelTag == LevelTag.CannonBullet){
				AIController aiController = levelObjectTagger.gameObject.GetComponent<AIController>();
				if(aiController!=null && iceBossAIController.activateDamage){
					iceBossAIController.activateDamage = false;
					aiController.InstantDeath();
				}
			}
		}
	}
}
