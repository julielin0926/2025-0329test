using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MicInput : MonoBehaviour
{
    public float loudnessThreshold = 0.1f;
    private AudioSource audioSource;

    public bool IsBlowing { get; private set; }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, true, 1, 44100);
        audioSource.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { }
        audioSource.Play();
    }

    void Update()
    {
        float[] data = new float[256];
        audioSource.GetOutputData(data, 0);
        float loudness = 0f;

        foreach (var sample in data)
            loudness += Mathf.Abs(sample);

        IsBlowing = loudness > loudnessThreshold;
    }
}
