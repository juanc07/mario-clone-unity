using UnityEngine;
using System.Collections;

public class FireFlyAnimationController : CharacterAnimationController {	
	public override void PlayDeath(){
		base.PlayDeath();
		if(!modelAnimation.IsPlaying(Animations.death.ToString())){
			modelAnimation.Play(Animations.death.ToString());
		}
	}
	
	public override void PlayIdle(){
		base.PlayIdle();
		modelAnimation.Play(Animations.idle.ToString());
	}
}
