using UnityEngine;
using UnityEngine.AI; // 引入导航命名空间
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CustomRayInteractor : MonoBehaviour
{
    public XRRayInteractor rayInteractor; // 连接到 XR Ray Interactor
    private LineRenderer lineRenderer;

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
    }

    void Update()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
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
    }

    private void LogWithName(string message)
    {
        Debug.Log($"Nicole: {message}");
    }
}
