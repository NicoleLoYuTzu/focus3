using UnityEngine;
using System.IO;

public static class CSVLogger
{
    // 改為靜態方法，但會傳遞 preview 參數
    public static void LogGameData(float time, float distance)
    {
        // 使用 Unity 提供的安全資料夾路徑，不需額外權限
        string filePath = Path.Combine(Application.persistentDataPath, "game_data.csv");

        // 如果檔案不存在，先寫入標題列
        if (!File.Exists(filePath))
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Time (seconds), Total Distance (meters), Date and Time, Preview Status"); // 加入 Preview Status 欄位
            }
        }

        string appName = Application.productName;

        // 寫入資料行
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            string currentTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            writer.WriteLine($"{time}, {distance}, {currentTime}, {appName}");
        }

        // 顯示實際儲存位置
        Debug.Log($"Data saved to: {filePath}");
    }
}
