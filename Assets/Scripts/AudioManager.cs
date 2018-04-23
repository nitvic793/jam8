﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource efxSource;
    public AudioSource rogerSource;
    public AudioSource musicSource;
    public AudioSource shootSource;
    public AudioSource wobbleSource;
    public static AudioManager instance = null;
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        rogerSource.clip = clip;
        shootSource.clip = clip;
        wobbleSource.clip = clip;

        efxSource.Play();
        rogerSource.Play();
        shootSource.Play();
        wobbleSource.Play();
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Length);

        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        //Set the pitch of the audio source to the randomly chosen pitch.
        efxSource.pitch = randomPitch;


        //Set the clip to the clip at our randomly chosen index.
        efxSource.clip = clips[randomIndex];

        //Play the clip.
        efxSource.Play();

    }

    public void RandomizeRogerSfx(params AudioClip[] clips)
    {
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        rogerSource.pitch = randomPitch;
        int randomIndex = Random.Range(0, clips.Length);
        rogerSource.clip = clips[randomIndex];
        rogerSource.PlayOneShot(rogerSource.clip);
    }

    public void RandomizeShootSfx(params AudioClip[] clips)
    {
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        shootSource.pitch = randomPitch;
        int randomIndex = Random.Range(0, clips.Length);
        shootSource.clip = clips[randomIndex];
        shootSource.PlayOneShot(shootSource.clip);
    }
    public void RandomizeWobbleSfx(params AudioClip[] clips)
    {
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        wobbleSource.pitch = randomPitch;
        int randomIndex = Random.Range(0, clips.Length);
        wobbleSource.clip = clips[randomIndex];
        wobbleSource.Play();
    }

}