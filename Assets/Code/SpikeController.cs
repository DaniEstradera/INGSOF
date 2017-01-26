using UnityEngine;
using System.Collections;

public class SpikeController : MonoBehaviour {
	public GameObject player;
	public GameObject playerCoop;

	float distance2PlayerOne;
	float distance2PlayerCoop;
	float distance2Player;

	float targetAngle;
	float currentAngle;
	public AnimationCurve attackCurve;
	Vector3 originalScale;
	float t = 0;
	bool attacking;
	public float speed;
	// Use this for initialization
	void Start () {
		originalScale = this.transform.localScale;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		distance2PlayerOne = Vector2.Distance((new Vector2 (player.transform.localPosition.x, player.transform.localPosition.y)), (new Vector2 (this.transform.position.x, this.transform.position.y)));

		if (playerCoop.activeInHierarchy) { 
			distance2PlayerCoop = Vector2.Distance((new Vector2 (playerCoop.transform.localPosition.x, playerCoop.transform.localPosition.y)), (new Vector2 (this.transform.position.x, this.transform.position.y)));
			distance2Player = Mathf.Min (distance2PlayerOne, distance2PlayerCoop);

		} else distance2Player = distance2PlayerOne;

		if (attacking) {

			if (t < 1) {
				t += Time.fixedDeltaTime * speed;
			} else {
				t = 0;
				attacking = false;
			}

			this.transform.localScale = new Vector3 (originalScale.x, originalScale.y * attackCurve.Evaluate(t), originalScale.z);

			if (t > 0.1f) {
				transform.up = Vector3.Lerp (transform.up, new Vector3 (targetRotation (selectPlayer()).x, targetRotation (selectPlayer()).y, 0), Time.fixedDeltaTime * 3);
			}

		} else if (distance2Player >= 3) {
			transform.up = Vector3.Lerp (transform.up, new Vector3 (targetRotation(selectPlayer()).x , targetRotation(selectPlayer()).y, 0),Time.fixedDeltaTime*3);
		} else {
			attacking = true;

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
