using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	public float Cadency = 1.0f;
	public GameObject Projectile;


	private GameObject Player;
    Ray shootRay;

    // Use this for initialization
    void Start () {
		Player = GameObject.Find("Ball");
	}
	
	// Update is called once per frame
	void Update () {
		Cadency -= Time.deltaTime;
        Vector3 diff = Player.transform.position - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if (Cadency <= 0.0f)
        {
            Cadency = 2.0f;
            Instantiate(Projectile, transform.position, Quaternion.Euler(0f, 0f, rot_z - 90));
        }
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

    }
}
