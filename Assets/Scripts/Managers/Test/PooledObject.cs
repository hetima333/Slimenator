using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour {

	void OnBecameInvisible(){
		// cube releases when invisible from camera
		// カメラから見えなくなったらキューブは解放される
		ObjectManager.Instance.ReleaseObject(this.gameObject);
	}

	void OnCollisionEnter(Collision col){
		// cube releases when hit plane
		// 床に接触したときにキューブは解放される
		if(col.gameObject.name == "Plane"){
			ObjectManager.Instance.ReleaseObject(this.gameObject);
		}
	}
}
