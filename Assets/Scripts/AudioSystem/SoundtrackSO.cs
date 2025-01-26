using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[CreateAssetMenu(fileName = "Soundtrack", menuName = "AudioSystem/Soundtrack List")]
public class SoundtrackSO : ScriptableObject
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private List<SoundEntry> _sounds;

    public AudioClip GetClipByName(string name)
    {
        foreach (var sound in _sounds)
        {
            if (sound.name == name)
            {
                return sound.clips[Random.Range(0, sound.clips.Count)] ;
            }
        }
        return null;
    }

    public AudioMixerGroup GetClipMixerGroup(AudioClip clip)
    {
        foreach (var sound in _sounds)
        {
            if (sound.clips.Contains(clip))
            {
                return _mixer.FindMatchingGroups(sound.mixerGroup)[0];
            }
        }
        return null;
    }
}

[System.Serializable]
public class SoundEntry
{
    public string name;
    public List<AudioClip> clips;
    public string mixerGroup;
}