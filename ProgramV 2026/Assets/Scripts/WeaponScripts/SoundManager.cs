using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class SoundInformation
{
    public string audioName;
    public AudioClip soundClip;
    public float volume = 1f;
}

public class SoundManager : MonoBehaviour
{
    public List<SoundInformation> sounds = new List<SoundInformation>();
    public float masterVolumeMultiplier;
    public GameObject audioObjectPrefab;

    public void PlaySound(int audioIndex, Transform spawnTransform)
    {
        GameObject soundPrefab = Instantiate(audioObjectPrefab, spawnTransform.position, Quaternion.identity, spawnTransform);

        AudioSource audioSource = soundPrefab.GetComponent<AudioSource>();

        audioSource.clip = sounds[audioIndex].soundClip;
        audioSource.volume = sounds[audioIndex].volume * masterVolumeMultiplier;
        audioSource.Play();

        if (!audioSource.isPlaying)
        {
            Destroy(soundPrefab);
        }
    }
}
