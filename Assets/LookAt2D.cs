using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt2D : MonoBehaviour {
	//public GameObject player;
	Vector3 target;
	// Use this for initialization
	void Start () {
		
	}

	void Update () {

		if (this.transform.GetComponent<ParticleSystem> ().time >=10) {
			Destroy (this.gameObject);
		}

		transform.LookAt(target,  Vector3.up);

	}
		
	public void setTarget (Vector2 lastCollision){
		target = lastCollision;
	}

}
