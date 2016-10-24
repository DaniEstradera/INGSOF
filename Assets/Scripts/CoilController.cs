using UnityEngine;
using System.Collections;

public class CoilController : MonoBehaviour {
	public GameObject ball;
	private SpriteRenderer sr; 
	private bool charge;

	public Vector2 scale;
	public Vector2 chargedScale;

	public Color chargedColor;
	public Color normalColor;

	void Start(){
		sr = ball.GetComponent<SpriteRenderer> ();
		sr.color = chargedColor;
	}


	void Update () {
		if (charge) {
			if (ball.transform.localScale.x < 4.3f) {
				//sr.color += new Color (0, 0, 0, Time.deltaTime*0.8f);
				ball.transform.localScale += new Vector3 (Time.deltaTime * 4.3f, Time.deltaTime * 4.3f, 0);
			} else {
				ball.transform.localScale = new Vector3 (4.3f, 4.3f, 0);
				//sr.color = chargedColor;
			}
		
		} else if (ball.transform.localScale.x > 0f) {
			//sr.color -= new Color (0, 0, 0f, Time.deltaTime*3.2f);
			ball.transform.localScale -= new Vector3 (Time.deltaTime * 17.2f, Time.deltaTime * 17.2f, 0);
		} else {
			ball.transform.localScale = new Vector3 (0, 0, 0);
			//sr.color = normalColor;
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == ("Player")) {
			charge = true;
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == ("Player")) {
			charge = false;
		}
	}
}
