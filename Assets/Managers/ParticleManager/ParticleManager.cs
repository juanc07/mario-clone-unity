using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ParticleManager : MonoBehaviour {
	
	private static ParticleManager instance;
	private static GameObject container;
	private ParticleConfigHolder particleConfig;
	
	private List<ParticleObject> particlePool = new List<ParticleObject>();
	
	
	public static ParticleManager GetInstance(){
		if(instance == null){
			container = new GameObject();
			container.name = "ParticleManager";
			instance = container.AddComponent(typeof(ParticleManager)) as ParticleManager;
			DontDestroyOnLoad(instance.gameObject);
		}
		return instance;
	}
	
	private void Awake(){
		particleConfig = (ParticleConfigHolder)Resources.Load("Config/ParticleConfig");
		//Debug.Log(" check particleConfig: " + particleConfig);
	}
	
	// Use this for initialization
	void Start (){
		Prewarm();
	}

	public void Prewarm(){
		Array particles =Enum.GetValues(typeof(ParticleEffect));
		int len = particles.Length;
		int extraCount = 30;
		for(int index =0;index<len;index++){
			ParticleEffect particle =(ParticleEffect)particles.GetValue(index);
			string particleName = Enum.GetName(typeof(ParticleEffect),particle);
			//Debug.Log(" particleName " + particleName);
			if(particleName.Equals(ParticleEffect.CollectCoin.ToString(),StringComparison.Ordinal)){
				for(int j =0;j<extraCount;j++){
					CreateParticle((ParticleEffect)particles.GetValue(index),new Vector3(0,0,0),new Vector3(0,0,0));
				}
			}else{
				CreateParticle((ParticleEffect)particles.GetValue(index),new Vector3(0,0,0),new Vector3(0,0,0));
			}
		}
		//Debug.Log("pre warm called");
	}
	
	private void Update(){
		CheckForNotPlayingParticle();
	}
	
	public void CreateParticle( ParticleEffect particleEffect, Vector3 position, Vector3 scale){
		ParticleObject particleObject =  SearchParicleById(particleEffect);
		if(particleObject==null){
			particleObject = new ParticleObject();
			particleObject.key = particleEffect;
			if(particleConfig.particles.Get(particleEffect)!=null){
				particleObject.particle= Instantiate( particleConfig.particles.Get(particleEffect),position,Quaternion.Euler(0,0,0)) as GameObject;
				particleObject.particle.gameObject.transform.parent = this.gameObject.transform;
				particleObject.particle.gameObject.transform.localScale = scale;
				particlePool.Add(particleObject);
				particleObject.particle.gameObject.GetComponent<ParticleSystem>().Play();
				//Debug.Log("create new particle");
			}else{
				Debug.Log("create particle failed: can't find " + particleEffect.ToString() + " particle effect");
			}
		}else{
			particleObject.particle.gameObject.transform.position = position;
			particleObject.particle.gameObject.transform.localScale = scale;
			particleObject.particle.gameObject.GetComponent<ParticleSystem>().Play();
			//Debug.Log("resuse particle");
		}
	}
	
	public void ClearParticlePool(){
		particlePool.Clear();
	}
	
	private ParticleObject SearchParicleById(ParticleEffect particleEffect){
		int count  = particlePool.Count;
		ParticleObject particleObject = null;
		
		for(int index=0;index<count;index++){
			if(particlePool[index]!=null && particlePool[index].particle != null){
				if(particlePool[index].key == particleEffect){
					if(!particlePool[index].particle.GetComponent<ParticleSystem>().isPlaying){
						particlePool[index].particle.gameObject.SetActive(true);
						particleObject = particlePool[index];
						return particleObject;
					}
				}
			}
		}
		
		return particleObject;
	}
	
	private void CheckForNotPlayingParticle(){
		int count  = particlePool.Count;	
		for(int index=0;index<count;index++){
			if(particlePool[index]!=null && particlePool[index].particle != null){
				if(!particlePool[index].particle.GetComponent<ParticleSystem>().isPlaying){
					particlePool[index].particle.gameObject.SetActive(false);
					//Debug.Log("set active false to not playing particles");
				}
			}
		}
	}
	
}
