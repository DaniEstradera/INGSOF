using UnityEngine;
using System.Collections;

public class ExplosionAnim : MonoBehaviour {
	public float speed;
	public AnimationCurve destroyCurve;
	float t = 0;
	Vector3 originalScale;
	// Use this for initialization
	void Start () {
		originalScale = this.transform.localScale;
	}

	// Update is called once per frame
	void FixedUpdate () {
		
		if (t < 1) {
			t += Time.fixedDeltaTime * speed;
		} else {
			Destroy (this.gameObject);
		}

		this.transform.localScale =  originalScale * destroyCurve.Evaluate(t);
	}
}