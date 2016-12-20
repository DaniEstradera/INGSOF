using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DeathAnim : MonoBehaviour {
	public AnimationCurve scaleCurve;
	private float t = 0f;
	public float speed;
	Vector3 originalScale;

	// Use this for initialization
	void Start () {
		float height = Camera.main.orthographicSize * 3.2f;
		float width = height * Camera.main.aspect;
		originalScale = new Vector3 (width/6, width/6, width/6);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		t += Time.fixedDeltaTime * speed;
		Scene scene = SceneManager.GetActiveScene();
		if (t > 1) {
			if (this.name == ("DeathBlanket"))
				SceneManager.LoadScene (scene.name);
			else if (this.name == ("WinBlanket")) {
				if (CameraController.levelCount == 6) {
					CameraController.levelCount = 0;
				} else {
					CameraController.levelCount += 1;
				}
				SceneManager.LoadScene ("Level" + CameraController.levelCount);
			}
		}
		transform.localScale = originalScale * scaleCurve.Evaluate(t);
	}
}
