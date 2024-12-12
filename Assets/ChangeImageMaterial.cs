using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI; // 必須引入以控制 UI 元件

public class ChangeImageMaterial : MonoBehaviour
{
    public Material newMaterial; // 新的材質


    // 更改 Image 的材質
    // 改变 Image 的材质，传入 UI 面板
    public void ChangeMaterial(GameObject uiPanel)
    {
        // 在 UIPanel 中找到目标 Image
        Image targetImage = uiPanel.GetComponentInChildren<Image>();

        if (targetImage != null)
        {
            targetImage.material = newMaterial; // 设置新材质
            Debug.Log($"Material changed successfully on {targetImage.name} in {uiPanel.name}.");
        }
        else
        {
            Debug.LogWarning($"No Image component found in the provided UIPanel: {uiPanel.name}.");
        }

    }

    private bool isMaterialChanged = false;




}
