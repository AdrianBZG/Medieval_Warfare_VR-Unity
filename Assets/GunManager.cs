using UnityEngine;
using System.Collections;

public class GunManager : MonoBehaviour {

    public GameObject bulletPrefab;
    public Transform spawnPos;
    public float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPos.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        }
	
	}
}
