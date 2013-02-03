using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {

    private GameObject GameMode;

    // Use this for initialization
    void Start () {
        GameMode = GameObject.Find("HUD");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().Win();
        }
    }
}
