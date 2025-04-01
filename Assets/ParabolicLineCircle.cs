using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;
using System.Linq;

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
            circleObject.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);

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
    }

    public void HideUIAndBall()
    {
        // 刪除並清空 ballInstances
        foreach (GameObject ball in ballInstances)
        {
            Destroy(ball);
        }
        ballInstances.Clear();

        // 刪除單一 ballInstance（如果存在）
        if (ballInstance != null)
        {
            Destroy(ballInstance);
            ballInstance = null; // 確保變數重置
        }

        // 刪除並清空 uiLineRenderers
        if (uiLineRenderers.Count > 0)
        {
            foreach (var lineRendererEntry in uiLineRenderers.Values)
            {
                if (lineRendererEntry != null)
                {
                    Destroy(lineRendererEntry.gameObject);
                }
            }
            uiLineRenderers.Clear();
        }
    }

    private string GetPositionRelation(Vector3 userPosition, Vector3 objectPosition)
    {
        // 使用者的正前方向
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera not found. Returning 'Unknown' as fallback.");
            return "Unknown";
        }

        Vector3 userForward = mainCamera.transform.forward;

        // 從使用者到目標的方向向量
        Vector3 targetDirection = objectPosition - userPosition;

        // 判斷前後
        float dotForward = Vector3.Dot(userForward, targetDirection.normalized);
        bool isInFront = dotForward > 0;

        if (isInFront)
            return "Front";
        else
            return "Back";
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

        HideUIAndBall(); // 清除先前的球體

        // **依據與 Camera 的距離排序，最遠的優先**
        // 預先對目標進行排序，不需要在每次迭代時再次計算距離
        targetUIPair.Sort((a, b) =>
        {
            float distA = Vector3.Distance(mainCamera.transform.position, a.targetObject.transform.position);
            float distB = Vector3.Distance(mainCamera.transform.position, b.targetObject.transform.position);
            return distA.CompareTo(distB); // 由遠到近排序
        });

        int totalPairs = targetUIPair.Count;

        // **遍歷每個目標，根據索引決定 LineRenderer 上的球體位置**
        for (int i = 0; i < totalPairs; i++)
        {
            var targetUIPairDetail = targetUIPair[i];
            if (targetUIPairDetail == null)
            {
                Log("ParabolicLineCircle: targetUIPairDetail is null!");
                continue;
            }

            RectTransform rectTransform = targetUIPairDetail.uiPanel.GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Log($"ParabolicLineCircle: The provided uiPanel '{targetUIPairDetail.uiPanel.name}' does not have a RectTransform component.");
                continue;
            }

            // **如果 uiPanel 名稱包含 "wing", "heart", "card"，則不創建球體，直接跳過**
            if (targetUIPairDetail.uiPanel.name.Contains("wing") ||
                targetUIPairDetail.uiPanel.name.Contains("heart") ||
                targetUIPairDetail.uiPanel.name.Contains("card"))
            {
                Log($"ParabolicLineCircle: Skipping ball creation for {targetUIPairDetail.uiPanel.name}");
                continue; // 直接跳過這次迴圈，不執行球體生成
            }

            Vector3 uiWorldPosition = Vector3.zero;
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, rectTransform.position);
            GameObject newBallInstance = null;

            Vector3 targetPosition = targetUIPairDetail.targetObject.transform.position;
            Vector3 userPosition = mainCamera.transform.position;

            // **計算球的位置**
            // 不再在這裡做排序，只是傳入已經排序的 targetUIPair
            Vector3 ballPosition = CalculateBallPositionByDistance(lineRenderer, userPosition, targetPosition, i, totalPairs);

            // **創建球體並放到正確的位置**
            newBallInstance = Instantiate(circleObject, ballPosition, Quaternion.identity);
            ballInstances.Add(newBallInstance);

            //// Check if the name of the UI panel contains "wing" or "heart"
            //bool hasWing = targetUIPairDetail.uiPanel.name.Contains("wing");
            //bool hasHeart = targetUIPairDetail.uiPanel.name.Contains("heart");


            //// If it contains wings or hearts, adjust line and ball size
            //if (hasWing || hasHeart)
            //{
            //    // Adjust ball size to be smaller
            //    newBallInstance.transform.localScale = new Vector3(0.0003f, 0.0003f, 0.0003f);  // Make the ball smaller
            //}
            // Check if the panel contains "wing" or "heart" in the name
            //if (targetUIPairDetail.uiPanel.name.Contains("wing") ||
            //    targetUIPairDetail.uiPanel.name.Contains("heart") ||
            //        targetUIPairDetail.uiPanel.name.Contains("card"))
            //{
            //    //// Set finer line width for wings or hearts
            //    //newLineRenderer.startWidth = 0.05f; // Thinner at the start
            //    //newLineRenderer.endWidth = 0.01f;   // Thinner at the end

            //    //// Set a different color (e.g., a soft pastel or other color) for wings or hearts
            //    //newLineRenderer.startColor = new Color(0.6f, 0.8f, 1f, 0.6f); // Light Blue with transparency
            //    //newLineRenderer.endColor = new Color(0.6f, 0.8f, 1f, 0.2f);   // More transparent blue at the end
            //    continue; // 直接結束函式，不建立 LineRenderer
            //}




            if (TryGetWorldPosition(rectTransform, screenPoint, mainCamera, out uiWorldPosition))
            {
                Vector3 connectionPoint = GetUIPanelConnectionPoint(rectTransform, uiWorldPosition);
                CreateLineRenderer(newBallInstance, connectionPoint, targetUIPairDetail.uiPanel);
            }
            else
            {
                Log("ParabolicLineCircle: Failed to convert RectTransform position to world position.");
            }
        }
    }


    private Vector3 CalculateBallPositionByDistance(LineRenderer lineRenderer, Vector3 userPosition, Vector3 targetPosition, int targetIndex, int totalPairs)
    {
        if (totalPairs <= 1)
            return lineRenderer.GetPosition(Mathf.FloorToInt(lineRenderer.positionCount * 0.3f)); // 單一目標時，放 30% 位置

        // 計算每個目標與使用者的距離
        List<float> distances = new List<float>();
        for (int i = 0; i < totalPairs; i++)
        {
            float distance = Vector3.Distance(userPosition, targetPosition);
            distances.Add(distance);
        }

        // 根據距離對目標進行排序，從最小到最大
        List<int> sortedIndices = new List<int>();
        for (int i = 0; i < totalPairs; i++)
        {
            sortedIndices.Add(i);
        }
        sortedIndices.Sort((a, b) => distances[a].CompareTo(distances[b])); // 按距離排序

        // 計算最接近的目標的索引，並根據最小距離開始
        float t = (float)targetIndex / (totalPairs - 1); // t 值在 0 ~ 1 之間
        int minIndex = Mathf.FloorToInt(lineRenderer.positionCount * 0.5f);
        int maxIndex = Mathf.FloorToInt(lineRenderer.positionCount * 0.8f);

        // 在 minIndex 到 maxIndex 之間進行插值
        int lerpIndex = Mathf.FloorToInt(Mathf.Lerp(minIndex, maxIndex, t));

        // 返回計算後的球體位置
        return lineRenderer.GetPosition(lerpIndex);
    }

    private Vector3 GetUIPanelConnectionPoint(RectTransform rectTransform, Vector3 uiWorldPosition)
    {
        // 取得 UI Panel 的寬度 & 高度
        float panelWidth = rectTransform.rect.width * rectTransform.lossyScale.x;
        float panelHeight = rectTransform.rect.height * rectTransform.lossyScale.y;

        // 取得拋物線的終點
        Vector3 lastPointOnLine = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

        // 判斷 UI 面板的位置
        bool isUIPanelOnRight = uiWorldPosition.x > lastPointOnLine.x;
        bool isUIPanelOnTop = uiWorldPosition.y > lastPointOnLine.y;

        if (isUIPanelOnRight && isUIPanelOnTop)
        {
            // UI 在右上角，連接到 UI 的左下角
            return uiWorldPosition + new Vector3(-panelWidth / 2, -panelHeight / 2, 0);
        }
        else if (isUIPanelOnRight && !isUIPanelOnTop)
        {
            // UI 在右下角，連接到 UI 的左上角
            return uiWorldPosition + new Vector3(-panelWidth / 2, panelHeight / 2, 0);
        }
        else if (!isUIPanelOnRight && isUIPanelOnTop)
        {
            // UI 在左上角，連接到 UI 的右下角
            return uiWorldPosition + new Vector3(panelWidth / 2, -panelHeight / 2, 0);
        }
        else
        {
            // UI 在左下角，連接到 UI 的右上角
            return uiWorldPosition + new Vector3(panelWidth / 2, panelHeight / 2, 0);
        }
    }



    private bool TryGetWorldPosition(RectTransform rectTransform, Vector3 screenPoint, Camera mainCamera, out Vector3 uiWorldPosition)
    {
        return RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPoint, mainCamera, out uiWorldPosition);
    }


    private void HandleBallPositions(List<CustomRayInteractor.TargetUIPair> targetUIPairList, Camera mainCamera)
    {
        if (targetUIPairList == null || targetUIPairList.Count == 0) return;

        Vector3 lineStart = lineRenderer.GetPosition(0);
        Vector3 lineEnd = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        Vector3 userPosition = mainCamera.transform.position;

        // 計算每個目標與使用者的距離
        List<(CustomRayInteractor.TargetUIPair, float)> targetDistances = new List<(CustomRayInteractor.TargetUIPair, float)>();
        foreach (var target in targetUIPairList)
        {
            float distance = Vector3.Distance(userPosition, target.targetObject.transform.position);
            targetDistances.Add((target, distance));
        }

        // 根據距離遞減排序 (距離最遠的在前方)
        targetDistances.Sort((a, b) => b.Item2.CompareTo(a.Item2));

        int totalTargets = targetDistances.Count;

        // 計算 `n` 個等間距點
        for (int i = 0; i < totalTargets; i++)
        {
            float t = (i + 1) / (float)(totalTargets + 1); // 避免 0% 和 100%，確保點落在中間
            Vector3 ballPositionOnLine = Vector3.Lerp(lineStart, lineEnd, t);

            // 創建或更新球體
            if (ballInstances.Count > i)
            {
                ballInstances[i].transform.position = ballPositionOnLine;
            }
            else
            {
                GameObject newBallInstance = Instantiate(circleObject, ballPositionOnLine, Quaternion.identity);
                ballInstances.Add(newBallInstance);
            }

            Log($"Ball {i + 1}/{totalTargets} positioned at: {ballPositionOnLine}");
        }
    }


    private void CreateLineRenderer(GameObject newBallInstance, Vector3 uiWorldPosition, GameObject uiPanel)
    {
        // 創建新的 LineRenderer
        LineRenderer newLineRenderer = new GameObject("UILineRenderer").AddComponent<LineRenderer>();
        newLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        newLineRenderer.useWorldSpace = true;

        // 設定 SortingLayer，確保在 UI 上層或適當的層級
        newLineRenderer.sortingLayerID = SortingLayer.NameToID("UI");
        newLineRenderer.sortingOrder = -1;

        // 設定線條的起點、終點
        newLineRenderer.positionCount = 2;
       

        // **設置首端粗、末端細**
        newLineRenderer.startWidth = 0.08f; // 線的起點較粗
        newLineRenderer.endWidth = 0.02f;   // 線的終點較細

        // **設置半透明白色**
        Color transparentWhite = new Color(1f, 1f, 1f, 0.6f); // 60% 透明
        newLineRenderer.startColor = transparentWhite;
        newLineRenderer.endColor = new Color(1f, 1f, 1f, 0.2f); // 末端更透明

      

        newLineRenderer.SetPosition(0, newBallInstance.transform.position);
        newLineRenderer.SetPosition(1, uiWorldPosition);

        // 更新字典
        uiLineRenderers[uiPanel] = newLineRenderer;
    }




    private void UpdateLineRenderer(GameObject uiPanel, GameObject newBallInstance)
    {
        if (uiLineRenderers.TryGetValue(uiPanel, out LineRenderer existingLineRenderer))
        {
            existingLineRenderer.SetPosition(0, newBallInstance.transform.position);
            existingLineRenderer.SetPosition(1, uiPanel.GetComponent<RectTransform>().position);

            Log($"Updated ballInstance line for UI Panel '{uiPanel.name}' with new ball position: {newBallInstance.transform.position}");
        }
    }

    public void OnUIPanelHiddenOrDestroyed(GameObject uiPanel)
    {
        // 當面板隱藏或銷毀時移除對應的 LineRenderer
        if (uiLineRenderers.TryGetValue(uiPanel, out LineRenderer lineRenderer))
        {
            Destroy(lineRenderer.gameObject); // 刪除 LineRenderer
            uiLineRenderers.Remove(uiPanel); // 從字典移除
            Log($"LineRenderer for UI Panel '{uiPanel.name}' has been removed.");
        }
    }

    private void Log(string message) { Debug.Log($"ParabolicLineCircle: {message}"); }
    private void LogWarning(string message) { Debug.LogWarning($"ParabolicLineCircle: {message}"); }
}
