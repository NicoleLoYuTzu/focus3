using UnityEngine;
using TMPro; // 如果你用 TextMeshPro，請加這行

public class PlayerTextUpdate : MonoBehaviour
{
    public TMP_Text infoText; // 使用 TextMeshPro，或改成 `public Text infoText;` 來使用舊版 UI

    private void OnTriggerEnter(Collider other)
    {
        // 使用 `other.gameObject.CompareTag` 檢查碰撞物件的 Tag
        Log($"PlayerTextUpdate other {other}");
        if (other.gameObject.CompareTag("MainCamera"))
        {
            infoText.text = "你站在地面上！";
            Log("PlayerTextUpdate OnTriggerEnter");
        }
    }

    private void Log(string message)
    {
        Debug.Log($"PlayerTextUpdate: {message}");
    }
}
