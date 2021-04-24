using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using utils;
using System.Linq;

namespace Audio {
	public enum MusicType{MainTheme, None};

	[System.Serializable]
	public class MusicTypeAudioClipMap{
		public MusicType type;
		public AudioClip clip;
	}

	[RequireComponent(typeof(AudioSourcePool))]
	[RequireComponent(typeof(AudioSource))]
	public class AudioManager : Singleton<AudioManager> {
		public FloatReference masterVolume;
		public FloatReference musicVolume;
		public FloatReference sfxVolume;

		AudioSource audioSource;
		AudioSourcePool pool;

		MusicType current = MusicType.None;

		[Header("Configurations")]
		public float fadeDuration;

		[Header("Musics")]
		public MusicTypeAudioClipMap[] musics;

		void Awake(){
			if (AudioManager.Instance != this) {
				Destroy (gameObject);
				return;
			}
			pool = GetComponent<AudioSourcePool>();
			audioSource = GetComponent<AudioSource> ();

			masterVolume.Variable.onValueChange += (newValue) => UpdateMusic(musicVolume, newValue);
			masterVolume.Variable.onValueChange += (newValue) => UpdateSFX(sfxVolume, newValue);

			musicVolume.Variable.onValueChange += (newValue) => UpdateMusic(newValue, masterVolume);
			sfxVolume.Variable.onValueChange += (newValue) => UpdateSFX(newValue, masterVolume);

			UpdateMusic(musicVolume, masterVolume);
			UpdateSFX(sfxVolume, masterVolume);

			DontDestroyOnLoad (gameObject);
		}

		public AudioSource PlaySfx(AudioClip clip){
			var _source = pool.GetObject ();

			_source.volume = sfxVolume;

			_source.clip = clip;
			_source.Play ();

			StartCoroutine (checkIsPlaying (_source));

			return _source;
		}

		public AudioSource PlaySfx(AudioClip clip, float time){
			var _source = pool.GetObject ();

			_source.volume = sfxVolume;

			_source.clip = clip;
			_source.time = Random.Range (0.05f, _source.clip.length - 0.05f);
			_source.Play ();

			return _source;
		}

		public AudioSource PlaySfx(AudioClip clip, bool loop){
			if(!loop){
				return PlaySfx (clip);
			}

			var _source = pool.GetObject ();
			_source.volume = sfxVolume;

			_source.clip = clip;
			_source.Play ();

			_source.loop = true;

			return _source;
		}

		IEnumerator checkIsPlaying(AudioSource source){
			yield return new WaitUntil (() => !source.isPlaying);

			pool.Disable (source);
		}

		public void StopFx(AudioSource source, AudioClip clip){
			source.loop = false;
			source.time = 0;
			pool.Disable (source, clip);
		}

		MusicTypeAudioClipMap FindMusic(MusicType type){
			MusicTypeAudioClipMap music = null;

			foreach (var _music in musics) {
				if (_music.type == type) {
					music = _music;
					break;
				}
			}

			return music;
		}

		public void FadeMusic(float time){
			audioSource.DOKill ();
			audioSource.DOFade (0f, time);
		}

		public void RandomMusic(MusicType[] types, float time=-1){
			int index = Random.Range(0, types.Length);

			PlayMusic(types[index], time);
		}

		public void PlayMusic(MusicType type, float time=-1){
			if (type == current) {
				return;
			}

			if (audioSource == null) {
				audioSource = GetComponent<AudioSource> ();
			}

			float fadeDuration = time == -1 ? this.fadeDuration : time;

			MusicTypeAudioClipMap music = FindMusic (type);

			float volume = GetVolume(musicVolume);

			audioSource.DOKill ();
			if (audioSource.isPlaying) {
				audioSource.DOFade (0, fadeDuration).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => {
					audioSource.clip = music.clip;
					audioSource.Play();
					audioSource.DOFade(volume, fadeDuration).SetEase(Ease.Linear);;
				});
			} else {
				audioSource.clip = music.clip;
				audioSource.Play();
				audioSource.volume = 0;
				audioSource.DOFade(volume, fadeDuration).SetEase(Ease.Linear);
			}

			current = type;
		}

		float GetVolume(float volume){
			return volume * masterVolume;
		}

		public void UpdateMusic(float value, float masterValue){
			audioSource.volume = value * masterValue;
		}

		public void UpdateSFX(float value, float masterValue){
			var objects = pool.GetAllActive();
			foreach (var obj in objects) {
				obj.volume = value * masterValue;
			}
		}

		public void StopAllFxs(){
			pool.StopAll ();
		}
	}
}
