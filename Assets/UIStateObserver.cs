using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class XRSimpleInteractableStateObserver : UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable
{
  

    // 在Awake中註冊新的事件
    protected override void Awake()
    {
        base.Awake();

        // 使用新的事件簽名
        hoverEntered.AddListener(OnHoverEnter);
        hoverExited.AddListener(OnHoverExit);
        selectEntered.AddListener(OnSelectEntered);
        selectExited.AddListener(OnSelectExited);
        focusEntered.AddListener(OnFocusEntered);
        focusExited.AddListener(OnFocusExited);
    }



    // Hover Entered 事件處理
    private void OnHoverEnter(HoverEnterEventArgs args)
    {
      
        Debug.Log($"{gameObject.name} is being hovered.");
    }

    // Hover Exited 事件處理
    private void OnHoverExit(HoverExitEventArgs args)
    {
        Debug.Log($"{gameObject.name} is no longer hovered.");
    }

    // Select Entered 事件處理
    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log($"{gameObject.name} has been selected.");
    }

    // Select Exited 事件處理
    private void OnSelectExited(SelectExitEventArgs args)
    {
        Debug.Log($"{gameObject.name} has been deselected.");
    }

    // Focus Entered 事件處理
    private void OnFocusEntered(FocusEnterEventArgs args)
    {
        Debug.Log($"{gameObject.name} has been focused.");
    }

    // Focus Exited 事件處理
    private void OnFocusExited(FocusExitEventArgs args)
    {
        Debug.Log($"{gameObject.name} has lost focus.");
    }

    // OnTriggerEnter 事件處理
    private void OnTriggerEnter(Collider other)
    {
            Debug.Log($"{gameObject.name} triggered by {other.gameObject.name}");
    }

    // OnTriggerExit 事件處理
    private void OnTriggerExit(Collider other)
    {
            Debug.Log($"{gameObject.name} trigger exited by {other.gameObject.name}");
    }

}
