using UnityEngine;
using System.Collections;

public class LavaBossController : EnemyController{

	public override void OnLevelStart ()
	{
		base.OnLevelStart ();
		gameDataManager.CurrentBossHP = hp/originalHp;
	}
	
	public override void OnGameRestart ()
	{
		base.OnGameRestart ();
		gameDataManager.CurrentBossHP = hp/originalHp;
	}

	public override void OnEnemyHit ()
	{
		base.OnEnemyHit ();
		gameDataManager.CurrentBossHP = hp/originalHp;
	}

	public override void OnEnemyDied ()
	{
		base.OnEnemyDied ();
		Invoke("Explosion1", 0.5f);
		Invoke("Explosion2", 1f);
		Invoke("Explosion3", 1.5f);
		Invoke("Explosion4", 2f);
		Invoke("Explosion5", 2.5f);
	}

	public override void ShowDeathParticle ()
	{
		base.ShowDeathParticle ();
		EnableDisableBody(true);
	}

	private void Explosion1(){
		soundManager.PlaySfx(SFX.CrateExplosion,1f);
		Vector3 newPosition =this.gameObject.transform.position;
		Vector3 scale = new Vector3(1f,1f,1f);
		particleManager.CreateParticle(ParticleEffect.LavaBossExplosion,newPosition,scale);
	}

	private void Explosion2(){
		soundManager.PlaySfx(SFX.CrateExplosion,1f);
		Vector3 newPosition =this.gameObject.transform.position;
		newPosition.x += 7f;
		newPosition.y += 7f;
		Vector3 scale = new Vector3(1f,1f,1f);
		particleManager.CreateParticle(ParticleEffect.LavaBossExplosion,newPosition,scale);
	}

	private void Explosion3(){
		soundManager.PlaySfx(SFX.CrateExplosion,1f);
		Vector3 newPosition =this.gameObject.transform.position;
		newPosition.x -= 4f;
		newPosition.y -= 0.5f;
		Vector3 scale = new Vector3(1f,1f,1f);
		particleManager.CreateParticle(ParticleEffect.LavaBossExplosion,newPosition,scale);
	}

	private void Explosion4(){
		soundManager.PlaySfx(SFX.CrateExplosion,1f);
		Vector3 newPosition =this.gameObject.transform.position;
		newPosition.x -= 7f;
		newPosition.y += 7f;
		Vector3 scale = new Vector3(1f,1f,1f);
		particleManager.CreateParticle(ParticleEffect.LavaBossExplosion,newPosition,scale);
	}


	private void Explosion5(){
		soundManager.PlaySfx(SFX.CrateExplosion,1f);
		Vector3 newPosition =this.gameObject.transform.position;
		newPosition.x += 7f;
		newPosition.y -= 0.5f;
		Vector3 scale = new Vector3(1f,1f,1f);
		particleManager.CreateParticle(ParticleEffect.LavaBossExplosion,newPosition,scale);

		soundManager.PlaySfx(SFX.BossDied);
		gameDataManager.UpdateScore(ScoreValue.LAVA_BOSS);
		gameDataManager.CurrentBossHP = 0;
	}
}