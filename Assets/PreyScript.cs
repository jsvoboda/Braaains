using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PreyScript : MonoBehaviour
{
    public GameObject Walls;
    public GameObject Zombie;
    public GameObject ScoreDaemon;
    public float FleeDistance;

    bool isFleeing;
    Vector3 currentDirection;
    Vector3 desiredDirection;
    Vector3 steering;
    List<Vector3> castingDirections = new List<Vector3>();

    void Start()
    {
        int vectCount = 8;
        for (int i = 0; i < vectCount; i++)
        {
            Vector3 v = Quaternion.Euler(0, i * (360/vectCount), 0) * new Vector3(1, 0, 0);
            castingDirections.Add(v);
        }


        currentDirection = (Random.rotation * new Vector3(1, 1, 1)).nullYAxis();
        desiredDirection = (Random.rotation * new Vector3(1, 1, 1)).nullYAxis().normalized;
    }

    void Update()
    {
        steering = (desiredDirection - currentDirection) * 0.1f;
        currentDirection = currentDirection + steering;

        transform.Translate(currentDirection.nullYAxis() * Time.deltaTime * 3);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.collider.gameObject.name == "Walls")
        {
            desiredDirection = - directionTo(other.collider.gameObject);
        }

        if (other.collider.gameObject.name == "Zombie")
        {
            ScoreDaemon.GetComponent<ScoreScript>().IncrementScore();
            Destroy(this.gameObject);
        }
    }

    //Approximate direction from this to closest point of the other object
    Vector3 directionTo(GameObject other)
    {
        List<RaycastHit> hits = new List<RaycastHit>();

        foreach (var d in castingDirections)
        {
            hits.AddRange(Physics.SphereCastAll(this.transform.position, 0.1f, d));
        }

        RaycastHit closest = hits[0];
        float minDist = closest.distance;
        foreach (var hit in hits)
        {
            if (hit.distance < minDist)
            {
                minDist = hit.distance;
                closest = hit;
            }
        }

        return Quaternion.Euler(0, Random.Range(-50, 50), 0)
            * (closest.point - this.transform.position).normalized;
    }
}
