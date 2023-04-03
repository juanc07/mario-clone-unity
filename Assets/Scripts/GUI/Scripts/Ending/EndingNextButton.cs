using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndingNextButton : MonoBehaviour {

	public GameObject[] endingSprites;
	private int ctr = 0;
	private int max =0;
	public GameObject endingLabel;
	private UILabel endingUILabel;

	private UIImageButton endButton;

	private GoTweenChain chain;
	private MySprite mySprite;
	private bool hasStarted = false;
	private bool isDoneTween = false;

	private List<UISprite> endingSpriteSet = new List<UISprite>();

	private ScenePreloader scenePreloader;
	private SoundManager soundManager;

	// Use this for initialization
	void Start () {
		soundManager = SoundManager.GetInstance();
		soundManager.OnSoundManagerReady+=OnSoundManagerReady;

		max = endingSprites.Length;

		for(int index=0;index<max;index++){
			UISprite tempUISprite =  endingSprites[index].gameObject.GetComponent<UISprite>();
			if(tempUISprite!=null){
				if(index == 0){
					tempUISprite.alpha = 1f; 
				}else{
					tempUISprite.alpha = 0f; 
				}
				endingSpriteSet.Add(tempUISprite);
			}
		}
		endingUILabel = endingLabel.gameObject.GetComponent<UILabel>();
		endButton = this.gameObject.GetComponent<UIImageButton>();
		endButton.target.alpha =0; 

		mySprite = new MySprite();
		mySprite.alpha = 1;
		mySprite.alpha2 = 0;
		mySprite.alpha3 = 0;
		mySprite.alpha4 = 0;
		mySprite.alpha5 = 0;
		mySprite.alpha6 = 0;

		AutoNext();
	}

	private void OnDestroy(){
		soundManager.OnSoundManagerReady-=OnSoundManagerReady;
	}

	private void OnSoundManagerReady(){
		soundManager.PlayBGM(BGM.EndingBGM);
	}
	
	private void OnClick(){
		if(scenePreloader==null){
			scenePreloader  = GameObject.FindObjectOfType(typeof(ScenePreloader)) as ScenePreloader;
		}
		if(isDoneTween){
			soundManager.PlayBGM(BGM.GrassBGM);
			scenePreloader.LoadScene(ScenePreloader.Scenes.Title);
		}
	}

	private void Manual(){
		ctr++;
		if(ctr>=max){
			ctr = 0;
		}
		
		if(ctr < max-1){
			endingLabel.SetActive(false); 
		}else{
			endingLabel.SetActive(true); 
		}
		
		for(int index=0;index<max;index++){
			if(index == ctr){
				endingSprites[index].gameObject.SetActive(true);
			}else{
				endingSprites[index].gameObject.SetActive(false);
			}
			
		}
		//Debug.Log("go next ending sprites");
	}

	private void AutoNext(){
		if(!hasStarted){
			hasStarted = true;
			if(soundManager.isReady){
				soundManager.PlayBGM(BGM.EndingBGM);
			}

			GoTween endingSpriteTween1 = new GoTween(mySprite,2f, new GoTweenConfig().floatProp( "alpha",0));
			GoTween endingSpriteTween2 = new GoTween(mySprite,2f, new GoTweenConfig().floatProp( "alpha2",1));
			GoTween endingSpriteTween3 = new GoTween(mySprite,2f, new GoTweenConfig().floatProp( "alpha2",0));
			GoTween endingSpriteTween4 = new GoTween(mySprite,2f, new GoTweenConfig().floatProp( "alpha3",1));
			GoTween endingSpriteTween5 = new GoTween(mySprite,2f, new GoTweenConfig().floatProp( "alpha3",0));
			GoTween endingSpriteTween6 = new GoTween(mySprite,2f, new GoTweenConfig().floatProp( "alpha4",1));
			GoTween endingSpriteTween7 = new GoTween(mySprite,2f, new GoTweenConfig().floatProp( "alpha5",1));
			GoTween endingSpriteTween8 = new GoTween(mySprite,2f, new GoTweenConfig().floatProp( "alpha6",1),OnDoneTween);
			
			chain = new GoTweenChain();
			chain.appendDelay(4.5f).append(endingSpriteTween1).append(endingSpriteTween2)
				.appendDelay(4f).append(endingSpriteTween3).append(endingSpriteTween4)
				.appendDelay(4f).append(endingSpriteTween5).append(endingSpriteTween6)
				.appendDelay(0.5f).append(endingSpriteTween7)
				.appendDelay(0.5f).append(endingSpriteTween8);
			chain.play();
		}
	}

	private void OnDoneTween(AbstractGoTween abstractGoTween){
		isDoneTween = true;
		//Debug.Log("tween is done!");
	}

	private void Update(){
		if(endingSpriteSet!=null && hasStarted){
			endingSpriteSet[0].alpha = mySprite.alpha;
			endingSpriteSet[1].alpha = mySprite.alpha2;
			endingSpriteSet[2].alpha = mySprite.alpha3;
			endingSpriteSet[3].alpha = mySprite.alpha4;
			endingUILabel.alpha = mySprite.alpha5;
			endButton.target.alpha = mySprite.alpha6;
		}
	}

	public class MySprite{
		public float alpha{set;get;}
		public float alpha2{set;get;}
		public float alpha3{set;get;}
		public float alpha4{set;get;}
		public float alpha5{set;get;}
		public float alpha6{set;get;}
	}
}
