using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (FixedPosition))]
public class Brick:MonoBehaviour,IDestroyable{
	public bool isDestroyable;
	private Vector3 originalPosition;
	private Vector3 targetPosition;

	public float targetValue =1f;
	public float smooth =15f;

	public bool allowToMove =true;

	public GameObject originalBrick;
	public GameObject targetBrick;
	public bool hasTargetBrick =false;
	public bool hasItem = false;
	public int itemCount =1;
	private int cacheItemCount;

	public GameObject[] items;
	public bool isMoving = false;

	//new
	private bool isMovingUp = false;
	private bool isReachUpTarget = false;
	private bool isMovingDown = false;
	private bool isReachDownTarget= false;

	private bool isDestroyed = false;

	private GameDataManager gameDataManager;
	private SoundManager soundManager;
	private ParticleManager particleManager;

	private ObjectPositionController objectPositionController;

	public bool isCoinBrick =false;
	public GameObject coinPrefab;
	private List<GameObject> coins = new List<GameObject>();
	private CoinJumperController currentCoinJumperController;

	private GameObject currentItem;

	private void Start(){
		objectPositionController = this.gameObject.GetComponent<ObjectPositionController>();
		originalPosition = this.gameObject.transform.position;
		cacheItemCount = itemCount;
		if(hasTargetBrick){
			Reset();
			//ShowHideOriginalBrick(true);
			//ShowHideTargetBrick(false);
		}

		gameDataManager = GameDataManager.GetInstance();
		soundManager = SoundManager.GetInstance();
		particleManager = ParticleManager.GetInstance();
		AddEvenyListener();
	}

	private void OnDestroy(){
		RemoveEventListener();
	}

	private void AddEvenyListener(){
		gameDataManager.OnGameRestart+=OnGameRestart;
	}

	private void RemoveEventListener(){
		gameDataManager.OnGameRestart-=OnGameRestart;
	}

	private void OnGameRestart(){
		Destroy(currentItem);
		Reset();
	}

	private void ShowHideTargetBrick(bool val){
		if(targetBrick!=null){
			targetBrick.SetActive(val);
		}
	}

	private void ShowHideOriginalBrick(bool val){
		if(originalBrick!=null){
			originalBrick.SetActive(val);
		}
	}

	public void DestroyBlock(){
		if(isDestroyable && !isDestroyed){
			isDestroyed = true;
			soundManager.PlaySfx(SFX.DestroyBrick);
			gameDataManager.UpdateScore(ScoreValue.BRICK);
			ShowDestroyParticle();
			//Invoke("RemoveBrick",0.2f);
			Invoke(Task.RemoveBrick.ToString(),0.2f);
		}
	}

	private void RemoveBrick(){
		objectPositionController.DeactivateAndReposition();
		//Destroy(this.gameObject);
	}

	private void Update(){
		if(allowToMove && isMoving){
			if(isMovingUp && !isReachUpTarget){
				MoveUp();
			}else if(isMovingDown && !isReachDownTarget){
				MoveDown();
			}
		}
	}

	public void AnimateBlock(){
		this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, targetPosition,smooth * Time.deltaTime );
	}

	private void Bounce(float speed){
		//targetPosition = new Vector3(originalPosition.x, Mathf.PingPong(Time.time * speed, 14) - 4, originalPosition.z);
		//targetPosition = new Vector3(originalPosition.x, Mathf.Sin(Time.time * speed) * 7 + 3, originalPosition.z);
		//targetPosition = new Vector3(originalPosition.x,originalPosition.y + Mathf.Sin(Time.time * speed) * targetValue, originalPosition.z);
	}

	public void MoveUp(){
		targetPosition = new Vector3(originalPosition.x,originalPosition.y + targetValue,originalPosition.z);
		this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, targetPosition,smooth * Time.deltaTime );
		if(this.gameObject.transform.position.y >=  (originalPosition.y + targetValue) - 0.5f){
			isMovingUp = false;
			isReachUpTarget = true;
			isMovingDown = true;
		}
	}

	public void MoveDown(){
		targetPosition = new Vector3(originalPosition.x,originalPosition.y,originalPosition.z);
		this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, targetPosition,smooth * Time.deltaTime );
		float targetDown = targetPosition.y + 0.00950f;
		if(this.gameObject.transform.position.y<= targetDown){
			isReachDownTarget = true;
			isMoving = false;
			isMovingDown = false;

			if(!isCoinBrick){
				allowToMove = false;
			}

			CheckForItem();
		}
	}

	private void CheckForItem(){
		if(hasItem){
			itemCount--;
			if(!isCoinBrick){
				SummonItem();
			}else{
				SummonCoin();
			}
		}

		if(itemCount <=0){
			hasItem = false;
			if(hasTargetBrick){
				ShowHideOriginalBrick(false);
				ShowHideTargetBrick(true);
			}
		}
	}

	public void AnimateBrick(){
		if(allowToMove && !isMoving && !isMovingUp && !isMovingDown){
			isReachUpTarget = false;
			isReachDownTarget = false;
			isMoving = true;
			isMovingUp = true;
		}
	}

	private void SummonCoin(){
		soundManager.PlaySfx(SFX.CoinSfx);
		Vector3 newPosition = this.gameObject.transform.position;
		newPosition.y += 1f; 
		Quaternion newRotation = this.gameObject.transform.rotation;

		GameObject coin = GetCoin();
		if(coin==null){
			coin = Instantiate( coinPrefab,newPosition,newRotation) as GameObject;
			coin.transform.parent = this.gameObject.transform;
			coin.AddComponent<CoinJumperController>();
			coins.Add(coin);
		}else{
			coin.SetActive( true );
			if(currentCoinJumperController!=null){
				currentCoinJumperController.AddJumpForce();
			}
			//Debug.Log("reuse coin for brick coin");
		}
	}

	private void SummonItem(){
		soundManager.PlaySfx(SFX.PowerupAppear);
		Vector3 newPosition = this.gameObject.transform.position;
		newPosition.y += 0.5f; 
		Quaternion newRotation = this.gameObject.transform.rotation;

		if(items.Length > 0){
			int randomItem = Random.Range(0,items.Length);
			currentItem = Instantiate( items.GetValue(randomItem) as Object,newPosition,newRotation) as GameObject;
			currentItem.SetActive( true );
			currentItem.transform.parent = this.gameObject.transform;
		}

		Invoke("CheckForItem", 1.5f);
	}

	private void ShowDestroyParticle(){
		soundManager.PlaySfx(SFX.SmallExplosion);		
		Vector3 newPosition =	this.gameObject.transform.position;
		//newPosition.y += 2f;
		Vector3 scale = new Vector3(7f,7f,7f);
		particleManager.CreateParticle(ParticleEffect.BrickDestroy,newPosition,scale);
	}


	private void Reset(){
		isDestroyed = false;
		ShowHideOriginalBrick(true);
		ShowHideTargetBrick(false);
		itemCount =  cacheItemCount;
		hasItem = true;
		hasTargetBrick = true;
		allowToMove = true;
	}

	private GameObject GetCoin(){
		int coinCount = coins.Count;
		GameObject foundCoin =null;

		for(int index =0; index<coinCount; index++){
			if(coins[index]!=null){
				currentCoinJumperController = coins[index].gameObject.GetComponent<CoinJumperController>();
				if(currentCoinJumperController!=null){
					if(!currentCoinJumperController.isActive){
						foundCoin = coins[index];
						break;
					}
				}
			}
		}

		return foundCoin;
	}
}
