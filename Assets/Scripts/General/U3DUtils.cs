using UnityEngine;
using System.Collections;

public class U3DUtils : MonoBehaviour {

	public static Vector3 SetVector3D( Vector3 origin, float targetX =0,float targetY =0, float targetZ = 0){
		Vector3 temp = origin;
		temp.x = targetX;
		temp.y = targetY;
		temp.z = targetZ;
		return temp;
	}
}
