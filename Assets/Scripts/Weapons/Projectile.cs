using UnityEngine;
using System.Collections;
using System;


public abstract class Projectile: MonoBehaviour{
	public GameObject prefab{set;get;}
	public int id{set;get;}
	public bool isActive{set;get;}

	public ProjectileType projectileType = ProjectileType.Fireball;
	public float disappearXRange= -74f;
	public float disappearYRange= 0;

	public GameDataManager gameDataManager;
	private Transform projectileTransform;

	private Action <bool,int,Transform>StatusChange;
	public event Action <bool,int, Transform>OnStatusChange{
		add{ StatusChange+=value;}
		remove{StatusChange-=value;}
	}

	public bool IsActive{
		set{
			if(value != isActive){
				isActive =value;
				if(null!=StatusChange){
					StatusChange(isActive,id,projectileTransform);
				}
			}
		}
		get{return isActive;}
	}

	public virtual void Start(){
		projectileTransform = this.gameObject.transform;
		gameDataManager = GameDataManager.GetInstance();
	}

	public virtual void Update(){
		CheckForPosition();
	}

	private void CheckForPosition(){
		if(this.gameObject.transform.position.x < disappearXRange && IsActive){
			IsActive =false;
		}
		
		if(this.gameObject.transform.position.y <= disappearYRange && IsActive){
			IsActive =false;
		}
	}
}
