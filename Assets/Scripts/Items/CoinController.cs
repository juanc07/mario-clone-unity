using UnityEngine;
using System.Collections;

public class CoinController : MonoBehaviour {
	private LevelManager levelManager;
	public GameObject playerHero{set;get;}
	public HeroController playerHeroController{set;get;}

	public float rangeToMove = 30f;
	public float distance{get;set;}
	public GameObject coinPrefab;

	private BoxCollider boxCollider;
	private GameDataManager  gameDataManager;

	private bool isCollected =false;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private bool hasCachePosition = false;

	// Use this for initialization
	void Start () {
		gameDataManager = GameDataManager.GetInstance();
		boxCollider = this.gameObject.GetComponent<BoxCollider>();
		levelManager = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;		 

		AddEventListener();
		InitPlayerReference();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}
	
	public virtual void AddEventListener(){
		if(gameDataManager!=null){
			gameDataManager.OnLevelStart+=OnLevelStart;
			gameDataManager.OnGameRestart+=OnGameRestart;
		}
	}
	
	public virtual void RemoveEventListener(){
		if(gameDataManager!=null){
			gameDataManager.OnLevelStart-=OnLevelStart;
			gameDataManager.OnGameRestart-=OnGameRestart;
		}
	}

	private void OnLevelStart(){
		CachePosition();
		ResetStatus();
		InitPlayerReference();
	}

	private void OnGameRestart(){
		CachePosition();
		ResetStatus();
		InitPlayerReference();
	}

	private void CachePosition(){
		if(!hasCachePosition){
			hasCachePosition = true;
			startPosition = this.gameObject.transform.position;
			endPosition = this.gameObject.transform.position;
			endPosition.y = this.gameObject.transform.position.y + 10000f;
		}
	}

	private void ResetStatus(){
		if(isCollected){
			isCollected = false;
			this.gameObject.transform.position = startPosition;
		}
	}


	private void InitPlayerReference(){
		if(levelManager.heroInstance!=null){
			playerHero = levelManager.heroInstance;
			playerHeroController = playerHero.gameObject.GetComponent<HeroController>();
		}
	}

	public void CheckIfNeedsToHide(){
		if(playerHero==null || isCollected)return;

		distance = playerHero.gameObject.transform.position.x - this.gameObject.transform.position.x;
		if(distance<=0){
			distance*=-1;
		}
		
		if(distance <= rangeToMove){
			boxCollider.enabled = true;
			coinPrefab.gameObject.SetActive(true);
		}else{
			boxCollider.enabled = false;
			coinPrefab.gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		CheckIfNeedsToHide();
	}

	public void Collect(){
		if(!isCollected){
			isCollected = true;
			this.gameObject.transform.position = endPosition;
		}
	}
}
