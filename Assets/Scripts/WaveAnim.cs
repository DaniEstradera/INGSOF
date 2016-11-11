using UnityEngine;
using System.Collections;

public class WaveAnim : MonoBehaviour {


	MaterialPropertyBlock materialProperty; //new
	public Gradient colorGradient; //new
	public Texture text; //new
	public float scale;

	Material mat;
	Vector3 originalScale;
	float originalBump;
	public AnimationCurve scaleCurve;
	public AnimationCurve bumpCurve;
	public float speed;
	private float t = 0f;
	float posZ;

	void Start () {
		
		originalScale = new Vector3 (scale, scale, scale);

		if (this.name == ("Blast(Clone)")) {
			
			materialProperty = new MaterialPropertyBlock ();
			materialProperty.SetTexture("_MainTex", text);
			this.GetComponent<SpriteRenderer> ().SetPropertyBlock(materialProperty);

		} else {
			mat = this.GetComponent<SpriteRenderer> ().material;
			originalBump = mat.GetFloat ("_BumpAmt");
			if (this.name == ("Bubble(Clone)"))
				posZ = transform.position.z - 3;
			else if (this.name == ("ShockWave(Clone)"))
				posZ = transform.position.z - 2;
		}
	
	}

	void FixedUpdate () {
		
		t += Time.fixedDeltaTime * speed;
		if (t > 1) {
			MonoBehaviour.Destroy (this.gameObject);
		}

		if (this.name == ("Blast(Clone)")) {
			
			Color targetColor = colorGradient.Evaluate (t);
			materialProperty.SetColor("_TintColor", targetColor);
			this.GetComponent<SpriteRenderer> ().SetPropertyBlock(materialProperty);
		
		} else {
			
			mat.SetFloat ("_BumpAmt", originalBump * bumpCurve.Evaluate (t));
			posZ -= Time.fixedDeltaTime;
			transform.position = new Vector3 (transform.position.x, transform.position.y, posZ);
		}


		transform.localScale = originalScale * scaleCurve.Evaluate(t);

	}
}
