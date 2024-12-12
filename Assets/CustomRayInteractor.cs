using UnityEngine;
using UnityEngine.AI; // 引入导航命名空间
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR;
using System.Collections.Generic; // 引入UI命名空間

public class CustomRayInteractor : MonoBehaviour
{

    [System.Serializable]
    public class TargetUIPair
    {
        public GameObject targetObject; // 目標物體
        public GameObject uiPanel; // 對應的 UI 面板
    }
    public ChangeImageMaterial changeImageMaterial; // 引用 ChangeImageMaterial 脚本

    public ParabolicLineCircle parabolicLineCircle; // 將其他腳本拖動到此引用
    private InputDevice controller;

    public XRRayInteractor rayInteractor; // 连接到 XR Ray Interactor
    private LineRenderer lineRenderer;
    public List<TargetUIPair> targetObjectsWithUI; // 多个目标物体及其对应的 UI
    private System.Collections.Generic.List<Vector3> checkSpherePositions = new List<Vector3>(); // 存储 CheckSphere 檢測點

    void Start()
    {


        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f; // 設置起始點的寬度
        lineRenderer.endWidth = 0.1f; // 設置結束點的寬度
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red; // 起始顏色
        lineRenderer.endColor = Color.blue; // 結束顏色
        lineRenderer.useWorldSpace = true; // 使用世界座標
        lineRenderer.enabled = false; // 初始時禁用路徑

        //// 確保所有 UI 元素最開始是隱藏的
        //foreach (var uiPair in targetObjectsWithUI)
        //{
        //    if (uiPair.Value != null)
        //    {
        //        uiPair.Value.SetActive(false);
        //    }
        //}

        // 確保所有 UI 元素最開始是隱藏的
        foreach (var uiPair in targetObjectsWithUI)
        {
            GameObject uiElement = uiPair.uiPanel; // 獲取對應的 UI 元素
            if (uiElement != null)
            {
                uiElement.SetActive(false); // 隱藏 UI 元素
            }
        }
        if (changeImageMaterial == null)
        {
            changeImageMaterial = FindObjectOfType<ChangeImageMaterial>();

            if (changeImageMaterial == null)
            {
                Debug.LogError("ChangeImageMaterial script is not found in the scene!");
            }
            else
            {
                Debug.Log("ChangeImageMaterial script assigned dynamically.");
            }
        }
    }

    void Update()
    {
        // 檢查 UI 面板是否顯示
        foreach (var uiPair in targetObjectsWithUI)
        {
            if (uiPair.uiPanel.activeSelf) // 檢查是否有激活的 UI
            {
                CheckTriggerAndChangeMaterial(uiPair.uiPanel); // 檢測扳機鍵並更換材質
            }
        }

        // 如果控制器在移動並且有有效的射線擊中
        if (IsControllerMoving() && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            Vector3 rayOrigin = rayInteractor.transform.position; // 射線的原始位置
            Vector3 hitPoint = hit.point; // 射線擊中的位置
            NavMeshPath path = new NavMeshPath();

            // 計算路徑
            if (NavMesh.CalculatePath(rayOrigin, hitPoint, NavMesh.AllAreas, path))
            {
                DrawPath(path); // 繪製路徑
            }
            else
            {
                lineRenderer.enabled = false; // 如果路徑無效，隱藏線
            }
        }
        else
        {
            lineRenderer.enabled = false; // 如果沒有擊中，隱藏線
            Debug.Log("UI 隱藏 - 控制器未移動或無有效射線擊中。");

            foreach (var uiPair in targetObjectsWithUI)
            {
                uiPair.uiPanel.SetActive(false); // 隱藏所有 UI 面板
            }
        }
    }

    private bool IsControllerMoving()
    {
        // 獲取右手控制器設備
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        Vector2 primary2DAxisValue;

        // 嘗試獲取操縱桿的值
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out primary2DAxisValue))
        {
            if (primary2DAxisValue == Vector2.zero)
            {
                Debug.LogWarning("方向桿未移動：值為零");
            }
            else
            {
                Debug.Log($"方向桿移動中：值為 {primary2DAxisValue}");
            }
            return primary2DAxisValue != Vector2.zero;
        }
        else
        {
            Debug.LogWarning("無法從控制器檢索 primary2DAxis 值。");
            return false;
        }
    }

    private void DrawPath(NavMeshPath path)
    {
        // 启用 LineRenderer
        lineRenderer.enabled = true;
        lineRenderer.positionCount = path.corners.Length;

        for (int i = 0; i < path.corners.Length; i++)
        {
            lineRenderer.SetPosition(i, path.corners[i]);
        }

        //// 檢測是否有交集並更新對應的UI
        //CheckIntersection(path);
        // 检测每个目标物体与路径的交集
        foreach (var pair in targetObjectsWithUI)
        {
            GameObject targetObject = pair.targetObject;
            GameObject uiPanel = pair.uiPanel;

            // 检测该物体是否与路径有交集
            if (CheckIntersection(path, targetObject))
            {
                ShowMessage(uiPanel); // 显示对应的 UI 面板
            }
            else
            {
                HideMessage(uiPanel); // 隐藏对应的 UI 面板
            }
        }
    }

    private void CheckTriggerAndChangeMaterial(GameObject uiPanel)
    {
        // 获取右手控制器设备
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        // 检测扳机按钮是否按下
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed) && isTriggerPressed)
        {
            if (changeImageMaterial != null)
            {
                changeImageMaterial.ChangeMaterial(uiPanel); // 调用 ChangeImageMaterial 的方法
                Debug.Log("Trigger button pressed, changing material...");
            }
            else
            {
                Debug.LogWarning("ChangeImageMaterial script is not assigned!");
            }
        }
    }

    private bool CheckIntersection(NavMeshPath path, GameObject targetObject)
    {
        Collider targetCollider = targetObject.GetComponent<Collider>();
        if (targetCollider == null)
        {
            LogWithName($"Target object {targetObject.name} does not have a collider.");
            return false;
        }

        //for (int i = 0; i < path.corners.Length; i++)
        //{
        //    Vector3 point = path.corners[i];
        //    if (Physics.CheckSphere(point, 0.1f, LayerMask.GetMask("Interactable")))
        //    {
        //        return true;
        //    }
        //}
        //return false;
        foreach (Vector3 corner in path.corners)
        {
            if (targetCollider.bounds.Contains(corner)) // 确认路径点是否在目标物体的碰撞范围内
            {
                return true;
            }
        }
        return false;
    }

    private void ShowMessage(GameObject uiPanel)
    {
        LogWithName($"Attempting to show UI Panel: {uiPanel?.name}");
        HideAllUI();
        if (uiPanel != null)
        {
            uiPanel.SetActive(true);
            parabolicLineCircle.ConnectObjectToUI(uiPanel);
            LogWithName($"UI Panel {uiPanel.name} is now visible.");
        }
    }

    private void HideMessage(GameObject uiPanel)
    {
        LogWithName($"Attempting to hide UI Panel: {uiPanel?.name}");
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
            LogWithName($"UI Panel {uiPanel.name} is now hidden.");
        }
    }

    private void HideAllUI()
    {

        // 確保所有 UI 元素最開始是隱藏的
        foreach (var uiPair in targetObjectsWithUI)
        {
            GameObject uiElement = uiPair.uiPanel; // 獲取對應的 UI 元素
            if (uiElement != null)
            {
                uiElement.SetActive(false); // 隱藏 UI 元素
            }
        }
    }

    private void LogWithName(string message)
    {
        Debug.Log($"Nicole: {message}");
    }


}
