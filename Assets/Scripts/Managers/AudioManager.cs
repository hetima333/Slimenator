using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioManager : SingletonMonoBehaviour<AudioManager> {

	// Audioファイルのパス
	private const string BGM_PATH = "Audio/BGM";
	private const string SE_PATH  = "Audio/SE";

	// オーディオソース
	private AudioSource _bgmSource;
	private List<AudioSource> _seSourceList = new List<AudioSource>();

	// オーディオクリップ
	private Dictionary<string, AudioClip> _bgmDic, _seDic;

	// SEの最大数
	private const int SE_MAX_NUM = 10;


	// MARK : 呼び出しタイミングをプロパティによるアクセス時に変更するかも知れない
	void Start(){
		// オーディオリスナーをアタッチ
		gameObject.AddComponent<AudioListener>();

		// BGM用のオーディオソースを付与
		_bgmSource = gameObject.AddComponent<AudioSource>();

		// TODO : BGM用設定
		_bgmSource.loop = true;

		// SEの数だけオーディオソースを付与
		for (int i = 0; i < SE_MAX_NUM; i++){
			var source = gameObject.AddComponent<AudioSource>();
			_seSourceList.Add(source);

			// TODO : SE用設定
		}

		// リソースフォルダから全SE&BGMのファイルを読み込みセット
		_bgmDic = new Dictionary<string, AudioClip> ();
		_seDic  = new Dictionary<string, AudioClip> ();

		object[] bgmList = Resources.LoadAll (BGM_PATH);
		object[] seList  = Resources.LoadAll (SE_PATH);

		foreach (AudioClip bgm in bgmList) {
			_bgmDic[bgm.name] = bgm;
		}
		foreach (AudioClip se in seList) {
			_seDic[se.name] = se;
		}
	}

	/// <summary>
	/// SEの再生
	/// </summary>
	/// <param name="name">ファイル名</param>
	public void PlaySE(string name){		
		// SEが存在するかを調べる
		if(_seDic.ContainsKey(name) != true){
			Debug.Log(name + "：という名前のSEは存在しません");
			return;
		}

		// 再生中でないソースを探す
		var source = _seSourceList.FirstOrDefault(x => x.isPlaying != true);
		
		// 再生中でないソースがあればSEを再生する
		if(source != null){
			source.PlayOneShot(_seDic[name] as AudioClip);
		}
		else{
			Debug.Log("再生に利用できるAudioSourceがありません");
		}
	}

	/// <summary>
	/// BGMの再生
	/// </summary>
	/// <param name="name">ファイル名</param>
	public void PlayBGM(string name){
		// BGMが存在するかを調べる
		if(_bgmDic.ContainsKey(name) != true){
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
	public void StopBGM(){
		_bgmSource.Stop();
	}
}
