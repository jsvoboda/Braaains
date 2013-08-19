using UnityEngine;
using System.Collections;

public class PreyScript : MonoBehaviour {

    public GameObject Zombie;
    public GameObject ScoreDaemon;

    public float FleeDistance;

    bool isFleeing;

    System.Random random;
    float previousTime;
    Vector3 moveDirection;

	// Use this for initialization
	void Start () {
        random = new System.Random();
        previousTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        isFleeing = (this.transform.position - Zombie.transform.position).magnitude < FleeDistance;
        
	}

    void FixedUpdate()
    {
        if (isFleeing)
        {
            if (random.NextDouble() * 5 < Time.time - previousTime)
            {
                //Vector3 newVector = Quaternion.Euler(0, Random.Range(-60, 60), 0) * (this.transform.position - Zombie.transform.position).normalized;

                moveDirection = Quaternion.Euler(0, Random.Range(-70, 70), 0) * (this.transform.position - Zombie.transform.position).normalized;
                rigidbody.AddForce(15 * moveDirection);

                //Vector3 horizontalMove = new Vector3(moveDirection.x, 0, moveDirection.z);
                //this.transform.Translate(horizontalMove * Time.deltaTime);
            }
        }
        else
        {
            if (random.NextDouble() * 15 < Time.time - previousTime)
            {
                moveDirection = Quaternion.Euler(0, Random.Range(-50, 50), 0) * moveDirection;
                rigidbody.AddForce(4 * moveDirection);
            }
        }
        
    }

    void OnCollisionEnter(Collision other)
    {
        //Debug.Log(other.gameObject.name);

        //if (other.gameObject.name == "Room")
        //{
        //    Debug.Log("points " + other.contacts.Length);
        //    Debug.Log("normal " + other.contacts[0].normal);

        //    moveDirection = -other.contacts[0].normal.normalized;
        //}

        if (other.gameObject.name == "Zombie")
        {
            ScoreDaemon.GetComponent<ScoreScript>().IncrementScore();
            Destroy(this.gameObject);
        }
    }
}
