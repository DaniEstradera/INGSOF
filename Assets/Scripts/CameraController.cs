﻿using UnityEngine;
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
		Vector3 playerPos = player.transform.position + offset; 
		Vector3 mousePos = new Vector3 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y, offset.z); 

		float distMouse2Player = Vector3.Distance(playerPos, mousePos); 
		float distCamera2Player = Vector3.Distance(playerPos, transform.position);

		Vector3 mouseVector = (mousePos - player.transform.position).normalized;

		//float maxDistance = maxDistance();
		/*
		float height = Camera.main.orthographicSize * 3.2f/4f;
		float width = height * Camera.main.aspect;
		if (height < width)
			maxDistance = height;
		else
			maxDistance = width;*/


		if (distMouse2Player >= maxDistance())
			distMouse2Player = maxDistance();
		
		Vector3 targetCameraPos = new Vector3 (mouseVector.x * distMouse2Player + playerPos.x, mouseVector.y * distMouse2Player + playerPos.y, offset.z);

		float speed = (distMouse2Player + distCamera2Player) / 2;
		transform.position = Vector3.Lerp (transform.position, targetCameraPos, Time.fixedDeltaTime * speed);
	

		if (shake > 0) {
			transform.position = NoiseGen.Shake (amplitude, frequency, octaves, persistance, lacunarity, burstFrequency, burstContrast, Time.time, transform.position);
			shake -= Time.fixedDeltaTime;
		} else
			shake = 0;
	
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
