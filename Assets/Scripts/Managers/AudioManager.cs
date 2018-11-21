using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SingletonMonoBehaviour<AudioManager> {

	// Audioファイルのパス
	private const string BGM_PATH = "Audio/BGM";
	private const string SE_PATH = "Audio/SE";

	// オーディオソース
	private AudioSource _bgmSource;
	private List<AudioSource> _seSourceList = new List<AudioSource>();

	// オーディオクリップ
	private Dictionary<string, AudioClip> _bgmDic,
	_seDic;

	// SEの最大数
	private const int SE_MAX_NUM = 10;

	/// <summary>
	/// マスター音量
	/// </summary>
	/// <value>0.0~1.2</value>
	public float MasterVolume {
		get {
			float vol;
			if (_mixer.GetFloat("MasterVolume", out vol)) {
				return (vol / 80.0f) + 1.0f;
			}
			return 0.0f;
		}
		set {
			_mixer.SetFloat("MasterVolume", -80.0f + Mathf.Clamp(value, 0.0f, 1.2f) * 80.0f);
		}
	}

	/// <summary>
	/// SE音量
	/// </summary>
	/// <value>0.0~1.2</value>
	public float SEVolume {
		get {
			float vol;
			if (_mixer.GetFloat("SEVolume", out vol)) {
				return (vol / 80.0f) + 1.0f;
			}
			return 0.0f;
		}
		set {
			_mixer.SetFloat("SEVolume", -80.0f + Mathf.Clamp(value, 0.0f, 1.2f) * 80.0f);
		}
	}

	/// <summary>
	/// BGM音量
	/// </summary>
	/// <value>0.0~1.2</value>
	public float BGMVolume {
		get {
			float vol;
			if (_mixer.GetFloat("BGMVolume", out vol)) {
				return (vol / 80.0f) + 1.0f;
			}
			return 0.0f;
		}
		set {
			_mixer.SetFloat("BGMVolume", -80.0f + Mathf.Clamp(value, 0.0f, 1.2f) * 80.0f);
		}
	}

	// オーディオミキサー
	private AudioMixer _mixer;

	// シーン読み込み前にインスタンスを生成
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void InitializeBeforeSceneLoad() {
		// ゲーム中に常に存在するオブジェクトを生成
		var manager = new GameObject("AudioManager", typeof(AudioManager));
		// シーンが変更されても破棄されないようにする
		GameObject.DontDestroyOnLoad(manager);
	}

	void Start() {
		// オーディオリスナーをアタッチ
		gameObject.AddComponent<AudioListener>();

		// オーディオミキサーの取得
		_mixer = Resources.Load("Audio/Mixer", typeof(AudioMixer)) as AudioMixer;

		#region BGM settings

		// BGM用のオーディオソースを付与
		_bgmSource = gameObject.AddComponent<AudioSource>();

		// ループを有効にする
		_bgmSource.loop = true;
		// 2Dサウンドにする
		_bgmSource.spatialBlend = 0.0f;
		// ミキサーグループの設定
		_bgmSource.outputAudioMixerGroup = _mixer.FindMatchingGroups("BGM") [0];

		#endregion

		#region  SE settings

		var seGroup = _mixer.FindMatchingGroups("SE") [0];

		_seSourceList = Enumerable.Range(0, SE_MAX_NUM)
			.Select(x => {
				var source = gameObject.AddComponent<AudioSource>();
				// ミキサーグループの設定
				source.outputAudioMixerGroup = seGroup;

				return source;
			})
			.ToList();

		#endregion

		// リソースフォルダから全SE&BGMのファイルを読み込みセット
		_bgmDic = Resources.LoadAll(BGM_PATH, typeof(AudioClip)).Select(x => x as AudioClip).ToDictionary(x => x.name);
		_seDic = Resources.LoadAll(SE_PATH, typeof(AudioClip)).Select(x => x as AudioClip).ToDictionary(x => x.name);
	}

	/// <summary>
	/// SEの再生
	/// </summary>
	/// <param name="name">ファイル名</param>
	public void PlaySE(string name, bool loop = false) {
		// SEが存在するかを調べる
		if (_seDic.ContainsKey(name) != true) {
			Debug.Log(name + "：という名前のSEは存在しません");
			return;
		}

		// 再生中でないソースを探す
		var source = _seSourceList.FirstOrDefault(x => x.isPlaying != true);
		source.loop = loop;

		// 再生中でないソースがあればSEを再生する
		if (source != null) {
			// ループ再生の場合はclipを変更してPlay()
			if (source.loop) {
				source.clip = _seDic[name] as AudioClip;
				source.Play();
			} else {
				source.clip = null;
				source.PlayOneShot(_seDic[name] as AudioClip);
			}
		} else {
			Debug.Log("再生に利用できるAudioSourceがありません");
		}
	}

	/// <summary>
	/// SEの停止
	/// </summary>
	/// <param name="name">ファイル名</param>
	public void StopSE(string name) {
		var source = _seSourceList.Where(x => x.clip != null).FirstOrDefault(x => name == x.clip.name);
		if (source == null) {
			return;
		}
		// 念のためループ解除
		source.loop = false;
		source.Stop();
	}

	/// <summary>
	/// SEのループ停止
	/// </summary>
	/// <param name="name">ファイル名</param>
	public void StopSELoop(string name) {
		var source = _seSourceList.Where(x => x.clip != null).FirstOrDefault(x => name == x.clip.name);
		if (source == null) {
			return;
		}
		source.loop = false;
	}

	/// <summary>
	/// BGMの再生
	/// </summary>
	/// <param name="name">ファイル名</param>
	public void PlayBGM(string name) {
		// BGMが存在するかを調べる
		if (_bgmDic.ContainsKey(name) != true) {
			Debug.Log(name + "：という名前のSEは存在しません");
			return;
		}

		// クリップの差し替え
		// MARK : 必要ならBGMをフェードする処理を記述する
		_bgmSource.clip = _bgmDic[name] as AudioClip;
		_bgmSource.Play();
	}

	/// <summary>
	/// BGMの停止
	/// </summary>
	public void StopBGM() {
		_bgmSource.Stop();
	}
}