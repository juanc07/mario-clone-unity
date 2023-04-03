using UnityEngine;
using System.Collections;

public class SoundManagerCaller : MonoBehaviour {

	private SoundManager soundManager;

	// Use this for initialization
	void Start (){
		soundManager = SoundManager.GetInstance();
		soundManager.OnSoundManagerReady+=OnSoundManagerReady;

	}

	private void OnSoundManagerReady(){
		soundManager.MuteBGM();
		soundManager.MuteSfx();

		soundManager.UnMuteBGM();
		soundManager.UnMuteSfx();

		//PlayBGM();
	}

	private void PlayBGM(){
		//soundManager.PlayBGM(BGM.steel_road,0.3f);
	}

	// Update is called once per frame
	void Update () {
		/*if (Input.GetMouseButtonDown(0) && soundManager.isReady){
			soundManager.PlaySfx(SFX.high_powered_pistol,0.6f);
		}*/			
	}
}
