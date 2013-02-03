using UnityEngine;
using System.Collections;

public class SeekerExploder : MonoBehaviour {

    public Vector3 explosionRadius = new Vector3(3, 3, 3);

    private GameObject Player;
    private bool explode;
    private float speed = 0.1f;

    // Use this for initialization
    void Start () {
		Player = GameObject.Find("Ball");
	}
	
	// Update is called once per frame
	void FixedUpdate() {
        if (!explode)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, 0.05f);
        } else
        {
            transform.localScale = Vector3.Lerp(explosionRadius, transform.localScale, speed * Time.deltaTime);

        }
		
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == ("Player")) {
            other.gameObject.GetComponent<BallController>().death();
		} else {
            explode = true;
        }
	}

}
