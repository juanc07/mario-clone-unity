using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundDistanceChecker : MonoBehaviour {
	private LevelManager levelManager;
	public GameObject playerHero{set;get;}
	public HeroController playerHeroController{set;get;}
	
	public float rangeToShowMesh = 30f;
	public float distance{get;set;}

	private GameDataManager  gameDataManager;
	private List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
	// Use this for initialization
	void Start () {
		gameDataManager = GameDataManager.GetInstance();
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
		meshRenderers.Clear();
		CacheAllMeshRenderer(this.gameObject);
	}
	
	private void OnGameRestart(){
		InitPlayerReference();
		meshRenderers.Clear();
		CacheAllMeshRenderer(this.gameObject);
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
		
		if(distance <= rangeToShowMesh){
			ActiveDeactivateMeshGround(true);
		}else{
			ActiveDeactivateMeshGround(false);
		}
	}

	private void CacheAllMeshRenderer(GameObject parent){
		int count =  parent.gameObject.transform.childCount;
		MeshRenderer meshRenderer = null;
		for(int index=0;index<count;index++){
			Transform child = parent.gameObject.transform.GetChild(index);
			meshRenderer = child.gameObject.GetComponent<MeshRenderer>();
			if(meshRenderer!=null){
				meshRenderers.Add(meshRenderer);
			}else{
				ActiveDeactivateMeshGround(child.gameObject);
				if(meshRenderer!=null){
					meshRenderers.Add(meshRenderer);
				}
			}
		}
	}

	private void ActiveDeactivateMeshGround(bool val){
		int count =  meshRenderers.Count;
		for(int index=0;index<count;index++){
			if(meshRenderers[index]!=null){
				meshRenderers[index].enabled = val;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		CheckIfNeedsToHide();
	}
}
