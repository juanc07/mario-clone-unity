using UnityEngine;
using System.Collections;

public class PlantEnemyController : MonoBehaviour {

	private Vector3 startPosition;
	private Vector3 endPosition;
	private bool isMoveUp =true;

	// Use this for initialization
	void Start () {
		startPosition = this.gameObject.transform.position;
		endPosition = startPosition;
		endPosition.y+=5f;
	}
	
	// Update is called once per frame
	void Update () {
		if(isMoveUp){
			this.gameObject.transform.position = Vector3.Lerp( this.gameObject.transform.position,endPosition,Time.deltaTime * 0.5f);
			if((this.gameObject.transform.position.y + 0.1f) >= endPosition.y){
				if(!IsInvoking("SwitchDirection")){
					Invoke("SwitchDirection",0.2f);
				}
			}
		}else{
			this.gameObject.transform.position = Vector3.Lerp( this.gameObject.transform.position,startPosition,Time.deltaTime * 0.5f);
			if((this.gameObject.transform.position.y - 0.1f) <= startPosition.y){
				if(!IsInvoking("SwitchDirection")){
					Invoke("SwitchDirection",0.2f);
				}
			}
		}
	}

	private void SwitchDirection(){
		if(isMoveUp){
			isMoveUp = false;
		}else{
			isMoveUp = true;
		}
	}
}
