using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	public float Cadency = 1.0f;
	public GameObject Projectile;
    public float projectileSpeed = 10;
    public float maxDistance;


	private GameObject Player;
    Ray shootRay;

    // Use this for initialization
    void Start () {
		Player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 diff = Player.transform.position - transform.position;
        if (diff.magnitude <= maxDistance)
        {
            Cadency -= Time.deltaTime;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            if (Cadency <= 0.0f)
            {
                Cadency = 2.0f;
                GameObject spawnedProjectile;
                spawnedProjectile = (Instantiate(Projectile, transform.position, Quaternion.Euler(0f, 0f, rot_z - 90))) as GameObject;
                spawnedProjectile.GetComponent<Rigidbody2D>().AddForce(transform.up * projectileSpeed);
            }
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
    }
}
