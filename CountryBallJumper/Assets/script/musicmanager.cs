using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class musicmanager : MonoBehaviour
{
    public static musicmanager instance; // Singleton instance
    public AudioClip[] musicClips;
    private AudioSource audioSource;
    private int currentClipIndex = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Müzik yöneticisini sahne yüklemelerinde yok etmeyi engeller
        }
        else
        {
            Destroy(gameObject); // Singleton oluşturma
            return;
        }

        audioSource = GetComponent<AudioSource>();
        PlayNextClip(); // İlk müziği başlat
    }

    void Update()
    {
        // Müzik bitince bir sonraki müziği başlat
        if (!audioSource.isPlaying)
        {
            PlayNextClip();
        }
    }

    void PlayNextClip()
    {
        audioSource.clip = musicClips[currentClipIndex];
        audioSource.Play();
        currentClipIndex = (currentClipIndex + 1) % musicClips.Length; // Döngü halinde sonraki müziği seç
    }
}
