using UnityEngine;
using System.Collections;

public class GroundCollideController : MonoBehaviour {
	private float offsetX=0.30f;
	private HeroController heroController;
	private SphereCollider sphereCollider;

	// Use this for initialization
	void Start () {
		heroController = this.gameObject.transform.parent.gameObject.GetComponent<HeroController>();
		sphereCollider = this.gameObject.GetComponent<SphereCollider>();
		heroController.OnHeroHitLevelObject += OnHeroHitLevelObject;
	}

	private void OnDestroy(){
		heroController.OnHeroHitLevelObject -= OnHeroHitLevelObject;
	}

	private void OnHeroHitLevelObject(LevelObjectTagger levelObject){
		if(levelObject!=null){
			if(levelObject.levelTag == LevelTag.Crate){
				sphereCollider.isTrigger = true;
			}else{
				sphereCollider.isTrigger = false;
			}
		}
	}

	private void Update(){
		if(heroController.isFacingRight){
			SwitchGrabCollider(0);
		}else if(heroController.isFacingLeft){
			SwitchGrabCollider(1);
		}
	}
	
	private void SwitchGrabCollider(int dir){
		Vector3 tempsphereColliderCenter = sphereCollider.center;
		if(dir==0){
			tempsphereColliderCenter.x=0;
		}else if(dir==1){
			tempsphereColliderCenter.x=-offsetX;
		}		
		sphereCollider.center = tempsphereColliderCenter;
	}
}
