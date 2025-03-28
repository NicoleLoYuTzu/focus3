using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    private string selectedLanguageKey = "selectedLanguage";
    private string tableName = "UI";  // 默認表格名稱 "UI"

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        string languageCode = PlayerPrefs.GetString(selectedLanguageKey, "zh-Hans");
        SetLanguage(languageCode);
    }

    // 根據語言設置自動獲取表格
    public void SetLanguage(string languageCode)
    {
        Debug.Log("SelectedSetLanguageSetLanguage locale: " + LocalizationSettings.SelectedLocale?.Identifier);



        Locale newLocale = null;

        // 自動設定語言
        switch (languageCode)
        {
            case "en":
                newLocale = LocalizationSettings.AvailableLocales.GetLocale("en");
                break;
            case "zh-Hans":
                newLocale = LocalizationSettings.AvailableLocales.GetLocale("zh-Hans");
                break;
            default:
                newLocale = LocalizationSettings.AvailableLocales.GetLocale("zh-Hans");
                break;
        }

        if (newLocale != null)
        {
            LocalizationSettings.SelectedLocale = newLocale;

            // ✅ 正確方式：直接指定 TableReference
            TableReference tableReference = tableName;  // ✅ 正確用法

            var table = LocalizationSettings.StringDatabase.GetTable(tableReference);

            if (table != null)
            {
                Debug.Log("SelectedSetLanguageSetLanguage Table loaded successfully.");
            }
            else
            {
                Debug.LogWarning("SelectedSetLanguageSetLanguage Table not found.");
            }
        }
        else
        {
            Debug.LogWarning("SelectedSetLanguageSetLanguage Selected locale is not available!");
        }
    }

    // 範例方法：簡化獲取表格的過程
    public string GetLocalizedString(string key)
    {
        var table = LocalizationSettings.StringDatabase.GetTable(tableName);
        if (table != null)
        {
            var entry = table.GetEntry(key);
            if (entry != null)
            {
                return entry.LocalizedValue;
            }
        }

        if (table != null)
        {
            Debug.Log("SelectedSetLanguageSetLanguage GetLocalizedString Table loaded successfully.");
        }
        else
        {
            Debug.LogWarning("SelectedSetLanguageSetLanguage GetLocalizedString Table not found.");
        }


        return $"Key {key} not found!";
    }
}
