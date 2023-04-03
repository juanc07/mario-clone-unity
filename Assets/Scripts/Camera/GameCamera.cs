using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	
	private Transform target;
	public LevelManager levelManager;
	public float distance;
	
	public bool hasSmoothing=false;
	public bool smoothX=false;
	public bool smoothY=false;
	public bool smoothZ=false;
	
	public float smoothing=0.5f;
	
	public float restrictUpY = 22f;
	public float restrictDownY = 11f;
	
	public bool followX = true;
	public bool followY =true;
	public bool followZ =true;

	private GameDataManager gameDataManager;
	
	// Use this for initialization
	void Start () {
		gameDataManager = GameDataManager.GetInstance();
		AddEventListener();
	}

	private void AddEventListener(){
		gameDataManager.OnLevelStart+=OnLevelStart;
		gameDataManager.OnGameRestart+=OnGameRestart;
	}

	private void RemoveEventListener(){
		gameDataManager.OnLevelStart-=OnLevelStart;
		gameDataManager.OnGameRestart-=OnGameRestart;
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void OnLevelStart(){
		target = levelManager.heroInstance.transform;
		Vector3 tempPosition = this.gameObject.transform.position;
		tempPosition.x = target.position.x;
		tempPosition.y = target.position.y;
		this.gameObject.transform.position = tempPosition;
		//Debug.Log("Game Camera OnLevelStart target x" + target.position.x + " y " + target.position.y );
	}

	private void OnGameRestart(){
		target = levelManager.heroInstance.transform;
		Vector3 tempPosition = this.gameObject.transform.position;
		tempPosition.x = target.position.x;
		tempPosition.y = target.position.y;
		this.gameObject.transform.position = tempPosition;
		//Debug.Log("Game Camera OnGameRestart target x" + target.position.x + " y " + target.position.y);
	}


	// Update is called once per frame
	void Update (){
		if(target==null)return;
		//Debug.Log("Game Camera OnLevelStart target x" + target.position.x + " y " + target.position.y );
		//Debug.Log("Update Game Camera");
		//if(!gameDataManager.IsLevelStart)return;

		Vector3 tempPosition = transform.position;
		
		if(hasSmoothing){
			float currSmoothing = smoothing * Time.deltaTime;
			
			if(followX){
				if(smoothX){
					tempPosition.x = Mathf.Lerp(tempPosition.x, target.position.x, currSmoothing);
				}else{
					tempPosition.x = target.position.x;
				}
			}
			
			if(followY){
				if(smoothY){
					if(target.position.y >  restrictDownY ){
						tempPosition.y = Mathf.Lerp(tempPosition.y, target.position.y, currSmoothing);
					}
				}else{
					if(target.position.y >  restrictDownY){
						tempPosition.y = target.position.y;
					}
				}
			}
			
			if(followZ){
				if(smoothZ){
					tempPosition.z = Mathf.Lerp(tempPosition.z, target.position.z - distance, currSmoothing);
				}else{
					tempPosition.z = target.position.z -distance;
				}
			}
		}else{
			if(followZ){
				tempPosition.z = target.position.z -distance;
			}
			
			if(followY){
				//tempPosition.y = target.position.y;
				if(target.position.y >  restrictDownY){
					tempPosition.y = target.position.y;
				}
			}
			
			if(followX){
				tempPosition.x = target.position.x;
			}
		}
		transform.position = tempPosition;
	}
}
