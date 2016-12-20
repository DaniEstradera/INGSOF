using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {
	public GameObject player;
	float targetAngle;
	float currentAngle;
	public float speed;
	public float rotationSpeed;
	float fix = 100f;
	float distance2Player;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void FixedUpdate () {
		//distance2Player = Vector2.Distance((new Vector2 (player.transform.localPosition.x, player.transform.localPosition.y)), (new Vector2 (this.transform.position.x, this.transform.position.y)));

		targetAngle = normalizeAngle (Mathf.Rad2Deg * Mathf.Atan2 (targetRotation ().y, targetRotation ().x));
		currentAngle = normalizeAngle (Mathf.Rad2Deg * Mathf.Atan2 (GetComponent<Rigidbody2D> ().velocity.y, GetComponent<Rigidbody2D> ().velocity.x));

		GetComponent<Rigidbody2D> ().velocity = getDeg2Coords (Mathf.MoveTowardsAngle (currentAngle, targetAngle, rotationSpeed*fix)) * speed;
		if (fix > 1) {
			fix -= Time.fixedDeltaTime * 1000;
		} else
			fix = 1;
		
	}


	void OnCollisionEnter2D(Collision2D other) {

		if (other.gameObject.tag != ("Enemy")) {
			if (other.gameObject.tag == ("Player") && PlayerController.power) {
			} else {
				GameObject original = this.transform.FindChild ("Explosion").gameObject;
				GameObject spawned = MonoBehaviour.Instantiate (original);
				spawned.transform.position = this.transform.position;
				spawned.SetActive (true);
				spawned.transform.localScale = original.transform.localScale;
				spawned.GetComponent<SpriteRenderer> ().material = new Material (original.GetComponent<SpriteRenderer> ().material);

				Destroy (this.gameObject);
			}
		}
	}
		
	Vector2 targetRotation () {

		Vector2 targetCoords = player.transform.localPosition - transform.localPosition;
		float module = Mathf.Sqrt (targetCoords.x * targetCoords.x + targetCoords.y * targetCoords.y);
		Vector2 targetVector = targetCoords / module;
		return targetVector;
	}	

	float normalizeAngle (float angle){
		if (angle < 0)
			angle += 360;
		else if (angle >= 360)
			angle -= 360;
		return angle;
	}

	Vector2 getDeg2Coords (float angle){
		return new Vector2 (Mathf.Cos (Mathf.Deg2Rad*angle), Mathf.Sin (Mathf.Deg2Rad*angle));
	}
	Vector2 getRad2Coords (float angle){
		return new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
	}
}
