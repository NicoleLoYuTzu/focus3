using UnityEngine;
using UnityEngine.AI; // 引入导航命名空间
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR;
using System.Collections.Generic;
using UnityEngine.UIElements; // 引入UI命名空間

public class CustomRayInteractor : MonoBehaviour
{

    [System.Serializable]
    public class TargetUIPair
    {
        public GameObject PreviewArea; // 目標物體
        public GameObject uiPanel; // 對應的 UI 面板
        public GameObject targetObject;
    }

    public ParabolicLineCircle parabolicLineCircle; // 將其他腳本拖動到此引用
    public CalculateUserToTaskDistance calculateUserToTaskDistance; // 將其他腳本拖動到此引用
    private InputDevice controller;
    private LineRenderer lineRenderer;        // LineRenderer component
    public XRRayInteractor rayInteractor; // 连接到 XR Ray Interactor
    public LineRenderer paraboliclineRenderer;
    public List<TargetUIPair> targetObjectsWithUI; // 多个目标物体及其对应的 UI
    private System.Collections.Generic.List<Vector3> checkSpherePositions = new List<Vector3>(); // 存储 CheckSphere 檢測點

    void Start()
    {

        HideAllUI();
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
      
    }

    void Update()
    {

        if (!paraboliclineRenderer.enabled)
        {
            Debug.Log($"paraboliclineRenderer.enabled {paraboliclineRenderer.enabled}");
            parabolicLineCircle.HideUIAndBall();
            HideAllUI();
        }
        
        // 如果控制器在移動並且有有效的射線擊中
        if (IsControllerMoving() && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            Vector3 rayOrigin = rayInteractor.transform.position; // 射線的原始位置
            Vector3 hitPoint = hit.point; // 射線擊中的位置
            NavMeshPath path = new NavMeshPath();
            Debug.Log("TryGetCurrent3DRaycastHit");
            // 計算路徑
            if (NavMesh.CalculatePath(rayOrigin, hitPoint, NavMesh.AllAreas, path))
            {
                Debug.Log("CalculatePath");
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
            //parabolicLineCircle.HideUIAndBall();
        }
    }

    private const float deadZone = 0.1f; // 死區閾值

    private bool IsControllerMoving()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        Vector2 primary2DAxisValue;

        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out primary2DAxisValue))
        {
            // 檢查是否超過死區範圍
            if (primary2DAxisValue.magnitude > deadZone)
            {
                Debug.Log($"111 value {primary2DAxisValue}");
                return true;
            }
            else
            {
                Debug.LogWarning("000 - Inside Dead Zone");
                return false;
            }
        }
        else
        {
            Debug.LogWarning("Cannot get primary2DAxis value.");
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

        Debug.Log($"CustomRayInteractor LineRenderer total {lineRenderer.positionCount} spot：");

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 point = lineRenderer.GetPosition(i);
            Debug.Log($"CustomRayInteractor spot {i}: {point}");
        }

        //// 檢測是否有交集並更新對應的UI
        //CheckIntersection(path);
        // 检测每个目标物体与路径的交集
        List<GameObject> intersectedUIPanels = new List<GameObject>();

        foreach (var pair in targetObjectsWithUI)
        {
            GameObject PreviewArea = pair.PreviewArea;
            GameObject uiPanel = pair.uiPanel;
            GameObject targetObject = pair.targetObject;

            // 检测该物体是否与路径有交集
            if (CheckIntersection(path, PreviewArea))
            {
                ShowMessage(uiPanel, targetObject); // 显示对应的 UI 面板
                intersectedUIPanels.Add(uiPanel); // Add the panel to the list
                calculateUserToTaskDistance.CalculateUserPositionToObject(targetObject,uiPanel);
            }
            else
            {
                HideMessage(uiPanel); // 隐藏对应的 UI 面板
            }
        }
        parabolicLineCircle.ConnectObjectToUI(intersectedUIPanels);

        //bool allNoIntersection = true;
        //foreach (var pair in targetObjectsWithUI)
        //{
        //    GameObject targetObject = pair.PreviewArea;

        //    if (CheckIntersection(path, targetObject))
        //    {
        //        allNoIntersection = false;
        //        break; // 找到一個有交集的物體，跳出迴圈
        //    }
        //}

        //// 如果全部都是沒有交集，則呼叫 HideUIAndBall
        //if (allNoIntersection)
        //{
        //    parabolicLineCircle.HideUIAndBall();
        //}


    }


    private bool CheckIntersection(NavMeshPath path, GameObject PreviewArea)
    {
        Collider targetCollider = PreviewArea.GetComponent<Collider>();
        if (targetCollider == null)
        {
            LogWithName($"Target object {PreviewArea.name} does not have a collider.");
            return false;
        }
        foreach (Vector3 corner in path.corners)
        {
            if (targetCollider.bounds.Contains(corner)) // 确认路径点是否在目标物体的碰撞范围内
            {
                return true;
            }
        }
        return false;
    }

    //private void ShowMessage(GameObject uiPanel, GameObject targetObject)
    //{
    //    LogWithName($"Attempting to show UI Panel: {uiPanel?.name}");
    //    if (uiPanel != null)
    //    {
           
    //        else
    //        {
    //            uiPanel.SetActive(true);
    //            LogWithName($"UI Panel {uiPanel.name} is now visible.");
    //        }
    //    }
    //}

    // 增加 UI 顯示狀態標誌
    private Dictionary<GameObject, bool> uiPanelState = new Dictionary<GameObject, bool>();

    // 顯示訊息，只有當 uiPanel 尚未顯示過時才會顯示
    private void ShowMessage(GameObject uiPanel, GameObject targetObject)
    {
          if (!paraboliclineRenderer.enabled)
            {
                Debug.Log($"paraboliclineRenderer.enabled {paraboliclineRenderer.enabled}");
                parabolicLineCircle.HideUIAndBall();
            }

        if (uiPanel != null && !uiPanelState.ContainsKey(uiPanel))
        {
            uiPanelState[uiPanel] = false; // 初始時設置為隱藏
        }

        if (uiPanel != null && !uiPanelState[uiPanel])
        {
            calculateUserToTaskDistance.UIPanelPosition(uiPanel, targetObject);
            uiPanel.SetActive(true);
            uiPanelState[uiPanel] = true;  // 設置為顯示

            LogWithName($"UI Panel {uiPanel.name} is now visible.");
        }
    }


    // 隱藏訊息，當控制器移出範圍時隱藏 UI
    private void HideMessage(GameObject uiPanel)
    {
        if (uiPanel != null && uiPanelState.ContainsKey(uiPanel))
        {
            uiPanel.SetActive(false);
            uiPanelState[uiPanel] = false; // 設置為隱藏
            LogWithName($"UI Panel {uiPanel.name} is now hidden.");
        }
    }

    private void HideAllUI()
    {
        // 确保所有 UI 元素最开始是隐藏的
        foreach (var uiPair in targetObjectsWithUI)
        {
            GameObject uiElement = uiPair.uiPanel; // 获取对应的 UI 元素
            if (uiElement != null)
            {
                uiElement.SetActive(false); // 隐藏 UI 元素

                // 更新状态为从未显示过
                if (uiPanelState.ContainsKey(uiElement))
                {
                    uiPanelState[uiElement] = false; // 设置为未显示过
                }
                else
                {
                    uiPanelState.Add(uiElement, false); // 如果没有记录，添加到字典并设置为 false
                }
            }
        }
    }


    private void LogWithName(string message)
    {
        Debug.Log($"Nicole: {message}");
    }


}
