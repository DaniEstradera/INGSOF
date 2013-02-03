using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {
	private float speed = 6f;
	private float maxSpeed =6f;
	private float friction = 5f;
	private float rotationSpeed = 8f;
	private float targetAngle;
	private float currentAngle;
	private float delay = 0.2f;
	private SpriteRenderer sr;
	private GameObject GameMode;
    private bool deathState = false;

    //public Camera camera;

    public Color normalColor;
	public Color chargedColor;

	void Start(){
		sr = GetComponent<SpriteRenderer>();
		GameMode = GameObject.Find("HUD");

	}

	void FixedUpdate () {

		targetAngle = normalizeAngle(Mathf.Rad2Deg * Mathf.Atan2(targetRotation().y, targetRotation().x));
		currentAngle = normalizeAngle(Mathf.Rad2Deg * Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.x));


		GetComponent<Rigidbody2D>().velocity = getDeg2Coords (Mathf.MoveTowardsAngle(currentAngle, targetAngle, 36/speed+4)) * speed;
		decceleration ();

	}

	Vector2 targetRotation () {
		Vector2 targetCoords = (Vector2)Input.mousePosition - new Vector2 (Screen.width/2, Screen.height/2);
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

	void decceleration(){

		if (speed > maxSpeed) {
			sr.color = chargedColor;
			if (delay > 0) {
				delay -= Time.fixedDeltaTime;
			} else {
				speed -= friction;
			}
		} else {
			sr.color = normalColor;
			speed = maxSpeed;
		}
	
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == ("Bouncer"))
			delay = 0.8f;
			speed = 12f;

	}

	public void death () {
        deathState = true;
		GameMode.GetComponent<GameMode>().GameIsOver = true;
        GameMode.GetComponent<GameMode>().StateOfTheGame = "GameOver";
    }

    public void Win()
    {
        deathState = true;
        GameMode.GetComponent<GameMode>().GameIsOver = true;
        GameMode.GetComponent<GameMode>().StateOfTheGame = "WinGame";
    }
}
	