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

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit.Interactors;
//using UnityEngine.XR;
//using UnityEngine.SceneManagement;  // 引入 SceneManager

//public class CollisionHandlerRestoreBuilding : MonoBehaviour
//{
//    // 用來設定不同場景的行為
//    private string currentScene;

//    // 用來儲存原始材質屬性
//    private Dictionary<Material, (float originalMode, float originalAlpha, Color originalColor)> originalMaterialProperties = new Dictionary<Material, (float, float, Color)>();

//    private void Start()
//    {
//        // 根據當前場景設定場景名稱
//        currentScene = SceneManager.GetActiveScene().name;
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("building"))
//        {
//            MeshRenderer renderer = collision.gameObject.GetComponent<MeshRenderer>();
//            if (renderer != null)
//            {
//                // 根據場景進行不同的處理
//                if (currentScene == "OutdoorScene")
//                {
//                    Debug.Log("Collided with: " + collision.gameObject.name + " - Hiding it.");
//                    renderer.enabled = false; // 隱藏建築
//                }
//                else if (currentScene == "IndoorScene")
//                {
//                    Debug.Log("Collided with: " + collision.gameObject.name + " - Changing materials to transparent.");

//                    // 儲存原始材質屬性
//                    SaveOriginalMaterialProperties(renderer);

//                    // 更改所有材質為 Transparent
//                    ChangeMaterialsToTransparent(renderer);
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
//                if (currentScene == "OutdoorScene")
//                {
//                    Debug.Log("Exited collision with: " + collision.gameObject.name + " - Restoring visibility.");
//                    renderer.enabled = true; // 恢復建築
//                }
//                else if (currentScene == "IndoorScene")
//                {
//                    Debug.Log("Exited collision with: " + collision.gameObject.name + " - Restoring original materials.");
//                    // 恢復原始材質
//                    RestoreOriginalMaterials(renderer);
//                }
//            }
//            else
//            {
//                Debug.LogWarning("Warning: " + collision.gameObject.name + " has no MeshRenderer!");
//            }
//        }
//    }

//    // 儲存原始材質的 Rendering Mode 和顏色
//    private void SaveOriginalMaterialProperties(MeshRenderer renderer)
//    {
//        foreach (Material mat in renderer.materials)
//        {
//            if (mat.HasProperty("_Mode"))
//            {
//                float originalMode = mat.GetFloat("_Mode");
//                Color originalColor = mat.GetColor("_Color");

//                // 儲存材質的原始屬性，包括透明度
//                if (!originalMaterialProperties.ContainsKey(mat))
//                {
//                    originalMaterialProperties[mat] = (originalMode, originalColor.a, originalColor);
//                }
//            }
//            else
//            {
//                // 如果材質沒有 _Mode 屬性，記錄其顏色並保存透明度
//                Color originalColor = mat.GetColor("_Color");
//                originalMaterialProperties[mat] = (0, originalColor.a, originalColor); // 0 是 Opaque 的默認模式
//            }
//        }
//    }

//    private void ChangeMaterialsToTransparent(MeshRenderer renderer)
//    {
//        foreach (Material mat in renderer.materials)
//        {
//            if (mat.HasProperty("_Mode"))
//            {
//                mat.SetFloat("_Mode", 3);  // 3 代表 Transparent 模式
//                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
//                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
//                mat.SetInt("_ZWrite", 0);
//                mat.DisableKeyword("_ALPHATEST_ON");
//                mat.EnableKeyword("_ALPHABLEND_ON");
//                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
//                mat.renderQueue = 3000;

//                // 設定透明度為 0.5
//                Color color = mat.GetColor("_Color");
//                mat.SetColor("_Color", new Color(color.r, color.g, color.b, 0.5f));
//            }
//            else
//            {
//                // 如果材質沒有 _Mode 屬性，處理它的顏色或其他屬性
//                Color color = mat.GetColor("_Color");
//                mat.SetColor("_Color", new Color(color.r, color.g, color.b, 0.5f)); // 透明度設為 0.5
//            }
//        }
//    }

//    public void RestoreOriginalMaterials(MeshRenderer renderer)
//    {
//        foreach (Material mat in renderer.materials)
//        {
//            if (mat.HasProperty("_Mode") && originalMaterialProperties.ContainsKey(mat))
//            {
//                // 恢復原始屬性
//                var originalProperties = originalMaterialProperties[mat];
//                mat.SetFloat("_Mode", originalProperties.originalMode);
//                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
//                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
//                mat.SetInt("_ZWrite", 1);
//                mat.EnableKeyword("_ALPHATEST_ON");
//                mat.DisableKeyword("_ALPHABLEND_ON");
//                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
//                mat.renderQueue = -1;

//                // 恢復原始顏色和透明度
//                mat.SetColor("_Color", new Color(originalProperties.originalColor.r, originalProperties.originalColor.g, originalProperties.originalColor.b, originalProperties.originalAlpha));
//            }
//            else
//            {
//                // 如果材質沒有 _Mode 屬性，恢復顏色和透明度
//                if (originalMaterialProperties.ContainsKey(mat))
//                {
//                    var originalProperties = originalMaterialProperties[mat];
//                    mat.SetColor("_Color", new Color(originalProperties.originalColor.r, originalProperties.originalColor.g, originalProperties.originalColor.b, originalProperties.originalAlpha));
//                }
//            }
//        }
//    }

//}

using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CollisionHandlerRestoreBuilding : MonoBehaviour
{
    // 用來設定不同場景的行為
    private string currentScene;
    private const string GlassMaterialName = "_2 (Instance)"; // 玻璃材質名稱
    private GameObject[] allBuildings;


    private void Start()
    {
        // 根據當前場景設定場景名稱
        currentScene = SceneManager.GetActiveScene().name;
        allBuildings = GameObject.FindGameObjectsWithTag("building");

    }

    public void OnCollisionEnter(Collision collision)
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
                    // 更改所有材質為 Transparent，排除玻璃材質
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

    // 修改所有材質的 Rendering Mode 為 Transparent，但排除玻璃材質
    private void ChangeMaterialsToTransparent(MeshRenderer renderer)
    {
        foreach (Material mat in renderer.materials)
        {
            if (mat.name == GlassMaterialName)
            {
                continue;  // 如果是玻璃材質，跳過不做處理
            }

            if (mat.HasProperty("_Mode"))
            {
                mat.SetFloat("_Mode", 3);  // Transparent 模式
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;

                Color color = mat.GetColor("_Color");
                mat.SetColor("_Color", new Color(color.r, color.g, color.b, 0.5f));
            }
        }
    }

    // 恢復所有材質的 Rendering Mode 為 Opaque，但排除玻璃材質
    public void ChangeMaterialsToOpaque(MeshRenderer renderer)
    {
        foreach (Material mat in renderer.materials)
        {
            if (mat.name == GlassMaterialName)
            {
                continue;  // 如果是玻璃材質，跳過不做處理
            }

            if (mat.HasProperty("_Mode"))
            {
                mat.SetFloat("_Mode", 0);  // Opaque 模式
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                mat.SetInt("_ZWrite", 1);
                mat.EnableKeyword("_ALPHATEST_ON");
                mat.DisableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = -1;

                Color color = mat.GetColor("_Color");
                mat.SetColor("_Color", new Color(color.r, color.g, color.b, 1f));
            }
        }
    }

    public void RestoreAllBuildings()
    {
       

        if (allBuildings.Length == 0)
        {
            Debug.LogWarning("Warning: No buildings found in the scene!");
            return;
        }

        Debug.Log("Restoring " + allBuildings.Length + " buildings.");

        foreach (GameObject building in allBuildings)
        {
            if (building == null)
            {
                Debug.LogWarning("Warning: Found a null reference in building list!");
                continue;
            }

            if (building.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            {
                // 只處理那些沒有處理過的建築
                renderer.enabled = true;
                Debug.Log("Restored visibility for: " + building.name);

               

                // 如果是 IndoorScene，調整材質的透明度
                if (SceneManager.GetActiveScene().name == "IndoorScene")
                {
                    ChangeMaterialsToOpaque(renderer);
                }
            }
            else
            {
                Debug.LogWarning("Warning: " + building.name + " has no MeshRenderer or has already been processed!");
            }
        }
    }
}
