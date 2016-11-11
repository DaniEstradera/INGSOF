using UnityEngine;
using System.Collections;

public class FlowerController : MonoBehaviour {
	private GameObject ball;
	private SpriteRenderer sr; 
	private bool charge;

	public Color chargedColor;
	public Color normalColor;

	private float delay = 1f;

	void Start(){
		ball = this.transform.FindChild ("Ball").gameObject;
		sr = ball.GetComponent<SpriteRenderer> ();
		sr.color = chargedColor;
	}


	void FixedUpdate () {
		if (charge) {
			if (ball.transform.localScale.x < 1f) {
				//sr.color += new Color (0, 0, 0, Time.deltaTime*0.8f);
				ball.transform.localScale += new Vector3 (Time.fixedDeltaTime*6f, Time.fixedDeltaTime*6f, 0);
			} else {
				ball.transform.localScale = new Vector3 (1,1,1);
				resetSize ();
				//sr.color = chargedColor;
			}

		} else if (ball.transform.localScale.x > 0f) {
			//sr.color -= new Color (0, 0, 0f, Time.deltaTime*3.2f);
			ball.transform.localScale -= new Vector3 (Time.fixedDeltaTime * 5f, Time.fixedDeltaTime * 5f, 0);
		} else {
			ball.transform.localScale = new Vector3 (0, 0, 0);
			//sr.color = normalColor;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == ("Player")) {
			charge = true;
		}
	}
	void resetSize () {
		if (delay > 0 && charge)
			delay -= Time.fixedDeltaTime;
		else {
			charge = false;
			delay = 1f;
		}
	}
}
