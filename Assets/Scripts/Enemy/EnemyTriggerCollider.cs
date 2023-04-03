using UnityEngine;
using System.Collections;

public class EnemyTriggerCollider : MonoBehaviour {

	private AIController aiController;

	// Use this for initialization
	void Start () {
		aiController = this.gameObject.transform.parent.gameObject.GetComponent<AIController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnTriggerEnter(Collider collider){
		LevelObjectTagger levelObjecttagger = collider.gameObject.GetComponent<LevelObjectTagger>();
		if(levelObjecttagger!=null){

			if(levelObjecttagger.levelTag == LevelTag.Spore){
				EnemyController  enemyController = levelObjecttagger.gameObject.transform.parent.gameObject.GetComponent<EnemyController>();
				if(enemyController!=null){
					if(enemyController.isAttacking){
						if(enemyController.currentAttackType == AttackType.Attack1){
							if(aiController!=null){
								aiController.HitByWeapon(levelObjecttagger);
							}
						}
					}
				}
			}

			if(levelObjecttagger.levelTag == LevelTag.EnemyBlocker){
				if(aiController!=null){
					aiController.CheckWhereToGo();
				}
			}
		}
	}

	private void OnTriggerStay(Collider collider){
		LevelObjectTagger levelObjecttagger = collider.gameObject.GetComponent<LevelObjectTagger>();
		if(levelObjecttagger!=null){
			
			if(levelObjecttagger.levelTag == LevelTag.Spore){
				EnemyController  enemyController = levelObjecttagger.gameObject.transform.parent.gameObject.GetComponent<EnemyController>();
				if(enemyController!=null){
					if(enemyController.isAttacking){
						if(enemyController.currentAttackType == AttackType.Attack1){
							if(aiController!=null){
								aiController.HitByWeapon(levelObjecttagger);
							}
						}
					}
				}
			}
		}
	}
}
