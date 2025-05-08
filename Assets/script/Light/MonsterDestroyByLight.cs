using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MonsterDestroyByLight : MonoBehaviour
{
    public Light playerSpotLight;            // 玩家手中的 Spot Light
    public string monsterTag = "Monster";    // 怪物的 tag
    public float lightRange = 20f;           // 光照最遠距離
    public float angleThreshold = 15f;       // 可被照射的最大角度
    public float destroyDelay = 2f;          // 持續照射多久後銷毀
    public float lightDuration = 3f;         // 手電筒持續亮的時間

    private KeywordRecognizer recognizer;
    private Dictionary<string, System.Action> actions = new Dictionary<string, System.Action>();
    private Dictionary<GameObject, Coroutine> trackingCoroutines = new Dictionary<GameObject, Coroutine>();
    private Coroutine flashlightCoroutine;

    void Start()
    {
        actions.Add("open", OnOpenCommand);
        recognizer = new KeywordRecognizer(actions.Keys.ToArray());
        recognizer.OnPhraseRecognized += OnRecognized;
        recognizer.Start();

        if (playerSpotLight != null)
            playerSpotLight.enabled = false;
    }

    private void OnRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("辨識到指令：" + args.text);
        actions[args.text].Invoke();
    }

    private void OnOpenCommand()
    {
        // 打開手電筒
        if (flashlightCoroutine != null)
            StopCoroutine(flashlightCoroutine);

        flashlightCoroutine = StartCoroutine(TemporarilyTurnOnLight());

        // 同時檢查怪物
        StartCoroutine(CheckMonstersUnderLight());
    }

    private IEnumerator TemporarilyTurnOnLight()
    {
        playerSpotLight.enabled = true;
        yield return new WaitForSeconds(lightDuration);
        playerSpotLight.enabled = false;
        flashlightCoroutine = null;
    }

    private IEnumerator CheckMonstersUnderLight()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag(monsterTag);

        foreach (GameObject monster in monsters)
        {
            if (trackingCoroutines.ContainsKey(monster)) continue;

            Vector3 dirToMonster = monster.transform.position - playerSpotLight.transform.position;
            float angle = Vector3.Angle(playerSpotLight.transform.forward, dirToMonster);
            float distance = dirToMonster.magnitude;

            if (angle < angleThreshold && distance <= lightRange)
            {
                Ray ray = new Ray(playerSpotLight.transform.position, dirToMonster);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, lightRange))
                {
                    if (hit.collider.gameObject == monster)
                    {
                        Coroutine c = StartCoroutine(DelayedDestroy(monster));
                        trackingCoroutines[monster] = c;
                    }
                }
            }
        }

        yield return null;
    }

    private IEnumerator DelayedDestroy(GameObject monster)
    {
        float timer = 0f;

        while (timer < destroyDelay)
        {
            if (monster == null) yield break;

            Vector3 dirToMonster = monster.transform.position - playerSpotLight.transform.position;
            float angle = Vector3.Angle(playerSpotLight.transform.forward, dirToMonster);
            float distance = dirToMonster.magnitude;

            Ray ray = new Ray(playerSpotLight.transform.position, dirToMonster);
            RaycastHit hit;

            if (!(angle < angleThreshold && distance <= lightRange && Physics.Raycast(ray, out hit, lightRange) && hit.collider.gameObject == monster))
            {
                trackingCoroutines.Remove(monster);
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(monster);
        trackingCoroutines.Remove(monster);
        Debug.Log("怪物被持續照射2秒後消失");
    }
}
