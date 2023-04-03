using UnityEngine;
using System.Collections;

public class IceBossController : EnemyController{
	public override void Start ()
	{
		base.Start ();
		gameDataManager = GameDataManager.GetInstance();
		gameDataManager.CurrentBossHP = originalHp;
		//Debug.Log("start BigMushroomController");
		//Invoke("ShowBossHp", 0.3f);
		//Invoke(Task.ShowBossHp.ToString(), 0.3f);
	}
	
	/*private void ShowBossHp(){
		gameDataManager.IsShowBossHP =true;
		AddEventListener();
	}*/

	/*public override void AddEventListener ()
	{
		base.AddEventListener ();
		this.OnHeroHit += OnBigMushroomHit;
	}
	
	public override void RemoveEventListener ()
	{
		base.RemoveEventListener ();
		this.OnHeroHit -= OnBigMushroomHit;
	}
	
	private void OnDestroy(){
		RemoveEventListener();
	}
	
	private void OnBigMushroomHit(){
		gameDataManager.CurrentBossHP = hp/originalHp;
	}*/

	public override void OnGameRestart ()
	{
		base.OnGameRestart ();
		gameDataManager.CurrentBossHP = hp/originalHp;
	}
	
	public override void OnLevelStart ()
	{
		base.OnLevelStart ();
		gameDataManager.CurrentBossHP = hp/originalHp;
	}


	public override void OnEnemyHit ()
	{
		base.OnEnemyHit ();
		gameDataManager.CurrentBossHP = hp/originalHp;
	}
	
	public override void ShowDeathParticle ()
	{
		base.ShowDeathParticle ();
		
		Vector3 newPosition =	this.gameObject.transform.position;
		newPosition.y += 1f;
		if(isMovingLeft){
			newPosition.x += 3f;
		}else{
			newPosition.x -= 3f;
		}
		
		Vector3 scale = new Vector3(7f,7f,7f);
		particleManager.CreateParticle(ParticleEffect.BossDeath,newPosition,scale);
		
		soundManager.PlaySfx(SFX.EnemyDied,1f);
		gameDataManager.UpdateScore(ScoreValue.ICE_BOSS);
	}
	
	
}