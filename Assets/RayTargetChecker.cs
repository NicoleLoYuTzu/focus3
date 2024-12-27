using System.Collections.Generic;
using UnityEngine;

public class RayTargetChecker : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor; // XRRayInteractor 參考
    private ParabolicLineCircle parabolicLineCircle; // 自定義的線條隱藏工具

    void Start()
    {
        // 嘗試從該物件獲取 XRRayInteractor
        rayInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
        if (rayInteractor == null)
        {
            Debug.LogError("XRRayInteractor is missing on this GameObject.");
            return;
        }

        // 獲取自定義的線條工具
        parabolicLineCircle = GetComponent<ParabolicLineCircle>();
        if (parabolicLineCircle == null)
        {
            Debug.LogWarning("ParabolicLineCircle is missing on this GameObject.");
        }
    }

    void Update()
    {
        if (rayInteractor == null || parabolicLineCircle == null)
        {
            Debug.LogWarning("RayInteractor or ParabolicLineCircle is null.");
            return;
        }

        // 檢查方向桿是否在動
        // 如果 velocity 是 float，直接檢查數值
        if (rayInteractor.velocity > 0)  // 當速度大於 0 時，表示方向桿有在動
        {
            // 如果沒有 hit 到任何物體
            if (!rayInteractor.hasSelection)
            {
                parabolicLineCircle.HideLineRenderers(); // 隱藏線條
            }
        }
    }
}
