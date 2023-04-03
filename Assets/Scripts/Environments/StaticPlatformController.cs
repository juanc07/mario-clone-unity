using UnityEngine;
using System.Collections;

public class StaticPlatformController : MonoBehaviour {

	public BoxCollider boxCollider{set;get;}

	// Use this for initialization
	void Start () {
		boxCollider = this.gameObject.GetComponent<BoxCollider>();
	}
}
