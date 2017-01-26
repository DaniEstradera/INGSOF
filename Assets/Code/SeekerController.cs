using UnityEngine;
using System.Collections;

public class SeekerController : MonoBehaviour {
	public GameObject player;
	public GameObject playerCoop;

	float targetAngle;
	float currentAngle;
	public float speed;
	public float rotationSpeed;

	float distance2PlayerOne;
	float distance2PlayerCoop;

	float distance2Player;

	Vector3 spawnPos;

	bool followPlayer = false;

	// Use this for initialization
	void Start () {
		spawnPos = this.transform.position;
	}

	// Update is called once per frame
	void FixedUpdate () {
		
		distance2PlayerOne = Vector2.Distance((new Vector2 (player.transform.localPosition.x, player.transform.localPosition.y)), (new Vector2 (this.transform.position.x, this.transform.position.y)));

		if (playerCoop.activeInHierarchy) { 
			distance2PlayerCoop = Vector2.Distance((new Vector2 (playerCoop.transform.localPosition.x, playerCoop.transform.localPosition.y)), (new Vector2 (this.transform.position.x, this.transform.position.y)));
			distance2Player = Mathf.Min (distance2PlayerOne, distance2PlayerCoop);

		} else distance2Player = distance2PlayerOne;

		//distance2PlayerCoop = Vector2.Distance((new Vector2 (playerCoop.transform.localPosition.x, playerCoop.transform.localPosition.y)), (new Vector2 (this.transform.position.x, this.transform.position.y)));

		

		if (followPlayer) {
			targetAngle = normalizeAngle (Mathf.Rad2Deg * Mathf.Atan2 (targetRotation (selectPlayer()).y, targetRotation (selectPlayer()).x));
			currentAngle = normalizeAngle (Mathf.Rad2Deg * Mathf.Atan2 (GetComponent<Rigidbody2D> ().velocity.y, GetComponent<Rigidbody2D> ().velocity.x));
			if (CameraController.power && distance2Player <= 5) {
				GetComponent<Rigidbody2D> ().velocity = getDeg2Coords (Mathf.MoveTowardsAngle (currentAngle, targetAngle + 180, rotationSpeed)) * speed * 4 / distance2Player;
				transform.up = Vector3.Lerp (transform.up, new Vector3 (targetRotation(selectPlayer()).x , targetRotation(selectPlayer()).y, 0),Time.fixedDeltaTime*20);
			} else {
				GetComponent<Rigidbody2D> ().velocity = getDeg2Coords (Mathf.MoveTowardsAngle (currentAngle, targetAngle, rotationSpeed)) * speed;

				transform.up = Vector3.Lerp (transform.up, GetComponent<Rigidbody2D> ().velocity, Time.fixedDeltaTime);
			}
		} else 
			GetComponent<Rigidbody2D> ().velocity = Vector3.zero;

		if (distance2Player <= 7) {
			followPlayer = true;
		}
		
		if (CameraController.power) {
			this.transform.FindChild ("CharPow").gameObject.SetActive (true);

			if (this.transform.FindChild ("CharPow").gameObject.GetComponent<SpriteRenderer> ().color.r <= 1)
				this.transform.FindChild ("CharPow").gameObject.GetComponent<SpriteRenderer> ().color += new Color (Time.fixedDeltaTime, Time.fixedDeltaTime, Time.fixedDeltaTime, 0) * 10;
			else {
				this.transform.FindChild ("TailRPow").GetComponent<TrailRenderer> ().time = 0.7f;
				this.transform.FindChild ("TailLPow").GetComponent<TrailRenderer> ().time = 0.7f;
			}

		} else {
			
			if (this.transform.FindChild ("TailRPow").GetComponent<TrailRenderer> ().time > 0f) {
				this.transform.FindChild ("TailRPow").GetComponent<TrailRenderer> ().time -= Time.fixedDeltaTime*2;
				this.transform.FindChild ("TailLPow").GetComponent<TrailRenderer> ().time -= Time.fixedDeltaTime*2;
			} else if (this.transform.FindChild ("CharPow").gameObject.GetComponent<SpriteRenderer> ().color.r >= 0) {
				this.transform.FindChild ("CharPow").gameObject.GetComponent<SpriteRenderer> ().color -= new Color (Time.fixedDeltaTime, Time.fixedDeltaTime, Time.fixedDeltaTime, 0) * 10;
			} else {
				this.transform.FindChild ("CharPow").gameObject.SetActive (false);
			}
			
		}
			
	}
		


	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == ("Player")) {
			if (CameraController.power) {
				this.transform.FindChild ("TailRPow").gameObject.SetActive (false);
				this.transform.FindChild ("TailLPow").gameObject.SetActive (false);
				this.transform.FindChild ("TailR").gameObject.SetActive (false);
				this.transform.FindChild ("TailL").gameObject.SetActive (false);

				Destroy (this.gameObject);
				transform.position = spawnPos;
				followPlayer = false;
				GetComponent<Rigidbody2D> ().velocity = Vector3.zero;


				this.transform.FindChild ("TailRPow").gameObject.SetActive (true);
				this.transform.FindChild ("TailLPow").gameObject.SetActive (true);
				this.transform.FindChild ("TailR").gameObject.SetActive (true);
				this.transform.FindChild ("TailL").gameObject.SetActive (true);

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

		//float module = Mathf.Sqrt (targetCoords.x * targetCoords.x + targetCoords.y * targetCoords.y);
		targetCoords.Normalize();
		return targetCoords;//targetVector
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
