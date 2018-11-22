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
	public AudioSource PlaySE(string name, bool loop = false, float volume = 1.0f) {
		// SEが存在するかを調べる
		if (_seDic.ContainsKey(name) != true) {
			Debug.Log(name + "：という名前のSEは存在しません");
			return null;
		}

		// 再生中でないソースを探す
		var source = _seSourceList.FirstOrDefault(x => x.isPlaying != true);
		source.loop = loop;

		// 再生中でないソースがあればSEを再生する
		if (source != null) {
			if (source.loop) {
				source.Play(_seDic[name], volume);
			} else {
				source.PlayOneShot(_seDic[name], volume);
			}

			return source;
		} else {
			Debug.Log("再生に利用できるAudioSourceがありません");
		}

		return null;
	}

	/// <summary>
	/// SEの再生及びフェードイン
	/// </summary>
	/// <param name="name">se name</param>
	/// <param name="fadeTime">fade time</param>
	/// <param name="startVolume">フェード開始時の音量</param>
	/// <param name="endVolume">フェード終了時の音量</param>
	public void PlaySEWithFadeIn(string name, float fadeTime, bool loop = false, float startVolume = 0.0f, float endVolume = 1.0f) {
		StartCoroutine(PlaySE(name, loop, startVolume).FadeIn(fadeTime, endVolume));
	}

	/// <summary>
	/// SEの停止
	/// </summary>
	/// <param name="name">ファイル名</param>
	public void StopSE(string name) {
		var source = FindSESource(name);
		if (source == null) {
			return;
		}
		// 念のためループ解除
		source.loop = false;
		source.Stop();
		source.clip = null;
	}

	/// <summary>
	/// SEのループ停止
	/// </summary>
	/// <param name="name">ファイル名</param>
	public void StopSELoop(string name) {
		var source = FindSESource(name);
		if (source == null) {
			return;
		}
		source.loop = false;
	}

	/// <summary>
	/// SEのフェードイン
	/// </summary>
	/// <param name="name">se name</param>
	/// <param name="fadeTime">fade time</param>
	public void FadeInSE(string name, float fadeTime, float endVolume = 1.0f) {
		var source = FindSESource(name);
		if (source == null) {
			return;
		}
		StartCoroutine(source.FadeIn(fadeTime, endVolume));
	}

	/// <summary>
	/// SEのフェードアウト
	/// </summary>
	/// <param name="name">se name</param>
	/// <param name="fadeTime">fade time</param>
	public void FadeOutSE(string name, float fadeTime, float endVolume = 0.0f) {
		var source = FindSESource(name);
		if (source == null) {
			return;
		}
		StartCoroutine(source.FadeOut(fadeTime, endVolume));
	}

	/// <summary>
	/// SEの停止とフェードアウト
	/// </summary>
	/// <param name="name">se name</param>
	/// <param name="fadeTime">fade time</param>
	public void StopSEWithFadeOut(string name, float fadeTime, float endVolume = 0.0f) {
		var source = FindSESource(name);
		if (source == null) {
			return;
		}

		StartCoroutine(source.FadeOut(fadeTime, endVolume, true));
	}

	/// <summary>
	/// BGMの再生
	/// </summary>
	/// <param name="name">ファイル名</param>
	public AudioSource PlayBGM(string name, float volume = 1.0f) {
		// BGMが存在するかを調べる
		if (_bgmDic.ContainsKey(name) != true) {
			Debug.Log(name + "：という名前のSEは存在しません");
			return null;
		}

		// クリップの差し替え
		// MARK : 必要ならBGMをフェードする処理を記述する
		_bgmSource.Play(_bgmDic[name], volume);

		return _bgmSource;
	}

	/// <summary>
	/// BGMの再生及びフェードイン
	/// </summary>
	/// <param name="name">bgm name</param>
	/// <param name="fadeTime">fade time</param>
	/// <param name="startVolume">フェード開始時の音量</param>
	/// <param name="endVolume">フェード終了時の音量</param>
	public void PlayBGMWithFadeIn(string name, float fadeTime, float startVolume = 0.0f, float endVolume = 1.0f) {
		StartCoroutine(PlayBGM(name, startVolume).FadeIn(fadeTime, endVolume));
	}

	/// <summary>
	/// BGMの停止
	/// </summary>
	public void StopBGM() {
		_bgmSource.Stop();
	}

	/// <summary>
	/// BGMのフェードイン
	/// </summary>
	/// <param name="fadeTime">fade time</param>
	public void FadeInBGM(float fadeTime, float endVolume = 1.0f) {
		StartCoroutine(_bgmSource.FadeIn(fadeTime, endVolume));
	}

	/// <summary>
	/// BGMのフェードアウト
	/// </summary>
	/// <param name="fadeTime">fade time</param>
	public void FadeOutBGM(float fadeTime, float endVolume = 0.0f) {
		StartCoroutine(_bgmSource.FadeOut(fadeTime, endVolume));
	}

	/// <summary>
	/// BGMの停止とフェードアウト
	/// </summary>
	/// <param name="fadeTime"></param>
	/// <param name="endVolume"></param>
	public void StopBGMWithFadeOut(float fadeTime, float endVolume = 0.0f) {
		StartCoroutine(_bgmSource.FadeOut(fadeTime, endVolume, true));
	}

	/// <summary>
	/// 名前からSEのオーディオソースを探す
	/// </summary>
	/// <param name="name">se name</param>
	/// <returns></returns>
	private AudioSource FindSESource(string name) {
		return _seSourceList.Where(x => x.clip != null).FirstOrDefault(x => name == x.clip.name);
	}
}