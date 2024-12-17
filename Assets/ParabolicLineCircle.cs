using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine;
using UnityEngine.XR;

public class ParabolicLineCircle : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual lineVisual; // Reference to XRInteractorLineVisual
    public GameObject circleObject;           // Circle 3D object to instantiate
    private LineRenderer lineRenderer;        // LineRenderer component

    private LineRenderer uiLineRenderer;  // 用於連接 UI 和圓形的直線

    public XRRayInteractor rayInteractor; // 连接到 XR Ray Interactor
    private GameObject ballInstance;          // Ball instance to hold the created object

    void Start()
    {
        // Check if lineVisual and circleObject are assigned
        if (lineVisual != null && circleObject != null)
        {
            circleObject.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);

            // Retrieve the LineRenderer component from the XRInteractorLineVisual
            lineRenderer = lineVisual.GetComponent<LineRenderer>();
          
            // 初始化 uiLineRenderer
            uiLineRenderer = new GameObject("UILineRenderer").AddComponent<LineRenderer>();
            uiLineRenderer.transform.SetParent(this.transform); // 把它設為當前物件的子物件（可選）
            uiLineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 使用適合的材質

            uiLineRenderer.useWorldSpace = true;

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
        Log($"ParabolicLineCircle LineRenderer enabled??? {lineRenderer.enabled} .");

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
        // 檢查並隱藏 uiLineRenderer
        if (uiLineRenderer != null)
        {
            if (uiLineRenderer.gameObject.activeSelf)
            {
                Log("Hiding uiLineRenderer...");
                uiLineRenderer.gameObject.SetActive(false);
            }
        }
    }


    public void ShowUIAndBall()
    {
        // 隱藏 ballInstance
        if (ballInstance != null)
        {
            ballInstance.SetActive(true);
        }

        // 隱藏 uiLineRenderer
        if (uiLineRenderer != null)
        {
            uiLineRenderer.gameObject.SetActive(true);
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




    public void ConnectObjectToUI(GameObject uiElement)
    {
        Log("ParabolicLineCircle: ConnectObjectToUI started.");

        if (ballInstance == null)
        {
            Log("ParabolicLineCircle: ballInstance is null!");
            return;
        }

        if (uiElement == null)
        {
           Log("ParabolicLineCircle: uiElement is null!");
            return;
        }

        RectTransform rectTransform = uiElement.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Log($"ParabolicLineCircle: The provided uiElement '{uiElement.name}' does not have a RectTransform component.");
            return;
        }

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Log("ParabolicLineCircle: No main camera found in the scene!");
            return;
        }

       

        Vector3 objectPosition = ballInstance.transform.position;

        // 將 RectTransform 轉換為世界座標
        Vector3 uiWorldPosition;
        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, rectTransform.position);



        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rectTransform,
            screenPoint,
            mainCamera,
            out uiWorldPosition))
            {
                Log($"ParabolicLineCircle: UI world position calculated: {uiWorldPosition},objectPosition {objectPosition}");
            // 更新 LineRenderer 的位置
        
            uiLineRenderer.positionCount = 2; // 設置 LineRenderer 的點數為 2，表示從起點到終點
          
            uiLineRenderer.startColor = Color.white; // 設置起點顏色
            uiLineRenderer.endColor = Color.white;   // 設置終點顏色
            uiLineRenderer.widthMultiplier = 0.05f;  // 設置線的寬度

            uiLineRenderer.SetPosition(0, objectPosition);  // 設置起點
            uiLineRenderer.SetPosition(1, uiWorldPosition); // 設置終點


        }
        else{
           Log("ParabolicLineCircle: Failed to convert RectTransform position to world position.");
        }
        
        
    }

    private void Log(string message) { Debug.Log($"ParabolicLineCircle: {message}"); }
    private void LogWarning(string message) { Debug.LogWarning($"ParabolicLineCircle: {message}"); }
}
