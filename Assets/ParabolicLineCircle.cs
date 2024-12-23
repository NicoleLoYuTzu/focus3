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
    private Dictionary<GameObject, LineRenderer> uiLineRenderers = new Dictionary<GameObject, LineRenderer>(); // 存儲UI元素對應的LineRenderer

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

        if (IsControllerMoving() && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (pointCount > 0)
            {
                Vector3[] linePoints = new Vector3[pointCount];
                lineRenderer.GetPositions(linePoints);
                Vector3 middlePoint = linePoints[pointCount / 2];

                // 只在 ballInstance 為 null 時創建一個新的球體
                if (ballInstance == null)
                {
                    ballInstance = Instantiate(circleObject, middlePoint, Quaternion.identity);
                    Log("ballInstance created.");
                }
                else
                {
                    // 如果 ballInstance 已經存在，則只更新它的位置
                    ballInstance.transform.position = middlePoint;
                    Log("ballInstance position updated.");
                }
            }
        }
        else
        {
            // 如果沒有命中，並且 ballInstance 存在，則清除 ballInstance
            if (ballInstance != null)
            {
                Destroy(ballInstance);  // 使用 Destroy 而不是設為 null
                ballInstance = null;
                Log("ballInstance destroyed because no hit detected.");
            }
        }
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


   

    public void ConnectObjectToUI(List<GameObject> uiElements)
    {
        Log("ParabolicLineCircle: ConnectObjectToUI started.");

        if (ballInstance == null)
        {
            Log("ParabolicLineCircle: ballInstance is null!");
            return;
        }

        if (uiElements == null || uiElements.Count == 0)
        {
            Log("ParabolicLineCircle: uiElements list is null or empty!");
            return;
        }

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Log("ParabolicLineCircle: No main camera found in the scene!");
            return;
        }

        Vector3 objectPosition = ballInstance.transform.position;

        // 遍歷每個 UI 元素
        foreach (GameObject uiElement in uiElements)
        {

            Log($"ParabolicLineCircle GameObject: {uiElement}");
            if (uiElement == null)
            {
                Log("ParabolicLineCircle: uiElement is null!");
                continue;
            }

            RectTransform rectTransform = uiElement.GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Log($"ParabolicLineCircle: The provided uiElement '{uiElement.name}' does not have a RectTransform component.");
                continue;
            }

            // 檢查是否已有該 UI 元素的 LineRenderer
            if (!uiLineRenderers.ContainsKey(uiElement))
            {
                // 將 RectTransform 轉換為世界座標
                Vector3 uiWorldPosition;
                Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, rectTransform.position);

                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    rectTransform,
                    screenPoint,
                    mainCamera,
                    out uiWorldPosition))
                {
                    Log($"ParabolicLineCircle: UI world position calculated: {uiWorldPosition}, objectPosition {objectPosition}");

                    // 為該 UI 元素創建並設置新的 LineRenderer
                    LineRenderer newLineRenderer = new GameObject("UILineRenderer").AddComponent<LineRenderer>();
                    newLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    newLineRenderer.useWorldSpace = true;

                    // 設定 LineRenderer 層級，確保它不會擋住 UI 元素
                    newLineRenderer.sortingLayerID = SortingLayer.NameToID("UI"); // 設置為 UI 層
                    newLineRenderer.sortingOrder = -1;  // 確保 LineRenderer 在 UI 之後繪製

                    // 設置 LineRenderer 的屬性
                    newLineRenderer.positionCount = 2;
                    newLineRenderer.startColor = Color.white;
                    newLineRenderer.endColor = Color.white;
                    newLineRenderer.widthMultiplier = 0.05f;

                    // 更新 LineRenderer 的位置
                    newLineRenderer.SetPosition(0, objectPosition);
                    newLineRenderer.SetPosition(1, uiWorldPosition);

                    // 將新創建的 LineRenderer 添加到字典中
                    uiLineRenderers.Add(uiElement, newLineRenderer);
                }
                else
                {
                    Log("ParabolicLineCircle: Failed to convert RectTransform position to world position.");
                }
            }
            else
            {
                // 更新現有的 LineRenderer 位置
                LineRenderer existingLineRenderer = uiLineRenderers[uiElement];
                existingLineRenderer.SetPosition(0, objectPosition);
                existingLineRenderer.SetPosition(1, uiElement.transform.position);
            }

        }
    }



    private void Log(string message) { Debug.Log($"ParabolicLineCircle: {message}"); }
    private void LogWarning(string message) { Debug.LogWarning($"ParabolicLineCircle: {message}"); }
}
