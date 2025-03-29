using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.SceneManagement;

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
            DontDestroyOnLoad(gameObject);  // 保證場景切換時不銷毀

            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 查詢當前場景的名稱
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("currentSceneNamecurrentSceneName : " + currentSceneName);
        string languageCode = PlayerPrefs.GetString(selectedLanguageKey, "zh-Hans");
        SetLanguage(languageCode);
    }

    private void OnEnable()
    {
        // 訂閱場景變更事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 取消訂閱，避免記憶體洩漏
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 切換場景時，檢查場景名稱來決定是否隱藏物件
        if (scene.name == "IndoorScene")
        {
            gameObject.SetActive(false); // 讓物件變為不可見
        }
        else
        {
            gameObject.SetActive(true); // 其他場景時確保物件可見
        }
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
