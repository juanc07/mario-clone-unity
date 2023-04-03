using UnityEngine;
using System.Collections;

public class UnBreakableDistanceChecker : MonoBehaviour {
	private LevelManager levelManager;
	public GameObject playerHero{set;get;}
	public HeroController playerHeroController{set;get;}
	
	public float rangeToMove = 40f;
	public float distance{get;set;}
	
	//private BoxCollider boxCollider;
	private GameDataManager  gameDataManager;
	// Use this for initialization
	void Start () {
		gameDataManager = GameDataManager.GetInstance();
		//boxCollider = this.gameObject.GetComponent<BoxCollider>();
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
		InitPlayerReference();
	}
	
	private void OnGameRestart(){
		InitPlayerReference();
	}
	
	
	private void InitPlayerReference(){
		if(levelManager.heroInstance!=null){
			playerHero = levelManager.heroInstance;
			playerHeroController = playerHero.gameObject.GetComponent<HeroController>();
		}
	}
	
	public void CheckIfNeedsToHide(){
		if(playerHero==null)return;
		
		distance = playerHero.gameObject.transform.position.x - this.gameObject.transform.position.x;
		if(distance<=0){
			distance*=-1;
		}
		
		if(distance <= rangeToMove){
			//boxCollider.enabled = true;
			ShowHideChildren(true);
		}else{
			//boxCollider.enabled = false;
			ShowHideChildren(false);
		}
	}

	private void ShowHideChildren(bool val){
		int count = this.gameObject.transform.childCount;
		for(int index=0;index<count;index++){
			Transform child = this.gameObject.transform.GetChild(index);
			child.gameObject.SetActive(val);
		}
	}
	
	// Update is called once per frame
	void Update () {
		CheckIfNeedsToHide();
	}
}
