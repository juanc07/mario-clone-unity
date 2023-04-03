using UnityEngine;
using System.Collections;

public class LavaBossVitalCollider : MonoBehaviour {

	public LavaBossAIController lavaAIController;
	private LevelObjectTagger levelObjectTagger;
	public AIController aiController;

	// Use this for initialization
	void Start (){
	}
	
	private void OnTriggerEnter(Collider collider){
		levelObjectTagger = collider.gameObject.GetComponent<LevelObjectTagger>();
		if(levelObjectTagger==null){
			levelObjectTagger = collider.gameObject.transform.GetComponentInChildren<LevelObjectTagger>();
		}
		
		if(levelObjectTagger!=null){
			if(levelObjectTagger.levelTag == LevelTag.Crate){
				CrateColliderController crateColliderController = levelObjectTagger.gameObject.GetComponent<CrateColliderController>();
				if(crateColliderController!=null){
					if(crateColliderController.isThrown){
						if(lavaAIController!=null){
							crateColliderController.ExplodeCrate();
							if(!lavaAIController.aiHeroController.IsDead){
								lavaAIController.TakeDamage();
							}
						}else{
							Debug.Log("LavaBossAiController is null!");
						}
					}else{
						crateColliderController.ExplodeCrate();
					}
				}
			}

			if(levelObjectTagger.levelTag == LevelTag.Hero || levelObjectTagger.levelTag == LevelTag.Mario){
				MarioController marioController = levelObjectTagger.gameObject.GetComponent<MarioController>();
				if(marioController!=null){
					if(aiController!=null){
						if(!aiController.aiHeroController.isAttacking && !aiController.aiHeroController.IsDead ){
							marioController.TakeDamage();
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
			if(levelObjectTagger.levelTag == LevelTag.Crate){
				CrateColliderController crateColliderController = levelObjectTagger.gameObject.GetComponent<CrateColliderController>();
				if(crateColliderController!=null){
					crateColliderController.ExplodeCrate();
				}
			}

			if(levelObjectTagger.levelTag == LevelTag.Hero || levelObjectTagger.levelTag == LevelTag.Mario){
				MarioController marioController = levelObjectTagger.gameObject.GetComponent<MarioController>();
				if(marioController!=null){
					if(aiController!=null){
						if(!aiController.aiHeroController.isAttacking && !aiController.aiHeroController.IsDead ){
							marioController.TakeDamage();
						}
					}
				}
			}
		}
	}
}
