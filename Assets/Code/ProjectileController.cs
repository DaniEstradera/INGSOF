using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {
	public GameObject player;
	public GameObject playerCoop;
	float targetAngle;
	float currentAngle;
	public float speed;
	public float rotationSpeed;
	float fix = 100f;
	float distance2PlayerOne;
	float distance2PlayerCoop;
	//float distance2Player;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void FixedUpdate () {
		//distance2Player = Vector2.Distance((new Vector2 (player.transform.localPosition.x, player.transform.localPosition.y)), (new Vector2 (this.transform.position.x, this.transform.position.y)));
		distance2PlayerOne = Vector2.Distance((new Vector2 (player.transform.localPosition.x, player.transform.localPosition.y)), (new Vector2 (this.transform.position.x, this.transform.position.y)));
		if (playerCoop.activeInHierarchy) { 
			distance2PlayerCoop = Vector2.Distance ((new Vector2 (playerCoop.transform.localPosition.x, playerCoop.transform.localPosition.y)), (new Vector2 (this.transform.position.x, this.transform.position.y)));
		}

		targetAngle = normalizeAngle (Mathf.Rad2Deg * Mathf.Atan2 (targetRotation (selectPlayer()).y, targetRotation (selectPlayer()).x));
		currentAngle = normalizeAngle (Mathf.Rad2Deg * Mathf.Atan2 (GetComponent<Rigidbody2D> ().velocity.y, GetComponent<Rigidbody2D> ().velocity.x));

		GetComponent<Rigidbody2D> ().velocity = getDeg2Coords (Mathf.MoveTowardsAngle (currentAngle, targetAngle, rotationSpeed*fix)) * speed;
		if (fix > 1) {
			fix -= Time.fixedDeltaTime * 1000;
		} else
			fix = 1;
		
	}


	void OnCollisionEnter2D(Collision2D other) {

		if (other.gameObject.tag != ("Enemy")) {
			if (other.gameObject.tag == ("Player") && CameraController.power) {
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
		
	GameObject selectPlayer() {

		if (distance2PlayerCoop < distance2PlayerOne && playerCoop.activeInHierarchy) {
			return playerCoop;
		} else
			return player;
	}

	Vector2 targetRotation (GameObject targetPlayer) {

		Vector2 targetCoords = targetPlayer.transform.localPosition - transform.localPosition;
		targetCoords.Normalize();
		return targetCoords;
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
