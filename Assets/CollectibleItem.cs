using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CollectibleItem : MonoBehaviour
{
    public string itemType; // "Wing" 或 "Heart"

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor hoveringInteractor; // 儲存 Hover 的控制器

    private void Start()
    {
        // 確保物件有 XR Simple Interactable
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        if (interactable == null)
        {
            interactable = gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        }

        // 綁定 Hover 事件
        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);
    }

    private void Update()
    {
        if (hoveringInteractor != null) // 確保目前有 Hover 物件
        {
            InputDevice leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

            bool isPressedLeft = false, isPressedRight = false;

            InputHelpers.IsPressed(leftHandDevice, InputHelpers.Button.Trigger, out isPressedLeft);
            InputHelpers.IsPressed(rightHandDevice, InputHelpers.Button.Trigger, out isPressedRight);

            if (isPressedLeft || isPressedRight) // 任何一隻手的 Trigger 被按下
            {
                Debug.Log("Trigger pressed on Hovered Collectible: " + itemType);
                GameManager.Instance.CollectItem(itemType); // 更新數量
                Destroy(gameObject); // 撿到後銷毀物件
            }
        }
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        hoveringInteractor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor; // ✅ 修正這裡
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        if (hoveringInteractor == args.interactorObject) // ✅ 修正這裡
        {
            hoveringInteractor = null;
        }
    }
}
