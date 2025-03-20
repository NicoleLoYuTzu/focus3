using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR;

public class CollisionHandlerRestoreBuilding : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("building"))
        {
            MeshRenderer renderer = collision.gameObject.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Debug.Log("Collided with: " + collision.gameObject.name + " - Hiding it.");
                renderer.enabled = false; // 隱藏建築
            }
            else
            {
                Debug.LogWarning("Warning: " + collision.gameObject.name + " has no MeshRenderer!");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("building"))
        {
            MeshRenderer renderer = collision.gameObject.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Debug.Log("Exited collision with: " + collision.gameObject.name + " - Restoring visibility.");
                renderer.enabled = true; // 恢復建築
            }
            else
            {
                Debug.LogWarning("Warning: " + collision.gameObject.name + " has no MeshRenderer!");
            }
        }
    }

   
}
