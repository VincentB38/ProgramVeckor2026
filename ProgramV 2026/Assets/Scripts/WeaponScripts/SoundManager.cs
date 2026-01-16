using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class SoundInformation
{
    public string audioName;
    public AudioClip soundClip;
    public float volume;
}

public class SoundManager : MonoBehaviour
{
    public List<SoundInformation> sounds = new List<SoundInformation>();
    public float masterVolumeMultiplier;
    public GameObject audioObjectPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(int audioIndex, Transform spawnTransform)
    {
        GameObject soundPrefab = Instantiate(audioObjectPrefab, spawnTransform.position, Quaternion.identity, spawnTransform);

        AudioSource audioSource = soundPrefab.GetComponent<AudioSource>();

        audioSource.clip = sounds[audioIndex].soundClip;
        audioSource.volume = sounds[audioIndex].volume * masterVolumeMultiplier;
        audioSource.Play();
    }
}
