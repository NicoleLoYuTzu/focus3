using UnityEngine;
using TMPro; // 如果你用 TextMeshPro，請加這行

public class PlayerTextUpdate : MonoBehaviour
{
    public TMP_Text infoText; // 使用 TextMeshPro，或改成 `public Text infoText;` 來使用舊版 UI
    public GameObject glove;
    public GameObject lightBeam;
    public GameObject lightBeamAnchor;
    private void OnTriggerEnter(Collider other)
    {
        // 使用 `other.gameObject.CompareTag` 檢查碰撞物件的 Tag
        Log($"PlayerTextUpdate other {other}");
        if (other.gameObject.CompareTag("MainCamera"))
        {

            //如果手套有拿到
            if (glove.active == true)
            {
                lightBeam.active = true;
                lightBeamAnchor.active = true;
                infoText.text = "你拿到手套！通往另一世界的大門已開啟!快去探索吧!";
            }
            
            Log("PlayerTextUpdate OnTriggerEnter");
        }
    }

    private void Log(string message)
    {
        Debug.Log($"PlayerTextUpdate: {message}");
    }
}
