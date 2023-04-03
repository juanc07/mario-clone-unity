using UnityEngine;
using System.Collections;

public class OptionPopupController : MonoBehaviour {

	public GameObject optionPopup;
	public GameObject retryButton;
	public GameObject levelSelectButton;
	public GameObject resumeButton;
	public UILabel optionLabel;

	private GoTween optionPopupTween;
	public UISprite blackBG;

	// Use this for initialization
	void Start () {
	}
	
	private void OnEnable(){
		Time.timeScale = 0;

		optionPopup.gameObject.transform.localScale = new Vector3(0,0,0);
		ScaleTweenProperty optionScaleProperty = new ScaleTweenProperty(new Vector3(1f,1f,1f));
		GoTweenConfig config =  new GoTweenConfig();
		config.addTweenProperty(optionScaleProperty);
		config.setEaseType(GoEaseType.ElasticOut);
		config.setUpdateType(GoUpdateType.TimeScaleIndependentUpdate);
		optionPopupTween = new GoTween( optionPopup.gameObject.transform,0.5f,config,OnTweenComplete);
		Go.addTween(optionPopupTween);
		CheckScene();
	}

	private void OnTweenComplete(AbstractGoTween abstractGoTween){
		//Time.timeScale = 0;
	}

	public void SkipAnimation(){
		optionPopupTween.complete();
		optionPopupTween.destroy();
		optionPopupTween =null;
		Time.timeScale = 1f;
	}

	private void CheckScene(){
		if(Application.loadedLevelName.Equals(ScenePreloader.Scenes.Title.ToString())){
			blackBG.alpha = 0.3f;
			optionLabel.text = "Option";
			ShowHideRetryButton(false);
			ShowHideLevelSelectButton(false);
			//Debug.Log("title detected");
		}else if(Application.loadedLevelName.Equals(ScenePreloader.Scenes.Game.ToString())){
			blackBG.alpha = 0.2f;
			optionLabel.text = "Paused";
			ShowHideRetryButton(true);
			ShowHideLevelSelectButton(true);
			//Debug.Log("game detected");
		}

		//Debug.Log("check scene!");
	}

	private void ShowHideRetryButton(bool val){
		retryButton.SetActive(val);
	}

	private void ShowHideLevelSelectButton(bool val){
		levelSelectButton.SetActive(val);
	}
}
