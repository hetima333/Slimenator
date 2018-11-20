using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProperties : ScriptableObject 
{
    [BackgroundColor(0.8f, 0.4f, 0.5f, 0.3f)]
    [Header("Audio")]
    [Tooltip("Audio clips, leave empty if no audio should be played")]
    [SerializeField]
    private AudioClip
        _CastingAudio,
        _EndingAudio;

    [Tooltip("Tick if audio should loop")]
    [SerializeField]
    private bool
        _IsCastingAudioLoop,
        _IsEndingAudioLoop;

    public AudioClip GetCastingAudio()
    {
        return _CastingAudio;
    }

    public AudioClip GetEndingAudio()
    {
        return _EndingAudio;
    }

    public bool IsCastingLoop()
    {
        return _IsCastingAudioLoop;
    }

    public bool IsEndingLoop()
    {
        return _IsEndingAudioLoop;
    }
}
