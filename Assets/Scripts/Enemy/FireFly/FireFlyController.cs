using UnityEngine;
using System.Collections;

public class FireFlyController : EnemyController{

	public override void OnLevelStart ()
	{
		base.OnLevelStart ();
		ApplyGravity = false;
	}

	public override void OnGameRestart ()
	{
		base.OnGameRestart ();
		ApplyGravity = false;
	}

	public override void OnEnemyHit ()
	{
		base.OnEnemyHit ();
		if(IsDead){
			ApplyGravity = true;
		}
	}

	public override void ShowDeathParticle ()
	{
		base.ShowDeathParticle ();
		
		Vector3 newPosition =	this.gameObject.transform.position;
		newPosition.y += 1f;
		Vector3 scale = new Vector3(4f,4f,4f);
		particleManager.CreateParticle(ParticleEffect.EnemyDied,newPosition,scale);
		
		soundManager.PlaySfx(SFX.EnemyDied,1f);
		gameDataManager.UpdateScore(ScoreValue.FIRE_FLY);
	}
}