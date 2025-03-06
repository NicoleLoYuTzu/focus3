//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using TMPro;

//public class countNumberToNextScene : MonoBehaviour
//{
//    public GameObject infoTextCanvas;
//    public TMP_Text infoText;
//    public string nextSceneName; // 設定下一個場景的名稱
//    private bool isCountingDown = false; // 防止多次觸發

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.gameObject.CompareTag("MainCamera") && !isCountingDown)
//        {
//            infoTextCanvas.active = true;
//            isCountingDown = true; // 防止重複觸發
//            StartCoroutine(CountdownAndChangeScene());
//        }
//    }

//    private IEnumerator CountdownAndChangeScene()
//    {
//        // 倒數 3, 2, 1, Go!
//        for (int i = 3; i >= 0; i--)
//        {
//            infoText.text = i > 0 ? i.ToString() : "Go!"; // 顯示倒數數字或 "Go!"
//            yield return new WaitForSeconds(1f); // 每 1 秒鐘顯示一次
//        }

//        // 等待最後一個 "Go!" 顯示完畢，之後再載入下一場景
//        yield return new WaitForSeconds(1f); // 給 "Go!" 1 秒鐘顯示時間

//        SceneManager.LoadScene(1); // 切換到下一個場景
//    }
//}

using System.Collections;
using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空間
using UnityEngine.SceneManagement; // 引入場景管理命名空間

public class countNumberToNextScene : MonoBehaviour
{
    public GameObject infoTextCanvas;
    public TMP_Text infoText; // 用於顯示倒數的 TMP_Text
    private void OnTriggerEnter(Collider other)
    {
       
        // 檢查碰撞的物件是否為 MainCamera
        Log($"countNumberToNextScene other {other}");
        if (other.gameObject.CompareTag("MainCamera"))
        {
            infoTextCanvas.active = true;
            // 開始倒數並跳轉到下一個場景
            StartCoroutine(CountdownAndLoadScene());
            Log("countNumberToNextScene OnTriggerEnter");
        }
    }

    // 倒數計時並加載下一個場景
    private IEnumerator CountdownAndLoadScene()
    {
        // 倒數 3、2、1
        for (int i = 3; i > 0; i--)
        {
            infoText.text = i.ToString();
            yield return new WaitForSeconds(1); // 等待 1 秒
        }

        // 跳轉到下一個場景
        infoText.text = "Go!";
        yield return new WaitForSeconds(1); // 等待 1 秒，顯示 "Go!" 文字
        SceneManager.LoadScene(1); // 替換為你要跳轉的場景名稱
    }

    private void Log(string message)
    {
        Debug.Log($"countNumberToNextScene: {message}");
    }
}
