using UnityEngine;
using System.Collections;

public class Seeker : MonoBehaviour {

	private GameObject Player;

	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Ball");
	}
	
	// Update is called once per frame
	void FixedUpdate() {

		transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, 0.05f);
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == ("Player")) {
            other.gameObject.GetComponent<BallController>().death();
		}
	}

}
