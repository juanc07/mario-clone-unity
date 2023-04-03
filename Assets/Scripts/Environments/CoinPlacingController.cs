using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent(typeof(FixedPosition))]
public class CoinPlacingController : MonoBehaviour {
	
	public GameObject coinPrefab;
	public int brickCount;
	public float brickSpacing=1.1f;
	private int brickCounter;
	
	public bool isClear = false;
	public bool isGenerate = false;
	public bool isVertical=false;
	
	void Awake () {
		if(Application.isPlaying){
			isClear = false;
			isGenerate =false;
		}
	}
	
	// Use this for initialization
	void Start () {
		if(Application.isPlaying){
			isClear = false;
			isGenerate =false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(isClear){
			ClearBrick();
		}
		
		if(isGenerate){
			GenerateBrick();
		}
	}
	
	private void GenerateBrick(){
		ClearBrick();
		for(int index=0;index<brickCount;index++){
			GameObject unbreakableBrick = Instantiate( coinPrefab ) as GameObject;
			unbreakableBrick.gameObject.transform.parent = this.gameObject.transform;
			Vector3 tempPosition = unbreakableBrick.gameObject.transform.position;
			if(isVertical){
				tempPosition.x =  this.gameObject.transform.position.x;
				tempPosition.y =  this.gameObject.transform.position.y + (index * brickSpacing);
			}else{
				tempPosition.x =  this.gameObject.transform.position.x + (index * brickSpacing);
				tempPosition.y =  this.gameObject.transform.position.y;
			}
			
			tempPosition.z =  0;
			unbreakableBrick.gameObject.transform.position = tempPosition;		
		}
	}
	
	private void ClearBrick(){
		int count = this.gameObject.transform.childCount-1;
		for(int index=count;index>=0;index--){
			Transform child = this.gameObject.transform.GetChild(index);
			DestroyImmediate(child.gameObject);
		}
	}
}
