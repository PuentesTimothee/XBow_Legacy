using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playlist : MonoBehaviour
{
  public AudioClip[] waveFiles;
  private AudioSource thisAudioSource;

  public float volMin;
  public float volMax;

  public float pitchMin;
  public float pitchMax;
  private int nbSong;

  public bool playOnAwake;

  void Awake () {
    thisAudioSource = GetComponent<AudioSource>();
    nbSong = Random.Range( 0, waveFiles.Length );

    if ( playOnAwake )
    {
      Play();
    }

  }

  void Start (){
    thisAudioSource.Play();
  }

  // Update is called once per frame
  void Update () {
    if(!thisAudioSource.isPlaying)
    Play();
  }

  void Play() {
    if ( thisAudioSource != null && thisAudioSource.isActiveAndEnabled )
    {
      //randomly apply a volume between the volume min max
      thisAudioSource.volume = Random.Range( volMin, volMax );

      //randomly apply a pitch between the pitch min max
      thisAudioSource.pitch = Random.Range( pitchMin, pitchMax );

      // play the sound
      thisAudioSource.PlayOneShot( waveFiles[nbSong] );
      nbSong++;
      if (nbSong >= waveFiles.Length)
        nbSong = 0;
    }
  }
}
