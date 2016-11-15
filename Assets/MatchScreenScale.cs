using UnityEngine;
using System.Collections;

public class MatchScreenScale : MonoBehaviour {

	void FixedUpdate () {


		float height = Camera.main.orthographicSize * 3.2f;
		float width = height * Camera.main.aspect;

		transform.localScale = new Vector3(width, height, 1.0f);
	}

}
