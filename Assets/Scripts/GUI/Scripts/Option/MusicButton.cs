using UnityEngine;
using System.Collections;

public class MusicButton : MonoBehaviour {

	private SoundManager soundManager;
	private UIToggle toggle;
	// Use this for initialization
	void Start () {
	}

	private void Awake(){
		soundManager = SoundManager.GetInstance();
		toggle = GetComponent<UIToggle>();
		EventDelegate.Add(toggle.onChange, Toggle);
		toggle.value =soundManager.isBgmOn;
	}

	public void Toggle ()
	{
		//Debug.Log("toggle onChange");
		if (enabled){
			if(soundManager!=null && toggle!=null){
				if(toggle.value){
					soundManager.UnMuteBGM();
					//Debug.Log("enable music");
				}else{
					soundManager.MuteBGM();
					//Debug.Log("disable music");
				}
			}
		}
	}
}
