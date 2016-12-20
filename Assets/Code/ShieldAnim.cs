using UnityEngine;
using System.Collections;

public class ShieldAnim : MonoBehaviour {
	public AnimationCurve alphaCurve;
	//Vector3 originalScale;
	//float scale;
	float t = 0;
	public float speed;
	Color aux;

	// Use this for initialization
	void Start () {
		
		aux = new Vector4 (1, 1, 1, 1);

	}
		
	// Update is called once per frame
	void FixedUpdate () {
		
		t += Time.fixedDeltaTime * speed;
		if (t > 1) {
			//Destroy (this);
			this.gameObject.SetActive(false);
		} 

		aux.r = 1 * alphaCurve.Evaluate(t);
		aux.g = aux.r;
		aux.g = aux.b;
		transform.GetComponent<SpriteRenderer> ().color = aux;
	


	}
	//public void setScale(float newScale){
		//scale = newScale;
	//}
	public void setT(float newT){
		t = newT;
	}
}
