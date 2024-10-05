using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractableLogger : MonoBehaviour
{
    private void OnEnable()
    {
        // 訂閱各個事件
        var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();

        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnFirstSelectEntered);
            interactable.hoverEntered.AddListener(OnHoverEntered);
            interactable.hoverExited.AddListener(OnHoverExited);
            interactable.selectExited.AddListener(OnLastSelectExited);

            // 新增 Focus 事件
            interactable.firstFocusEntered.AddListener(OnFirstFocusEntered);
            interactable.lastFocusExited.AddListener(OnLastFocusExited);

            // 新增 Hover 的 Focus 事件
            interactable.firstHoverEntered.AddListener(OnFirstHoverEntered);
            interactable.lastHoverExited.AddListener(OnLastHoverExited);

            // 如果是 XRGrabInteractable，訂閱 grab 和 ungrab 事件
            if (interactable is UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable)
            {
                grabInteractable.selectEntered.AddListener(OnActivated);
                grabInteractable.selectExited.AddListener(OnDeactivated);
            }

            //// 如果是傳送區域，訂閱傳送事件
            //if (interactable is UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationArea teleportInteractable)
            //{
            //    teleportInteractable.teleporting.AddListener(OnTeleporting);
            //}
        }
    }

    private void OnDisable()
    {
        // 取消訂閱各個事件
        var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();

        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnFirstSelectEntered);
            interactable.hoverEntered.RemoveListener(OnHoverEntered);
            interactable.hoverExited.RemoveListener(OnHoverExited);
            interactable.selectExited.RemoveListener(OnLastSelectExited);

            // 新增 Focus 事件
            interactable.firstFocusEntered.RemoveListener(OnFirstFocusEntered);
            interactable.lastFocusExited.RemoveListener(OnLastFocusExited);

            // 新增 Hover 的 Focus 事件
            interactable.firstHoverEntered.RemoveListener(OnFirstHoverEntered);
            interactable.lastHoverExited.RemoveListener(OnLastHoverExited);

            if (interactable is UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable)
            {
                grabInteractable.selectEntered.RemoveListener(OnActivated);
                grabInteractable.selectExited.RemoveListener(OnDeactivated);
            }

            //if (interactable is UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationArea teleportInteractable)
            //{
            //    teleportInteractable.teleporting.RemoveListener(OnTeleporting);
            //}
        }
    }

    // 設置事件的 Debug.Log() 方法
    void OnTeleporting(UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportingEventArgs args)
    {
        Debug.Log("InteractableLogger Teleporting: " + args.interactorObject);
    }

    void OnFirstHoverEntered(HoverEnterEventArgs args)
    {
        Debug.Log("InteractableLogger First Hover Entered: " + args.interactorObject);
    }

    void OnLastHoverExited(HoverExitEventArgs args)
    {
        Debug.Log("InteractableLogger Last Hover Exited: " + args.interactorObject);
    }

    void OnHoverEntered(HoverEnterEventArgs args)
    {
        Debug.Log("InteractableLogger Hover Entered: " + args.interactorObject);
    }

    void OnHoverExited(HoverExitEventArgs args)
    {
        Debug.Log("InteractableLogger Hover Exited: " + args.interactorObject);
    }

    void OnFirstSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("InteractableLogger First Select Entered: " + args.interactorObject);
    }

    void OnLastSelectExited(SelectExitEventArgs args)
    {
        Debug.Log("InteractableLogger Last Select Exited: " + args.interactorObject);
    }

    void OnActivated(SelectEnterEventArgs args)
    {
        Debug.Log("InteractableLogger Activated: " + args.interactorObject);
    }

    void OnDeactivated(SelectExitEventArgs args)
    {
        Debug.Log("InteractableLogger Deactivated: " + args.interactorObject);
    }

    // 新增 Focus 事件
    void OnFirstFocusEntered(FocusEnterEventArgs args)
    {
        Debug.Log("InteractableLogger First Focus Entered: " + args.interactorObject);
    }

    void OnLastFocusExited(FocusExitEventArgs args)
    {
        Debug.Log("InteractableLogger Last Focus Exited: " + args.interactorObject);
    }
}
