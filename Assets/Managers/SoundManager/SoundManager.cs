using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SoundManager : MonoBehaviour {
	
	private static SoundManager instance;
	private static GameObject container;
	
	private List<AudioData> sfxCollection = new List<AudioData>();
	private List<AudioData> bgmCollection = new List<AudioData>();
	public AudioSource bgmAudioSource;
	public bool isInitializedSfx =false;
	
	//new  extra audio source
	private List<AudioSource> extraSfx =new List<AudioSource>();
	
	private SoundConfig soundConfig;	
	public bool isReady =false;


	public bool isSfxOn = true;
	public bool isBgmOn =true;
	private float lastBgmVolume =1f;


	private Action SoundManagerReady;
	public event Action OnSoundManagerReady{
		add{SoundManagerReady+=value;}
		remove{SoundManagerReady-=value;}
	}
	
	public static SoundManager GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name ="SoundManager";
			instance = container.AddComponent(typeof(SoundManager)) as SoundManager;
			DontDestroyOnLoad(instance);
		}
		
		return instance;
	}
	
	private void OnDestroy(){
		int sfxCount = sfxCollection.Count;
		for(int index=0;index < sfxCount;index++){
			sfxCollection[index] = null;
		}
		sfxCollection.Clear();
		
		int bgmCount = bgmCollection.Count;
		for(int index=0;index < bgmCount;index++){
			bgmCollection[index] = null;
		}
		bgmCollection.Clear();
		
		sfxCollection = null;
		bgmCollection = null;
		//Debug.Log("SoundManager pooled Audio Data cleared");
	}
	
	
	private AudioClip CheckCachedSFX( SFX sfx ){
		int count = sfxCollection.Count;
		AudioClip clip = null;
		for(int index=0;index < count;index++){
			if(sfxCollection[index].name.Equals(sfx.ToString(),StringComparison.Ordinal)){
				clip = sfxCollection[index].clip;
				break;
			}
		}
		
		if(clip==null){
			clip = soundConfig.GetSFX(sfx);
			AudioData audioData = new AudioData();
			audioData.id = sfxCollection.Count+1;
			audioData.name = sfx.ToString();
			audioData.clip = clip;
			audioData.type = AudioData.AudioDataType.SFX;
			sfxCollection.Add(audioData);
			//Debug.Log("sfx not in cache, cache: " + sfxName + " now!");
		}else{
			//Debug.Log("sfx used cached: " + sfxName);
		}
		
		return clip;
	}
	
	public void PlaySfx(SFX sfxName, float volume =1f){
		AudioSource audioSfx = SearchForAudioSource();
		AudioClip clip = CheckCachedSFX(sfxName);
		if(clip!=null){
			if(!isSfxOn){
				volume = 0;
			}
			audioSfx.loop =false;
			audioSfx.clip = clip;
			audioSfx.volume = volume;
			audioSfx.Play();
		}else{
			Debug.Log("Sfx not yet loaded!, please check your sound config");
		}
	}
	
	public void PlayBGM(BGM bgmName, float volume =1f, bool isLoop =true){
		AudioClip clip = CheckCachedBGM(bgmName);
		if(clip!=null){
			if(!isBgmOn){
				volume = 0;
			}else{
				lastBgmVolume = volume;
			}

			bgmAudioSource.loop =isLoop;
			bgmAudioSource.clip = clip;
			bgmAudioSource.volume = volume;
			bgmAudioSource.Play();
		}else{
			Debug.Log("bgm not yet loaded!, please check your sound config");
		}
	}
	
	
	private AudioClip CheckCachedBGM( BGM bgm ){
		int count = bgmCollection.Count;
		AudioClip clip = null;
		for(int index=0;index < count;index++){
			if(bgmCollection[index].name.Equals(bgm.ToString(),StringComparison.Ordinal)){
				clip = bgmCollection[index].clip;
				break;
			}
		}
		
		if(clip==null){
			clip = soundConfig.GetBGM(bgm);
			AudioData audioData = new AudioData();
			audioData.id = bgmCollection.Count+1;
			audioData.name = bgm.ToString();
			audioData.clip = clip;
			audioData.type = AudioData.AudioDataType.BGM;
			bgmCollection.Add(audioData);
			//Debug.Log("bgm not in cache, cache: " + bgmName + " now!");
		}else{
			//Debug.Log("bgm used cached: " + bgmName);
		}
		
		return clip;
	}
	
	private void EnableOrCreateAudioListener(){
		AudioListener[] audioListeners = GameObject.FindObjectsOfType(typeof(AudioListener)) as AudioListener[];
		foreach(AudioListener audioListener in  audioListeners ){
			audioListener.enabled = false;
		}
		
		AudioListener ownAudioListener = this.gameObject.GetComponent<AudioListener>();
		if(!ownAudioListener){
			this.gameObject.AddComponent<AudioListener>();
		}else{
			ownAudioListener.enabled = true;
		}
	}
	
	private AudioSource CreateAudioSource(string audioSourceName){
		AudioSource audioSource;
		
		GameObject audioSourceHolder = new GameObject();
		audioSourceHolder.name = audioSourceName;
		audioSourceHolder.transform.parent = this.gameObject.transform;
		audioSourceHolder.AddComponent<AudioSource>();
		audioSource = audioSourceHolder.GetComponent<AudioSource>();
		
		return audioSource;
	}
	
	private void CreateSFXAndBGMHolder(){
		//init sfx and bgm holder
		if(!isInitializedSfx){
			int sfxCount = 18;
			for(int index=0;index<sfxCount;index++){
				extraSfx.Add(CreateAudioSource("SFX_"+extraSfx.Count));
			}
			bgmAudioSource = CreateAudioSource("BGM");
			isInitializedSfx = true;
		}
	}
	
	private AudioSource SearchForAudioSource(){
		int audioSourceCnt = extraSfx.Count;
		AudioSource found = null;
		for(int index=0;index<audioSourceCnt;index++){
			if(extraSfx[index]!= null){
				if(!extraSfx[index].isPlaying){
					found = extraSfx[index];
					break;
				}
			}
		}
		
		if(found == null){
			extraSfx.Add(CreateAudioSource("SFX_"+extraSfx.Count));
			found = extraSfx[extraSfx.Count-1];
		}
		
		return found;
	}
	
	public void SetSFXVolume(float volume = 1f){
		int sfxCount = extraSfx.Count;
		for(int index=0;index<sfxCount;index++){
			if(extraSfx[index]!=null){
				extraSfx[index].volume = volume;
			}
		}
	}
	
	public void SetBGMVolume(float volume = 1f){
		bgmAudioSource.volume = volume;
	}

	public void MuteBGM(){
		isBgmOn =false;
		SetBGMVolume(0);
	}
	
	public void UnMuteBGM(){
		isBgmOn =true;
		SetBGMVolume(lastBgmVolume);
	}
	
	public void MuteSfx(){
		isSfxOn =false;
		SetSFXVolume(0);
	}
	
	public void UnMuteSfx(){
		isSfxOn =true;
		SetSFXVolume(1f);
	}

	// Use this for initialization
	void Start (){
		soundConfig = (SoundConfig)Resources.Load("Config/SoundConfig");
		CreateSFXAndBGMHolder();
		EnableOrCreateAudioListener();
		
		if(null!=SoundManagerReady){
			isReady = true;
			SoundManagerReady();
		}
	}
	
	private void OnLevelWasLoaded(){
		EnableOrCreateAudioListener();
	}
}
