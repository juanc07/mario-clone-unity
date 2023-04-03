using UnityEngine;
using System.Collections;

public class ScreenManagerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Screen.orientation = ScreenOrientation.LandscapeRight;
		Screen.orientation = ScreenOrientation.AutoRotation;
	}
}
