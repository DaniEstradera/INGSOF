using UnityEngine;
using System.Collections;

public class EstelaScript : MonoBehaviour {
	float speed = 2.25f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!PlayerController.power) {
			
			if (this.GetComponent<TrailRenderer> ().time >= 0) {
				
				this.GetComponent<TrailRenderer> ().time -= Time.fixedDeltaTime * speed;
				this.GetComponent<TrailRenderer> ().endWidth -= Time.fixedDeltaTime*speed*2;
				this.GetComponent<TrailRenderer> ().startWidth -= Time.fixedDeltaTime*speed*2;

			} 
		} else {
			this.GetComponent<TrailRenderer> ().time = 0.5f;
			this.GetComponent<TrailRenderer> ().endWidth = 2;
			this.GetComponent<TrailRenderer> ().startWidth = 2;

		}

	}
}
