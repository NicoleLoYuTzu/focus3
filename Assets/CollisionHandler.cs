using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("CollisionHandler initialized and ready to detect collisions.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter triggered! Collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("building"))
        {
            Debug.Log("Collided with a building: " + collision.gameObject.name + ". Disabling it.");
            collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            Debug.Log("Collided object is not a building. No action taken.");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("OnCollisionExit triggered! Exited from: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("building"))
        {
            Debug.Log("Exited from building: " + collision.gameObject.name + ". Enabling it.");
            collision.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

}

