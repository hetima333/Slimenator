using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : SingletonMonoBehaviour<FadeManager> {

    // フェードに使うための透明度
    private float _fadeAlpha = 0.0f;

    [SerializeField]
    private Image _panelImage;

    enum FadeState
    {
        In = 0,
        Out,
        Finished
    };

    private FadeState _fadeState = FadeState.Out;

    private void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        UpdatePanelAlpha();
    }

    // Update is called once per frame
    void Update () {
        if (_fadeState == FadeState.Finished)
            return;

        UpdatePanelAlpha();
    }

    private void UpdatePanelAlpha()
    {
        Color tmpColor = _panelImage.color;
        tmpColor.a = _fadeAlpha;
        _panelImage.color = tmpColor;
    }

    // シーン変更を行わない場合は scneNameに""(空文字列)を指定
    public void StartTransition(float duration, string sceneName)
    {
        StartCoroutine(TransScene(duration, sceneName));
    }

    // シーン遷移用コルーチン
    IEnumerator TransScene(float duration, string sceneName)
    {
        float time = 0.0f;

        _fadeState = FadeState.Out;
        // 透明度を減少させる
        while (time < duration)
        {
            _fadeAlpha = Mathf.Lerp(0, 1, time / duration);

            time += Time.deltaTime;

            yield return 0;
        }


        // シーンのロード
        if(sceneName != "")
            SceneManager.LoadScene(sceneName);

        _fadeState = FadeState.In;
        time = 0.0f;
        // 透明度を増加させる
        while (time < duration)
        {
            _fadeAlpha = Mathf.Lerp(1, 0, time / duration);

            time += Time.deltaTime;

            yield return 0;
        }
        _fadeState = FadeState.Finished;

    }

    override protected bool CheckInstance() 
    {
        if (instance == null)
        {
            instance = this;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }

        Destroy(this.gameObject);
        return false;
    }
}
