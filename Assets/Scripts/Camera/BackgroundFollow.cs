using UnityEngine;
using System.Collections;

public class BackgroundFollow : MonoBehaviour {
	
	//public Transform target;
	public LevelManager levelManager;
	private HeroController heroController;
	public float distance;
	
	public bool hasSmoothing=false;
	public bool smoothX=false;	
	public float smoothing=0.5f;
	private GameDataManager gameDataManager;
	
	
	// Use this for initialization
	void Start (){
		//gameDataManager = GameDataManager.GetInstance();
		//gameDataManager.OnLevelStart+=OnLevelStart;
	}

	private void OnDestroy(){
		//gameDataManager.OnLevelStart-=OnLevelStart;
		//RemoveEventListener();
	}

	private void OnLevelStart(){
		//AddEventListener();
	}

	private void AddEventListener(){
		heroController = levelManager.heroInstance.GetComponent<HeroController>();
		heroController.OnHeroMoveRight+=OnHeroMoveRight;
		heroController.OnHeroMoveLeft+=OnHeroMoveLeft;
		Debug.Log( "check hero local Transform  " + levelManager.heroInstance.transform.localPosition.x );
		Debug.Log( "check hero Transform  " + levelManager.heroInstance.transform.position.x );
	}

	private void RemoveEventListener(){
		heroController.OnHeroMoveRight-=OnHeroMoveRight;
		heroController.OnHeroMoveLeft-=OnHeroMoveLeft;
	}

	private void OnHeroMoveRight(){
		MoveBackground(0);
	}

	private void OnHeroMoveLeft(){
		MoveBackground(1);
	}

	private void MoveBackground(int dir){
		Vector3 tempPosition = transform.position;
		if(dir == 0){
			tempPosition.x +=0.1f;
		}else{
			tempPosition.x -=0.1f;
		}

		transform.localPosition = tempPosition;
		//Debug.Log ("background following hero");
	}
	
	/*// Update is called once per frame
	void Update (){
		Vector3 tempPosition = transform.position;
		
		if(hasSmoothing){
			float currSmoothing = smoothing * Time.deltaTime;
			if(smoothX){
				tempPosition.x = Mathf.Lerp(tempPosition.x, target.position.x, currSmoothing);
			}else{
				tempPosition.x = target.position.x;
			}
		}else{
			tempPosition.x = target.localPosition.x;
		}
		transform.localPosition = tempPosition;
		Debug.Log ("background following hero");
	}*/
}
