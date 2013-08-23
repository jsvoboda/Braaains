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

    float startTime = 0;

    //Vector3[] castingDirections = new Vector3[] {
    //    new Vector3(0,0,1),
    //    new Vector3(1,0,1),
    //    new Vector3(1,0,0),
    //    new Vector3(1,0,-1),
    //    new Vector3(0,0,-1),
    //    new Vector3(-1,0,-1),
    //    new Vector3(-1,0,0),
    //    new Vector3(-1,0,1)
    //};

    //float startTime;
    //float turnTime;

    void Start()
    {
        int vectCount = 8;
        for (int i = 0; i < vectCount; i++)
        {
            Vector3 v = Quaternion.Euler(0, i * (360/vectCount), 0) * new Vector3(1, 0, 0);
            castingDirections.Add(v);
        }


        currentDirection = (Random.rotation * new Vector3(1, 1, 1)).nullYAxis();
        //currentDirection = new Vector3(0, 0, 0);
        desiredDirection = (Random.rotation * new Vector3(1, 1, 1)).nullYAxis().normalized;
    }

    void Update()
    {
        //var fracComplete = (Time.time - startTime) / journeyTime;
        var rotation = Vector3.Lerp(currentDirection, desiredDirection, 2 * (Time.time - startTime));

        Debug.Log(rotation);

        //steering = desiredDirection - currentDirection;
        //currentDirection = currentDirection + steering;
        //Debug.Log(rotation.nullYAxis() * Time.deltaTime * 5);
        transform.Translate(rotation.nullYAxis() * Time.deltaTime * 3);
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

        foreach (var hit in hits)
        {
            var mark = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            mark.transform.position = hit.point;
            mark.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }

        var mainMark = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mainMark.transform.position = closest.point;
        mainMark.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        startTime = Time.time;

        return Quaternion.Euler(0, Random.Range(-30, 30), 0)
            * (closest.point - this.transform.position).normalized;

        //return closest.point - this.transform.position;
    }
}
