using UnityEngine;
using System.Collections;

public class SeekerController : MonoBehaviour {
	public GameObject player;
	float targetAngle;
	float currentAngle;
	public float speed;
	public float rotationSpeed;

    float distance2Player;

	Vector3 spawnPos;

	bool followPlayer = false;
    bool LastUpdatePlayerPower = false;
    private AudioSource source;
    public AudioClip DodgeSound;

    // Use this for initialization
    void Start () {
		spawnPos = this.transform.position;
        GetComponent<AudioSource>().playOnAwake = false;
        //GetComponent<AudioSource>().clip = saw;
    }

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate () {
		distance2Player = Vector2.Distance((new Vector2 (player.transform.localPosition.x, player.transform.localPosition.y)), (new Vector2 (this.transform.position.x, this.transform.position.y)));
		if (followPlayer) {
			targetAngle = normalizeAngle (Mathf.Rad2Deg * Mathf.Atan2 (targetRotation ().y, targetRotation ().x));
			currentAngle = normalizeAngle (Mathf.Rad2Deg * Mathf.Atan2 (GetComponent<Rigidbody2D> ().velocity.y, GetComponent<Rigidbody2D> ().velocity.x));
			if (PlayerController.power && distance2Player <= 5) {
				GetComponent<Rigidbody2D> ().velocity = getDeg2Coords (Mathf.MoveTowardsAngle (currentAngle, targetAngle + 180, rotationSpeed)) * speed * 4 / distance2Player;
				transform.up = Vector3.Lerp (transform.up, new Vector3 (targetRotation().x , targetRotation().y, 0),Time.fixedDeltaTime*20);
			} else {
				GetComponent<Rigidbody2D> ().velocity = getDeg2Coords (Mathf.MoveTowardsAngle (currentAngle, targetAngle, rotationSpeed)) * speed;

				transform.up = Vector3.Lerp (transform.up, GetComponent<Rigidbody2D> ().velocity, Time.fixedDeltaTime);
			}
		} else 
			GetComponent<Rigidbody2D> ().velocity = Vector3.zero;

		if (distance2Player <= 7) {
			followPlayer = true;
		}
		
		if (PlayerController.power) {
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
        if (PlayerController.power && !LastUpdatePlayerPower) {
            OnDodgePlayer();
        }
        LastUpdatePlayerPower = PlayerController.power;

    }
		
    void OnDodgePlayer() {
        Debug.Log("DODGE");
        source.PlayOneShot(DodgeSound);
    }

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == ("Player")) {
			if (PlayerController.power) {
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
