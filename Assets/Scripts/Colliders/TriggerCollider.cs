using UnityEngine;
using System.Collections;

public class TriggerCollider : MonoBehaviour {
	
	private GameDataManager gameDataManager;
	private SoundManager soundManager;
	private ParticleManager particleManager;
	private MarioController marioController;

	private EnemyController  enemyController;
	// Use this for initialization
	void Start () {
		gameDataManager = GameDataManager.GetInstance();
		particleManager = ParticleManager.GetInstance();
		soundManager = SoundManager.GetInstance();

		marioController =  this.gameObject.transform.parent.gameObject.GetComponent<MarioController>();
	}	

	private void OnTriggerEnter(Collider col){
		LevelObjectTagger levelObject = col.gameObject.GetComponent<LevelObjectTagger>();
		if(levelObject!=null){
			if(levelObject.levelTag == LevelTag.Exit){
				if(!gameDataManager.IsLevelComplete && !gameDataManager.player.IsDead){
					//soundManager.PlaySfx(SFX.FinishLevel);
					gameDataManager.IsLevelComplete = true;				
				}
			}


			if(levelObject.levelTag == LevelTag.Coin){
				Vector3 newPosition =	this.gameObject.transform.position;
				//newPosition.y += 1f;
				Vector3 scale = new Vector3(0.5f,0.5f,0.5f);
				particleManager.CreateParticle(ParticleEffect.CollectCoin,newPosition,scale);

				soundManager.PlaySfx(SFX.CoinSfx,1f);
				gameDataManager.Coin++;
				gameDataManager.UpdateScore(ScoreValue.COIN);

				CoinController coinController = levelObject.gameObject.GetComponent<CoinController>();
				coinController.Collect();
				//Destroy(col.gameObject);
			}

			if(levelObject.levelTag == LevelTag.Spore){
				enemyController = levelObject.gameObject.transform.parent.gameObject.GetComponent<EnemyController>();
				if(enemyController!=null){
					if(enemyController.isAttacking){
						if(enemyController.currentAttackType == AttackType.Attack1){
							//if(!gameDataManager.player.IsInvulnerable){
								marioController.TakeDamage();
							//}
						}
					}
				}
			}

			if(levelObject.levelTag == LevelTag.PlantEnemy){
				marioController.TakeDamage();
			}

			if(levelObject.levelTag == LevelTag.FireBallEnemy){
				marioController.KillMario();
			}

			if(levelObject.levelTag == LevelTag.LavaBossAttackCollider){
				enemyController = levelObject.gameObject.transform.parent.gameObject.GetComponent<EnemyController>();
				if(enemyController!=null){
					if(enemyController.isAttacking){
						if(enemyController.currentAttackType == AttackType.Attack1){
							marioController.TakeDamage();
						}
					}
				}
			}
		}
	}

	private void OnTriggerStay(Collider col){
		if(col ==null ) return;

		LevelObjectTagger levelObject = col.gameObject.GetComponent<LevelObjectTagger>();
		if(levelObject!=null){
			if(levelObject.levelTag == LevelTag.Spore){
				enemyController = levelObject.gameObject.transform.parent.gameObject.GetComponent<EnemyController>();
				if(enemyController!=null){
					if(enemyController.isAttacking){
						if(enemyController.currentAttackType == AttackType.Attack1){
							//if(!gameDataManager.player.IsInvulnerable){
								marioController.TakeDamage();
							//}
						}
					}
				}
			}else if(levelObject.levelTag == LevelTag.PlantEnemy){
				marioController.TakeDamage();
			}else if(levelObject.levelTag == LevelTag.Enemy /*|| levelObject.levelTag == LevelTag.Boss*/){
				enemyController = levelObject.gameObject.transform.parent.gameObject.GetComponent<EnemyController>();
				if(enemyController!=null){
					if(!enemyController.IsDead){
						marioController.TakeDamage();
					}
				}
			}
		}
	}

	private void OnTriggerExit(Collider col){
		LevelObjectTagger levelObjecttagger = col.gameObject.GetComponent<LevelObjectTagger>();
		if(levelObjecttagger!=null){
			if(levelObjecttagger.levelTag == LevelTag.StaticPlatform){
				StaticPlatformController staticPlatformController = levelObjecttagger.gameObject.GetComponent<StaticPlatformController>();
				if(staticPlatformController!=null){
					staticPlatformController.boxCollider.isTrigger = false;
				}
			}
		}
	}

}
