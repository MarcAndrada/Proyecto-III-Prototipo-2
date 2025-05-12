using UnityEngine;

public class AmbientSoundController : MonoBehaviour
{
    public static AmbientSoundController instance;

    [SerializeField]
    private AudioSource[] sources;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance);


        instance = this;
    }


    public void PlaySound(AudioClip _clip, float _volume, float _pitch)
    {
        AudioSource audioSource = GetUnusedAS();

        if (!audioSource)
            return;

        audioSource.clip = _clip;
        audioSource.volume = _volume;
        audioSource.pitch = _pitch;

        audioSource.Play();
    }


    private AudioSource GetUnusedAS()
    {
        foreach (AudioSource source in sources) 
        {
            if (source.clip == null || !source.isPlaying)
                return source;
        }

        return null;
    }
}
