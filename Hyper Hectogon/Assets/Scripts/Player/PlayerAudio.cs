using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] pianoOctave;
    [SerializeField] private AudioClip[] wikbeats;
    [SerializeField] private AudioClip heal;

    private AudioSource audioSource;
    private int pianoOctaveLength;
    private int wikbeatsLength;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pianoOctaveLength = pianoOctave.Length;
        wikbeatsLength = wikbeats.Length;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="effect">0:ColorUp, 1:Enemy, 2:AngleUp</param>
    /// <param name="track"></param>
    public void PlaySound(int effect, int track = 0)
    {
        if(effect == 0) // colorup : pianooctave
        {
            track %= pianoOctaveLength;
            audioSource.PlayOneShot(pianoOctave[track], 1.0f);
        }
        else if(effect == 1) // enemy : wikbeats
        {
            track %= wikbeatsLength;
            audioSource.PlayOneShot(wikbeats[track], 1.0f);
        }
        else if(effect == 2) // angleup : heal
        {
            audioSource.PlayOneShot(heal, 1.0f);
        }
    }
}
