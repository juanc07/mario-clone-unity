using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class FixedPosition : MonoBehaviour {

	public bool isFixedX=false;
	public bool isFixedY=false;
	public bool isFixedZ=true;

	public bool cacheIntialX=false;
	public bool cacheIntialY=false;
	public bool cacheIntialZ=false;

	public float originX=0;
	public float originY=0;
	public float originZ=0;

	//private HeroController heroController;
	private LevelManager levelManager;
	public GameObject playerHero{set;get;}

	private bool isOn = true;
	private LevelObjectTagger levelObjectTagger;

	// Use this for initialization
	void Start (){
		//heroController = this.gameObject.GetComponent<HeroController>();
		levelObjectTagger = this.gameObject.GetComponent<LevelObjectTagger>();
		if(levelObjectTagger!=null){
			if(levelObjectTagger.levelTag == LevelTag.Ground 
			   || levelObjectTagger.levelTag == LevelTag.UnBreakableBrick
			   || levelObjectTagger.levelTag == LevelTag.Exit
			   || levelObjectTagger.levelTag == LevelTag.Entrance
			   ){
				isOn = false;
			}
		}


		levelManager = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
		if(levelManager.heroInstance!=null){
			playerHero = levelManager.heroInstance;
		}

		//if(heroController!=null && Application.isPlaying){
		//	this.enabled =false;
		//}

		if(cacheIntialX){
			originX = this.gameObject.transform.position.x;
		}

		if(cacheIntialY){
			originY = this.gameObject.transform.position.y;
		}

		if(cacheIntialZ){
			originZ = this.gameObject.transform.position.z;
		}
	}
	
	// Update is called once per frame
	void Update (){
		if(isOn){
			Vector3 tempPosition = this.gameObject.transform.position;
			
			if(isFixedX){
				tempPosition.x =originX;
			}
			
			if(isFixedY){
				tempPosition.y =originY;
			}
			
			if(isFixedZ){
				tempPosition.z =originZ;
			}
			
			this.gameObject.transform.position = tempPosition;
		}
	}
}
