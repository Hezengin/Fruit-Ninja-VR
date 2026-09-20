using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [SerializeField] private GameObject soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        GameObject audioSourceObject = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        AudioSource audioSource = audioSourceObject.GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSourceObject, clipLength);
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClips, Transform spawnTransform, float volume)
    {
        GameObject audioSourceObject = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        AudioSource audioSource = audioSourceObject.GetComponent<AudioSource>();
        int random = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[random];
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSourceObject, clipLength);
    }
}
