using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceSpawnObject : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    // 語音物品對應的 Prefab（在 Inspector 中設定）
    public GameObject ballPrefab;
    public GameObject flashlightPrefab;
    public GameObject capsulePrefab;

    void Start()
    {
        // 建立語音指令與動作對應
        actions.Add("ball", SpawnBall);
        actions.Add("flashlight", Spawnflashlight);
        actions.Add("capsule", SpawnCapsule);

        // 初始化語音辨識器
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log("辨識到指令: " + speech.text);
        actions[speech.text].Invoke();
    }

    private void SpawnBall()
    {
        SpawnItem(ballPrefab);
    }

    private void Spawnflashlight()
    {
        SpawnItem(flashlightPrefab);
    }

    private void SpawnCapsule()
    {
        SpawnItem(capsulePrefab);
    }

    private void SpawnItem(GameObject prefab)
    {
        if (prefab != null)
        {
            Vector3 playerPosition = transform.position;

            // 在玩家前上方生成（避免卡頭）
            Vector3 offset = transform.forward * -2f + Vector3.up * 3f;
            Vector3 spawnPosition = playerPosition + offset;

            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }
}
