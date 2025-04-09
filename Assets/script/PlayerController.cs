/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public PitchVisualizer pitchVisualizer; // 連結 UI 顯示腳本
    AudioClip micInput;
    int sampleRate = 44100; // 標準音頻取樣率

    void Start()
    {
        micInput = Microphone.Start(null, true, 1, sampleRate); // 開啟麥克風
        StartCoroutine(ProcessAudio()); // 開始偵測音調
    }
    IEnumerator ProcessAudio()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // 每 0.1 秒更新一次
            float pitch = GetPitch();
            Debug.Log("偵測到的音調頻率: " + pitch);
            pitchVisualizer.UpdatePitch(pitch); // 傳送音調數據給 UI
        }
    }

    float GetPitch()
    {
        if (micInput == null) return 0f;

        float[] samples = new float[1024];
        micInput.GetData(samples, 0); // 取得麥克風的音訊資料

        float frequency = FFTAnalysis(samples); // 執行 FFT 分析
        return frequency;
    }


    float FFTAnalysis(float[] spectrum)
    {
        int index = 0;
        float maxMagnitude = 0f;

        for (int i = 0; i < spectrum.Length; i++)
        {
            if (spectrum[i] > maxMagnitude)
            {
                maxMagnitude = spectrum[i];
                index = i;
            }
        }

        float freq = index * 44100 / spectrum.Length; // 計算頻率
        return freq;
    }


}*/



/*
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public PitchVisualizer pitchVisualizer; // UI曲線顯示器
    public float volumeThreshold = 0.02f;   // 收音門檻（音量超過才偵測）

    private AudioClip micInput;
    private int sampleRate = 44100;

    void Start()
    {
        micInput = Microphone.Start(null, true, 1, sampleRate);
        StartCoroutine(ProcessAudio());
    }

    IEnumerator ProcessAudio()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            float volume = GetVolume();
            if (volume > volumeThreshold)
            {
                float pitch = GetPitch();
                pitchVisualizer.UpdatePitch(pitch);
                Debug.Log($"音量: {volume}，音調: {pitch}");
            }
            else
            {
                Debug.Log($"音量太低：{volume}（忽略）");
            }
        }
    }

    float GetVolume()
    {
        if (micInput == null) return 0f;

        float[] samples = new float[128];
        micInput.GetData(samples, 0);

        float sum = 0f;
        foreach (float sample in samples)
        {
            sum += sample * sample;
        }
        return Mathf.Sqrt(sum / samples.Length); // RMS 音量
    }

    float GetPitch()
    {
        if (micInput == null) return 0f;

        float[] samples = new float[1024];
        micInput.GetData(samples, 0);

        return FFTAnalysis(samples);
    }

    float FFTAnalysis(float[] samples)
    {
        int index = 0;
        float maxMagnitude = 0f;

        for (int i = 0; i < samples.Length; i++)
        {
            float magnitude = Mathf.Abs(samples[i]);
            if (magnitude > maxMagnitude)
            {
                maxMagnitude = magnitude;
                index = i;
            }
        }

        return index * sampleRate / samples.Length;
    }
}*/



using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public PitchVisualizer pitchVisualizer; // 顯示聲音曲線的腳本（LineRenderer）
    public float volumeThreshold = 0.02f; // 收音門檻（音量超過才偵測）

    private AudioClip micInput; // 麥克風輸入
    private int sampleRate = 44100; // 標準音頻取樣率

    void Start()
    {
        // 開始錄音（null表示預設麥克風、循環錄音、錄1秒、採樣率44100）
        micInput = Microphone.Start(null, true, 1, sampleRate);

        // 啟動協程，每隔一段時間抓取音訊並做處理
        StartCoroutine(ProcessAudio());
    }



    // 每 0.1 秒分析一次聲音
    IEnumerator ProcessAudio()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // 每0.1秒更新一次

            float volume = GetVolume(); // 取得當前音量
            float pitch = (volume > volumeThreshold) ? GetPitch() : 0f; // 音量夠大才分析音調
            pitchVisualizer.UpdatePitch(pitch); // 傳給顯示器畫出來

            Debug.Log($"音量: {volume}, 音調: {pitch}");
        }
    }



    // 計算音量（使用 RMS：實際能量感的平均值）
    float GetVolume()
    {
        if (micInput == null) return 0f;

        float[] samples = new float[128]; // 短時間內的音訊資料
        micInput.GetData(samples, 0); // 從音訊中取得資料

        float sum = 0f;
        foreach (float sample in samples)
        {
            sum += sample * sample; // 音訊的能量
        }
        return Mathf.Sqrt(sum / samples.Length); // 返回 RMS 音量（越大聲值越高）
    }



    // 呼叫 FFT 分析找出主要音調(最大聲)頻率（單位：Hz）
    float GetPitch()
    {
        if (micInput == null) return 0f;

        float[] samples = new float[1024]; // 比音量更多的音訊資料
        micInput.GetData(samples, 0);

        return FFTAnalysis(samples); // 做快速傅立葉轉換分析頻率
    }



    // FFT 分析：找出最大聲音對應的頻率（簡易方式）
    float FFTAnalysis(float[] samples)
    {
        int index = 0;
        float maxMagnitude = 0f;

        // 找出最大聲音所在的位置
        for (int i = 0; i < samples.Length; i++)
        {
            float magnitude = Mathf.Abs(samples[i]);
            if (magnitude > maxMagnitude)
            {
                maxMagnitude = magnitude;
                index = i;
            }
        }

        // 轉換為實際頻率（赫茲）
        return index * sampleRate / samples.Length;
    }
}

