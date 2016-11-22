using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == ("Player")) {
			if (PlayerController.power) {
				//MonoBehaviour.Destroy (this.gameObject);
				transform.position = Vector2.zero;
			}
		}
	}
}
