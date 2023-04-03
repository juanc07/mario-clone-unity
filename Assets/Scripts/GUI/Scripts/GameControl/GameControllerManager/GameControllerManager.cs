using UnityEngine;
using System.Collections;
using System;

public class GameControllerManager : MonoBehaviour {
	

	private static GameControllerManager instance;
	private static GameObject container;


	private Action<ButtonInput> DownInput;
	public event Action<ButtonInput> OnDownInput{
		add{DownInput+=value;}
		remove{DownInput-=value;}
	}

	private Action<ButtonInput> UpInput;
	public event Action<ButtonInput> OnUpInput{
		add{UpInput+=value;}
		remove{UpInput-=value;}
	}

	private Action<ButtonInput> HeldInput;
	public event Action<ButtonInput> OnHeldInput{
		add{HeldInput+=value;}
		remove{HeldInput-=value;}
	}


	public static GameControllerManager GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name = "GameControllerManager";
			instance =container.AddComponent(typeof(GameControllerManager)) as GameControllerManager;
			DontDestroyOnLoad(instance.gameObject);
		}

		return instance;
	}

	// Use this for initialization
	void Start (){
	}
	
	#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX || UNITY_WEBPLAYER
	// Update is called once per frame
	void Update (){
		if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) ){
			if(null!=DownInput){
				DownInput(ButtonInput.Down);
			}
			//Debug.Log("e pressed");
		}

		if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.RightAlt) ){
			if(null!=DownInput){
				DownInput(ButtonInput.Grab);
			}
			//Debug.Log("e pressed");
		}

		if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.RightControl) ){
			if(null!=DownInput){
				DownInput(ButtonInput.FireWeapon);
			}
			//Debug.Log("e pressed");
		}

		if(Input.GetButtonDown(ButtonInput.Start.ToString())){
			//Debug.Log("Start");
			if(null!=DownInput){
				DownInput(ButtonInput.Start);
			}
		}
		
		if(Input.GetButtonDown(ButtonInput.Back.ToString())){
			if(null!=DownInput){
				DownInput(ButtonInput.Back);
			}
			//Debug.Log("Back");
		}
		
		if(Input.GetButtonDown(ButtonInput.LB.ToString())){
			if(null!=DownInput){
				DownInput(ButtonInput.LB);
			}
			//Debug.Log("LB");
		}
		
		if(Input.GetButtonDown(ButtonInput.RB.ToString())){
			if(null!=DownInput){
				DownInput(ButtonInput.RB);
			}
			//Debug.Log("RB");
		}
		
		if(Input.GetButtonDown(ButtonInput.LAP.ToString())){
			if(null!=DownInput){
				DownInput(ButtonInput.LAP);
			}
			//Debug.Log("LAP");
		}
		
		if(Input.GetButtonDown(ButtonInput.RAP.ToString())){
			if(null!=DownInput){
				DownInput(ButtonInput.RAP);
			}
			//Debug.Log("RAP");
		}
		
		if(Input.GetButtonDown(ButtonInput.A.ToString())){
			if(null!=DownInput){
				DownInput(ButtonInput.A);
			}
			//Debug.Log("A");
		}
		
		if(Input.GetButtonDown(ButtonInput.B.ToString())){
			if(null!=DownInput){
				DownInput(ButtonInput.B);
			}
			//Debug.Log("B");
		}

		if(Input.GetButtonDown(ButtonInput.X.ToString())){
			if(null!=DownInput){
				DownInput(ButtonInput.X);
			}
			//Debug.Log("B");
		}

		if(Input.GetButtonDown(ButtonInput.Y.ToString())){
			if(null!=DownInput){
				DownInput(ButtonInput.Y);
			}
			//Debug.Log("B");
		}

		if(Input.GetButtonDown(ButtonInput.Jump.ToString())){
			if(null!=DownInput){
				DownInput(ButtonInput.Jump);
			}
			//Debug.Log("B");
		}

		//up
		if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow) ){
			if(null!=UpInput){
				UpInput(ButtonInput.Down);
			}
			//Debug.Log("e key up");
		}

		if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.RightAlt) ){
			if(null!=UpInput){
				UpInput(ButtonInput.Grab);
			}
			//Debug.Log("e key up");
		}

		if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.RightControl) ){
			if(null!=UpInput){
				UpInput(ButtonInput.FireWeapon);
			}
			//Debug.Log("e pressed");
		}

		if(Input.GetButtonUp(ButtonInput.Start.ToString())){
			//Debug.Log("Start");
			if(null!=UpInput){
				UpInput(ButtonInput.Start);
			}
		}
		
		if(Input.GetButtonUp(ButtonInput.Back.ToString())){
			if(null!=UpInput){
				UpInput(ButtonInput.Back);
			}
			//Debug.Log("Back");
		}
		
		if(Input.GetButtonUp(ButtonInput.LB.ToString())){
			if(null!=UpInput){
				UpInput(ButtonInput.LB);
			}
			//Debug.Log("LB");
		}
		
		if(Input.GetButtonUp(ButtonInput.RB.ToString())){
			if(null!=UpInput){
				UpInput(ButtonInput.RB);
			}
			//Debug.Log("RB");
		}
		
		if(Input.GetButtonUp(ButtonInput.LAP.ToString())){
			if(null!=UpInput){
				UpInput(ButtonInput.LAP);
			}
			//Debug.Log("LAP");
		}
		
		if(Input.GetButtonUp(ButtonInput.RAP.ToString())){
			if(null!=UpInput){
				UpInput(ButtonInput.RAP);
			}
			//Debug.Log("RAP");
		}
		
		if(Input.GetButtonUp(ButtonInput.A.ToString())){
			if(null!=UpInput){
				UpInput(ButtonInput.A);
			}
			//Debug.Log("A");
		}
		
		if(Input.GetButtonUp(ButtonInput.B.ToString())){
			if(null!=UpInput){
				UpInput(ButtonInput.B);
			}
			//Debug.Log("B");
		}
		
		if(Input.GetButtonUp(ButtonInput.X.ToString())){
			if(null!=UpInput){
				UpInput(ButtonInput.X);
			}
			//Debug.Log("B");
		}
		
		if(Input.GetButtonUp(ButtonInput.Y.ToString())){
			if(null!=UpInput){
				UpInput(ButtonInput.Y);
			}
			//Debug.Log("B");
		}

		if(Input.GetButtonUp(ButtonInput.Jump.ToString())){
			if(null!=UpInput){
				UpInput(ButtonInput.Jump);
			}
			//Debug.Log("B");
		}

		//held
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ){
			if(null!=HeldInput){
				HeldInput(ButtonInput.Down);
			}
			//Debug.Log("e key held");
		}

		if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.RightAlt) ){
			if(null!=HeldInput){
				HeldInput(ButtonInput.Grab);
			}
			//Debug.Log("e key held");
		}

		if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.RightControl) ){
			if(null!=HeldInput){
				HeldInput(ButtonInput.FireWeapon);
			}
			//Debug.Log("e pressed");
		}

		if(Input.GetButton(ButtonInput.Start.ToString())){
			//Debug.Log("Start");
			if(null!=HeldInput){
				HeldInput(ButtonInput.Start);
			}
		}
		
		if(Input.GetButton(ButtonInput.Back.ToString())){
			if(null!=HeldInput){
				HeldInput(ButtonInput.Back);
			}
			//Debug.Log("Back");
		}
		
		if(Input.GetButton(ButtonInput.LB.ToString())){
			if(null!=HeldInput){
				HeldInput(ButtonInput.LB);
			}
			//Debug.Log("LB");
		}
		
		if(Input.GetButton(ButtonInput.RB.ToString())){
			if(null!=HeldInput){
				HeldInput(ButtonInput.RB);
			}
			//Debug.Log("RB");
		}
		
		if(Input.GetButton(ButtonInput.LAP.ToString())){
			if(null!=HeldInput){
				HeldInput(ButtonInput.LAP);
			}
			//Debug.Log("LAP");
		}
		
		if(Input.GetButton(ButtonInput.RAP.ToString())){
			if(null!=HeldInput){
				HeldInput(ButtonInput.RAP);
			}
			//Debug.Log("RAP");
		}
		
		if(Input.GetButton(ButtonInput.A.ToString())){
			if(null!=HeldInput){
				HeldInput(ButtonInput.A);
			}
			//Debug.Log("A");
		}
		
		if(Input.GetButton(ButtonInput.B.ToString())){
			if(null!=HeldInput){
				HeldInput(ButtonInput.B);
			}
			//Debug.Log("B");
		}
		
		if(Input.GetButton(ButtonInput.X.ToString())){
			if(null!=HeldInput){
				HeldInput(ButtonInput.X);
			}
			//Debug.Log("B");
		}
		
		if(Input.GetButton(ButtonInput.Y.ToString())){
			if(null!=HeldInput){
				HeldInput(ButtonInput.Y);
			}
			//Debug.Log("B");
		}

		if(Input.GetButton(ButtonInput.Jump.ToString())){
			if(null!=HeldInput){
				HeldInput(ButtonInput.Jump);
			}
			//Debug.Log("B");
		}
		
		float h = Input.GetAxis("Horizontal");
		if(h < 0 || Input.GetKey(KeyCode.LeftArrow)){
			//move left
			if(null!=HeldInput){
				HeldInput(ButtonInput.Left);
			}
		}else if(h > 0 || Input.GetKey(KeyCode.RightArrow)){
			//move right"
			if(null!=HeldInput){
				HeldInput(ButtonInput.Right);
			}
		}else{
			//do nothing reset left and right
			if(null!=UpInput){
				if(Input.GetKeyUp(KeyCode.LeftArrow)){
					UpInput(ButtonInput.Left);
				}else{
					UpInput(ButtonInput.Right);
				}
			}
		}
		
		float v = Input.GetAxis("Vertical");
		
		if(v < 0 || Input.GetKey(KeyCode.DownArrow)){
			//down
			if(null!=HeldInput){
				HeldInput(ButtonInput.Down);
			}
		}else if(v > 0 || Input.GetKey(KeyCode.UpArrow) ){
			//up
			if(null!=HeldInput){
				HeldInput(ButtonInput.Up);
			}
		}else{
			//do nothing reset up and down
			if(null!=UpInput){
				if(Input.GetKeyUp(KeyCode.DownArrow)){
					UpInput(ButtonInput.Down);
				}else{
					UpInput(ButtonInput.Up);
				}
			}
		}
		
		float f3 = Input.GetAxis("Fire3");
		bool downX = Input.GetButtonDown("X");
		bool heldX = Input.GetButton("X");
		bool upX = Input.GetButtonUp("X");
		
		if(f3>0 || downX){
			//Debug.Log("fire3!");
		}
		
		if(heldX || Input.GetKey(KeyCode.LeftShift)){
			//holdx
		}else if(upX){
			//upx
		}else{
			//do nothing reset last x action
		}
		
		
		//float f1 = Input.GetAxis("Fire1");
		bool heldA = Input.GetButton("A");
		bool upA = Input.GetButtonUp("A");		

		bool heldY = Input.GetButton("Y");
		bool upY = Input.GetButtonUp("Y");		

		
		if(heldA || heldY){
			//holdA
		}else if(upA || upY ){
			//upA
		}		

		bool downB= Input.GetButtonDown("B");
		bool heldB = Input.GetButton("B");
		bool upB = Input.GetButtonUp("B");
		
		if(heldB){
			//hold b
		}else if(upB){
			//up b
		}
		
		if(downB){
			//down b
		}
	}
	#endif
}
