using UnityEngine;
using System.Collections;

public class BouncerController : MonoBehaviour {
	private SpriteRenderer sr; 
	private bool bounce;
	public Color chargeColor;
	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (bounce){
			sr.color = chargeColor;
			bounce = false;
		} else if (sr.color != Color.white){
			sr.color += new Color (Time.deltaTime, Time.deltaTime, Time.deltaTime);
		} else sr.color = new Color (1, 1, 1, 1);
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == ("Player")) {
			bounce = true;
		}
	}
	void OnCollisionExit2D(Collision2D other) {
		if (other.gameObject.tag == ("Player")) {
			bounce = false;
		}
	}

}

