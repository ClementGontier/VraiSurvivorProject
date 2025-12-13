using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip gameoverson;
    public AudioClip levelupson;
    public AudioClip xpson;
    //public AudioClip victoireson;
    public AudioClip bouleson;
    public AudioClip pelleson;
    public AudioClip faucilleson;
    public AudioClip sonepee;
    public AudioClip musiqueniveau;
    public AudioClip degat;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.PlayOneShot(clip);
    }
}

