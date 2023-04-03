using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CombineMesh : MonoBehaviour {

	public bool isCombine = false;
	public Material meshMaterial;

	public bool isRevert = false;
	private Vector3 cachePosition;
	private bool hasCombine = false;

	private LevelManager levelManager;

	// Use this for initialization
	void Start (){
		levelManager = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
		if(Application.isPlaying){
			if(!levelManager.isDebug){
				isCombine = true;
			}
		}
	}

	private void CombineNow(){
		if(!hasCombine){
			cachePosition = this.gameObject.transform.position;

			MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
			CombineInstance[] combine = new CombineInstance[meshFilters.Length];
			int i = 0;
			while (i < meshFilters.Length) {
				combine[i].mesh = meshFilters[i].sharedMesh;
				combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
				meshFilters[i].gameObject.SetActive(false);
				i++;
			}

			if(!this.gameObject.transform.GetComponent<MeshFilter>()){
				this.gameObject.AddComponent<MeshFilter>();
			}
			
			if(!this.transform.GetComponent<MeshRenderer>()){
				this.gameObject.AddComponent<MeshRenderer>();
			}

			this.transform.GetComponent<MeshFilter>().mesh = new Mesh();
			this.transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine,true);
			this.transform.gameObject.SetActive(true);
			this.transform.GetComponent<MeshRenderer>().sharedMaterial = meshMaterial;
			this.transform.GetComponent<MeshRenderer>().renderer.sharedMaterial.mainTextureScale = new Vector2(this.gameObject.transform.localScale.x,1);
			
			Vector3 tempTransform = this.gameObject.transform.position;
			tempTransform.z = 0;
			this.gameObject.transform.position = tempTransform;			

			hasCombine = true;
		}
	}

	private void RemoveMesh(){
		MeshFilter meshFilter = this.gameObject.GetComponent<MeshFilter>();
		if(meshFilter!=null){
			DestroyImmediate(meshFilter);
		}

		MeshRenderer meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
		if(meshRenderer!=null){
			DestroyImmediate(meshRenderer);
		}
	}


	private void EnableDisableAllChild( GameObject parent, bool val ){
		int count = parent.gameObject.transform.childCount;
		for(int index=0;index<count;index++){
			Transform child = parent.gameObject.transform.GetChild(index);
			if(child!=null){
				child.gameObject.SetActive(val);
				EnableDisableAllChild(child.gameObject,val);
			}
		}
	}

	
	// Update is called once per frame
	void Update () {
		if(isCombine){
			CombineNow();
			EnableDisableAllChild(this.gameObject,false);
			isCombine = false;
		}

		if(isRevert){
			RemoveMesh();
			EnableDisableAllChild(this.gameObject,true);
			if(hasCombine){
				this.gameObject.transform.position = cachePosition;
				hasCombine = false;
			}
			isRevert = false;
		}
	}
}
