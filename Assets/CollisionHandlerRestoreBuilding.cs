//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit.Interactors;
//using UnityEngine.XR;

//public class CollisionHandlerRestoreBuilding : MonoBehaviour
//{
//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("building"))
//        {
//            MeshRenderer renderer = collision.gameObject.GetComponent<MeshRenderer>();
//            if (renderer != null)
//            {
//                Debug.Log("Collided with: " + collision.gameObject.name + " - Hiding it.");
//                renderer.enabled = false; // 隱藏建築
//            }
//            else
//            {
//                Debug.LogWarning("Warning: " + collision.gameObject.name + " has no MeshRenderer!");
//            }
//        }
//    }

//    private void OnCollisionExit(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("building"))
//        {
//            MeshRenderer renderer = collision.gameObject.GetComponent<MeshRenderer>();
//            if (renderer != null)
//            {
//                Debug.Log("Exited collision with: " + collision.gameObject.name + " - Restoring visibility.");
//                renderer.enabled = true; // 恢復建築
//            }
//            else
//            {
//                Debug.LogWarning("Warning: " + collision.gameObject.name + " has no MeshRenderer!");
//            }
//        }
//    }


//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR;
using UnityEngine.SceneManagement;  // 引入 SceneManager

public class CollisionHandlerRestoreBuilding : MonoBehaviour
{
    // 用來設定不同場景的行為
    private string currentScene;

    private void Start()
    {
        // 根據當前場景設定場景名稱
        currentScene = SceneManager.GetActiveScene().name;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("building"))
        {
            MeshRenderer renderer = collision.gameObject.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                // 根據場景進行不同的處理
                if (currentScene == "OutdoorScene")
                {
                    Debug.Log("Collided with: " + collision.gameObject.name + " - Hiding it.");
                    renderer.enabled = false; // 隱藏建築
                }
                else if (currentScene == "IndoorScene")
                {
                    Debug.Log("Collided with: " + collision.gameObject.name + " - Changing materials to transparent.");

                    // 更改所有材質為 Transparent
                    ChangeMaterialsToTransparent(renderer);
                }
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
                if (currentScene == "OutdoorScene")
                {
                    Debug.Log("Exited collision with: " + collision.gameObject.name + " - Restoring visibility.");
                    renderer.enabled = true; // 恢復建築
                }
                else if (currentScene == "IndoorScene")
                {
                    Debug.Log("Exited collision with: " + collision.gameObject.name + " - Changing materials back to opaque.");
                    // 恢復材質為 Opaque
                    ChangeMaterialsToOpaque(renderer);
                }
            }
            else
            {
                Debug.LogWarning("Warning: " + collision.gameObject.name + " has no MeshRenderer!");
            }
        }
    }

    // 修改所有材質的 Rendering Mode 為 Transparent
    private void ChangeMaterialsToTransparent(MeshRenderer renderer)
    {
        foreach (Material mat in renderer.materials)
        {
            if (mat.HasProperty("_Mode"))
            {
                mat.SetFloat("_Mode", 3);  // 3 代表 Transparent 模式
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;

                // 更改 Albedo 的 Alpha 通道為 0.5
                Color color = mat.GetColor("_Color");
                mat.SetColor("_Color", new Color(color.r, color.g, color.b, 0.5f));
            }
        }
    }

    // 恢復所有材質的 Rendering Mode 為 Opaque
    private void ChangeMaterialsToOpaque(MeshRenderer renderer)
    {
        foreach (Material mat in renderer.materials)
        {
            if (mat.HasProperty("_Mode"))
            {
                mat.SetFloat("_Mode", 0);  // 0 代表 Opaque 模式
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                mat.SetInt("_ZWrite", 1);
                mat.EnableKeyword("_ALPHATEST_ON");
                mat.DisableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = -1;

                // 恢復 Albedo 的 Alpha 通道為 1
                Color color = mat.GetColor("_Color");
                mat.SetColor("_Color", new Color(color.r, color.g, color.b, 1f));
            }
        }
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit.Interactors;
//using UnityEngine.XR;

//public class CollisionHandlerRestoreBuilding : MonoBehaviour
//{
//    public string sceneType; // 用來判斷當前場景的類型 (OutdoorScene 或 IndoorScene)

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("building"))
//        {
//            MeshRenderer renderer = collision.gameObject.GetComponent<MeshRenderer>();
//            if (renderer != null)
//            {
//                if (sceneType == "OutdoorScene")
//                {
//                    Debug.Log("Exited collision with: " + collision.gameObject.name + " - Restoring visibility.");
//                    renderer.enabled = false; // 恢復建築
//                }

//                //如果是 IndoorScene，調整材質的透明度
//                if (sceneType == "IndoorScene")
//                {
//                    AdjustMaterialTransparency(collision.gameObject, 0.5f);
//                }
//            }
//            else
//            {
//                Debug.LogWarning("Warning: " + collision.gameObject.name + " has no MeshRenderer!");
//            }
//        }
//    }

//    private void OnCollisionExit(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("building"))
//        {
//            MeshRenderer renderer = collision.gameObject.GetComponent<MeshRenderer>();
//            if (renderer != null)
//            {

//                if (sceneType == "OutdoorScene")
//                {
//                    Debug.Log("Exited collision with: " + collision.gameObject.name + " - Restoring visibility.");
//                    renderer.enabled = true; // 恢復建築
//                }

//                // 如果是 IndoorScene，恢復材質的透明度
//                if (sceneType == "IndoorScene")
//                {
//                    AdjustMaterialTransparency(collision.gameObject, 1f);
//                }
//            }
//            else
//            {
//                Debug.LogWarning("Warning: " + collision.gameObject.name + " has no MeshRenderer!");
//            }
//        }
//    }

//    // 新的 RestoreAllBuildings 方法
//    public void RestoreAllBuildings()
//    {
//        GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("building");

//        if (allBuildings.Length == 0)
//        {
//            Debug.LogWarning("Warning: No buildings found in the scene!");
//            return;
//        }

//        Debug.Log("Restoring " + allBuildings.Length + " buildings.");

//        foreach (GameObject building in allBuildings)
//        {
//            if (building == null)
//            {
//                Debug.LogWarning("Warning: Found a null reference in building list!");
//                continue;
//            }

//            if (building.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
//            {
//                renderer.enabled = true;
//                Debug.Log("Restored visibility for: " + building.name);

//                // 如果是 IndoorScene，調整材質的透明度
//                if (sceneType == "IndoorScene")
//                {
//                    AdjustMaterialTransparency(building, 1f);
//                }
//            }
//            else
//            {
//                Debug.LogWarning("Warning: " + building.name + " has no MeshRenderer!");
//            }
//        }
//    }

//    // 根據場景類型調整材質的透明度
//    private void AdjustMaterialTransparency(GameObject building, float alphaValue)
//    {
//        Material[] materials = building.GetComponent<MeshRenderer>().materials;

//        foreach (var material in materials)
//        {
//            // 更改 Rendering Mode 和透明度
//            if (alphaValue == 1f)
//            {
//                material.SetFloat("_Mode", 0); // Opaque
//                Color color = material.GetColor("_Color");
//                color.a = 1f; // 恢復不透明
//                material.SetColor("_Color", color);
//            }
//            else
//            {
//                material.SetFloat("_Mode", 3); // Transparent
//                Color color = material.GetColor("_Color");
//                color.a = alphaValue; // 設置為透明度
//                material.SetColor("_Color", color);
//            }
//        }
//    }
//}
