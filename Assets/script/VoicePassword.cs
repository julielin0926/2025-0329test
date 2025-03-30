using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Voicekeyword : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> passwords = new Dictionary<string, Action>();
    public string correctPassword = "Hello World"; // 設定語音密碼


    void Start()
    {
        // 將密碼關鍵字加入 Dictionary，辨識到時執行 Unlock()
        passwords.Add(correctPassword, Unlock);


        // 初始化語音辨識
        keywordRecognizer = new KeywordRecognizer(passwords.Keys.ToArray()); // 建立關鍵字辨識器，辨識字詞
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech; // 當辨識到語音時，執行 RecognizedSpeech
        keywordRecognizer.Start(); // 開始監聽語音指令

    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log("辨識到指令: " + speech.text);
        if (passwords.ContainsKey(speech.text)) // 如果辨識到的文字是密碼
        {
            passwords[speech.text].Invoke(); // 執行對應的動作
        }
    }



    private void Unlock()
    {
        Debug.Log("密碼正確 解鎖成功！");
    }



}