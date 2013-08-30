using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PreyScript : MonoBehaviour
{
    public GameObject Walls;
    public GameObject Zombie;
    public GameObject ScoreDaemon;
    //public float FleeDistance;

    PreyState state = new PreyState();
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
            Vector3 v = Quaternion.Euler(0, i * (360 / vectCount), 0) * new Vector3(1, 0, 0);
            castingDirections.Add(v);
        }

        currentDirection = (Random.rotation * new Vector3(1, 1, 1)).nullYAxis();
        desiredDirection = (Random.rotation * new Vector3(1, 1, 1)).nullYAxis().normalized;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spawnPrey(1);
        }

        steering = (desiredDirection - currentDirection) * 0.1f;
        currentDirection = currentDirection + steering;

        if (state.CanRelax())
        {
            Debug.Log("a"); 
            desiredDirection = Quaternion.Euler(0, Random.Range(-60, 60), 0)
                * currentDirection;
        }

        if (steering.magnitude < 0.001)
        {
            desiredDirection = Quaternion.Euler(0, Random.Range(-60, 60), 0)
                * currentDirection;
        }

        transform.Translate(currentDirection.nullYAxis() * Time.deltaTime * 1.5f);
    }

    public void WallInRadius(Collider wall)
    {
        Debug.Log("WallInRadius");
        state.NearWall = true;
        desiredDirection = -directionTo(wall.collider.gameObject);
    }

    public void WallOutOfRadius()
    {
        Debug.Log("WallOutOfRadius");
        state.NearWall = false;
    }

    public void PreyInRadius(Collider prey)
    {
        Debug.Log("PreyInRadius");
        //desiredDirection = -directionTo(prey.collider.gameObject);
        ////desiredDirection = Vector3.Lerp(desiredDirection, -directionTo(prey.collider.gameObject), .5f);
    }

    public void ZombieInRadius(Collider zombie)
    {
        Debug.Log("ZombieInRadius");
        state.NearZombie = true;
        desiredDirection = -directionTo(zombie.collider.gameObject);
    }

    //void OnTriggerEnter(Collider other)
    //{

    //    if (other.collider.gameObject.name == "Walls")
    //    {
    //        desiredDirection = - directionTo(other.collider.gameObject);
    //    }

    //    if (other.collider.gameObject.name == "Zombie")
    //    {
    //        ScoreDaemon.GetComponent<ScoreScript>().IncrementScore();
    //        Destroy(this.gameObject);
    //    }
    //}

    //void OnTriggerStay(Collider other)
    //{
    //    if (other.collider.gameObject.name == "Walls")
    //    {
    //        desiredDirection = -directionTo(other.collider.gameObject);
    //    }

    //}

    //Approximate direction from this to closest point of the other object
    Vector3 directionTo(GameObject other)
    {
        List<RaycastHit> hits = new List<RaycastHit>();
        float distance = 3;
        foreach (var d in castingDirections)
        {
            //hits.AddRange(Physics.SphereCastAll(this.transform.position, 0.2f, d));
            hits.AddRange(Physics.RaycastAll(this.transform.position, d, distance));
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

        return Quaternion.Euler(0, Random.Range(-80, 80), 0)
            * (closest.point - this.transform.position).normalized;
    }

    void spawnPrey(int count)
    {
        Object original = (Object)this.gameObject;
        for (int i = 0; i < count; i++)
        {
            var clone = Instantiate(original);
        }
    }
}

public class PreyState
{
    public bool NearZombie { get; set; }
    public bool NearWall { get; set; }

    public bool CanRelax()
    {
        return !(NearWall || NearZombie);
    }
}
