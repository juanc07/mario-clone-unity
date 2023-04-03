using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UIAnchorRunOnceSetter : MonoBehaviour {

	private int uiAnchorSet=0;
	public bool runOnce =false;
	public bool start =false;
	// Use this for initialization
	void Start (){
	
	}

	private void SetAllAnchors( GameObject parent, bool val ){
		int childCount = parent.transform.transform.childCount;
		for(int index =0; index<childCount;index++){
			Transform child = parent.transform.transform.GetChild(index);
			UIAnchor uiAnchor = child.gameObject.GetComponent<UIAnchor>();
			if(uiAnchor!=null){
				uiAnchor.runOnlyOnce = val;
				uiAnchorSet++;
			}
			SetAllAnchors(child.gameObject,val);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(start){
			uiAnchorSet = 0;
			SetAllAnchors(this.gameObject,runOnce);
			Debug.Log("anchorSets " + uiAnchorSet);
			UIAnchorRunOnceSetter uiAnchorRunOnceSetter = this.gameObject.GetComponent<UIAnchorRunOnceSetter>();
			DestroyImmediate(uiAnchorRunOnceSetter);
		}
	}
}
