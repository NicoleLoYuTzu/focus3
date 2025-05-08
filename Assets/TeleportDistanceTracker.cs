using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportDistanceTracker : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;
    private Vector3 lastPosition;
    public float totalDistance = 0f;
    public bool isTrackingEnabled = false;  // 控制是否啟用計算

    void Start()
    {
        rayInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>(); // 這裡是控制器上面的 XRRayInteractor

        // 訂閱觸發事件
        rayInteractor.selectExited.AddListener(OnTeleportEnd);
        lastPosition = transform.position;
    }

    private void OnTeleportEnd(SelectExitEventArgs args)
    {
        if (!isTrackingEnabled) return;  // 如果追蹤沒啟用，則不做任何處理

        // 確認 Teleport 完成後的邏輯
        float distance = Vector3.Distance(transform.position, lastPosition);
        totalDistance += distance;
        lastPosition = transform.position;

        PlayerPrefs.SetFloat("totalDistance", totalDistance); // 保存数据
        PlayerPrefs.Save();

        // 輸出結果
        Debug.Log($"OnTeleportEnd Teleport Distance: {distance} | Total Distance: {totalDistance}");
    }

    void Update()
    {
        // 你可以在這裡更新其他邏輯
    }
}
