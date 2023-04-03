using UnityEngine;
using System.Collections;

public class LavaBossAnimationController : CharacterAnimationController {	
	public override void PlayDeath(){
		base.PlayDeath();
		if(!modelAnimation.IsPlaying(Animations.death.ToString())){
			modelAnimation.Play(Animations.death.ToString());
		}
	}

	public override void PlayHit(){
		base.PlayHit();
		if(!modelAnimation.IsPlaying(Animations.hit.ToString())){
			modelAnimation.Play(Animations.hit.ToString());
		}
	}
	
	public override void PlayIdle(){
		base.PlayIdle();
		modelAnimation.Play(Animations.idle.ToString());
	}

	public override void Attack1 ()
	{
		base.Attack1 ();
		if(modelAnimation.GetClip(Animations.attack01.ToString()) != null){
			if(!modelAnimation.IsPlaying(Animations.attack01.ToString())){
				modelAnimation.Play(Animations.attack01.ToString());
			}
		}
	}
	
	public override void Attack2 ()
	{
		base.Attack2 ();
		if(modelAnimation.GetClip(Animations.attack02.ToString()) != null){
			if(!modelAnimation.IsPlaying(Animations.attack02.ToString())){
				modelAnimation.Play(Animations.attack02.ToString());
			}
		}
	}
}
