using UnityEngine;using System.Collections;

public class FallingPlatformController : MonoBased {

	private Rigidbody fallingPlatformRigidBody;
	private float fallDelay = 0.5f;
	public float downForce = -1f;

	public float moveForce = 1f;
	public float moveMaxForce = 1f;

	private GameDataManager gameDataManager;
	private ObjectPositionController objectPositionController;
	private bool hasFall = false;

	private SoundManager soundManager;

	public override void Start (){
		gameDataManager = GameDataManager.GetInstance();		
		soundManager = SoundManager.GetInstance();
		fallingPlatformRigidBody = this.gameObject.GetComponent<Rigidbody>();
		objectPositionController = this.gameObject.GetComponent<ObjectPositionController>();
		EnableDisableGravity(false);
		base.Start ();
	}

	public override void OnDestroy ()
	{
		base.OnDestroy ();
	}

	public override void AddEventListner ()
	{
		base.AddEventListner ();
		gameDataManager.OnGameRestart+=OnGameRestart;
		gameDataManager.OnLevelStart+=OnLevelStart;
	}


	public override void RemoveEventListner ()
	{
		base.RemoveEventListner ();
		gameDataManager.OnGameRestart-=OnGameRestart;
		gameDataManager.OnLevelStart-=OnLevelStart;
	}

	private void OnLevelStart(){
		Reset();
	}

	private void OnGameRestart(){
		Reset();
	}

	private void Reset(){
		hasFall = false;
		EnableDisableGravity(false);
	}

	private void FixedUpdate(){
		if(fallingPlatformRigidBody.useGravity){
			fallingPlatformRigidBody.AddForce( new Vector3(0,downForce,0), ForceMode.VelocityChange );
		}
	}
	
	private void EnableDisableGravity(bool val){
		fallingPlatformRigidBody.useGravity = val;
		if(val){
			fallingPlatformRigidBody.isKinematic = false;
		}else{
			fallingPlatformRigidBody.isKinematic = true;
		}
	}

	public void ActivateFall(){
		Invoke(Task.Fall.ToString(),fallDelay);
	}

	private void Fall(){
		if(!hasFall){
			soundManager.PlaySfx(SFX.FallingEnemy,1f);
			hasFall = true;
			EnableDisableGravity(true);
		}
	}
}
