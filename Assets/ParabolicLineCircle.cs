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

        HideUIAndBall(); // Clear previous ball instances

        foreach (CustomRayInteractor.TargetUIPair targetUIPairDetail in targetUIPair)
        {
            if (targetUIPairDetail == null)
            {
                Log("ParabolicLineCircle: targetUIPairDetail is null!");
                continue;
            }

            Log($"ParabolicLineCircle: Processing TargetUIPair with UI Panel: {targetUIPairDetail.uiPanel.name}");

            RectTransform rectTransform = targetUIPairDetail.uiPanel.GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Log($"ParabolicLineCircle: The provided uiPanel '{targetUIPairDetail.uiPanel.name}' does not have a RectTransform component.");
                continue;
            }

            Vector3 uiWorldPosition = Vector3.zero;
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, rectTransform.position);
            GameObject newBallInstance = null;

            if (!uiLineRenderers.ContainsKey(targetUIPairDetail.uiPanel))
            {
                if (TryGetWorldPosition(rectTransform, screenPoint, mainCamera, out uiWorldPosition))
                {
                    Vector3 connectionPoint = GetUIPanelConnectionPoint(rectTransform, uiWorldPosition);
                    newBallInstance = HandleBallPosition(targetUIPairDetail, mainCamera, rectTransform, connectionPoint, ref newBallInstance);
                    CreateLineRenderer(newBallInstance, connectionPoint, targetUIPairDetail.uiPanel);
                }
                else
                {
                    Log("ParabolicLineCircle: Failed to convert RectTransform position to world position.");
                }
            }
            else
            {
                Vector3 connectionPoint = GetUIPanelConnectionPoint(rectTransform, uiWorldPosition);
                newBallInstance = HandleBallPosition(targetUIPairDetail, mainCamera, rectTransform, connectionPoint, ref newBallInstance);
                UpdateLineRenderer(targetUIPairDetail.uiPanel, newBallInstance);
            }
        }
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

    private GameObject HandleBallPosition(CustomRayInteractor.TargetUIPair targetUIPairDetail, Camera mainCamera, RectTransform rectTransform, Vector3 uiWorldPosition, ref GameObject newBallInstance)
    {
        Vector3 lineStart = lineRenderer.GetPosition(0);
        Vector3 lineEnd = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

        Vector3 userPosition = mainCamera.transform.position;
        Vector3 targetPosition = targetUIPairDetail.targetObject.transform.position;
        Vector3 ballPositionOnLine = Vector3.zero;

        if (rayInteractor != null && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            Vector3 hitPoint = hit.point;
            Debug.Log($"Ray hit point: {hitPoint}");

            float lineEndToUser = Vector3.Distance(hitPoint, userPosition);
            float targetToUser = Vector3.Distance(targetPosition, userPosition);

            Log($"ParabolicLineCircle: lineEndToUser '{lineEndToUser}' targetToUser {targetToUser}.");

            string positionRelation = GetPositionRelation(userPosition, targetPosition);

            ballPositionOnLine = CalculateBallPosition(lineStart, lineEnd, lineEndToUser, targetToUser, positionRelation);

            // Create or update the ball instance
            if (newBallInstance == null)
            {
                newBallInstance = Instantiate(circleObject, ballPositionOnLine, Quaternion.identity);
                ballInstances.Add(newBallInstance);
            }
            else
            {
                newBallInstance.transform.position = ballPositionOnLine;
            }

            Log($"ParabolicLineCircle: Updated ballInstance for UI Panel '{targetUIPairDetail.uiPanel.name}' to position: {ballPositionOnLine}");
        }
        return newBallInstance;
    }

    private Vector3 CalculateBallPosition(Vector3 lineStart, Vector3 lineEnd, float lineEndToUser, float targetToUser, string positionRelation)
    {
        // Log 輸出
        Log($"CalculateBallPosition: lineStart = {lineStart}, lineEnd = {lineEnd}, lineEndToUser = {lineEndToUser}, targetToUser = {targetToUser}, positionRelation = {positionRelation}");

        // 獲取線段的中間點
        int middleIndex = lineRenderer.positionCount / 2;
        Vector3 middlePoint = lineRenderer.GetPosition(middleIndex);

        Vector3 ballPositionOnLine = Vector3.zero;

        if (positionRelation == "Front")
        {
            if (lineEndToUser < targetToUser)
            {
                ballPositionOnLine = lineEnd;
                Log($"CalculateBallPosition: Front case, ball placed at lineEnd = {lineEnd}");
            }
            else
            {
                ballPositionOnLine = middlePoint;
            }
        }
        else if (positionRelation == "Back")
        {
            //if (lineEndToUser < targetToUser)
            //{
            //    ballPositionOnLine = lineStart;
            //    Log($"CalculateBallPosition: Back case, ball placed at lineStart = {lineStart}");
            //}
            //else
            //{
                ballPositionOnLine = middlePoint;
            //}
        }

        Log($"CalculateBallPosition: Final ball position = {ballPositionOnLine}");

        return ballPositionOnLine;
    }


    //private void CreateLineRenderer(GameObject newBallInstance, Vector3 uiWorldPosition, GameObject uiPanel)
    //{
    //    // 創建新的 LineRenderer
    //    LineRenderer newLineRenderer = new GameObject("UILineRenderer").AddComponent<LineRenderer>();
    //    newLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    //    newLineRenderer.useWorldSpace = true;

    //    newLineRenderer.sortingLayerID = SortingLayer.NameToID("UI");
    //    newLineRenderer.sortingOrder = -1;

    //    newLineRenderer.positionCount = 2;
    //    newLineRenderer.startColor = Color.white;
    //    newLineRenderer.endColor = Color.white;
    //    newLineRenderer.widthMultiplier = 0.05f;

    //    newLineRenderer.SetPosition(0, newBallInstance.transform.position);
    //    newLineRenderer.SetPosition(1, uiWorldPosition);

    //    // 更新字典
    //    uiLineRenderers[uiPanel] = newLineRenderer;
    //}

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
