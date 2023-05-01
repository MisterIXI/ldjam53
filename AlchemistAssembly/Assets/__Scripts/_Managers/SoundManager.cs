using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] SoundPool;
    [SerializeField] private AudioClip[] musicPool;
    [SerializeField] private GameObject AudioSpawnObject;
    [SerializeField] private AudioSource MusicObject;
    private int currentMusicIndex = 0;
    private int lastCheckFrame = -1;
    private readonly Queue<AudioSource> audioSources = new Queue<AudioSource>();
    private readonly LinkedList<AudioSource> inuse = new LinkedList<AudioSource>();
    private readonly Queue<LinkedListNode<AudioSource>> nodePool = new Queue<LinkedListNode<AudioSource>>();
    private float setting_SFX_Volume =40;
    private float setting_Music_Volume = 40;
    public static SoundManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }
    public enum Sound
    {
        Ambients = 0,
        Ambient2 = 1,
        Ambients3 = 2,
        Menu_Click_Bass = 3,
        BeeAproach = 4,
        Bee_Landing = 5,
        Brewing = 6,
        Buiding_Complete = 7,
        Menu_Click = 8,
        Mining = 9,
        Minecart_Moving = 10,
        Object_placing = 11,
        Potion_complete = 12,
        Track_Click = 13,
        Track_Friction = 14,
        Waterpump = 15

    }
    public static void PlayInvalidActionSound()
    {
        Debug.Log("Invalid Action Sound");
    }
    private void CheckInUse()
    {
        var node = inuse.First;
        while (node != null)
        {
            var current = node;
            node = node.Next;

            if (!current.Value.isPlaying)
            {
                audioSources.Enqueue(current.Value);
                inuse.Remove(current);
                nodePool.Enqueue(current);
            }
        }
    }
    public void PlaySound(Sound soundType, Vector3 point, float volume)
    {
        AudioSource source;

        if (lastCheckFrame != Time.frameCount)
        {
            lastCheckFrame = Time.frameCount;
            CheckInUse();
        }

        if (audioSources.Count == 0)
            source = GameObject.Instantiate(AudioSpawnObject).GetComponent<AudioSource>();
        else
            source = audioSources.Dequeue();

        if (nodePool.Count == 0)
            inuse.AddLast(source);
        else
        {
            var node = nodePool.Dequeue();
            node.Value = source;
            inuse.AddLast(node);
        }

        source.transform.position = point;
        source.clip = SoundPool[(int)soundType];
        source.volume = volume;
        source.Play();
    }
    public void PlayMusicNext()
    {
        MusicObject.PlayOneShot(musicPool[currentMusicIndex], 0.5f); // GAME VARIABLE TODO

        Invoke(nameof(EventOnEnd), musicPool[currentMusicIndex].length);
    }

    void EventOnEnd()
    {
        if (Application.isEditor) Debug.LogWarning("audio finished!");
        currentMusicIndex++;
        if (currentMusicIndex >= musicPool.Length)
            currentMusicIndex = 0;
        PlayMusicNext();
    }
}