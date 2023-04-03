using UnityEngine;
using System.Collections;

public class SfxButton : MonoBehaviour {
	
	private SoundManager soundManager;
	private UIToggle toggle;
	
	// Use this for initialization
	void Start () {
		
	}
	
	private void Awake(){
		soundManager = SoundManager.GetInstance();
		toggle = GetComponent<UIToggle>();
		EventDelegate.Add(toggle.onChange, Toggle);
		toggle.value =soundManager.isSfxOn;
	}
	
	public void Toggle ()
	{
		//Debug.Log("toggle onChange");
		if (enabled){
			if(soundManager!=null && toggle!=null){
				if(toggle.value){
					soundManager.UnMuteSfx();
					//Debug.Log("enable sfx");
				}else{
					soundManager.MuteSfx();
					//Debug.Log("disable sfx");
				}
			}
		}
	}
}
