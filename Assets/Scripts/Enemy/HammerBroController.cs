using UnityEngine;
using System.Collections;

public class HammerBroController : EnemyController{

	public override void ShowDeathParticle (){
		base.ShowDeathParticle ();
		Vector3 newPosition =	this.gameObject.transform.position;
		newPosition.y += 1f;
		Vector3 scale = new Vector3(5f,5f,5f);
		particleManager.CreateParticle(ParticleEffect.EnemyDied,newPosition,scale);
		
		soundManager.PlaySfx(SFX.EnemyDied,1f);
		gameDataManager.UpdateScore(ScoreValue.HAMMER_BRO);
	}
}