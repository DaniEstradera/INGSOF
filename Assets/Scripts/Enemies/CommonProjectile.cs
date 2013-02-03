using UnityEngine;
using System.Collections;

public class CommonProjectile : MonoBehaviour {

	//public float speed = 10f;

	// Use this for initialization
	void Start () {
		//GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.forward * speed);

	}
	
	// Update is called once per frame
	void FixedUpdate() {
		//transform.position = Vector3.MoveTowards(transform.position, transform.position + (transform.up * speed), 1f);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == ("Player"))
		{
			other.gameObject.GetComponent<PlayerController>().death();
		}else {
			//Destroy(gameObject);
		}
	}
}
