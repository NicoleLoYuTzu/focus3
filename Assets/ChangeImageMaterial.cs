using UnityEngine;
using UnityEngine.UI; // 必須引入以控制 UI 元件

public class ChangeImageMaterial : MonoBehaviour
{
    public Material newMaterial; // 新的材質

    // 更改當前物件或其子物件的 Image 的材質
    public void ChangeMaterial()
    {
        // 嘗試從當前物件或其子物件中找到 Image
        Image targetImage = GetComponentInChildren<Image>();

        if (targetImage != null)
        {
            targetImage.material = newMaterial; // 設置新材質
            Debug.Log($"Material changed successfully on {targetImage.name}.");
        }
        else
        {
            Debug.LogWarning("No Image component found on this object or its children.");
        }
    }

    // 重設當前物件或其子物件的 Image 的材質
    public void ResetMaterial()
    {
        // 嘗試從當前物件或其子物件中找到 Image
        Image targetImage = GetComponentInChildren<Image>();

        if (targetImage != null)
        {
            targetImage.material = null; // 將材質設置為 None
            Debug.Log($"Material reset to None on {targetImage.name}.");
        }
        else
        {
            Debug.LogWarning("No Image component found on this object or its children.");
        }
    }
}
