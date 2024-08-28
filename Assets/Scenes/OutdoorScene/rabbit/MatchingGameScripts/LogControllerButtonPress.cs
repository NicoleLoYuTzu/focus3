using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class LogControllerButtonPress : MonoBehaviour
{
    private XRController xrController;

    void Start()
    {
        // 获取与该脚本挂载的对象相关的 XRController 组件
        xrController = GetComponent<XRController>();
        if (xrController == null)
        {
            Debug.LogError("XRController component not found on this GameObject.");
        }
    }

    void Update()
    {
        if (xrController != null)
        {
            CheckButtonPress();
        }
    }

    private void CheckButtonPress()
    {
        // 检查Trigger按钮是否被按下
        if (xrController.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerValue) && triggerValue)
        {
            Debug.Log("Trigger button pressed");
        }

        // 检查Grip按钮是否被按下
        if (xrController.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool gripValue) && gripValue)
        {
            Debug.Log("Grip button pressed");
        }

        // 检查Primary按钮（通常是A或X按钮）是否被按下
        if (xrController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryValue) && primaryValue)
        {
            Debug.Log("Primary button pressed");
        }

        // 检查Secondary按钮（通常是B或Y按钮）是否被按下
        if (xrController.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryValue) && secondaryValue)
        {
            Debug.Log("Secondary button pressed");
        }
    }
}
