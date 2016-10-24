using UnityEngine;
using System.Collections;

public class MaintainScale : MonoBehaviour {

	private Vector3 originalParentScale;
	private Vector3 originalThisScale;

	// Use this for initialization
	void Start () {
	
		originalParentScale = transform.parent.localScale;
		originalThisScale = transform.localScale;

	}
	
	// Update is called once per frame
	void Update () {
	
		float proportion = this.transform.parent.localScale.x / originalParentScale.x;
		this.transform.localScale = originalThisScale / proportion;

	}
}
