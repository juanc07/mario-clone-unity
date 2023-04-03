using UnityEngine;
using System.Collections;

public class ObjectPositionController : MonoBehaviour {
	private bool isActivated =false;
	private GameDataManager gameDataManager;
	private Vector3 originalPosition;
	private Vector3 safePosition;
	private Rigidbody rigidBody;
	
	// Use this for initialization
	void Start () {
		gameDataManager = GameDataManager.GetInstance();
		AddEventListener();
		
		originalPosition = this.gameObject.transform.position;
		safePosition = originalPosition;
		safePosition.y += 1000f;

		rigidBody = this.gameObject.GetComponent<Rigidbody>();
	}
	
	private void AddEventListener(){
		gameDataManager.OnGameRestart+= OnGameRestart;
	}
	
	private void RemoveEventListener(){
		if(gameDataManager!=null){
			gameDataManager.OnGameRestart-= OnGameRestart;
		}
	}
	
	private void OnDestroy(){
		RemoveEventListener();
	}
	
	private void OnGameRestart(){
		isActivated = false;
		if(rigidBody!=null){
			rigidBody.isKinematic = true;
		}
		MoveToOriginalPosition();
		ActivateDeactivate(true);
	}
	
	// Update is called once per frame
	void Update () {
		if( this.gameObject.transform.position.y <=0 &&  !isActivated){
			DeactivateAndReposition();
		}
	}
	
	private void ActivateDeactivate(bool val){
		this.gameObject.SetActive(val);
	}
	
	public void DeactivateAndReposition(){
		isActivated = true;
		ActivateDeactivate(false);
		MoveToSafePosition();
	}
	
	private void MoveToSafePosition(){
		this.gameObject.transform.position = safePosition;
	}
	
	private void MoveToOriginalPosition(){
		this.gameObject.transform.position = originalPosition;
	}
}
