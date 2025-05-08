using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceMovement1 : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    private Vector3 movementDirection = Vector3.zero; // 角色移動方向
    public float moveSpeed = 3.0f; // 移動速度
    private bool isMoving = false; // 角色是否在移動

    void Start()
    {
        // 設定語音指令與對應的函式
        actions.Add("forward", Forward);
        actions.Add("back", Back);
        actions.Add("up", Up);
        actions.Add("down", Down);
        actions.Add("stop", StopMovement); // 停止移動

        // 初始化語音辨識
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log("辨識到指令: " + speech.text);
        CancelInvoke("StartMovement"); // 先取消之前的移動 (避免指令重疊)
        actions[speech.text].Invoke();
        Invoke("StartMovement", 0.1f); // 延遲 0.1 秒後開始移動
    }

    private void StartMovement()
    {
        isMoving = true; // 確保 0.5 秒後移動才開始
    }

    private void Forward()
    {
        movementDirection = Vector3.right; // 設定移動方向為向右（X 軸正方向）
        isMoving = true; // 啟動移動
    }

    private void Back()
    {
        movementDirection = Vector3.left; // 設定移動方向為向左（X 軸負方向）
        isMoving = true;
    }

    private void Up()
    {
        movementDirection = Vector3.up; // 設定移動方向為向上（Y 軸正方向）
        isMoving = true;
    }

    private void Down()
    {
        movementDirection = Vector3.down; // 設定移動方向為向下（Y 軸負方向）
        isMoving = true;
    }

    private void StopMovement()
    {
        movementDirection = Vector3.zero; // 停止移動
        isMoving = false;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.Translate(movementDirection * moveSpeed * Time.deltaTime); // 讓角色持續移動
        }
    }
}
