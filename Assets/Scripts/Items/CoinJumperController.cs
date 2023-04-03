using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class CoinJumperController : MonoBehaviour {

	private Rigidbody body;
	private Vector3 startPosition;
	private Vector3 endPosition;
	public bool isActive {set;get;}

	private GameDataManager gameDataManager;

	// Use this for initialization
	void Start () {
		gameDataManager = GameDataManager.GetInstance();
		startPosition = this.gameObject.transform.position;
		endPosition = startPosition;
		endPosition.y += 2f;
		AddJumpForce();
	}

	public void AddJumpForce(){
		isActive = true;
		body = this.gameObject.GetComponent<Rigidbody>();
		if(body!=null){
			body.useGravity = true;
			body.isKinematic = false;
			body.AddForce(new Vector3(0,20f,0),ForceMode.VelocityChange);
			//Debug.Log("add jump force to coin");
		}
	}

	private void Update(){
		if( this.gameObject.transform.position.y >= endPosition.y && isActive ){
			if(body!=null){
				body.useGravity = false;
				body.velocity = Vector3.zero;
				body.isKinematic = true;
				isActive = false;

				gameDataManager.Coin++;
				gameDataManager.UpdateScore(ScoreValue.COIN);

				this.gameObject.transform.position = startPosition;
				this.gameObject.SetActive(false);
			}
		}
	}
}
