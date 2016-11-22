using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour {

	public static float playerSpeed = 6f;
	public static bool power;

	private float currentSpeed;
	private float powerSpeed = playerSpeed * 2f;

	private float delay = 0.4f;

	private float targetAngle;
	private float currentAngle;

	public GameObject dash;
	public GameObject spark;
	public GameObject spark2;
	public GameObject deathBlanket;

	bool timeWarp;

	//float 

	void Start(){
		
	}

	void FixedUpdate () {
		
		targetAngle = normalizeAngle(Mathf.Rad2Deg * Mathf.Atan2(targetRotation().y, targetRotation().x));
		currentAngle = normalizeAngle(Mathf.Rad2Deg * Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.x));
		GetComponent<Rigidbody2D>().velocity = getDeg2Coords (Mathf.MoveTowardsAngle(currentAngle, targetAngle, 36f/currentSpeed+4f)) * currentSpeed;
		updateSpeed (); 
		dashParticles ();

	}

	Vector2 targetRotation () {
		Vector3 thisPositionInCamera = Camera.main.WorldToScreenPoint (this.transform.position);
		Vector2 targetCoords = (Vector2)Input.mousePosition - new Vector2 (thisPositionInCamera.x, thisPositionInCamera.y);
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

	void updateSpeed(){
		if (currentSpeed > playerSpeed) {
			if (delay > 0)
				delay -= Time.fixedDeltaTime;
			else
				currentSpeed -= 12f / currentSpeed;	
		} else {
			currentSpeed = playerSpeed;
			power = false;
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == ("Bouncer")){
			delay = 0.4f;
			currentSpeed = powerSpeed;
			power = true;
			SpawnEffect ("ShockWave");
			SpawnEffect ("Bubble");
			SpawnEffect ("Blast");
			//sparkParticles ();
			//Camera.main.GetComponent<CameraController> ().SetShake (0.1f);
			//timeWarp = true;
		}

		if (other.gameObject.tag == ("Enemy")){

			if (!power) {
				death ();
			}

			delay = 0.4f;
			currentSpeed = powerSpeed;
			power = true;
			SpawnEffect ("ShockWave");
			SpawnEffect ("Bubble");
			SpawnEffect ("Blast");
			//sparkParticles ();
			Camera.main.GetComponent<CameraController> ().SetShake (0.1f);
			timeWarp = true;

		}


	}



	void Update() {
		if (timeWarp) {
			if (Time.timeScale > 0.2F)
				Time.timeScale = 0.2F;
			else
				timeWarp = false;
		} else {
			if (Time.timeScale < 1.0F)
				Time.timeScale += 0.1f;
			else
				Time.timeScale = 1;
		}
	}

	void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.tag == ("Bouncer")){
			sparkParticles (spark);
			sparkParticles (spark2);
		}

	}

	void SpawnEffect(string s) {
		GameObject original = this.transform.FindChild (s).gameObject;
		GameObject spawned = MonoBehaviour.Instantiate (original);
		spawned.transform.position = this.transform.position;
		spawned.SetActive(true);
		spawned.transform.localScale = original.transform.localScale;
		spawned.GetComponent<SpriteRenderer> ().material = new Material (original.GetComponent<SpriteRenderer> ().material);
	}

	void dashParticles (){
		
		ParticleSystem aux = dash.GetComponent<ParticleSystem>();
		var em = aux.emission;
		em.enabled = true;
		em.type = ParticleSystemEmissionType.Time;

		if (power)
			em.rate = 40f;
		else 
			em.rate = 0f;
		
		dash.transform.eulerAngles = new Vector3 (0, 0,currentAngle+90);
	}

	void sparkParticles (GameObject spark){
		spark.SetActive (true);
		spark.GetComponent<ParticleSystem>().time = 0;
		spark.transform.eulerAngles = new Vector3 (0, 0,currentAngle+90-135);
		spark.GetComponent<ParticleSystem>().Play(true);

	}

	void death () {
		deathBlanket.SetActive(true);

	}

}
	