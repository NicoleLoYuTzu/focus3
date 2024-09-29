using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CirclePreviewDetector : MonoBehaviour
{
    public GameObject previewPrefab; // 要顯示的 Preview 預製物件
    public float circleRadius = 10f;  // 圓形範圍的半徑
    public Transform player;          // 玩家物件的 Transform

    private GameObject currentPreview; // 當前顯示的 Preview

    void Update()
    {
        Vector3 playerPathEnd = player.position + GetControllerDirection() * 5f; // 設定距離為5單位

        if (IsLineIntersectingCircle(player.position, playerPathEnd, transform.position, circleRadius))
        {
            ShowPreview();
        }
        else
        {
            HidePreview();
        }
    }

    private Vector3 GetControllerDirection()
    {
        Vector3 direction = Vector3.zero;

        // 獲取左手或右手控制器的輸入
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand); // 使用右手控制器
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 inputAxis))
        {
            direction = new Vector3(inputAxis.x, 0, inputAxis.y).normalized; // 計算方向
        }

        return direction;
    }

    private bool IsLineIntersectingCircle(Vector3 lineStart, Vector3 lineEnd, Vector3 circleCenter, float circleRadius)
    {
        Vector3 lineDir = lineEnd - lineStart;
        Vector3 lineToCircle = circleCenter - lineStart;

        float projection = Vector3.Dot(lineToCircle, lineDir.normalized);
        Vector3 closestPoint = lineStart + lineDir.normalized * projection;
        Vector3 closestToCircle = circleCenter - closestPoint;

        return closestToCircle.sqrMagnitude <= circleRadius * circleRadius;
    }

    private void ShowPreview()
    {
        if (currentPreview == null)
        {
            currentPreview = Instantiate(previewPrefab, transform.position, Quaternion.identity);
        }
        currentPreview.SetActive(true);
    }

    private void HidePreview()
    {
        if (currentPreview != null)
        {
            currentPreview.SetActive(false);
        }
    }
}
