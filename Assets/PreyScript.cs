using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets;

public class PreyScript : MonoBehaviour
{
    public GameObject Walls;
    public GameObject Zombie;
    public GameObject ScoreDaemon;
    public float WallRepulsionRadius;

    PreyState state = new PreyState();

    bool isFleeing;
    Vector3 currentDirection;
    Vector3 desiredDirection;
    Vector3 steering;
    List<Vector3> castingDirections = new List<Vector3>();

    int scanCounter = 0;

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

        //if (state.CanRelax())
        //{
        //    desiredDirection = Quaternion.Euler(0, Random.Range(-60, 60), 0)
        //        * currentDirection;
        //    Debug.Log("desired direction " + desiredDirection);
        //}

        if (steering.magnitude < 0.001)
        {
            desiredDirection = Quaternion.Euler(0, Random.Range(-60, 60), 0)
                * currentDirection;
        }

        transform.Translate(currentDirection.nullYAxis() * Time.deltaTime * 3);
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.collider.gameObject.name == "Walls")
    //    {
    //        this.WallInRadius(other);
    //        //desiredDirection = -directionTo(other.collider.gameObject);
    //    }

    //    //if (other.collider.gameObject.name == "Zombie")
    //    //{
    //    //    ScoreDaemon.GetComponent<ScoreScript>().IncrementScore();
    //    //    Destroy(this.gameObject);
    //    //}
    //}

    void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay");
        Debug.Log(other.name);
        scanCounter++;
        if (state.NearWall == false && scanCounter % 10 != 0)
        {
            return;
        }

        if (other.collider.gameObject.name == "Walls")
        {
            
            //if (state.NearWall == false && scanCounter % 10 != 0 || state.NearWall == true)
            {
                Tuple<Vector3, float> wallInfo = directionToWall(other.collider.gameObject);

                if (wallInfo.Item2 < WallRepulsionRadius)
                {
                    this.WallInRadius(wallInfo);
                }
                else
                {
                    this.WallOutOfRadius();
                }
            }

            //desiredDirection = -directionTo(other.collider.gameObject);
        }
    }

    public void WallInRadius(Tuple<Vector3, float> wallInfo)
    {
        Debug.Log("WallInRadius");
        state.NearWall = true;
        desiredDirection = -wallInfo.Item1;

        //desiredDirection = -directionToWall(wall.collider.gameObject).Item1;
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
        //desiredDirection = -directionTo(zombie.collider.gameObject);
    }

    /// <summary>
    /// Approximate direction from this to closest point of the other large object (wall)
    /// </summary>
    /// <param name="other"></param>
    /// <returns>direction</returns>
    Tuple<Vector3, float> directionToWall(GameObject other)
    {
        List<RaycastHit> hits = new List<RaycastHit>();

        foreach (var d in castingDirections)
        {
            //TODO: specify distance to reduce number of hits
            hits.AddRange(Physics.RaycastAll(this.transform.position, d));
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

        return new Tuple<Vector3, float>(Quaternion.Euler(0, Random.Range(-50, 50), 0)
            * (closest.point - this.transform.position).normalized,
            minDist);
    }

    void spawnPrey(int count)
    {
        Object original = (Object)this.gameObject;
        for (int i = 0; i < count; i++)
        {
            Instantiate(original);
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