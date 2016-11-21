using UnityEngine;
using System.Collections;

public class CoilController : MonoBehaviour {
	private GameObject ball;
	private SpriteRenderer sr; 
	private bool shrink;
	private bool charge;

	float delay = 0f;
	float t = 0f;
	float speedGrowth = 1f;
	float speedShrink = 2f;
	Vector3 originalScale;
	public AnimationCurve scaleCurve;

	public Color chargedColor;

	void Start(){
		ball = this.transform.FindChild ("Ball").gameObject;
		originalScale = this.transform.localScale;
		sr = ball.GetComponent<SpriteRenderer> ();
		sr.color = chargedColor;

	}


	void FixedUpdate () {
		
		ball.transform.localScale = originalScale * scaleCurve.Evaluate(t);

		if (shrink) {
			if (t > 0) {
				t -= Time.fixedDeltaTime * speedShrink;
			} else
				t = 0;
		} 

		resetSize ();

	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == ("Player")) {
			if (t < 1) {
				t += Time.fixedDeltaTime * speedGrowth;
			} else {
				t = 1;
				delay = 1f;
			}
			shrink = false;
			
		} 
	}
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == ("Player")) {
			if (t < 1) {
				shrink = true;
			}
		}
	}

	void resetSize () {
		if (delay > 0)
			delay -= Time.fixedDeltaTime;
		else {
			shrink = true;;
		}
	}
}
