using DG.Tweening;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public enum AudioType {
	NONE,
	DEFAULT,
	INGAME
}
	
public static class AudioManager {

	private static AudioType currentPlayType = AudioType.NONE;
	private static string audioPrefabPath = null;
	private static string soundSourceUID = null;

	public static void PlayMusic(AudioType audioType, string path, bool isListener) {

		if(!MusicPlayable(audioType))
			return;
		if(!PrefabSet())
			SetAudioPrefabPath();
		if(soundSourceUID == null) {
			PlayNewAudio(audioType, path);
		} else {
			ChangeToNewMusic(audioType, path, isListener);
		}
	}

	public static void SetAudioPrefabPath(string path) {
		audioPrefabPath = path;
	}

	public static void SetAudioPrefabPath() {
		audioPrefabPath = "Torch/Prefabs/DefaultAudio";
	}

	public static void ChangeTheVolumeOfBackgoundMusic(float volume) {
		AudioSource audioSource = GetAudioSource();

		audioSource.volume = volume;
	}

	public static AudioSource LoadSound(string uid, string url, bool loop) {
		AudioClip clip = (AudioClip)Resources.Load(url, typeof(AudioClip));
		GameObject audioPlayer = Torch.GetTransform(uid).gameObject;

		if(clip == null)
			return null;

		AudioSource source = (AudioSource)audioPlayer.GetComponent("AudioSource");
		if(source == null)
			source = (AudioSource)audioPlayer.AddComponent<AudioSource>();
		source.clip = clip;
		source.loop = loop;
		return source;
	}

	private static bool MusicPlayable(AudioType audioType) {
		if(audioType == currentPlayType || audioType == AudioType.NONE)
			return false;
		return true;
	}

	private static bool PrefabSet() {
		if(audioPrefabPath == null)
			return false;
		return true;
	}

	private static void InitiateAudio() {
		string audioSourceUid = Torch.CreateObject(null);
		Torch.SetPrefab(audioSourceUid, audioPrefabPath);
		soundSourceUID = audioSourceUid;

	}

	private static void RemoveListener() {
		string audioSourceUid = soundSourceUID;
		Transform audTrans = Torch.GetTransform(audioSourceUid);
		audTrans.GetComponent<AudioListener>().enabled = false;
	}

	private static void ChangeMusic(AudioType audioType, string path, bool isListener) {
		string audioSourceUid = soundSourceUID;
		Torch.DestroyObject(audioSourceUid);
		soundSourceUID = null;

		PlayMusic(audioType, path, isListener);
		if(!isListener)
			RemoveListener();
	}

	private static void PlayNewAudio(AudioType audioType, string path) {
		InitiateAudio();
		string audioSourceUid = soundSourceUID;
		AudioSource aud = LoadSound(audioSourceUid, path, true);
		float volume = 0;
		DOTween.To(() => volume, x => volume = x, volume + 0.5f, 2.7f)
				.OnUpdate(() => ChangeTheVolumeOfBackgoundMusic(volume))
				.SetEase(Ease.Linear);
		aud.Play();
		currentPlayType = audioType;
	}

	private static void ChangeToNewMusic(AudioType audioType, string path, bool isListener) {
		float volume = 0.5f;
		DOTween.To(() => volume, x => volume = x, 0, 0.5f)
				.OnUpdate(() => ChangeTheVolumeOfBackgoundMusic(volume))
				.SetEase(Ease.Linear).OnComplete(() => ChangeMusic(audioType, path, isListener));
	}


	private static AudioSource GetAudioSource() {
		string audioSourceUid = soundSourceUID;
		Transform audioTrans = Torch.GetTransform(audioSourceUid);
		AudioSource audioSource = audioTrans.GetComponent<AudioSource>() as AudioSource;

		return audioSource;
	}

	public static void CreateSoundEffect(string url, Vector3 playPos) {
		string uid = Torch.CreateObject(null);
		Torch.SetPrefab(uid, "Prefabs/SoundEffect");
		GameObject audioPlayer = Torch.GetTransform(uid).gameObject;
		Torch.SetTransformPosition(uid, playPos);
		AudioClip clip = (AudioClip)Resources.Load(url, typeof(AudioClip));

		if(clip == null)
			return;

		AudioSource source = (AudioSource)audioPlayer.GetComponent("AudioSource");
		if(source == null)
			source = (AudioSource)audioPlayer.AddComponent<AudioSource>();
		source.clip = clip;
		source.loop = false;
		source.volume = 1.0f;
		source.Play();
		source.DOFade(0f, 1f).OnComplete(() => Torch.DestroyObject(uid));
	}
}

