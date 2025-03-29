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
    private Vector3 movementDirection = Vector3.zero; // ���Ⲿ�ʤ�V
    public float moveSpeed = 3.0f; // ���ʳt��
    private bool isMoving = false; // ����O�_�b����

    void Start()
    {
        // �]�w�y�����O�P�������禡
        actions.Add("forward", Forward);
        actions.Add("back", Back);
        actions.Add("up", Up);
        actions.Add("down", Down);
        actions.Add("stop", StopMovement); // �����

        // ��l�ƻy������
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log("���Ѩ���O: " + speech.text);
        CancelInvoke("StartMovement"); // ���������e������ (�קK���O���|)
        actions[speech.text].Invoke();
        Invoke("StartMovement", 0.1f); // ���� 0.1 ���}�l����
    }

    private void StartMovement()
    {
        isMoving = true; // �T�O 0.5 ��Ჾ�ʤ~�}�l
    }

    private void Forward()
    {
        movementDirection = Vector3.right; // �]�w���ʤ�V���V�k�]X �b����V�^
        isMoving = true; // �Ұʲ���
    }

    private void Back()
    {
        movementDirection = Vector3.left; // �]�w���ʤ�V���V���]X �b�t��V�^
        isMoving = true;
    }

    private void Up()
    {
        movementDirection = Vector3.up; // �]�w���ʤ�V���V�W�]Y �b����V�^
        isMoving = true;
    }

    private void Down()
    {
        movementDirection = Vector3.down; // �]�w���ʤ�V���V�U�]Y �b�t��V�^
        isMoving = true;
    }

    private void StopMovement()
    {
        movementDirection = Vector3.zero; // �����
        isMoving = false;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.Translate(movementDirection * moveSpeed * Time.deltaTime); // ��������򲾰�
        }
    }
}
