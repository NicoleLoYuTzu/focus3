using UnityEngine;

public class PreviewInRange : MonoBehaviour
{
    public GameObject previewPrefab; // 預覽畫面Prefab
    public float radius = 5f; // 設定圓形半徑
    public Camera mainCamera; // 需要在Inspector中設置的相機引用
    public GameObject teleportInteractor; // 右手控制器
    private GameObject currentPreview;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;

    private void Start()
    {
        if (mainCamera == null)
        {
            Debug.LogError("[PREVIEW] Main camera not set!");
            return;
        }

        rayInteractor = teleportInteractor.GetComponentInChildren<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
        if (rayInteractor == null)
        {
            Debug.LogError("[PREVIEW] XRRayInteractor not found in camera's children!");
            return;
        }

        // 預先實例化預覽物件
        currentPreview = Instantiate(previewPrefab, mainCamera.transform.position, Quaternion.identity);
        currentPreview.SetActive(false); // 初始時隱藏預覽物件
        currentPreview.transform.SetParent(mainCamera.transform); // 設定預覽物件為相機的子物件
        Debug.Log("[PREVIEW] Preview object instantiated and set as child of the main camera.");
    }

    private void Update()
    {
        // 檢查射線的碰撞
        RaycastHit hit;
        if (rayInteractor.TryGetCurrent3DRaycastHit(out hit))
        {
            // 計算控制器到碰撞點的距離
            float distance = Vector3.Distance(hit.point, teleportInteractor.transform.position);
            Debug.Log($"[PREVIEW] Ray hit at {hit.point}, distance from interactor: {distance}");

            // 檢查是否在範圍內
            if (distance <= radius)
            {
                currentPreview.SetActive(true); // 顯示預覽物件
                currentPreview.transform.position = hit.point; // 更新預覽物件位置
                currentPreview.transform.rotation = Quaternion.LookRotation(hit.normal); // 調整預覽物件的朝向
                Debug.Log("[PREVIEW] Preview object is now visible.");
            }
            else
            {
                currentPreview.SetActive(false); // 隱藏預覽物件
                Debug.Log("[PREVIEW] Preview object is out of range and hidden.");
            }
        }
        else
        {
            currentPreview.SetActive(false); // 隱藏預覽物件
            Debug.Log("[PREVIEW] No ray hit detected; preview object is hidden.");
        }
    }
}
