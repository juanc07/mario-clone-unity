using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FireballJumperController : MonoBehaviour {
	
	private Rigidbody body;
	private Vector3 startPosition;
	private Vector3 endPosition;
	public bool isActive {set;get;}
	private Transform fireballTransform;

	private float upForce = 22.5f;
	private float downForce = -3f;
	// Use this for initialization
	void Start () {
		fireballTransform = this.gameObject.transform;
		startPosition = this.gameObject.transform.position;
		endPosition = startPosition;
		endPosition.y += 2f;
		body = this.gameObject.GetComponent<Rigidbody>();
		//AddJumpForce();
		InvokeRandom();
	}
	
	private void AddJumpForce(){
		if(!isActive){
			if(body!=null){
				body.useGravity = true;
				body.isKinematic = false;
				body.AddForce(new Vector3(0,upForce,0),ForceMode.VelocityChange);
				//Debug.Log("add jump force to coin");
			}
			isActive = true;
		}
	}

	private void Update(){
		if(body!=null){
			body.AddForce(new Vector3(0,downForce,0),ForceMode.Force);
		}
		if( fireballTransform.position.y <= startPosition.y && isActive ){
			if(body!=null){
				isActive = false;
				body.useGravity = false;
				body.velocity = Vector3.zero;
				body.isKinematic = true;
				fireballTransform.position = startPosition;
				InvokeRandom();
			}
		}
	}

	private void InvokeRandom(){
		float randomTime = Random.Range(0.01f,0.5f);
		Invoke("AddJumpForce",randomTime);
	}
}
