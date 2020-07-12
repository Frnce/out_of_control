using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource sfxSource = null;
    [SerializeField]
    private AudioSource bgmSource = null;
    [SerializeField]
    private float lowPitchRange = .95f;
    [SerializeField]
    private float hightPitchrange = 1.05f;

    private static AudioManager instance = null;
    public static AudioManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            return null;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
    public void PlaySingle(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }
    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, hightPitchrange);

        sfxSource.pitch = randomPitch;

        sfxSource.PlayOneShot(clips[randomIndex]);
    }
}
