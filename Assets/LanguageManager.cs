using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    // 語言設定的 key，假設會從 PlayerPrefs 讀取
    private string selectedLanguageKey = "selectedLanguage";

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

    // 設置語言的方法，根據 PlayerPrefs 或其他方法進行設置
    private void Start()
    {
        string languageCode = PlayerPrefs.GetString(selectedLanguageKey, "zh-Hans");  // 默認語言為 繁體中文
        SetLanguage(languageCode);
    }

    // 根據語言代碼設置語言
    public void SetLanguage(string languageCode)
    {
        Locale newLocale = null;
        string tableName = "UI";  // 設定你的表格名稱

        switch (languageCode)
        {
            case "en":  // 英文
                newLocale = LocalizationSettings.AvailableLocales.GetLocale("en");
                break;
            case "zh-Hans":  // 簡體中文
                newLocale = LocalizationSettings.AvailableLocales.GetLocale("zh-Hans");
                break;
            default:
                newLocale = LocalizationSettings.AvailableLocales.GetLocale("zh-Hans");  // 默認為簡體中文
                break;
        }

        if (newLocale != null)
        {
            // 設定語言
            LocalizationSettings.SelectedLocale = newLocale;

            // 嘗試加載指定的語言表格（UI）
            var table = LocalizationSettings.StringDatabase.GetTable(tableName);

            if (table != null)
            {
                Debug.Log($"{tableName} table loaded successfully.");

                // 顯示表格內容
                ShowTableContents(table);
            }
            else
            {
                Debug.LogWarning($"{tableName} table could not be found.");
            }
        }
        else
        {
            Debug.LogWarning("Selected locale is not available!");
        }
    }

    // 顯示表格內容（這裡應該傳入 StringTable）
    private void ShowTableContents(StringTable table)
    {
        // 使用 table.GetEntries() 方法來獲取表格條目
        foreach (var entry in table)
        {
            Debug.Log($"ShowTableContentsShowTableContents Key: {entry.Key}, Value: {entry.Value.GetLocalizedString()}");
        }
    }
}
