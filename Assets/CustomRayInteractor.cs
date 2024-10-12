using UnityEngine;
using UnityEngine.AI; // 引入导航命名空间
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR;

using System.Collections.Generic; // 引入UI命名空間

public class CustomRayInteractor : MonoBehaviour
{
    public XRRayInteractor rayInteractor; // 连接到 XR Ray Interactor
    private LineRenderer lineRenderer;
    public GameObject uiPanel; // 用来展示画面的UI元素
    public GameObject targetObject; // 目標物體，與其進行交集檢測
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

        // 確保 UI 元素最開始是隱藏的
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (IsControllerMoving()&&rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            Vector3 rayOrigin = rayInteractor.transform.position; // 射线的原始位置
            Vector3 hitPoint = hit.point; // 射线击中的位置
            LogWithName($"rayInteractor rayInteractor.transform.position {rayOrigin}");
            LogWithName($"rayInteractor hit.point {hit.point}");
            NavMeshPath path = new NavMeshPath();
            // 计算路径
            if (NavMesh.CalculatePath(rayOrigin, hitPoint, NavMesh.AllAreas, path))
            {
                LogWithName($"rayInteractor rayInteractor.transform.position {rayOrigin}");
                LogWithName($"rayInteractor hit.point {hit.point}");
                DrawPath(path); // 绘制路径
            }
            else
            {
                lineRenderer.enabled = false; // 如果路径无效，隐藏线
            }
        }
        else
        {
            LogWithName("No hit detected.");
            lineRenderer.enabled = false; // 如果没有击中，隐藏线
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
            // 如果操縱桿的值不為 (0,0)，表示控制器正在移動
            return primary2DAxisValue != Vector2.zero;
        }
        else
        {
            // 無法獲取操縱桿值，可能是設備不支持或未連接
            Debug.LogWarning("Unable to retrieve primary2DAxis value from the controller.");
            return false;
        }
    }



    private void DrawPath(NavMeshPath path)
    {
        // 启用 LineRenderer
        lineRenderer.enabled = true;

        // 检查路径的角点数量
        LogWithName($"Path corners count: {path.corners.Length}");

        // 设置 LineRenderer 的点数
        lineRenderer.positionCount = path.corners.Length;

        // 将路径的角点赋值给 LineRenderer
        for (int i = 0; i < path.corners.Length; i++)
        {
            lineRenderer.SetPosition(i, path.corners[i]);
        }

        // 检查 LineRenderer 是否与目标物体有交集
        if (CheckIntersection(path))
        {
            ShowMessage(); // 只有在交集檢測為真時顯示畫面
        }
    }

    private bool CheckIntersection(NavMeshPath path)
    {
        if (targetObject == null)
        {
            LogWithName("No target object assigned for intersection check.");
            return false;
        }

        Collider targetCollider = targetObject.GetComponent<Collider>();
        if (targetCollider == null)
        {
            LogWithName("Target object does not have a collider.");
            return false;
        }

        // 檢查每個角點是否與目標物體的碰撞體有交集
        for (int i = 0; i < path.corners.Length; i++)
        {
            Vector3 point = path.corners[i];

            // 使用小球檢測與目標物體是否有交集
            if (Physics.CheckSphere(point, 0.1f, LayerMask.GetMask("Interactable")))
            {
                LogWithName($"Intersection detected at point {point}");
                return true; // 有交集
            }
        }

        LogWithName("No intersection detected.");
        return false; // 沒有交集
    }

    private void ShowMessage()
    {
        // 如果有UI面板，顯示它
        if (uiPanel != null)
        {
            uiPanel.SetActive(true); // 顯示 UI 面板
            LogWithName("UI Panel is now visible.");
        }
        else
        {
            LogWithName("No UI Panel assigned.");
        }
    }

    private void LogWithName(string message)
    {
        Debug.Log($"Nicole: {message}");
    }


    private void OnDrawGizmos()
    {
        // 通过 Gizmos 可视化 CheckSphere 的位置和半径
        Gizmos.color = Color.green; // 設置Gizmos的顏色
        foreach (Vector3 position in checkSpherePositions)
        {
            Gizmos.DrawWireSphere(position, 0.1f); // 繪製CheckSphere檢測位置
        }
    }


}
