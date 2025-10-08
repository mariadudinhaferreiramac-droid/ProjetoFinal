using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance { get; private set; }

    [Header("Music Clips")]
    public AudioClip gameplayMusic;
    public AudioClip deathMusic;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayGameplayMusic();
    }

    public void PlayGameplayMusic()
    {
        if (gameplayMusic != null)
        {
            audioSource.clip = gameplayMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void PlayDeathMusic()
    {
        if (deathMusic != null)
        {
            audioSource.clip = deathMusic;
            audioSource.loop = true; 
            audioSource.Play();
        }
    }
}
