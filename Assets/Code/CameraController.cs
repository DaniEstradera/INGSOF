using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public static int levelCount = 0;
	public static bool power;
	public int currentLevel;
	public GameObject player;
	public GameObject playerCoop;
	float distanceBetweenPlayersX;
	float distanceBetweenPlayersY;
	private Vector3 offset;
	private float shake;
	public AnimationCurve zoom;

	[Range(0,1)]
	public float amplitude = 0.2f;
	[Range(0.00001f, 0.99999f)]
	public float frequency = 0.98f;

	[Range(1,4)]
	public int octaves = 2;

	[Range(0.00001f,5)]
	public float persistance = 0.2f;
	[Range(0.00001f,100)]
	public float lacunarity = 20;

	[Range(0.00001f, 0.99999f)]
	public float burstFrequency = 0.5f;

	[Range(0,5)]
	public int  burstContrast = 2;


	void Start () {
		offset = transform.position - GetCenterOfPlayers();
		levelCount = currentLevel;

	}
		

	void Update(){
		if (Input.GetKeyDown(KeyCode.P)){
			if (Time.timeScale > 0) {
				Time.timeScale = 0;
			} else
				Time.timeScale = 1;
		}


		if (Input.GetKeyDown(KeyCode.R)){
			if (!transform.FindChild ("WinBlanket").gameObject.activeInHierarchy) {
				transform.FindChild ("DeathBlanket").gameObject.SetActive (true);
				Time.timeScale = 1;
			}

		}

		if (Input.GetKeyDown(KeyCode.F)){
			if (!transform.FindChild ("DeathBlanket").gameObject.activeInHierarchy) {
				transform.FindChild ("WinBlanket").gameObject.SetActive (true);
				Time.timeScale = 1;
			}

		}

		if (Input.GetKeyDown(KeyCode.B)){
			if (!transform.FindChild ("DeathBlanket").gameObject.activeInHierarchy){
				
				if (levelCount == 0) {
					if (!transform.FindChild ("WinBlanket").gameObject.activeInHierarchy) {
						transform.FindChild ("DeathBlanket").gameObject.SetActive (true);
						Time.timeScale = 1;
					}
				} else {
					levelCount = levelCount - 2;
					transform.FindChild ("WinBlanket").gameObject.SetActive (true);
					Time.timeScale = 1;
				}
			}

		}

	}

	void FixedUpdate () {
		if (playerCoop.activeInHierarchy) {
			power = player.GetComponent<PlayerController> ().power || playerCoop.GetComponent<PlayerController> ().power;
		} else
			power = player.GetComponent<PlayerController> ().power;
		
		Vector3 playerPos = GetCenterOfPlayers() + offset;
        Vector2 fakeMousePosition;
		Vector3 mousePos;
        if (player.GetComponent<PlayerController>().useGamePad){
            fakeMousePosition = player.GetComponent<PlayerController>().GetAxis();
			//mousePos = new Vector3(player.GetComponent<Rigidbody2D> ().velocity.x * 1 + playerPos.x, player.GetComponent<Rigidbody2D> ().velocity.y * 1 + playerPos.y, offset.z);
			mousePos = playerPos;
		} else {
            fakeMousePosition = (Vector2)Input.mousePosition;
			mousePos = new Vector3 (Camera.main.ScreenToWorldPoint (fakeMousePosition).x, Camera.main.ScreenToWorldPoint (fakeMousePosition).y, offset.z);

        }
		//mousePos = new Vector3 (Camera.main.ScreenToWorldPoint (fakeMousePosition).x, Camera.main.ScreenToWorldPoint (fakeMousePosition).y, offset.z); 
		float distMouse2Player = Vector3.Distance(playerPos, mousePos); 
		float distCamera2Player = Vector3.Distance(playerPos, transform.position);

		Vector3 mouseVector = (mousePos - GetCenterOfPlayers()).normalized;
	

		if (distMouse2Player >= maxDistance())
			distMouse2Player = maxDistance();
		
		Vector3 targetCameraPos = new Vector3 (mouseVector.x * distMouse2Player + playerPos.x, mouseVector.y * distMouse2Player + playerPos.y, offset.z);

		float speed = (distMouse2Player + distCamera2Player) / 2;
		//transform.position = targetCameraPos;
		transform.position = Vector3.Lerp (transform.position, targetCameraPos, Time.fixedDeltaTime * speed);

		if (playerCoop.activeInHierarchy) {
			distanceBetweenPlayersX = Mathf.Abs (player.transform.localPosition.x - playerCoop.transform.localPosition.x);
			distanceBetweenPlayersY = Mathf.Abs (player.transform.localPosition.y - playerCoop.transform.localPosition.y);
			Camera.main.orthographicSize = Mathf.Lerp (Camera.main.orthographicSize, Mathf.Max (distanceBetweenPlayersX - 10, distanceBetweenPlayersY), Time.fixedDeltaTime * speed);
		}

		if (Camera.main.orthographicSize < 5)
			Camera.main.orthographicSize = 5;
	
		//if (distanceBetweenPlayersY > 5) Camera.main.orthographicSize = distanceBetweenPlayersY;
		//Camera.main.orthographicSize = 5 * zoom.Evaluate(shake*10f);

		if (shake > 0) {
			transform.position = NoiseGen.Shake (amplitude, frequency, octaves, persistance, lacunarity, burstFrequency, burstContrast, Time.time, transform.position);
			shake -= Time.fixedDeltaTime;

		} else
			shake = 0;

	
	}

	Vector3 GetCenterOfPlayers() {
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		Vector3 center = new Vector3(0,0,0);

		for (int i= 0; i<players.Length; i++){
			center += players[i].transform.position;
		}
		center /= players.Length;
		return center;
	}	

	public void SetShake(float t){
		shake = t;
	}

	float maxDistance(){
		float height = Camera.main.orthographicSize * 3.2f/4F;
		float width = height * Camera.main.aspect;
		if (height < width)
			return height;
		else
			return width;
	}

}
