using UnityEngine;
using System.Collections;

public class CannonBulletAnimationController : CharacterAnimationController {
	
	public override void PlayHit(){
		base.PlayHit();

	}
	
	public override void PlayDeath(){
		base.PlayDeath();

	}
	
	public override void PlayIdle(){
		base.PlayIdle();

	}
	
	public override void PlayWalk(){
		base.PlayWalk();
	}
	
	public override void PlayRun(){
		base.PlayRun();
	}
	
	public override void PlayJump(){
		base.PlayJump();
	}
	
	public override void PlayFalling(){
		base.PlayFalling();
	}
	
	public override void PlayFalling2(){
		base.PlayFalling2();
		if(!modelAnimation.IsPlaying(Animations.jump.ToString())){
			if(modelAnimation.GetClip(Animations.falling2.ToString()) != null){
				modelAnimation.Play(Animations.falling2.ToString());
			}
		}
	}
}
