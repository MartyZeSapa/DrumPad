using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    public static AudioSourcePool Instance;

    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private int poolSize = 20;

    private Queue<AudioSource> availableSources = new Queue<AudioSource>();
    private List<AudioSource> allSources = new List<AudioSource>();

    void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        ExpandPool(poolSize);
    }

    private void ExpandPool(int increaseBy)
    {
        for (int i = 0; i < increaseBy; i++)
        {
            AudioSource src = Instantiate(audioSourcePrefab, transform);
            src.playOnAwake = false;
            src.loop = false;
            src.gameObject.SetActive(false);
            availableSources.Enqueue(src);
            allSources.Add(src);
        }
    }

    public AudioSource GetSource()
    {
        if (availableSources.Count == 0)
        {
            ExpandPool(5); // Expand by a small number instead of one to minimize instantiation overhead
        }

        AudioSource src = availableSources.Dequeue();
        src.gameObject.SetActive(true);
        return src;
    }

    public void ReturnSource(AudioSource src)
    {
        src.Stop();
        src.gameObject.SetActive(false);
        availableSources.Enqueue(src);
    }

    void OnDestroy()
    {
        // Clean up all AudioSource objects when the pool is destroyed
        foreach (var src in allSources)
        {
            Destroy(src.gameObject);
        }
    }
}
