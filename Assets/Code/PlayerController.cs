﻿using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour {

	public static float playerSpeed = 6f;
	//public static bool //power;
	public bool power;

	private float currentSpeed;
	private float powerSpeed = playerSpeed * 2f;

	private float powerDuration = 0.4f;

	private float targetAngle;
	private float currentAngle;
    
    private float axisX;
    private float axisY;
    //private Vector2 axisVector;

    private GameObject GameMode;
	private bool deathState = false;

	public GameObject deathBlanket;
    public GameObject winBlanket;
    public bool useGamePad;
    public int playerNumber;

    public AudioClip[] BounceSound;
    public AudioClip WinSound;
    public AudioClip DeathSound;
    public AudioClip NullSound;
    private AudioSource source;
    private int BounceSoundCount=0;
    private float TimeBetweenBounces=0;

    bool isOnEndGame=false;
    bool timeWarp;

    void Awake() {
        source = GetComponent<AudioSource>();
        isOnEndGame = false;
        GameSettings Settings = GameObject.Find("GameSettings").GetComponent<GameSettings>();
        if (Settings.DualStick) {
            playerNumber = (playerNumber == 2 ? 9 : playerNumber);
            useGamePad = true;
        } else {
            useGamePad = (playerNumber == 2 ? Settings.Player2GamePad : Settings.Player1GamePad);
        }
    }

	void Start(){
		GameMode = GameObject.Find("HUD");
    }

	void FixedUpdate () {
		if (deathState)
		{
			SpawnEffect("ShockWave");
			SpawnEffect("Bubble");
			SpawnEffect("Blast");
			return;
		}
        
		targetAngle = normalizeAngle(Mathf.Rad2Deg * Mathf.Atan2(targetRotation().y, targetRotation().x));
		currentAngle = normalizeAngle(Mathf.Rad2Deg * Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.x));
		//GetComponent<Rigidbody2D>().velocity = getDeg2Coords (Mathf.MoveTowardsAngle(currentAngle, targetAngle, 36f/currentSpeed+4f)) * currentSpeed;
		GetComponent<Rigidbody2D>().velocity = getDeg2Coords (Mathf.MoveTowardsAngle(currentAngle, targetAngle, 36f/currentSpeed+10f)) * currentSpeed;
		transform.up = GetComponent<Rigidbody2D> ().velocity;

		if (transform.eulerAngles.y == 180){ //patch
			transform.right = GetComponent<Rigidbody2D> ().velocity;
		}


		updateSpeed (); 
		dashParticles ();
		if (power) {
			this.transform.FindChild ("Pulse").gameObject.SetActive (true);
			this.transform.FindChild ("Char").gameObject.SetActive (false);
			this.transform.FindChild ("CharPow").GetComponent<TrailRenderer> ().time = 0.35f;

		} else {
			this.transform.FindChild ("Pulse").gameObject.SetActive (false);
			this.transform.FindChild ("Char").gameObject.SetActive (true);
			if (this.transform.FindChild ("CharPow").GetComponent<TrailRenderer> ().time > 0f) {
				this.transform.FindChild ("CharPow").GetComponent<TrailRenderer> ().time -= Time.fixedDeltaTime;
			}
		}
	}

	Vector2 targetRotation () {
		Vector3 thisPositionInCamera = Camera.main.WorldToScreenPoint (this.transform.position);
        Vector2 targetVector;

		if (useGamePad) {
			targetVector = GetAxis ();
			if (GetAxis().x == 0 && GetAxis().y == 0) {
				targetVector = GetComponent<Rigidbody2D> ().velocity;
				targetVector.Normalize ();

			}
		} else {
			
			Vector2 targetCoords = (Vector2)Input.mousePosition - new Vector2 (thisPositionInCamera.x, thisPositionInCamera.y);
			targetCoords.Normalize ();
			targetVector = targetCoords;
		}

		return targetVector;
	}
	
	public Vector2 GetAxis() {
		Vector2 axisVector = new Vector2 (0.0f, 0.0f);
        if (Input.GetAxis("Horizontal"+playerNumber.ToString()) != 0){
			
            axisVector.x = Input.GetAxis("Horizontal" + playerNumber.ToString());
        }
        if (Input.GetAxis("Vertical" + playerNumber.ToString()) != 0)
        {
            axisVector.y = Input.GetAxis("Vertical" + playerNumber.ToString());
        }
               
        axisVector.Normalize();

		return axisVector;
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
			if (powerDuration > 0)
				powerDuration -= Time.fixedDeltaTime;
			else
				currentSpeed -= 12f / currentSpeed;	
		} else {
			currentSpeed = playerSpeed;
			power = false;
		}
	}

	void OnCollisionEnter2D(Collision2D other) {

        AudioClip CollisionAudio = NullSound;
        if (isOnEndGame) return;
        if (other.gameObject.tag == ("Bouncer")){
			powerDuration = 0.4f;
			currentSpeed = powerSpeed;
			power = true;
			SpawnEffect ("ShockWave");
			SpawnEffect ("Bubble");
			SpawnEffect ("Blast");

            Spawnlightning(new Vector2 (this.transform.position.x, this.transform.position.y));

			this.transform.FindChild ("Shield").gameObject.SetActive(false);
			this.transform.FindChild ("Shield").gameObject.SetActive(true);
			this.transform.FindChild ("Shield").GetComponent<ShieldAnim> ().setT (0f);

            TimeBetweenBounces = Time.time - TimeBetweenBounces;
            if (TimeBetweenBounces > 1)
            {
                BounceSoundCount = 0;
            }
            TimeBetweenBounces = Time.time;
            CollisionAudio = BounceSound[BounceSoundCount];
            BounceSoundCount = Mathf.Clamp(++BounceSoundCount, 0, BounceSound.Length - 1);
        }

		if (other.gameObject.tag == ("Enemy")){

			if (!power) {
				death ();
                CollisionAudio = DeathSound;
            }

			powerDuration = 0.4f;
			currentSpeed = powerSpeed;
			power = true;
			SpawnEffect ("ShockWave");
			SpawnEffect ("Bubble");
			SpawnEffect ("Blast");
			this.transform.FindChild ("Shield").gameObject.SetActive(false);
			this.transform.FindChild ("Shield").gameObject.SetActive(true);
			this.transform.FindChild ("Shield").GetComponent<ShieldAnim> ().setT (0f);

			Camera.main.GetComponent<CameraController> ().SetShake (0.1f);
            //timeWarp = true;
            


        }
		if (other.gameObject.tag == ("Finish")) {
			win ();
            CollisionAudio = WinSound;

        }

        PlayCollisionSound(CollisionAudio);
    }
		
	void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.tag == ("Bouncer")){
			SparkParticles ("Spark");
			SparkParticles ("Spark2");
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

		GameObject dash = this.transform.FindChild ("Dash").gameObject;

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

	void Spawnlightning(Vector2 lastCollision){
		GameObject original = this.transform.FindChild ("Lightning").gameObject;
		GameObject spawned = MonoBehaviour.Instantiate (original);
		spawned.transform.position =  this.transform.position;
		spawned.transform.SetParent (this.transform);
		spawned.GetComponent<LookAt2D> ().setTarget (lastCollision);
		spawned.SetActive (true);
		spawned.GetComponent<ParticleSystem>().time = 0;
		spawned.GetComponent<ParticleSystem>().Play(true);

	}

	void SparkParticles (string s){
		GameObject spark = this.transform.FindChild (s).gameObject;
		spark.SetActive (true);
		spark.GetComponent<ParticleSystem>().time = 0;
		spark.transform.eulerAngles = new Vector3 (0, 0,currentAngle+90-135);
		spark.GetComponent<ParticleSystem>().Play(true);

	}
	public void death () {
		if (!winBlanket.activeInHierarchy)
        {
            deathBlanket.SetActive(true);
            isOnEndGame = true;
        }
		
	}
    

	public void win () {
		if (!deathBlanket.activeInHierarchy)
        {
            winBlanket.SetActive(true);
            isOnEndGame = true;
        }
            
	}


    void PlayCollisionSound(AudioClip CollisionAudio) {
        source.PlayOneShot(CollisionAudio);
    }
}
