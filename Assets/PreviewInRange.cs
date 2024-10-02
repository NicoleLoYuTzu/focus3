using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class PreviewInRangeVR : MonoBehaviour
{

    public float radius = 5f; // 圓形範圍的半徑
    public GameObject previewPrefab; // 預覽物件
    private Camera mainCamera; // 主攝影機
    private GameObject previewInstance; // 獲取 phantopreview 物件
    private Preview previewScript; // 獲取 Preview 腳本
    private bool isPreviewShown = false; // 記錄預覽是否顯示

    void Start()
    {
        mainCamera = Camera.main; // 獲取主要相機
        previewInstance = mainCamera.transform.Find("PhantoPreview").gameObject; // 獲取 phantopreview 物件
        previewScript = previewInstance.GetComponent<Preview>(); // 獲取 Preview 腳本
        HidePreview(); // 初始狀態隱藏預覽
        Debug.Log("[PREVIEW] PreviewInRangeVR script started, preview hidden by default.");
    }

    void Update()
    {
        // 檢測控制器是否有移動
        if (IsControllerMoving())
        {
            Debug.Log("[PREVIEW] Controller is moving, performing raycast check.");
            // 當控制器移動時進行交集檢測
            PerformRaycastCheck();
        }
        else
        {
            Debug.Log("[PREVIEW] Controller is not moving.");
            // 如果控制器沒有移動且預覽顯示中，隱藏預覽物件
            if (isPreviewShown)
            {
                HidePreview();
                isPreviewShown = false;
                Debug.Log("[PREVIEW] Preview hidden because controller stopped moving.");
            }
        }
    }

    private bool IsControllerMoving()
    {
        // 檢查控制器的 2D 軸（操縱桿）輸入，用於檢測前後左右的移動
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        Vector2 primary2DAxisValue;

        // 嘗試獲取操控桿的值
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out primary2DAxisValue))
        {
            Debug.Log($"[PREVIEW] Controller primary2DAxisValue: {primary2DAxisValue}");
            // 如果操縱桿的值不為 (0,0)，表示控制器正在移動
            return primary2DAxisValue != Vector2.zero;
        }

        // 嘗試獲取其他可能的控制器輸入（如觸摸板或其他）
        float triggerValue;
        if (device.TryGetFeatureValue(CommonUsages.trigger, out triggerValue) && triggerValue > 0.1f)
        {
            Debug.Log($"[PREVIEW] Controller trigger value: {triggerValue}");
            return true; // 假設觸發器被按下表示正在移動
        }

        Debug.LogWarning("[PREVIEW] Failed to get movement input from controller.");
        return false;
    }


    private bool IsRayInRange(Vector3 rayOrigin, Vector3 rayDirection, Vector3 objectCenter, float radius)
    {
        // 計算從射線起始點到物件中心的向量
        Vector3 toObject = objectCenter - rayOrigin;

        // 計算射線與物件中心的垂直距離
        float projection = Vector3.Dot(toObject, rayDirection.normalized);

        // 確保投影點在射線的範圍內
        if (projection < 0)
        {
            // 投影點在射線的起始點之前
            return false;
        }

        // 計算投影點的位置
        Vector3 closestPointOnRay = rayOrigin + rayDirection.normalized * projection;

        // 計算投影點到物件中心的距離
        float distanceToCenter = Vector3.Distance(closestPointOnRay, objectCenter);

        // 判斷距離是否在範圍內
        return distanceToCenter <= radius;
    }

    private void PerformRaycastCheck()
    {
        // 獲取控制器的前進方向，並進行 Raycast 檢測
        Ray ray = new Ray(transform.position, transform.forward);
        float distance = radius;

        // 這裡的 objectCenter 是您物件的中心
        Vector3 objectCenter = transform.position; // 或者是您物件的其他位置

        // 檢查射線是否在範圍內
        if (IsRayInRange(ray.origin, ray.direction, objectCenter, radius))
        {
            // 若射線與範圍相交，執行相應邏輯
            ShowPreview();
            isPreviewShown = true;
            Debug.Log("[PREVIEW] Preview shown because ray is within range.");
        }
        else
        {
            // 若射線不在範圍內，隱藏預覽物件
            HidePreview();
            isPreviewShown = false;
            Debug.Log("[PREVIEW] Preview hidden because ray is out of range.");
        }
    }


    //private void PerformRaycastCheck()
    //{
    //    // 獲取控制器的前進方向，並進行 Raycast 檢測
    //    Ray ray = new Ray(transform.position, transform.forward);
    //    float distance = radius;

    //    Debug.Log($"[PREVIEW] Performing raycast with ray origin: {transform.position}, direction: {transform.forward}, distance: {distance}");

    //    // 檢測直線與圓形的交集
    //    if (Physics.Raycast(ray, out RaycastHit hit, distance))
    //    {
    //        Vector3 hitPoint = hit.point;
    //        Vector3 center = transform.position;

    //        Debug.Log($"[PREVIEW] Raycast hit at {hitPoint}, checking distance to center.");

    //        // 計算到圓心的距離
    //        float distanceToCenter = Vector3.Distance(hitPoint, center);
    //        Debug.Log($"[PREVIEW] Distance to center: {distanceToCenter}, Radius: {radius}");

    //        if (distanceToCenter <= radius && !isPreviewShown)
    //        {
    //            // 若進入範圍且預覽未顯示，顯示預覽物件
    //            ShowPreview();
    //            isPreviewShown = true;
    //            Debug.Log("[PREVIEW] Preview shown because object is within range.");
    //        }
    //        else if (distanceToCenter > radius && isPreviewShown)
    //        {
    //            // 若離開範圍且預覽顯示中，隱藏預覽物件
    //            HidePreview();
    //            isPreviewShown = false;
    //            Debug.Log("[PREVIEW] Preview hidden because object left range.");
    //        }
    //    }
    //    else if (isPreviewShown)
    //    {
    //        // 若沒有檢測到任何物體且預覽顯示中，隱藏預覽物件
    //        HidePreview();
    //        isPreviewShown = false;
    //        Debug.Log("[PREVIEW] Preview hidden because no object was hit by the raycast.");
    //    }
    //}

    private void ShowPreview()
    {
        previewScript.Show(); // 調用 Preview 腳本中的 Show 方法
        Debug.Log("[PREVIEW] Preview object shown.");
    }

    private void HidePreview()
    {
        previewScript.Hide(); // 調用 Preview 腳本中的 Hide 方法
        Debug.Log("[PREVIEW] Preview object hidden.");
    }

    private void OnDrawGizmos()
    {
        // 設置 Gizmos 顏色
        Gizmos.color = Color.red;
        // 劃出圓形範圍
        Gizmos.DrawWireSphere(transform.position, radius);
        Debug.Log("[PREVIEW] Gizmos drawn to represent detection radius.");
    }
}
