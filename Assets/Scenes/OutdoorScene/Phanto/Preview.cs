using UnityEngine;

public class Preview : MonoBehaviour
{
    public GameObject uiElement; // 要顯示或隱藏的 UI 元素

    // 顯示 UI
    public void Show()
    {
        uiElement.SetActive(true); // 將 UI 設置為可見
        Debug.Log("UI 顯示了！"); // 日誌輸出
    }

    // 隱藏 UI
    public void Hide()
    {
        uiElement.SetActive(false); // 將 UI 設置為隱藏
        Debug.Log("UI 隱藏了！"); // 日誌輸出
    }
}


//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.XR.Interaction.Toolkit;

//[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor))]
//public class Preview : MonoBehaviour
//{
//    [SerializeField]
//    public GameObject objectB; // 物體B
//    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor = null;

//    void Start()
//    {
//        rayInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
//        rayInteractor.hoverEntered.AddListener(Test);
//    }

//    private void Test(HoverEnterEventArgs arg0)
//    {
//        Debug.Log("Hover Enter Triggered");
//    }

//    //void Update()
//    //{
//    //    if (rayInteractor.IsOverUIGameObject())
//    //    {
//    //        Debug.Log(rayInteractor.interactablesHovered.Count);
//    //        Debug.Log(rayInteractor.GetOldestInteractableHovered());
//    //    }
//    //}
//    private void OnHoverEnter(UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor)
//    {
//        Debug.Log("光束進入物體A！"); // 日誌輸出
//        ShowObjectB(interactor);
//    }

//    private void OnHoverExit(UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor)
//    {
//        Debug.Log("光束離開物體A！"); // 日誌輸出
//        HideObjectB(interactor);
//    }

//    private void ShowObjectB(UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor)
//    {
//        objectB.SetActive(true); // 顯示物體B
//        Debug.Log("物體B顯示了！"); // 日誌輸出
//    }

//    private void HideObjectB(UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor)
//    {
//        objectB.SetActive(false); // 隱藏物體B
//        Debug.Log("物體B隱藏了！"); // 日誌輸出
//    }
//}