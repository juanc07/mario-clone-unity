using UnityEngine;
using System.Collections;

public class MobileController : MonoBehaviour {

	public GameObject mobileUI;
	public bool autoHideMobileUI=false;

	#if UNITY_EDITOR
	// Use this for initialization
	void Start () {
		if(autoHideMobileUI){
			mobileUI.gameObject.SetActive(false);
		}
	}
	#endif
}
