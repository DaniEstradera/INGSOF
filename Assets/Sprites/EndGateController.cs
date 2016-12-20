using UnityEngine;
using System.Collections;

public class EndGateController : MonoBehaviour {

	public GameObject condition0;
	public GameObject condition1;
	public GameObject condition2;
	public GameObject condition3;
	public GameObject condition4;
	public GameObject condition5;
	public GameObject condition6;
	public GameObject condition7;
	public GameObject condition8;

	public AnimationCurve scaleCurve;
	public AnimationCurve openedCurve;
	GameObject opened;
	GameObject locked;
	Vector3 originalScale;
	Vector3 openedOriginalScale;
	public float speed;
	float rotationSpeed = 75;
	float t = 0;
	// Use this for initialization
	void Start () {
		locked = this.transform.FindChild ("Locked").gameObject;
		opened = this.transform.FindChild ("Opened").gameObject;

		originalScale = locked.transform.localScale;
		openedOriginalScale = opened.transform.localScale;
	}

	// Update is called once per frame
	void FixedUpdate () {
		
		this.transform.Rotate (0, 0, Time.fixedDeltaTime * rotationSpeed);

		if (condition0 == null && condition1 == null && condition2 == null && condition3 == null && condition4 == null && condition5 == null && condition6 == null && condition7 == null && condition8 == null) {

			opened.SetActive (true);
			if (t < 1) {
				t += Time.fixedDeltaTime * speed;
			} else {
				locked.SetActive (false);
			}
				
			locked.transform.localScale = originalScale * scaleCurve.Evaluate (t);
			opened.transform.localScale = openedOriginalScale * openedCurve.Evaluate (t);

		} else {
			locked.SetActive (true);
			opened.SetActive (false);
		}
	}
}
