using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBGM : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;
    AudioSource audioSource;
    Skybox_material skyMat;

    // 이름값을 받아오고 이름값에 맞는 
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        skyMat = FindObjectOfType<Skybox_material>();        
    }

    private void OnEnable()
    {
        audioSource.clip = audioClips[skyMat.i];        
        audioSource.Play();        
    }


}
