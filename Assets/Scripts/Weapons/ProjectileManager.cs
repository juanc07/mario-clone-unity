using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ProjectileManager : MonoBehaviour {

	private LevelManager levelManager;
	private GameObject fireballHolder;
	private List<Projectile> fireballProjectiles = new List<Projectile>();

	private GameObject iceballHolder;
	private List<Projectile> iceballProjectiles = new List<Projectile>();

	public Projectiles projectiles;

	private Action <bool,int,Transform>ActivateDeactivateProjectile;
	public event Action <bool,int,Transform>OnActivateDeactivateProjectile{
		add{ActivateDeactivateProjectile+=value;}
		remove{ActivateDeactivateProjectile-=value;}
	}

	// Use this for initialization
	void Start () {
		levelManager = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
		if(fireballHolder==null){
			fireballHolder = new GameObject();
			fireballHolder.name = "FireballHolder";
			fireballHolder.gameObject.transform.parent = levelManager.gameObject.transform;
		}

		if(iceballHolder==null){
			iceballHolder = new GameObject();
			iceballHolder.name = "IceballHolder";
			iceballHolder.gameObject.transform.parent = levelManager.gameObject.transform;
		}
	}
	
	private void OnDestroy(){
		RemoveEventListener();
	}

	private GameObject GetProjectilePrefab(ProjectileType projectileType){
		GameObject projectilePrefab=null;

		if(projectileType == ProjectileType.Fireball){
			projectilePrefab = projectiles.fireballPrefab;
		}else if(projectileType == ProjectileType.Iceball){
			projectilePrefab = projectiles.iceballPrefab;
		}else if(projectileType == ProjectileType.BossIceBall){
			projectilePrefab = projectiles.bossIceballPrefab;
		}

		return projectilePrefab;
	}

	public void CreateFireBallProjectile(ProjectileType projectileType,Vector3 projectilePosition, Quaternion projectileRotation, HeroController heroController,float forwardForce,string ownerId){
		Projectile projectile = SearchForInActiveFireBallProjectile();

		if(projectile==null){
			GameObject projectileModel = Instantiate(GetProjectilePrefab(projectileType)) as GameObject;
			projectileModel.gameObject.transform.parent = fireballHolder.gameObject.transform;
			projectileModel.gameObject.transform.position = projectilePosition;
			projectileModel.gameObject.transform.rotation = projectileRotation;

			FireballController fireballController = projectileModel.GetComponent<FireballController>();
			fireballController.forwardForce = forwardForce;
			fireballController.heroController = heroController;
			fireballController.prefab = projectileModel;
			fireballController.ownerId = ownerId;
			fireballController.id = fireballProjectiles.Count;
			fireballController.isActive = true;
			fireballController.OnStatusChange+=OnFireBallStatusChange;
			fireballProjectiles.Add(fireballController);
			fireballController.Shoot();
		}else{
			projectile.gameObject.transform.position = projectilePosition;
			projectile.gameObject.transform.rotation = projectileRotation;

			FireballController fireballController = projectile.prefab.GetComponent<FireballController>();
			fireballController.forwardForce = forwardForce;
			fireballController.heroController = heroController;
			fireballController.ownerId = ownerId;
			fireballController.ResetData();
			fireballController.Shoot();
		}
	}

	private void ActivateDeactivateFireBallProjectile(bool val,int id,Transform projectileTransform){
		int count = fireballProjectiles.Count;
		for(int index=0;index<count;index++){
			if(fireballProjectiles[index]!=null){
				if(fireballProjectiles[index].id==id){
					fireballProjectiles[index].prefab.SetActive(val);
					break;
				}
			}
		}
	}

	private Projectile SearchForInActiveFireBallProjectile(){
		int count = fireballProjectiles.Count;
		Projectile projectile=null;
		for(int index=0;index<count;index++){
			if(fireballProjectiles[index]!=null){
				if(!fireballProjectiles[index].isActive){
					fireballProjectiles[index].prefab.SetActive(true);
					fireballProjectiles[index].isActive = true;
					projectile=fireballProjectiles[index];
					break;
				}
			}
		}

		return projectile;
	}

	private void RemoveEventListener(){
		int count = fireballProjectiles.Count;
		for(int index=0;index<count;index++){
			if(fireballProjectiles[index]!=null){
				fireballProjectiles[index].OnStatusChange -= OnFireBallStatusChange;
			}
		}

		int iceBallCount = iceballProjectiles.Count;
		for(int index=0;index<iceBallCount;index++){
			if(iceballProjectiles[index]!=null){
				iceballProjectiles[index].OnStatusChange -= OnIceBallStatusChange;
			}
		}
	}

	private void OnFireBallStatusChange(bool val,int id, Transform projectileTransform){
		ActivateDeactivateFireBallProjectile(val,id,projectileTransform);
	}


	public void CreateIceBallProjectile(ProjectileType projectileType,Vector3 projectilePosition, Quaternion projectileRotation, HeroController heroController,float forwardForce,string ownerId){
		Projectile projectile = SearchForInActiveIceBallProjectile();
		
		if(projectile==null){
			GameObject projectileModel = Instantiate(GetProjectilePrefab(projectileType)) as GameObject;
			projectileModel.gameObject.transform.parent = iceballHolder.gameObject.transform;
			projectileModel.gameObject.transform.position = projectilePosition;
			projectileModel.gameObject.transform.rotation = projectileRotation;

			IceballController iceballController = projectileModel.GetComponent<IceballController>();
			iceballController.forwardForce = forwardForce;
			iceballController.heroController = heroController;
			iceballController.prefab = projectileModel;
			iceballController.id = iceballProjectiles.Count;
			iceballController.isActive = true;
			iceballController.ownerId = ownerId;
			iceballController.OnStatusChange+=OnIceBallStatusChange;
			iceballProjectiles.Add(iceballController);
			iceballController.Shoot();			
		}else{
			projectile.gameObject.transform.position = projectilePosition;
			projectile.gameObject.transform.rotation = projectileRotation;

			IceballController iceballController = projectile.prefab.GetComponent<IceballController>();
			iceballController.ownerId = ownerId;
			iceballController.ResetData();
			iceballController.Shoot();
		}
	}

	private Projectile SearchForInActiveIceBallProjectile(){
		int count = iceballProjectiles.Count;
		Projectile projectile=null;
		for(int index=0;index<count;index++){
			if(iceballProjectiles[index]!=null){
				if(!iceballProjectiles[index].isActive){
					iceballProjectiles[index].prefab.SetActive(true);
					iceballProjectiles[index].isActive = true;
					projectile=iceballProjectiles[index];
					break;
				}
			}
		}
		
		return projectile;
	}

	private void ActivateDeactivateIceBallProjectile(bool val,int id,Transform projectileTransform){
		int count = iceballProjectiles.Count;
		for(int index=0;index<count;index++){
			if(iceballProjectiles[index]!=null){
				if(iceballProjectiles[index].id==id){
					iceballProjectiles[index].prefab.SetActive(val);
					break;
				}
			}
		}
	}

	private void OnIceBallStatusChange(bool val,int id,Transform projectileTransform){
		//Debug.Log("1st OnIceBallStatusChange isActivate " + val );
		if(null!=ActivateDeactivateProjectile){
			//Debug.Log("2nd OnIceBallStatusChange isActivate " + val );
			ActivateDeactivateProjectile(val,id,projectileTransform);
		}
		ActivateDeactivateIceBallProjectile(val,id,projectileTransform);
	}
}
