using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CollisionHandlerRestoreBuilding : MonoBehaviour
{
    // 用來設定不同場景的行為
    //private string currentScene;
    private const string GlassMaterialName = "_2 (Instance)"; // 玻璃材質名稱
    private GameObject[] allBuildings;


    private void Start()
    {
        // 根據當前場景設定場景名稱
        //currentScene = SceneManager.GetActiveScene().name;
        allBuildings = GameObject.FindGameObjectsWithTag("building");

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("building"))
        {
            MeshRenderer renderer = collision.gameObject.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                ChangeMaterialsToTransparent(renderer);
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
                ChangeMaterialsToOpaque(renderer);
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

}
