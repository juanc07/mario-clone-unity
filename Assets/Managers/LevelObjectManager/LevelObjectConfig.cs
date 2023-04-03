using UnityEngine;
using System.Collections;

public class LevelObjectConfig : ScriptableObject {
	[System.Serializable]
	public class LevelObjectDictionary:InspectorDictionary<LevelItemType,GameObject>{}
	public LevelObjectDictionary levelObjectDictionary = new LevelObjectDictionary();

	public GameObject GetObject(LevelItemType type){
		return levelObjectDictionary.Get(type);
	}
}
