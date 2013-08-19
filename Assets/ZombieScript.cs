using UnityEngine;
using System.Collections;

public class ZombieScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            //this.transform.Translate(new Vector3(0, 0, 10 * Input.GetAxis("Vertical")));
            this.rigidbody.AddForce(new Vector3(0, 0, 10 * Input.GetAxis("Vertical")));
            //Debug.Log(new Vector3(0, 0, Input.GetAxis("Horizontal")));
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            //this.transform.Translate(new Vector3(10 * Input.GetAxis("Horizontal"), 0, 0));
            this.rigidbody.AddForce(new Vector3(10 * Input.GetAxis("Horizontal"), 0, 0));
            //Debug.Log(new Vector3(0, 0, Input.GetAxis("Vertical")));
        }
    }
}
