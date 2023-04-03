using UnityEngine;
using System.Collections;

public class PositionController : MonoBehaviour {
	
	public GameObject character;
	private HeroController heroController;
	public float delay = 1f;
	public float duration = 1f;
	private bool isActivated =false;
	private GameDataManager gameDataManager;
	private Vector3 originalPosition;
	private Vector3 safePosition;
	
	// Use this for initialization
	void Start () {
		gameDataManager = GameDataManager.GetInstance();
		heroController = character.GetComponent<HeroController>();
		AddEventListener();
		
		originalPosition = this.gameObject.transform.position;
		safePosition = originalPosition;
		safePosition.y += 1000f;
	}
	
	private void AddEventListener(){
		gameDataManager.OnGameRestart+= OnGameRestart;
		heroController.OnHeroRevive += OnHeroRevive;
	}
	
	private void RemoveEventListener(){
		if(gameDataManager!=null){
			gameDataManager.OnGameRestart-= OnGameRestart;
			heroController.OnHeroRevive -= OnHeroRevive;
		}
	}
	
	private void OnDestroy(){
		RemoveEventListener();
	}
	
	private void OnGameRestart(){
		isActivated = false;
		MoveToOriginalPosition();
		ActivateDeactivate(true);
	}

	private void OnHeroRevive(){
		isActivated = false;
		MoveToOriginalPosition();
		ActivateDeactivate(true);
	}
	
	// Update is called once per frame
	void Update () {
		if(heroController.IsDead && !isActivated){
			isActivated = true;
			Invoke(Task.DeactivateAndRepositionCharacter.ToString(),delay);
		}
	}
	
	private void ActivateDeactivate(bool val){
		this.gameObject.SetActive(val);
	}
	
	private void DeactivateAndRepositionCharacter(){
		ActivateDeactivate(false);
		Invoke(Task.RemoveCharacter.ToString(),duration);
	}
	
	private void RemoveCharacter(){
		MoveToSafePosition();
	}
	
	private void MoveToSafePosition(){
		this.gameObject.transform.position = safePosition;
	}
	
	private void MoveToOriginalPosition(){
		this.gameObject.transform.position = originalPosition;
	}
}
