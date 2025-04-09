using UnityEngine;

public class MicInput : MonoBehaviour
{
    [Header("吹氣判斷")]
    public float ambientLevelMultiplier = 2.0f; // 幾倍環境音才算吹氣
    public int sampleWindow = 128;              // 音量採樣範圍

    [Header("目前狀態（只讀）")]
    public float currentLoudness;
    public float ambientLevel;
    public bool IsBlowing { get; private set; }

    private AudioClip micRecord;
    private string micDevice;

    void Start()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogWarning("沒有找到麥克風設備！");
            return;
        }

        micDevice = Microphone.devices[0];
        micRecord = Microphone.Start(micDevice, true, 10, AudioSettings.outputSampleRate);
        ambientLevel = 0.01f; // 初始估計環境音
    }

    void Update()
    {
        currentLoudness = GetLoudness();

        // 平滑追蹤背景音（只在音量較低時更新）
        if (currentLoudness < ambientLevel)
        {
            ambientLevel = Mathf.Lerp(ambientLevel, currentLoudness, Time.deltaTime * 2f);
        }

        // 判定是否吹氣（音量 > 環境音 * 倍率）
        IsBlowing = currentLoudness > ambientLevel * ambientLevelMultiplier;
    }

    float GetLoudness()
    {
        if (micRecord == null || !Microphone.IsRecording(micDevice)) return 0f;

        float[] data = new float[sampleWindow];
        int micPos = Microphone.GetPosition(micDevice) - sampleWindow;
        if (micPos < 0) return 0f;

        micRecord.GetData(data, micPos);
        float levelMax = 0;
        for (int i = 0; i < sampleWindow; i++)
        {
            float wavePeak = Mathf.Abs(data[i]);
            if (wavePeak > levelMax)
                levelMax = wavePeak;
        }
        return levelMax;
    }
}
