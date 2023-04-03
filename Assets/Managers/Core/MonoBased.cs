using UnityEngine;
using System.Collections;

public abstract class MonoBased : MonoBehaviour {

	public virtual void Start(){
		AddEventListner();
	}
	
	public virtual void OnDestroy(){
		RemoveEventListner();
	}
	
	
	// Use this for initialization
	public virtual void AddEventListner() {
		
	}
	
	public virtual void RemoveEventListner() {
		
	}

	public virtual void Invoker( Task task,float delay ){
		Invoke(task.ToString(), delay);
	}
}
