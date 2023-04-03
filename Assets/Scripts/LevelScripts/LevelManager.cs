using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public GameObject[] levelChunks;
	public GameObject[] backgrounds;
	public bool turnOffBackground= false;

	public GameObject levelHolder;
	public GameObject heroPrefab;
	public GameObject heroInstance{set;get;}
	public bool isDebug =false;

	public bool isLevelSkip =false;
	public int levelDebug =0;

	public int chunkLength = 126;
	public int offsetX = 26;
	private Vector3 heroPosition;

	private GameDataManager gameDataManager;
	public GameObject lavaEffect;

	void Start(){
		gameDataManager = GameDataManager.GetInstance();

		if(isLevelSkip){
			if(!gameDataManager.hasSkipLevel){
				gameDataManager.hasSkipLevel = true;
				gameDataManager.Level = levelDebug;
			}
		}

		int level = gameDataManager.player.Level;

		if(level > levelChunks.Length-1){
			gameDataManager.player.Level = 0;
			level = gameDataManager.player.Level;
		}

		//Debug.Log( "check level " + level );

		if(!turnOffBackground){
			if(level > 10 && level < 22 ){
				backgrounds[0].gameObject.SetActive(false);
				backgrounds[1].gameObject.SetActive(true);
				backgrounds[2].gameObject.SetActive(false);
				lavaEffect.SetActive(false);
			}else if(level > 21){
				backgrounds[0].gameObject.SetActive(false);
				backgrounds[1].gameObject.SetActive(false);
				backgrounds[2].gameObject.SetActive(true);
				lavaEffect.SetActive(true);
			}else{
				backgrounds[0].gameObject.SetActive(true);
				backgrounds[1].gameObject.SetActive(false);
				backgrounds[2].gameObject.SetActive(false);
				lavaEffect.SetActive(false);
			}
		}

		//Debug.Log("level " + level);
		if(!isDebug){
			GameObject levelChunk = Instantiate(levelChunks[level]) as GameObject;
			Vector3 levelChunkPosition =  levelChunk.gameObject.transform.position;
			levelChunkPosition.x = 0;
			levelChunk.gameObject.transform.position = levelChunkPosition;
			levelChunk.transform.parent = levelHolder.transform;
		}

		heroInstance = Instantiate(heroPrefab) as GameObject;
		heroInstance.gameObject.transform.parent = levelHolder.transform;

		//Invoke("LevelStart",0.05f);
		Invoke(Task.LevelStart.ToString(),0.05f);
	}

	private void LevelStart(){
		gameDataManager.IsLevelStart = true;
	}
}
