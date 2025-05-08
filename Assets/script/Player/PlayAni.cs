using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class PlayAni : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    public Light sceneLight;               // 場景環境燈
    public GameObject flashlightObject;    // 玩家手電筒模型
    public float pickUpRange = 3f;         // 可撿距離

    private Coroutine lightCoroutine;
    private Animator animator;             // 播放撿東西動畫用

    void Start()
    {
        animator = GetComponent<Animator>();
        //flashlightObject.SetActive(false);

        // 加入語音指令
        actions.Add("turn on", TurnOnLight);
        actions.Add("get", HandlePickup);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log("辨識到指令: " + speech.text);
        if (actions.ContainsKey(speech.text))
        {
            actions[speech.text].Invoke();
        }
    }

    private void TurnOnLight()
    {
        if (sceneLight != null)
        {
            if (lightCoroutine != null)
                StopCoroutine(lightCoroutine);

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

    private void HandlePickup()
    {
        //flashlightObject.SetActive(true);// 開啟手電筒（可見 + 光源開啟）
        // 播放撿起動畫
        if (animator != null)
        {
            animator.SetTrigger("pickup");
        }

        // 撿起最近的物品
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("pickup");
        GameObject nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject item in pickups)
        {
            float distance = Vector3.Distance(transform.position, item.transform.position);
            if (distance < minDistance && distance <= pickUpRange)
            {
                minDistance = distance;
                nearest = item;
            }
        }

        if (nearest != null)
        {
            Debug.Log("撿起物品：" + nearest.name);
            Destroy(nearest); // 模擬撿起
            

        }
        else
        {
            Debug.Log("附近沒有可撿的物品。");
        }
    }
}
