using UnityEngine;
using System.Collections;

public class CrateColliderController : EventListener {

	private GameDataManager gameDataManager;
	private ParticleManager particleManager;
	private SoundManager soundManager;

	private LevelObjectTagger levelObjectTagger;
	private LevelObjectPositionController levelObjectPositionController;
	public bool isThrown{set;get;}

	private Rigidbody body; 
	private LevelObjectTagger crateLevelObjectTagger;
	private Transform parentTransform;
	private Transform crateTransform;

	public override void Start ()
	{
		gameDataManager = GameDataManager.GetInstance();
		soundManager = SoundManager.GetInstance();
		particleManager = ParticleManager.GetInstance();
		levelObjectPositionController = this.gameObject.GetComponent<LevelObjectPositionController>();
		body = this.gameObject.GetComponent<Rigidbody>();
		crateTransform = this.gameObject.transform;
		parentTransform = this.gameObject.transform.parent;

		crateLevelObjectTagger = this.gameObject.GetComponent<LevelObjectTagger>();
		base.Start ();
	}

	public override void AddEventListner ()
	{
		base.AddEventListner ();
		gameDataManager.OnGameRestart += OnGameRestart;
		gameDataManager.OnLevelStart += OnLevelStart;
	}

	public override void RemoveEventListner ()
	{
		base.RemoveEventListner ();
		gameDataManager.OnGameRestart -= OnGameRestart;
		gameDataManager.OnLevelStart -= OnLevelStart;
	}

	private void OnGameRestart(){
		this.gameObject.transform.parent = parentTransform;
		isThrown = false;
		if(!body.isKinematic){
			body.velocity = Vector3.zero;
		}
	}

	private void OnLevelStart(){
		this.gameObject.transform.parent = parentTransform;
		isThrown =false;
	}

	private void Update(){
		body.AddForce(new Vector3(0,-3f,0f),ForceMode.Force);
		if(crateTransform.position.y <= 0){
			TurnOnOffGravity(false);
		}
	}

	private void OnCollisionEnter(Collision collision){
		levelObjectTagger = collision.gameObject.transform.GetComponentInChildren<LevelObjectTagger>();
		if(levelObjectTagger!=null){
			//Debug.Log("crate hit something check level tag " + levelObjectTagger.levelTag);
			if(levelObjectTagger.levelTag == LevelTag.Boss || levelObjectTagger.levelTag == LevelTag.Enemy){
				if(isThrown){
					AIController aiController = levelObjectTagger.gameObject.GetComponent<AIController>();
					if(aiController!=null){
						aiController.HitByWeapon(crateLevelObjectTagger);
						ExplodeCrate();
					}
					//ContactPoint contact = collision.contacts[0];
				}
			}else if(levelObjectTagger.levelTag == LevelTag.Ground 
			         || levelObjectTagger.levelTag == LevelTag.Crate
			         || levelObjectTagger.levelTag == LevelTag.Well
			         ){
				isThrown = false;
			}
		}else{
			isThrown = false;
		}
	}

	//public void ExplodeCrate( ContactPoint contact ){
	public void ExplodeCrate(){
		//Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		//Vector3 pos = contact.point;
		Vector3 pos = this.gameObject.transform.position;
		particleManager.CreateParticle( ParticleEffect.CrateExplosion,pos,new Vector3(3f,3f,3f));
		levelObjectPositionController.MoveToSafePosition();
		soundManager.PlaySfx(SFX.CrateExplosion,1f);
		//Destroy(gameObject);
	}

	public void TurnOnOffGravity(bool val){
		body.useGravity = val;

		this.gameObject.transform.parent = parentTransform;
		isThrown = false;

		if(val){
			body.isKinematic = true;
			levelObjectPositionController.MoveToOriginalPosition();
			body.isKinematic = false;
			body.constraints = RigidbodyConstraints.None;
			body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
			body.velocity = Vector3.zero;
		}else{
			body.isKinematic = true;
			body.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
			levelObjectPositionController.MoveToSafePosition();
		}
	}
}
