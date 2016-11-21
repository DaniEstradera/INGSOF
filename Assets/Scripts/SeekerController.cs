using UnityEngine;
using System.Collections;

public class SeekerController : MonoBehaviour {
	public GameObject player;
	float targetAngle;
	float currentAngle;
	public float speed;
	public float rotationSpeed;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void FixedUpdate () {

		targetAngle = normalizeAngle(Mathf.Rad2Deg * Mathf.Atan2(targetRotation().y, targetRotation().x));
		currentAngle = normalizeAngle(Mathf.Rad2Deg * Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.x));
		GetComponent<Rigidbody2D>().velocity = getDeg2Coords (Mathf.MoveTowardsAngle(currentAngle, targetAngle,rotationSpeed)) * speed;


	}

	Vector2 targetRotation () {
		//Vector3 thisPositionInCamera = Camera.main.WorldToScreenPoint (this.transform.position);
		Vector2 targetCoords = player.transform.localPosition - transform.localPosition;//(Vector2)Input.mousePosition - new Vector2 (thisPositionInCamera.x, thisPositionInCamera.y);
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
