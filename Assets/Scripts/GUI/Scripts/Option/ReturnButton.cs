using UnityEngine;
using System.Collections;

public class ReturnButton : MonoBehaviour {

	public GameObject optionPopupPanel;
	public OptionPopupController optionPopupController;

	// Use this for initialization
	void Start () {
	
	}
	
	private void OnClick(){
		optionPopupController.SkipAnimation();
		optionPopupPanel.SetActive(false);
	}
}
