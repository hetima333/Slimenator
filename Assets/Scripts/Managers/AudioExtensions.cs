using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioExtensions {

	public static void Play(this AudioSource source, AudioClip clip = null, float volume = 1.0f) {
		if (clip == null) {
			return;
		}

		source.clip = clip;
		source.volume = volume;
		source.Play();
	}

	public static void PlayOneShot(this AudioSource source, AudioClip clip = null, float volume = 1.0f) {
		if (clip == null) {
			return;
		}

		source.volume = volume;
		source.PlayOneShot(clip);
	}

	public static IEnumerator FadeIn(this AudioSource source, float fadeTime, float endVolume = 1.0f) {
		while (source.volume < endVolume) {
			float temp = source.volume + (Time.deltaTime / fadeTime);

			source.volume = temp > endVolume ? endVolume : temp;

			yield return null;
		}
	}

	public static IEnumerator FadeOut(this AudioSource source, float fadeTime, float endVolume = 0.0f) {
		while (source.volume > endVolume) {
			float temp = source.volume - (Time.deltaTime / fadeTime);

			source.volume = temp < endVolume ? endVolume : temp;

			yield return null;
		}
	}
}