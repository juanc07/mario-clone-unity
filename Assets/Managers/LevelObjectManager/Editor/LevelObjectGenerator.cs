using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class LevelObjectGenerator:EditorWindow{

	//public UnityEngine.Object groundBlock;
	//private string groundLength;
	//public Transform parent;
	private GameObject levelObjectPrefab;
	private int levelObjectItemIndex;
	private LevelObjectConfig levelObjectConfig;
	private string configPath= "Assets/Resources/Config/LevelObjectConfig.asset";

	private string levelItemName="";
	private Array levelObjectItems;
	private int levelObjectItemCount;
	private string[] levelObjectItemNames;

	[HideInInspector]
	private GameObject newGroundBlock;

	[HideInInspector]
	private GroundSizeController groundSizeController;

	[MenuItem("Custom Editor/LevelObjectGenerator /Setup...", false, 1)]
	public static void MenuItemSetup() {
		EditorWindow.GetWindow(typeof(LevelObjectGenerator));
	}
	
	private void CreateFolder(string path, string folderName){
		string resourcesPath= "Assets/Resources";
		if (!System.IO.Directory.Exists(resourcesPath)){
			AssetDatabase.CreateFolder("Assets", "Resources");
		}
		
		if (!System.IO.Directory.Exists(path + "/" + folderName)){
			AssetDatabase.CreateFolder(path, folderName);
		}
	}
	
	private void OnGUI(){
		// Title
		GUILayout.BeginArea(new Rect(20, 20, position.width - 40, position.height));
		GUILayout.Label("LevelObjectGenerator", EditorStyles.boldLabel);
		GUILayout.Space(10);

		//groundBlock =  EditorGUILayout.ObjectField("groundBlock",groundBlock,typeof(object));
		//parent = Selection.activeTransform;
		//EditorGUILayout.ObjectField("Parent",parent,typeof(Transform));
		//groundLength = EditorGUILayout.TextField("ground length: ",groundLength);

		// Setup button
		/*if (GUILayout.Button("Create Ground Prefabs")) {
			newGroundBlock = Instantiate(groundBlock) as GameObject;
			newGroundBlock.transform.parent = parent.gameObject.transform;
			groundSizeController = newGroundBlock.GetComponent<GroundSizeController>();
			groundSizeController.length = System.Int32.Parse(groundLength); 
			groundSizeController.isClear =true;
			groundSizeController.generate =true;
			groundSizeController.combineMeshNow =true;
		}else */


		LevelObjectConfig levelObjectConfigHolder =(LevelObjectConfig)ScriptableWizard.CreateInstance(typeof(LevelObjectConfig));
		
		levelObjectItems = Enum.GetValues(typeof(LevelItemType));
		levelObjectItemCount = levelObjectItems.Length;
		levelObjectItemNames = Enum.GetNames(typeof(LevelItemType));
		


		if (GUILayout.Button("Create Config")) {
			CreateFolder("Assets/Resources","Config");


			
			for(int index=0;index<levelObjectItemCount;index++){
				levelObjectConfigHolder.levelObjectDictionary.Set((LevelItemType)levelObjectItems.GetValue(index),null);
			}

			DeleteConfig();

			Debug.Log("creating newlevel object config");
			AssetDatabase.CreateAsset( levelObjectConfigHolder,"Assets/Resources/Config/LevelObjectConfig.asset" );
			AssetDatabase.SaveAssets();

			EditorUtility.FocusProjectWindow();
			Selection.activeObject = levelObjectConfigHolder;
			AssetDatabase.Refresh();

			EditorUtility.DisplayDialog("Success: ", "LevelObject Config created successfully","ok");
		}else if (GUILayout.Button("Delete Config")) {
			bool option =  EditorUtility.DisplayDialog(
				"Are you sure you want to delete this config?",
				"Please choose one of the following options.",
				"Yes",
				"No"
				);

			if(option){
				DeleteConfig();			
			}
		}

		GUILayout.Space(10);
		levelItemName = EditorGUILayout.TextField("LevelObject Name: ", levelItemName);
		if(GUI.Button (new Rect(210,105,100,30), "Add LevelObject")){
			if(!levelItemName.Equals("",StringComparison.Ordinal)){
				CreateEnum(levelItemName,"LevelItemType","Assets/Managers/LevelObjectManager/");
			}else{
				EditorUtility.DisplayDialog("Failed: ", "levelItem name is empty!, please enter levelItem name","ok");
			}
			
		}


		GUILayout.Space(50);
		levelObjectItemIndex = EditorGUILayout.Popup("LevelObjectType:",levelObjectItemIndex, levelObjectItemNames, EditorStyles.popup);
		levelObjectPrefab =(GameObject)EditorGUILayout.ObjectField("levelObjectPrefab",levelObjectPrefab,typeof(GameObject),false);
		if(GUI.Button (new Rect(260,200,50,30), "Save")){
			Load();
			if(levelObjectConfig==null){
				EditorUtility.DisplayDialog("Failed: ", "levelObject Config is missing, please create levelObject Config 1st","ok");
				return;
			}
			levelObjectConfig.levelObjectDictionary.Set((LevelItemType)levelObjectItems.GetValue(levelObjectItemIndex),levelObjectPrefab);
			Save();
			Debug.Log("save");
		}
		
		if(GUI.Button (new Rect(0,200,60,30), "Refresh")){
			AssetDatabase.Refresh();
		}

		if(GUI.Button (new Rect(190,200,60,30), "Create")){
			Load();
			if(levelObjectConfig==null){
				EditorUtility.DisplayDialog("Failed: ", "levelObject Config is missing, please create levelObject Config 1st","ok");
				return;
			}
			GameObject levelitem = Instantiate(levelObjectConfig.levelObjectDictionary.Get(LevelItemType.Ground)) as GameObject;
			groundSizeController = levelitem.GetComponent<GroundSizeController>();
			groundSizeController.length =3;
			groundSizeController.isClear =true;
			groundSizeController.generate =true;
			groundSizeController.combineMeshNow =true;
		}

		GUILayout.EndArea();
	}

	private void DeleteConfig(){
		if (System.IO.File.Exists(configPath)){
			Debug.Log("deleting old level object config");
			AssetDatabase.DeleteAsset(configPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}



	private void CreateEnum( string newEnumName, string enumFileName,string folderDirectorySavePath ){
		string path =folderDirectorySavePath + enumFileName + ".cs";
		string[] levelItemObjectNames = Enum.GetNames(typeof(LevelItemType));
		
		foreach(string levelItemObjectName in levelItemObjectNames ){
			if(levelItemObjectName.Equals(newEnumName,StringComparison.Ordinal)){
				EditorUtility.DisplayDialog("failed: ", "Particle name: " + newEnumName + " already exist! ","ok");
				return;
			}
		}
		
		System.Array.Resize(ref levelItemObjectNames,levelItemObjectNames.Length+1);
		levelItemObjectNames[levelItemObjectNames.Length-1] = newEnumName;
		//ParticleEffect[] particleEffectValue = (ParticleEffect[])Enum.GetValues(typeof(ParticleEffect));
		int len = levelItemObjectNames.Length;
		int count = 0;
		
		if(File.Exists(path)){
			AssetDatabase.DeleteAsset(path);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
		
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		sb.Append("//" + enumFileName +" LIST \n");
		sb.Append("public enum " + enumFileName +"{\n");
		
		foreach( string levelItemObjectName in levelItemObjectNames ){
			if(len == 1){
				sb.Append("\t\t"+levelItemObjectName+ "\t\t\t\t\t\t\t\t" + "=" + "\t\t\t\t\t " + count +"\n" );
			}else{
				if(count<(len - 1)){
					//sb.Append("\t\t"+levelItemObjectName+","+"\n");
					sb.Append("\t\t"+levelItemObjectName + "\t\t\t\t\t\t\t\t" + "=" + "\t\t\t\t\t " + count +" , " + "\n" );
				}else{
					//sb.Append("\t\t"+levelItemObjectName+"\n");
					sb.Append("\t\t"+levelItemObjectName + "\t\t\t\t\t\t\t\t" + "=" + "\t\t\t\t\t " + count +"\n" );
				}
				count++;
			}			
		}
		sb.Append("}");
		
		File.WriteAllText(path,sb.ToString());
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();


		levelObjectItems = Enum.GetValues(typeof(LevelItemType));
		levelObjectItemCount = levelObjectItems.Length;
		levelObjectItemNames = Enum.GetNames(typeof(LevelItemType));
	}

	private void Load(){
		levelObjectConfig = (LevelObjectConfig)Resources.Load("Config/LevelObjectConfig");
	}
	
	private void Save(){
		EditorUtility.SetDirty(levelObjectConfig);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = levelObjectConfig;
	}
}
