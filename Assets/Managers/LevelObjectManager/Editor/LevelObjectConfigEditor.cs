using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(LevelObjectConfig))]
public class LevelObjectConfigEditor : Editor {

	private static bool showLevelObject;

	public override void OnInspectorGUI(){
		showLevelObject = EditorGUILayout.Foldout(showLevelObject, "Level Object Items");


		if(showLevelObject) {
			EditorGUI.indentLevel++;
			
			LevelObjectConfig.LevelObjectDictionary  levelObjectDictionary = ((LevelObjectConfig)target).levelObjectDictionary;
			
			string[] levelObjectNames = System.Enum.GetNames(typeof(LevelItemType));
			LevelItemType[] levelObjectTypes = (LevelItemType[])System.Enum.GetValues(typeof(LevelItemType));
			int levelObjectCount = levelObjectNames.Length;
			for(int index = 0; index < levelObjectCount; index++) {
				EditorGUILayout.BeginHorizontal();
				
				EditorGUILayout.PrefixLabel(levelObjectNames[index]);
				LevelItemType key = levelObjectTypes[index];
				levelObjectDictionary.Set(key,(GameObject)EditorGUILayout.ObjectField((GameObject)levelObjectDictionary.Get(key),typeof(GameObject), false));
				
				EditorGUILayout.EndHorizontal();
			}
			
			EditorGUILayout.BeginHorizontal();
			
			Color original = GUI.color;
			
			GUI.color = Color.red;
			if(GUILayout.Button("Reset Listing")) {
				levelObjectDictionary.Clear();
			}
			
			GUI.color = Color.green;
			if (GUILayout.Button("Save Changes")) {
				EditorUtility.SetDirty(target as LevelObjectConfig);
				AssetDatabase.SaveAssets();
			}
			
			GUI.color = original;
			
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel--;
		}
	}
}
