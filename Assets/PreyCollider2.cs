using UnityEngine;
using System.Collections;

public class PreyCollider2 : MonoBehaviour
{
    public GameObject Prey;

    void OnTriggerStay(Collider other)
    {
        if (other.collider.gameObject.name == "Zombie")
        {
            Prey.GetComponent<PreyScript>().ZombieInRadius(other);
        }
    }
}
