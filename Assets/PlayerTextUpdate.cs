using UnityEngine;
using TMPro; // 如果你用 TextMeshPro，請加這行

public class PlayerTextUpdate : MonoBehaviour
{
    //public TMP_Text infoText; // 使用 TextMeshPro，或改成 `public Text infoText;` 來使用舊版 UI
    public GameObject hintCanvas;
    public TriggerAnchorToShowDetail detailManager;

    void Start(){
        hintCanvas.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        // 使用 `other.gameObject.CompareTag` 檢查碰撞物件的 Tag
        Log($"PlayerTextUpdate other {other}");
        if (other.gameObject.CompareTag("MainCamera"))
        {

            ////如果手套有拿到
            //if (glove.active == true)
            //{
            //    lightBeam.active = true;
            //    lightBeamAnchor.active = true;
            //    infoText.text = "你拿到手套！通往另一世界的大門已開啟!快去探索吧!";
            //}
            hintCanvas.SetActive(true);
            detailManager.SetCurrentTargetInRange(gameObject);


            Log("PlayerTextUpdate OnTriggerEnter");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 當 MainCamera 離開時，隱藏提示 UI
        if (other.gameObject.CompareTag("MainCamera"))
        {
            Debug.Log("MainCamera 離開觸發區，隱藏提示 UI");

            hintCanvas.SetActive(false);
            detailManager.ClearCurrentTarget();

        }
    }



    private void Log(string message)
    {
        Debug.Log($"PlayerTextUpdate: {message}");
    }
}
