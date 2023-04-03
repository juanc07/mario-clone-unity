using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent(typeof(LevelObjectTagger))]
[RequireComponent(typeof(FixedPosition))]
public class UnBreakbleBrickController : MonoBehaviour {

	public GameObject unbreakableBrickPrefab;
	public int brickCount;
	public float brickSpacing=1.1f;
	private int brickCounter;

	public bool isClear = false;
	public bool isGenerate = false;
	private BoxCollider boxCollider;
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
			GameObject unbreakableBrick = Instantiate( unbreakableBrickPrefab ) as GameObject;
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

		boxCollider = this.gameObject.GetComponent<BoxCollider>();
		if(boxCollider==null){
			this.gameObject.AddComponent<BoxCollider>();
		}else{
			float offsetX = (brickSpacing % 1) * 0.5f;
			float platformSizeX = brickSpacing * brickCount;
			Vector3  tempBoxColliderSize = boxCollider.size;

			if(isVertical){
				tempBoxColliderSize.x = 1f;
				tempBoxColliderSize.y = platformSizeX;
				boxCollider.size = tempBoxColliderSize;
			}else{
				tempBoxColliderSize.x = platformSizeX;
				tempBoxColliderSize.y = 1f;
				boxCollider.size = tempBoxColliderSize;
			}
			
			Vector3  tempBoxColliderCenter = boxCollider.center;
			if(isVertical){
				tempBoxColliderCenter.x = 0;
				tempBoxColliderCenter.y = (platformSizeX * 0.5f) - (0.5f + offsetX);
				boxCollider.center = tempBoxColliderCenter;
			}else{
				tempBoxColliderCenter.x = (platformSizeX * 0.5f) - (0.5f + offsetX);
				tempBoxColliderCenter.y = 0;
				boxCollider.center = tempBoxColliderCenter;
			}
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
