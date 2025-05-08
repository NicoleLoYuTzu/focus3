using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class TeleportDistanceTrackerScene2 : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;
    private Vector3 lastPosition;
    private float totalDistance = 0f;

    void Start()
    {
        rayInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>(); // 這裡是控制器上面的 XRRayInteractor

        // 訂閱觸發事件
        rayInteractor.selectExited.AddListener(OnTeleportEnd);
        lastPosition = transform.position;

        // 加載之前保存的 totalDistance
        totalDistance = PlayerPrefs.GetFloat("totalDistance", 0f);  // 如果沒找到，預設為 0
    }

    private void OnTeleportEnd(SelectExitEventArgs args)
    {

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
