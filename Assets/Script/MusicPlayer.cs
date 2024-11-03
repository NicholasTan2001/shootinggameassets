using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MusicPlayer : MonoBehaviour
{
 private AudioSource AudioSource;
 public Slider volumeSlider;
 private float musicVolume = 0f;
 public GameObject ObjectMusic;
 void Start()
 {
 ObjectMusic = GameObject.FindWithTag("GameMusic");
 AudioSource = ObjectMusic.GetComponent<AudioSource>();
 musicVolume = PlayerPrefs.GetFloat("volume");
 AudioSource.volume = musicVolume;
 volumeSlider.value = musicVolume;
 }
 void Update()
 {
 AudioSource.volume = musicVolume;
 PlayerPrefs.SetFloat("volume", musicVolume);
 }
 public void updateVolume( float volume)
 {
 musicVolume = volume;
 }
 public void musicReset()
 {
 PlayerPrefs.DeleteKey("volume");
 AudioSource.volume = 1;
 volumeSlider.value = 1;
 }
}