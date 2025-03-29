using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class CaterpillarGameManager : MonoBehaviour
{
    public TMP_Text infoText;
    public GameObject glove;
    public GameObject lightBeam;
    public GameObject lightBeamAnchor;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable hintButton; // 提示1按鈕
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable passButton; // 提示2按鈕

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor hoveringInteractor;

    public AudioSource audioSource;  // 🔹 音效播放元件
    public AudioClip Clicked;     // 🔹 按下 hintButton2 時播放的音效
    public AudioClip Success;     // 🔹 按下 hintButton1 時播放的音效
    public AudioClip Failed;     // 🔹 按下 hintButton2 時播放的音效

    private bool isSoundPlayed = false; // 是否播放過音效的標誌


    private void Start()
    {
        lightBeam.SetActive(false);
        lightBeamAnchor.SetActive(false);

        // 監聽 Hover 進出事件
        hintButton.hoverEntered.AddListener(OnHoverEnter);
        hintButton.hoverExited.AddListener(OnHoverExit);

        passButton.hoverEntered.AddListener(OnHoverEnter);
        passButton.hoverExited.AddListener(OnHoverExit);
    }

    private void Update()
    {
        if (hoveringInteractor != null)
        {
            // 取得左右手控制器
            InputDevice leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

            bool isPressedLeft = false, isPressedRight = false;

            // 偵測 Trigger 按鈕
            InputHelpers.IsPressed(leftHandDevice, InputHelpers.Button.Trigger, out isPressedLeft);
            InputHelpers.IsPressed(rightHandDevice, InputHelpers.Button.Trigger, out isPressedRight);

            if (isPressedLeft || isPressedRight)
            {
                Debug.Log("CaterpillarGameManager Trigger button pressed!");  // 🔹 記錄按鈕是否被按到

                // 判斷 Hover 的物件來顯示不同的提示
                if (hoveringInteractor.interactablesHovered.Contains(hintButton))
                {
                    Debug.Log("CaterpillarGameManager Hint button pressed!");  // 🔹 檢查是否偵測到 hintButton

                    infoText.text = LanguageManager.Instance.GetLocalizedString("CaterPillarGreetingWords");
                    PlaySound(Clicked);
                    isSoundPlayed = true; // 標記音效已經播放
                    //infoText.text = "「哦，是你啊，來這裡做什麼？你應該知道我不會隨便幫忙的吧。除非……你找到那隻笨兔子的手套，那可是打開秘密大門的關鍵。我可沒耐心等太久，去吧，等你拿到手套再來找我。」";
                }
                else if (hoveringInteractor.interactablesHovered.Contains(passButton))
                {
                    Debug.Log("CaterpillarGameManager Pass button pressed!");  // 🔹 檢查是否偵測到 passButton

                    if (glove.activeSelf == true)
                    {
                        Debug.Log("CaterpillarGameManager Glove is active! Unlocking door...");  // 🔹 確保手套狀態正確
                        lightBeam.SetActive(true);
                        lightBeamAnchor.SetActive(true);
                        glove.SetActive(false);
                        infoText.text = LanguageManager.Instance.GetLocalizedString("CaterPillarDoorOpened");

                        PlaySound(Success);
                        isSoundPlayed = true; // 標記音效已經播放
                        //infoText.text = "你拿到手套！通往另一世界的大門已開啟!快去探索吧!";
                    }
                    else
                    {
                        Debug.Log("CaterpillarGameManager Glove is NOT active!");  // 🔹 提示手套沒有被取得
                                                                                   //infoText.text = "你沒有給我手套, 我是不可能開啟另一世界的大門的!~~";
                        PlaySound(Failed);
                        isSoundPlayed = true; // 標記音效已經播放
                        infoText.text = LanguageManager.Instance.GetLocalizedString("CaterPillarDoorCannotOpen");

                    }
                }
            }
        }
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        hoveringInteractor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
        Debug.Log("CaterpillarGameManager Hover enter: " + args.interactableObject);  // 🔹 記錄 Hover 進入的物件
        isSoundPlayed = false; // 重新進入 hover 區域時重置音效播放標誌
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        Debug.Log("CaterpillarGameManager Hover exit: " + args.interactableObject);  // 🔹 記錄 Hover 離開的物件
        hoveringInteractor = null;
        isSoundPlayed = false; // 重新進入 hover 區域時重置音效播放標誌
    }


    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // 🔹 播放指定音效
        }
    }
}
