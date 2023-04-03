using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Player{
	public int coin;
	private Action CoinUpdate;
	public event Action OnCoinUpdate{
		add{CoinUpdate+=value;}
		remove{CoinUpdate-=value;}
	}

	public int hp;
	private Action HpUpdate;
	public event Action OnHpUpdate{
		add{ HpUpdate+=value;}
		remove{HpUpdate-=value;}
	}

	public int life;
	private Action LifeUpdate;
	public event Action OnLifeUpdate{
		add{ LifeUpdate+=value;}
		remove{LifeUpdate-=value;}
	}

	public bool isDead;
	private Action PlayerDead;
	public event Action OnPlayerDead{
		add{PlayerDead+=value;}
		remove{PlayerDead-=value;}
	}

	public bool isInvulnerable;
	private Action <bool>PlayerInvulnerableChange;
	public event Action <bool>OnPlayerInvulnerableChange{
		add{PlayerInvulnerableChange+=value;}
		remove{PlayerInvulnerableChange-=value;}
	}

	public bool isGotFireball;
	private Action <bool>GotFireball;
	public event Action <bool>OnGotFireball{
		add{GotFireball+=value;}
		remove{GotFireball-=value;}
	}

	private Action PlayerRevive;
	public event Action OnPlayerRevive{
		add{PlayerRevive+=value;}
		remove{PlayerRevive-=value;}
	}

	private int score;
	private Action ScoreUpdate;
	public event Action OnScoreUpdate{
		add{ScoreUpdate+=value;}
		remove{ScoreUpdate-=value;}
	}

	private int hiScore;
	private Action HiScoreUpdate;
	public event Action OnHiScoreUpdate{
		add{HiScoreUpdate+=value;}
		remove{HiScoreUpdate-=value;}
	}

	private int level;
	private Action LevelUpdate;
	public event Action OnLevelUpdate{
		add{LevelUpdate+=value;}
		remove{LevelUpdate-=value;}
	}

	private Action MaxCoin;
	public event Action OnMaxCoin{
		add{MaxCoin+=value;}
		remove{MaxCoin-=value;}
	}

	public int Score{
		set{ score = value;
			if(null!=ScoreUpdate ){
				ScoreUpdate();
			}
		}
		get{ return score;}
	}

	public int HiScore{
		set{ hiScore = value;
			if(null!=HiScoreUpdate ){
				HiScoreUpdate();
			}
		}
		get{ return hiScore;}
	}

	public int Level{
		set{ level = value;
			if(null!=LevelUpdate ){
				LevelUpdate();
			}
		}
		get{ return level;}
	}


	public int Coin{
		set{coin =value;
			if(coin >=100){
				coin = 0;
				//life++;
				Life++;
				if(null!=MaxCoin){
					MaxCoin();
				}
			}
			if(null!= CoinUpdate){
				CoinUpdate();
			}
			//Debug.Log(" coin update ");
		}
		get{return coin;}
	}

	public int HP{
		set{hp =value;
			if(null!= HpUpdate){
				HpUpdate();
			}
		}
		get{return hp;}
	}

	public int Life{
		set{life =value;
			if(null!= LifeUpdate){
				LifeUpdate();
			}
		}
		get{return life;}
	}

	public bool IsDead{
		set{
			isDead =value;
			if(isDead){
				if(null!=PlayerDead){
					PlayerDead();
				}
			}else if(!isDead){
				if(null!= PlayerRevive){
					hp =3;
					PlayerRevive();
				}
			}
		}

		get{return isDead;}
	}

	public bool IsInvulnerable{
		set{ isInvulnerable = value;
			if(null!=PlayerInvulnerableChange){
				PlayerInvulnerableChange(isInvulnerable);
			}
		}

		get{
			return isInvulnerable;
		}
	}

	public bool IsGotFireball{
		set{ isGotFireball = value;
			if(null!=GotFireball){
				GotFireball(isGotFireball);
			}
		}
		
		get{
			return isGotFireball;
		}
	}
}
