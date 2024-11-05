using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSpawner : MonoBehaviour
{
    public GameObject buttonPrefab; // 预制的 Button 模板，包含两个 Image
    public Transform parentTransform; // 用来放置生成的 Button 的父物件（ImageContainer）
    private List<Sprite> imagesArray = new List<Sprite>(); // 使用 List 可以灵活添加或删除图片

    void Start()
    {
        LogInfo("Start called. Updating images based on condition.");
        UpdateImagesBasedOnCondition("phanto"); // 使用 phanto 作为条件
        GenerateImages();
      
    }

    void UpdateImagesBasedOnCondition(string condition)
    {
        imagesArray.Clear();
        LogInfo($"Updating images based on condition: {condition}");

        if (condition == "phanto")
        {
            // 加入兔子和毛毛虫的图片
            Sprite rabbitSprite = Resources.Load<Sprite>("Rabbit/RABBIT1");
            Sprite caterpillarSprite = Resources.Load<Sprite>("Caterpillar/CATERPILLER");

            if (rabbitSprite != null)
            {
                imagesArray.Add(rabbitSprite); // RABBIT1 图片
                LogInfo("Added rabbit sprite.");
            }
            else
            {
                LogWarning("Rabbit sprite not found!");
            }

            if (caterpillarSprite != null)
            {
                imagesArray.Add(caterpillarSprite); // 毛毛虫图片
                LogInfo("Added caterpillar sprite.");
            }
            else
            {
                LogWarning("Caterpillar sprite not found!");
            }
        }
        else if (condition == "anotherCondition")
        {
            imagesArray.Add(Resources.Load<Sprite>("Sprites/Image3"));
            imagesArray.Add(Resources.Load<Sprite>("Sprites/Image4"));
            imagesArray.Add(Resources.Load<Sprite>("Sprites/Image5"));
        }
    }

    void GenerateImages()
    {
        //// 清空现有子物件以防重复生成
        //foreach (Transform child in parentTransform)
        //{
        //    Destroy(child.gameObject);
        //}

        LogInfo($"Generating {imagesArray.Count} images.");

        for (int i = 0; i < imagesArray.Count; i += 2)
        {
            GameObject newButton = Instantiate(buttonPrefab, parentTransform);
            LogInfo($"Instantiated new button for image: {imagesArray[i].name}");

            PrintHierarchy(parentTransform);

            // 为兔子的 Button 设置星星
            if (imagesArray[i].name == "RABBIT1")
            {
                Transform starTransform = newButton.transform.Find("stars"); // 假设星星的名称为 "StarImage"
                if (starTransform != null)
                {
                    starTransform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Rabbit/stars"); // 加载星星图片
                    LogInfo("Set star image for rabbit.");
                }
                // 还可以设置兔子的图像
                Transform rabbitTransform = newButton.transform.Find("image"); // 假设兔子的名称为 "RabbitImage"
                if (rabbitTransform != null)
                {
                    rabbitTransform.GetComponent<Image>().sprite = imagesArray[i]; // 设置兔子的图像
                    LogInfo("Set rabbit image.");
                }
            }
            // 为毛毛虫的 Button 设置背景和图像
            else if (imagesArray[i].name == "CATERPILLER")
            {
                newButton.GetComponent<Image>().color = Color.gray; // 设置背景为灰色
                LogInfo("Set button background color to gray for caterpillar.");

                Transform starTransform = newButton.transform.Find("stars"); // 假设星星的名称为 "StarImage"
                if (starTransform != null)
                {
                    starTransform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Caterpillar/bull.jpg"); // 加载星星图片
                    LogInfo("Set star image for caterpillar.");
                }
                Transform caterpillarTransform = newButton.transform.Find("image"); // 假设毛毛虫的名称为 "CaterpillarImage"
                if (caterpillarTransform != null)
                {
                    caterpillarTransform.GetComponent<Image>().sprite = imagesArray[i]; // 设置毛毛虫的图像
                    LogInfo("Set caterpillar image.");
                }
            }
        }
        LogInfo("Image generation completed.");
    }

    private void LogInfo(string message)
    {
        Debug.Log($"ImageSpawner LogInfo: {message}");
    }

    private void LogWarning(string message)
    {
        Debug.LogWarning($"ImageSpawner LogWarning: {message}");
    }

    // 更新的 PrintHierarchy 方法，顯示名稱和層次
    void PrintHierarchy(Transform transform, int level = 0)
    {
        // 印出縮排以顯示層級
        string indent = new string(' ', level * 2);
        Debug.Log($"{indent}{transform.name}");

        // 遞迴列出子物件
        foreach (Transform child in transform)
        {
            PrintHierarchy(child, level + 1);
        }
    }
}
