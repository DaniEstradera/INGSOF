using UnityEngine;
using System.Collections;

public class MatchScreenScale : MonoBehaviour {

	void FixedUpdate () {
		float height = (float)Camera.main.orthographicSize*3.2f; 
		float width = height * Screen.width / Screen.height; 
		transform.localScale = new Vector3(width, height, 1.0f);
	}

}
