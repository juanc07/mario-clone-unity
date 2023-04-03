using UnityEngine;
using System.Collections;

public class CannonBulletController : EnemyController{
	
	public override void ShowDeathParticle ()
	{
		base.ShowDeathParticle ();
		
		Vector3 newPosition =	this.gameObject.transform.position;
		newPosition.y += 1f;
		Vector3 scale = new Vector3(4f,4f,4f);
		particleManager.CreateParticle(ParticleEffect.Explosion1,newPosition,scale);

		soundManager.PlaySfx(SFX.CannonBullet,1f);
		gameDataManager.UpdateScore(ScoreValue.CANNON_BULLET);
	}
}