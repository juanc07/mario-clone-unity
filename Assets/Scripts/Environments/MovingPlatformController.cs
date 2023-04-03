using UnityEngine;using System.Collections;

public class MovingPlatformController : MonoBehaviour {
	
	private Rigidbody movingPlatformRigidBody;
	private float fallDelay = 0.5f;
	public float downForce = -1f;

	public float moveForce = 1f;
	public float moveMaxForce = 4f;

	public bool isMovingRight{set;get;}
	public bool isMovingLeft{set;get;}

	public bool isMovingUp{set;get;}
	public bool isMovingDown{set;get;}

	public bool isHorizontal = true;

	public float targetY = 25f;
	private float startY;

	public float targetX = 25f;
	//private float startX;

	private DistanceChecker distanceChecker;
	// Use this for initialization
	void Start () {
		startY = this.gameObject.transform.position.y;
		//startX = this.gameObject.transform.position.x;

		movingPlatformRigidBody = this.gameObject.GetComponent<Rigidbody>();
		distanceChecker = this.gameObject.GetComponent<DistanceChecker>();
		//EnableDisableGravity(false);
		if(isHorizontal){
			movingPlatformRigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ; 
			MoveLeft();
		}else{
			movingPlatformRigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
			MoveUp();
		}
	}

	private void FixedUpdate(){
		if(movingPlatformRigidBody.useGravity){
			movingPlatformRigidBody.AddForce( new Vector3(0,downForce,0), ForceMode.VelocityChange );
		}
		
		if(distanceChecker.isActive){
			if(isHorizontal){
				if(isMovingLeft && !isMovingRight){
					movingPlatformRigidBody.AddForce( new Vector3(-moveForce,0,0), ForceMode.Force );
					//Debug.Log("platform moving left");
				}
				
				if(!isMovingLeft && isMovingRight){
					movingPlatformRigidBody.AddForce( new Vector3(moveForce,0,0), ForceMode.Force );
					//Debug.Log("platform moving right");
				}
			}else{
				if(!isMovingLeft && !isMovingRight && !isMovingDown && isMovingUp){
					movingPlatformRigidBody.AddForce( new Vector3(0,moveForce,0), ForceMode.Force );
					//Debug.Log("up this position " + this.gameObject.transform.position.y);
				}
				
				if(!isMovingLeft && !isMovingRight && isMovingDown && !isMovingUp){
					movingPlatformRigidBody.AddForce( new Vector3(0,-moveForce,0), ForceMode.Force );
					//Debug.Log("down this position " + this.gameObject.transform.position.y);
				}

				if(isMovingUp && this.gameObject.transform.position.y >= targetY){
					MoveDown();
				}

				if(isMovingDown && this.gameObject.transform.position.y <= startY){
					MoveUp();
				}
			}
		}else{
			movingPlatformRigidBody.velocity = Vector3.zero;
		}
		movingPlatformRigidBody.velocity = Vector3.ClampMagnitude(movingPlatformRigidBody.velocity,moveMaxForce);
	}

	private void OnCollisionEnter(Collision collision){
		LevelObjectTagger levelObjecttagger = collision.gameObject.GetComponent<LevelObjectTagger>();
		if(levelObjecttagger!=null){
			if(levelObjecttagger.levelTag == LevelTag.Ground){
				movingPlatformRigidBody.velocity = Vector3.zero;
				//Debug.Log("moving platform hit something!! " + collision.gameObject.name);
				if(isMovingLeft){
					MoveRight();
				}else{
					MoveLeft();
				}
			}
		}
	}

	private void MoveRight(){
		isMovingUp = false;
		isMovingDown = false;
		isMovingLeft = false;
		isMovingRight = true;
	}


	private void MoveLeft(){
		isMovingUp = false;
		isMovingDown = false;
		isMovingLeft = true;
		isMovingRight = false;
	}

	private void MoveUp(){
		isMovingUp = true;
		isMovingDown = false;
		isMovingLeft = false;
		isMovingRight = false;
	}

	private void MoveDown(){
		isMovingUp = false;
		isMovingDown = true;
		isMovingLeft = false;
		isMovingRight = false;
	}
	
	private void EnableDisableGravity(bool val){
		if(movingPlatformRigidBody!=null){
			movingPlatformRigidBody.useGravity = val;
			if(val){
				movingPlatformRigidBody.isKinematic = false;
			}else{
				movingPlatformRigidBody.isKinematic = true;
			}
		}
	}
	
	public void ActivateFall(){
		//Invoke("Fall",fallDelay);
		Invoke(Task.Fall.ToString(),fallDelay);
	}
	
	private void Fall(){
		EnableDisableGravity(true);
	}
}
