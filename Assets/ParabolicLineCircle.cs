using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class ParabolicLineCircle : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual lineVisual; // Reference to XRInteractorLineVisual
    public GameObject circleObject;           // Circle 3D object to instantiate
    private LineRenderer lineRenderer;        // LineRenderer component
    public XRRayInteractor rayInteractor; // 连接到 XR Ray Interactor
    private GameObject ballInstance;          // Ball instance to hold the created object
    private List<GameObject> ballInstances = new List<GameObject>(); // 用來儲存所有創建的 ballInstance

    private Dictionary<GameObject, LineRenderer> uiLineRenderers = new Dictionary<GameObject, LineRenderer>(); // 存儲UI元素對應的LineRenderer
    private LineRenderer existingLineRenderer;
    private LineRenderer newLineRenderer;
    void Start()
    {
        // Check if lineVisual and circleObject are assigned
        if (lineVisual != null && circleObject != null)
        {
            circleObject.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);

            // Retrieve the LineRenderer component from the XRInteractorLineVisual
            lineRenderer = lineVisual.GetComponent<LineRenderer>();
        }
    }

    void Update()
    {
        if (lineRenderer == null)
        {
            Log("ParabolicLineCircle lineRenderer is null, skipping Update logic.");
            return;  // 如果 lineRenderer 為 null，就跳過後續邏輯
        }

        Log("Update method running...");

        int pointCount = lineRenderer.positionCount;
        Log($"ParabolicLineCircle LineRenderer has {pointCount} points.");

        //if (IsControllerMoving() && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        //{
        //    if (pointCount > 0)
        //    {
        //        Vector3[] linePoints = new Vector3[pointCount];
        //        lineRenderer.GetPositions(linePoints);
        //        Vector3 middlePoint = linePoints[pointCount / 2];

        //        // 只在 ballInstance 為 null 時創建一個新的球體
        //        if (ballInstance == null)
        //        {
        //            ballInstance = Instantiate(circleObject, middlePoint, Quaternion.identity);
        //            Log("ballInstance created.");
        //        }
        //        else
        //        {
        //            // 如果 ballInstance 已經存在，則只更新它的位置
        //            ballInstance.transform.position = middlePoint;
        //            Log("ballInstance position updated.");
        //        }
        //    }
        //}
        //else
        //{
        //    // 如果沒有命中，並且 ballInstance 存在，則清除 ballInstance
        //    if (ballInstance != null)
        //    {
        //        Destroy(ballInstance);  // 使用 Destroy 而不是設為 null
        //        ballInstance = null;
        //        Log("ballInstance destroyed because no hit detected.");
        //    }
        //}
    }

    public void HideUIAndBall()
    {
        // 紀錄當前狀態
        Log($"ballInstance: {ballInstance}"); // 確認 ballInstance 的狀態

        // 檢查並隱藏 ballInstance
        if (ballInstance != null)
        {
            Destroy(ballInstance);
        }

        // 檢查並隱藏所有 uiLineRenderers
        if (uiLineRenderers != null && uiLineRenderers.Count > 0)
        {
            Log("Hiding uiLineRenderers...");

            foreach (var lineRendererEntry in uiLineRenderers)
            {
                LineRenderer lineRenderer = lineRendererEntry.Value;
                if (lineRenderer != null)
                {
                    // 清除已繪製的線條
                    lineRenderer.positionCount = 0;

                    // 隱藏 LineRenderer
                    lineRenderer.gameObject.SetActive(false);
                }
            }

            // 清空 uiLineRenderers 字典
            uiLineRenderers.Clear();
        }
    }
    public void ClearOldBalls()
    {
        foreach (GameObject ball in ballInstances)
        {
            Destroy(ball);  // 刪除每一個圓球物件
        }
        ballInstances.Clear();  // 清空列表
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
            Debug.LogWarning("無法從控制器檢索 primary2DAxis 值。");
            return false;
        }
    }



    public void ConnectObjectToUI(List<CustomRayInteractor.TargetUIPair> targetUIPair)
    {
        Log("ParabolicLineCircle: ConnectObjectToUI started.");

        if (targetUIPair == null || targetUIPair.Count == 0)
        {
            Log("ParabolicLineCircle: targetUIPair list is null or empty!");
            return;
        }

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Log("ParabolicLineCircle: No main camera found in the scene!");
            return;
        }

        ClearOldBalls(); // 清除之前的球实例

        foreach (CustomRayInteractor.TargetUIPair targetUIPairDetail in targetUIPair)
        {
            Log($"Processing TargetUIPair: {targetUIPairDetail}");
            if (targetUIPairDetail == null)
            {
                Log("ParabolicLineCircle: targetUIPairDetail is null!");
                continue;
            }

            RectTransform rectTransform = targetUIPairDetail.uiPanel.GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Log($"ParabolicLineCircle: The provided uiPanel '{targetUIPairDetail.uiPanel}' does not have a RectTransform component.");
                continue;
            }

            Vector3 uiWorldPosition;
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, rectTransform.position);

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                rectTransform,
                screenPoint,
                mainCamera,
                out uiWorldPosition))
            {
                Log($"UI world position calculated: {uiWorldPosition}");

                Vector3 lineStart = lineRenderer.GetPosition(0); // 起始点
                Vector3 lineEnd = lineRenderer.GetPosition(1);   // 终点

                Vector3 userPosition = mainCamera.transform.position; // 用户位置为主相机位置
                float distanceToUser = Vector3.Distance(rectTransform.position, userPosition);

                float maxParabolicDistance = 100f;
                float ratio = Mathf.InverseLerp(0f, maxParabolicDistance, distanceToUser);

                Vector3 ballPositionOnLine;
                if (distanceToUser > maxParabolicDistance)
                {
                    ballPositionOnLine = Vector3.Dot(rectTransform.position - userPosition, lineEnd - lineStart) > 0
                        ? lineEnd
                        : lineStart;
                }
                else
                {
                    ballPositionOnLine = Vector3.Lerp(lineStart, lineEnd, ratio);
                }

                GameObject newBallInstance = Instantiate(circleObject, ballPositionOnLine, Quaternion.identity);
                ballInstances.Add(newBallInstance); // 添加到列表中
                Log($"Created new ballInstance at: {newBallInstance.transform.position}");

                LineRenderer newLineRenderer = new GameObject("UILineRenderer").AddComponent<LineRenderer>();
                newLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                newLineRenderer.useWorldSpace = true;

                newLineRenderer.sortingLayerID = SortingLayer.NameToID("UI");
                newLineRenderer.sortingOrder = -1;

                newLineRenderer.positionCount = 2;
                newLineRenderer.startColor = Color.white;
                newLineRenderer.endColor = Color.white;
                newLineRenderer.widthMultiplier = 0.05f;

                newLineRenderer.SetPosition(0, newBallInstance.transform.position);
                newLineRenderer.SetPosition(1, uiWorldPosition);

                uiLineRenderers[targetUIPairDetail.uiPanel] = newLineRenderer; // 更新字典
            }
            else
            {
                Log("Failed to convert RectTransform position to world position.");
            }
        }
    }


    public void HideLineRenderers()
    {
        if (existingLineRenderer != null)
        {
            // 清除現有線的點數，隱藏線條
            existingLineRenderer.positionCount = 0;
            existingLineRenderer.gameObject.SetActive(false);
            Log("Existing LineRenderer hidden.");
        }
        else
        {
            LogWarning("Existing LineRenderer is null.");
        }

        if (newLineRenderer != null)
        {
            // 清除新線的點數，隱藏線條
            newLineRenderer.positionCount = 0;
            newLineRenderer.gameObject.SetActive(false);
            Log("New LineRenderer hidden.");
        }
        else
        {
            LogWarning("New LineRenderer is null.");
        }
    }





    private void Log(string message) { Debug.Log($"ParabolicLineCircle: {message}"); }
    private void LogWarning(string message) { Debug.LogWarning($"ParabolicLineCircle: {message}"); }
}
