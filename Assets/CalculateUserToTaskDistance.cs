using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalculateUserToTaskDistance : MonoBehaviour
{
    void Update()
    {
        // 可根據需求調用 CalculateUserPositionToObject，傳入目標物件
    }

    public void CalculateUserPositionToObject(GameObject targetObject)
    {
        Vector3 objectPosition = targetObject.transform.position;

        // 獲取使用者的位置
        Vector3 userPosition = GetUserPosition();

        // 計算使用者到目標的距離
        float distance = Vector3.Distance(userPosition, objectPosition);

        // 判斷目標在使用者的左方、右方、前方或後方
        string positionRelation = GetPositionRelation(userPosition, objectPosition);

        // 查找場景中的 TMP Text 物件，名稱為 "targetObject+_distance"
        string textObjectName = $"{targetObject.name}_distance";

        GameObject textObject = GameObject.Find(textObjectName);
        if (textObject != null)
        {
            // 嘗試獲取 TextMeshPro (UI) 組件
            TextMeshProUGUI textUIComponent = textObject.GetComponent<TextMeshProUGUI>();
            if (textUIComponent != null)
            {
                // 更新文字內容為距離和位置關係
                textUIComponent.text = $"{distance:F2}m, {positionRelation}";
            }
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
        Vector3 userRight = mainCamera.transform.right;

        // 從使用者到目標的方向向量
        Vector3 targetDirection = objectPosition - userPosition;

        // 判斷前後
        float dotForward = Vector3.Dot(userForward, targetDirection.normalized);
        bool isInFront = dotForward > 0;

        // 判斷左右
        float dotRight = Vector3.Dot(userRight, targetDirection.normalized);
        string leftOrRight = dotRight > 0 ? "Right" : "Left";

        // 綜合判斷
        if (isInFront)
        {
            return $"In Front ({leftOrRight})";
        }
        else
        {
            return $"Behind ({leftOrRight})";
        }
    }

    private Vector3 GetUserPosition()
    {
        // 假設使用 XR Rig，抓取主攝影機的位置（通常表示使用者位置）
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            return mainCamera.transform.position;
        }
        else
        {
            Debug.LogWarning("Main Camera not found. Returning Vector3.zero as fallback.");
            return Vector3.zero; // 回傳預設值
        }
    }
}
