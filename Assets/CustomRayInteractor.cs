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

    }

    void Update()
    {
        if (IsControllerMoving() && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            Vector3 rayOrigin = rayInteractor.transform.position; // 射线的原始位置
            Vector3 hitPoint = hit.point; // 射线击中的位置
            NavMeshPath path = new NavMeshPath();

            // 计算路径
            if (NavMesh.CalculatePath(rayOrigin, hitPoint, NavMesh.AllAreas, path))
            {
                DrawPath(path); // 绘制路径
            }
            else
            {
                lineRenderer.enabled = false; // 如果路径无效，隐藏线
            }
        }
        else
        {
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
            return primary2DAxisValue != Vector2.zero;
        }
        else
        {
            Debug.LogWarning("Unable to retrieve primary2DAxis value from the controller.");
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

    private bool CheckIntersection(NavMeshPath path, GameObject targetObject)
    {
        Collider targetCollider = targetObject.GetComponent<Collider>();
        if (targetCollider == null)
        {
            LogWithName($"Target object {targetObject.name} does not have a collider.");
            return false;
        }

        for (int i = 0; i < path.corners.Length; i++)
        {
            Vector3 point = path.corners[i];
            if (Physics.CheckSphere(point, 0.1f, LayerMask.GetMask("Interactable")))
            {
                return true;
            }
        }
        return false;
    }

    private void ShowMessage(GameObject uiPanel)
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(true); // 顯示UI面板
            LogWithName("UI Panel is now visible.");
        }
        else
        {
            LogWithName("No UI Panel assigned.");
        }
    }

    private void HideMessage(GameObject uiPanel)
    {
        // 如果 UI 面板存在，隐藏它
        if (uiPanel != null)
        {
            uiPanel.SetActive(false); // 隐藏 UI 面板
            LogWithName($"{uiPanel.name} is now hidden.");
        }
        else
        {
            LogWithName("No UI Panel assigned to hide.");
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Vector3 position in checkSpherePositions)
        {
            Gizmos.DrawWireSphere(position, 0.1f); // 繪製CheckSphere檢測位置
        }
    }
}
