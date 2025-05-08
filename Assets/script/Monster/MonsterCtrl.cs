using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class MonsterCtrl : MonoBehaviour
{
    public Transform player;
    public float pressureDistance = 10f;         // 最遠開始影響壓力的距離
    public float maxPressure = 100f;             // 壓力最大值
    public float pressureIncreaseRate = 20f;     // 每秒壓力增加速率

    private float currentPressure = 0f;

    public Animator MonsterAni;

    public Light sceneLight;
    public float flickerSpeed = 0.1f;

    public Camera Camera;
    public float shakeIntensity = 0.05f;
    public float shakeFrequency = 5f;

    public AudioSource monsterAudio;

    private NavMeshAgent agent;
    private float originalLightIntensity;
    private Vector3 originalCameraPos;

    void Start()
    {
        monsterAudio = GetComponent<AudioSource>();
        MonsterAni = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        originalCameraPos = Camera.transform.localPosition;

        if (sceneLight != null)
            originalLightIntensity = sceneLight.intensity;
 
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > 5f)
        {
            // 開始追蹤
            agent.SetDestination(player.position);

            float normalizedDistance = Mathf.Clamp01(1f - (distance / pressureDistance));

            // 壓力累加
            currentPressure += normalizedDistance * pressureIncreaseRate * Time.deltaTime;
            currentPressure = Mathf.Clamp(currentPressure, 0, maxPressure);

            // 音量依距離調整

            if (monsterAudio != null)
            {
                monsterAudio.volume = normalizedDistance;
                if (!monsterAudio.isPlaying)
                    monsterAudio.Play();
            }


            // 閃爍環境光
            if (sceneLight != null)
            {
                float flicker = Mathf.PerlinNoise(Time.time * 10f, 0.0f);
                sceneLight.intensity = Mathf.Lerp(originalLightIntensity * 0.5f, originalLightIntensity, flicker * normalizedDistance);
            }

            // 鏡頭晃動
            if (Camera != null)
            {
                float shake = Mathf.Sin(Time.time * shakeFrequency) * shakeIntensity * normalizedDistance;
                Vector3 shakeOffset = new Vector3(shake, shake, 0);
                Camera.transform.localPosition = originalCameraPos + shakeOffset;
            }

            // 動畫控制：如果怪物正在移動就播放 run 動畫
            if (MonsterAni != null && agent != null)
            {
                float currentDistance = Vector3.Distance(transform.position, player.position);

                if (currentDistance > 3f)
                {
                    MonsterAni.SetBool("run", true);
                }
                else
                {
                    MonsterAni.SetBool("run", false);
                }
            }
        }
    }

    public float GetCurrentPressure()
    {
        return currentPressure;
    }
}
