using UnityEngine;
using System.Collections;

public class ParticleManagerCaller : MonoBehaviour {

	private ParticleManager particleManager;

	// Use this for initialization
	void Start () {
		particleManager = ParticleManager.GetInstance();
		//InvokeRepeating("ShowParticle",0.3f,0.3f);
		InvokeRepeating(Task.ShowParticle.ToString(),0.3f,0.3f);
	}
	
	private void ShowParticle(){
		Vector3 scale = new Vector3(2f,2f,2f);
		particleManager.CreateParticle(ParticleEffect.Hit1,this.gameObject.transform.position,scale);
	}
}
