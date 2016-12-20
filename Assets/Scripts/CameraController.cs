using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
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

	}
		
	void FixedUpdate () {
		Vector3 playerPos = GetCenterOfPlayers() + offset;
        Vector2 fakeMousePosition;
        if (player.GetComponent<PlayerController>().useGamePad){
            fakeMousePosition = player.GetComponent<PlayerController>().GetAxis();
        } else {
            fakeMousePosition = (Vector2)Input.mousePosition;
        }
		Vector3 mousePos = new Vector3 (Camera.main.ScreenToWorldPoint (fakeMousePosition).x, Camera.main.ScreenToWorldPoint (fakeMousePosition).y, offset.z); 

		float distMouse2Player = Vector3.Distance(playerPos, mousePos); 
		float distCamera2Player = Vector3.Distance(playerPos, transform.position);

		Vector3 mouseVector = (mousePos - GetCenterOfPlayers()).normalized;
	

		if (distMouse2Player >= maxDistance())
			distMouse2Player = maxDistance();
		
		Vector3 targetCameraPos = new Vector3 (mouseVector.x * distMouse2Player + playerPos.x, mouseVector.y * distMouse2Player + playerPos.y, offset.z);

		float speed = (distMouse2Player + distCamera2Player) / 2;
		transform.position = Vector3.Lerp (transform.position, targetCameraPos, Time.fixedDeltaTime * speed);



	
		Camera.main.orthographicSize = 5 * zoom.Evaluate(shake*10f);

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
		float height = Camera.main.orthographicSize * 3.2f/4f;
		float width = height * Camera.main.aspect;
		if (height < width)
			return height;
		else
			return width;
	}

}
