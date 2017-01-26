using UnityEngine;
using System.Collections;

public class DestructibleWall : MonoBehaviour {
	public bool hard;
    public float speed;
    private bool destroy;
	public AnimationCurve destroyCurve;
	float t = 0;
	Vector3 originalScale;
	GameObject wallPow;
    // Use this for initialization
    void Start () {
		originalScale = this.transform.localScale;
		if (hard) {
			wallPow = this.transform.FindChild ("WallPow").gameObject;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (hard) {
			if (CameraController.power) {
				gameObject.tag = "Bouncer";
				wallPow.gameObject.SetActive (true);

				if (wallPow.GetComponent<SpriteRenderer> ().color.r <= 1)
					wallPow.GetComponent<SpriteRenderer> ().color += new Color (Time.fixedDeltaTime, Time.fixedDeltaTime, Time.fixedDeltaTime, 0) * 10;
			

			} else {
				gameObject.tag = "Untagged";
				if (wallPow.GetComponent<SpriteRenderer> ().color.r >= 0) {
					wallPow.GetComponent<SpriteRenderer> ().color -= new Color (Time.fixedDeltaTime, Time.fixedDeltaTime, Time.fixedDeltaTime, 0) * 10;
				} else {
					wallPow.SetActive (false);
				}

			}
		}


		if (destroy) {
			if (t < 1) {
				t += Time.fixedDeltaTime * speed;
			} else {
				Destroy (this.gameObject);
			}
		}
		this.transform.localScale =  originalScale * destroyCurve.Evaluate(t);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == ("Player"))
        {
			if (hard && CameraController.power)
            	destroy = true;
			else if (!hard)
				destroy = true;
        }
    }
}
