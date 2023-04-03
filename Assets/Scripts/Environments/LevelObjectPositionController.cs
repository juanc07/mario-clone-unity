using UnityEngine;
using System.Collections;

public class LevelObjectPositionController : MonoBehaviour {
	private GameDataManager gameDataManager;
	private Vector3 originalPosition;
	private Vector3 safePosition;
	private Transform container;
	private Quaternion originalRotation;
	
	// Use this for initialization
	void Start () {
		gameDataManager = GameDataManager.GetInstance();
		AddEventListener();
		
		originalPosition = this.gameObject.transform.position;
		safePosition = originalPosition;
		safePosition.y -= 100f;

		originalRotation = this.gameObject.transform.rotation;
		container = this.gameObject.transform.parent;
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
		MoveToOriginalPosition();
	}
	
	private void Deactivate(bool val){
		this.gameObject.SetActive(val);
	}	

	
	public void MoveToSafePosition(){
		this.gameObject.transform.parent = container;
		this.gameObject.transform.position = safePosition;
		Deactivate(false);
	}
	
	public void MoveToOriginalPosition(){
		this.gameObject.transform.rotation = originalRotation;
		this.gameObject.transform.position = originalPosition;
		Deactivate(true);
	}
}
