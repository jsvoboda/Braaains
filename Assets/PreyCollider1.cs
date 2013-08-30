using UnityEngine;
using System.Collections;

public class PreyCollider1 : MonoBehaviour {

    public GameObject Prey;

    void OnTriggerStay(Collider other)
    {
        if(other.collider.gameObject.name == "Walls")
        {
            Prey.GetComponent<PreyScript>().WallInRadius(other);
        }

        //if (other.collider.gameObject.tag == "Prey")
        //{
        //    Prey.GetComponent<PreyScript>().PreyInRadius(other);
        //}
    }

    void OnTriggerExit(Collider other)
    {
        if (other.collider.gameObject.name == "Walls")
        {
            Prey.GetComponent<PreyScript>().WallOutOfRadius();
        }
    }
}
