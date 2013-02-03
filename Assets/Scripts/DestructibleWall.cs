using UnityEngine;
using System.Collections;

public class DestructibleWall : MonoBehaviour {

    private float timeDestroy = 1.5f;
    private bool destroy;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (!destroy) return;
        timeDestroy -= Time.smoothDeltaTime;

        Color color = GetComponent<SpriteRenderer>().color;
        float alpha = color.a;
        color.a = Mathf.Lerp(alpha, 0, 0.01f);
        GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, color.a);

        

        if (timeDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            destroy = true;
        }
    }
}
