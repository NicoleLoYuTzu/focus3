using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class AliceGameManager : MonoBehaviour
{
    public GameObject potion; // 放大藥水物件
    public TextMeshProUGUI infoTextUI; // 顯示訊息的 UI
    public GameObject animatedObject;


    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable hintButton; // 提示按鈕
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable endButton; // 結束按鈕

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor hoveringInteractor; // 紀錄 Hover 的控制器

    private void Start()
    {
        if (infoTextUI != null)
        {
            infoTextUI.text = "";
        }

        // 綁定 Hover 事件到提示按鈕
        if (hintButton != null)
        {
            hintButton.hoverEntered.AddListener(OnHintHoverEnter);
            hintButton.hoverExited.AddListener(OnHoverExit);
        }

        // 綁定 Hover 事件到結束按鈕
        if (endButton != null)
        {
            endButton.hoverEntered.AddListener(OnEndHoverEnter);
            endButton.hoverExited.AddListener(OnHoverExit);
        }
    }

    private void Update()
    {
        if (hoveringInteractor != null) // 確保目前有 Hover 的控制器
        {
            // 取得左右手裝置
            InputDevice leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

            bool isPressedLeft = false, isPressedRight = false;

            // 偵測 Trigger 按鈕
            InputHelpers.IsPressed(leftHandDevice, InputHelpers.Button.Trigger, out isPressedLeft);
            InputHelpers.IsPressed(rightHandDevice, InputHelpers.Button.Trigger, out isPressedRight);

            if (isPressedLeft || isPressedRight) // 任何一隻手的 Trigger 被按下
            {
                if (hoveringInteractor.hasSelection) return; // 避免重複觸發

                // 判斷 Hover 的物件來決定要執行哪個按鈕
                if (hoveringInteractor.interactablesHovered.Contains(endButton))
                {
                    EndGame();
                }
                else if (hoveringInteractor.interactablesHovered.Contains(hintButton))
                {
                    GiveHint();
                }
            }
        }
    }

    private void OnHintHoverEnter(HoverEnterEventArgs args)
    {
        hoveringInteractor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
        if (infoTextUI != null)
        {
            infoTextUI.text = "為什麼我變得那麼小... 幫我想想辦法";
        }
    }

    private void OnEndHoverEnter(HoverEnterEventArgs args)
    {
        hoveringInteractor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
        if (infoTextUI != null)
        {
            infoTextUI.text = "準備變大!";
        }
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        hoveringInteractor = null;
        if (infoTextUI != null)
        {
            infoTextUI.text = "";
        }
    }



    private void GiveHint()
    {
        if (infoTextUI != null)
        {
            infoTextUI.text = "也許有什麼能讓我變回來...";
        }
    }

    private void EndGame()
    {
        if (potion != null) // 確保藥水存在
        {
            if (infoTextUI != null)
            {
                infoTextUI.text = "放大藥水!! 謝謝你!! 我要喝下去了!";
            }

            StartCoroutine(ShowMessageThenGrow());
        }
        else
        {
            if (infoTextUI != null)
            {
                infoTextUI.text = "沒拿到藥水啊，你能不能幫我想想辦法?";
            }
        }
    }

    private IEnumerator ShowMessageThenGrow()
    {
        yield return new WaitForSeconds(2f); // 等待 2 秒讓玩家看到訊息

        PlayAnimation();
    }


    private void PlayAnimation()
    {
        if (animatedObject != null)
        {
            Animation animation = animatedObject.GetComponent<Animation>();
            if (animation != null)
            {
                Debug.Log("GameManager: Animation component found on target object.");
                animation.enabled = true; // 啟用動畫元件
                Debug.Log("GameManager: Animation component enabled.");
                animation.Play(); // 播放動畫
                Debug.Log("GameManager: Playing animation.");
            }
            else
            {
                Debug.LogError("GameManager: No Animation component found on animatedObject.");
            }
        }
        else
        {
            Debug.LogError("GameManager: animatedObject is not assigned.");
        }
    }


}

