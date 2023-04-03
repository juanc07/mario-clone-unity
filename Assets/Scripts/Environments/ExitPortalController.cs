using UnityEngine;
using System.Collections;

public class ExitPortalController : MonoBehaviour {
	private GameDataManager gameDataManager;
	private SphereCollider sphereCollider;
	private MeshRenderer meshRenderer;
	// Use this for initialization
	void Start () {
		gameDataManager = GameDataManager.GetInstance();
		sphereCollider = this.gameObject.GetComponent<SphereCollider>();
		meshRenderer = this.gameObject.GetComponent<MeshRenderer>();

		AddEventListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEventListener(){
		gameDataManager.OnShowBossHp+= OnShowBossHp;
		gameDataManager.OnBossDied+= OnBossDied;
	}

	private void RemoveEventListener(){
		if(gameDataManager!=null){
			gameDataManager.OnShowBossHp-= OnShowBossHp;
			gameDataManager.OnBossDied-= OnBossDied;
		}
	}

	private void OnShowBossHp(){
		ShowHidePortal(false);
	}

	private void OnBossDied(){
		ShowHidePortal(true);
	}

	private void ShowHidePortal(bool val){
		if(sphereCollider!=null){
			sphereCollider.enabled = val;
		}

		if(meshRenderer!=null){
			meshRenderer.enabled = val;
		}
	}

}
