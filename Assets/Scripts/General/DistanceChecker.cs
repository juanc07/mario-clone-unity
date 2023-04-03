using UnityEngine;
using System.Collections;

public class DistanceChecker : MonoBehaviour {
	private LevelManager levelManager;
	public GameObject playerHero{set;get;}
	public HeroController playerHeroController{set;get;}
	
	public float rangeToMove = 30f;
	public float distance{get;set;}
	public GameObject modelPrefab;
	
	//private BoxCollider boxCollider;
	private GameDataManager  gameDataManager;

	public bool isActive{set;get;}
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
			/*if(boxCollider!=null){
				boxCollider.enabled = true;
			}*/
			modelPrefab.gameObject.SetActive(true);
			isActive = true;
		}else{
			/*if(boxCollider!=null){
				boxCollider.enabled = false;
			}*/
			modelPrefab.gameObject.SetActive(false);
			isActive = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		CheckIfNeedsToHide();
	}
}
