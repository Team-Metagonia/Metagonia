using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    AudioSource _audioSource;
    [SerializeField] AudioClip[] _audioclips;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX()
    {
        int randomIndex = Random.Range(0, _audioclips.Length);

        _audioSource.PlayOneShot(_audioclips[randomIndex]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
