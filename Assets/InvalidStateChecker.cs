using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractionLayerChecker : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor; // 參考該物件的 XRRayInteractor
    public InteractionLayerMask teleportLayerMask; // 定義有效的 Teleport 層級
    private ParabolicLineCircle parabolicLineCircle;

    void Start()
    {
        // 嘗試從該物件獲取 XRRayInteractor
        rayInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();

        parabolicLineCircle = GetComponent<ParabolicLineCircle>();

        if (rayInteractor == null)
        {
            Debug.LogError("XRRayInteractor is missing on this GameObject.");
        }
    }

    void Update()
    {
        if (rayInteractor == null)
            return;

        // 檢查 InteractionLayerMask 是否包含 Teleport
        if (!IsLayerMaskTeleport(rayInteractor.interactionLayers))
        {
            parabolicLineCircle.HideLineRenderers();
        }
    }

    // 判斷 InteractionLayerMask 是否包含 Teleport 層
    bool IsLayerMaskTeleport(InteractionLayerMask layerMask)
    {
        // 使用位運算檢查 teleportLayerMask 是否包含在 layerMask 中
        return (teleportLayerMask & layerMask) == layerMask;
    }


}
