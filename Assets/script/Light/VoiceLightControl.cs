using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceLightControl : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    public Light sceneLight; // 指定場景中的環境燈光
    private Coroutine lightCoroutine;
   

    void Start()
    {
        // 確保燈光初始為關閉
        if (sceneLight != null)
        {
            sceneLight.enabled = false;
        }

        // 加入語音指令
        actions.Add("o", TurnOnLight);
        

        // 初始化語音辨識
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log("辨識到指令: " + speech.text);
        actions[speech.text].Invoke();
    }

    private void TurnOnLight()
    {
        if (sceneLight != null)
        {
            // 如果之前有啟動過協程，先停止它，避免重疊
            if (lightCoroutine != null)
            {
                StopCoroutine(lightCoroutine);
            }
            // 啟動新的開燈協程
            lightCoroutine = StartCoroutine(TemporarilyTurnOnLight());
        }
    }

    
    private IEnumerator TemporarilyTurnOnLight()
    {
        sceneLight.enabled = true;
        yield return new WaitForSeconds(3f);
        sceneLight.enabled = false;
        lightCoroutine = null;
    }
}