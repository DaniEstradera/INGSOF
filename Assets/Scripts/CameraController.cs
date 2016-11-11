using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	private Vector3 offset;
	private float shake;

	[Range(0,100)]
	public float amplitude = 1;
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
		offset = transform.position - player.transform.position;
	}
		
	void FixedUpdate () {
		Vector3 targetPosition = player.transform.position + offset;

		//Vector3 targetBetween = (targetPosition + Camera.main.ScreenToWorldPoint (Input.mousePosition)) / 2f;
		//Debug.Log (""+ Vector3.Distance(targetPosition, Camera.main.ScreenToWorldPoint (Input.mousePosition)));
		//Debug.Log (""+ Vector3.Distance(targetPosition, Camera.main.ScreenToWorldPoint (Input.mousePosition)));

		float distance = Vector3.Distance(targetPosition, Camera.main.ScreenToWorldPoint (Input.mousePosition))+1;

		Vector3 targetBetween = Vector3.Lerp (targetPosition, Camera.main.ScreenToWorldPoint (Input.mousePosition), (1f/distance));

		if (distance > 5) {
			transform.position = Vector3.Lerp (transform.position, targetBetween, Time.fixedDeltaTime*distance);
			//transform.position = Vector3.Lerp (transform.position, targetPosition, Time.fixedDeltaTime);
		} else {
			transform.position = Vector3.Lerp (transform.position, targetBetween, Time.fixedDeltaTime*distance);//*(Vector3.Distance(transform.position, targetPosition2)));
		}
		if (shake > 0) {
			transform.position = NoiseGen.Shake (amplitude, frequency, octaves, persistance, lacunarity, burstFrequency, burstContrast, Time.time, transform.position);
			shake -= Time.fixedDeltaTime;
		} else
			shake = 0;
	
	}

	public void SetShake(float t){
		shake = t;
	}
}
