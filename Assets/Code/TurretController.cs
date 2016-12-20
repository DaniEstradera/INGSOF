using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour {
	public GameObject player;
	float distance2Player;
	float targetAngle;
	float currentAngle;
	bool attacking;
	float t = 0;
	public float speed;
	Vector3 originalScale;
	public AnimationCurve attackCurve;

	// Use this for initialization
	void Start () {
		originalScale = this.transform.FindChild ("Charge").gameObject.transform.localScale;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		distance2Player = Vector2.Distance((new Vector2 (player.transform.localPosition.x, player.transform.localPosition.y)), (new Vector2 (this.transform.position.x, this.transform.position.y)));
		//transform.up = Vector3.Lerp (transform.up, new Vector3 (targetRotation().x , targetRotation().y, 0),Time.fixedDeltaTime*3);


		if (attacking) {

			if (t < 1) {
				t += Time.fixedDeltaTime * speed;
			} else {
				t = 0;
				SpawnEffect ("Projectile");
				attacking = false;
			}

			this.transform.FindChild ("Charge").gameObject.transform.localScale = originalScale * attackCurve.Evaluate (t);

			if (t > 0.1f) {
				transform.up = Vector3.Lerp (transform.up, new Vector3 (targetRotation ().x, targetRotation ().y, 0), Time.fixedDeltaTime * 3);
			}

		} else if (distance2Player >= 5) {
			transform.up = Vector3.Lerp (transform.up, new Vector3 (targetRotation().x , targetRotation().y, 0),Time.fixedDeltaTime*3);
		} else {
			attacking = true;

		}
			
	}



	void SpawnEffect(string s) {
		GameObject original = this.transform.FindChild (s).gameObject;
		GameObject spawned = MonoBehaviour.Instantiate (original);
		spawned.transform.position = original.transform.position;
		spawned.SetActive(true);
		spawned.transform.localScale = original.transform.lossyScale;
		spawned.GetComponent<SpriteRenderer> ().material = new Material (original.GetComponent<SpriteRenderer> ().material);

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
