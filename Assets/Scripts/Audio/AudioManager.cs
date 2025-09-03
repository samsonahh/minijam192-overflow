using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioClip bgm;
    [SerializeField] private AudioClip oceanWaves;
    private const string VolumeParameter = "MasterVolume";

    private const string OneShotNamePrefix = "OneShot";
    private const string LoopingNamePrefix = "Looping";

    private void Start()
    {
        PlayLooping(bgm, null, null, 0.3f);
        PlayLooping(oceanWaves, null, null, 0.6f);
    }

    public void PlayOneShot(AudioClip clip, Vector3? position = null, float volume = 1f, float pitch = 1f)
    {
        if (clip == null)
            return;

        AudioSource source = InstantiateSource($"{OneShotNamePrefix}_{clip.name}", position != null);
        source.transform.position = position ?? Vector3.zero;

        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();

        Destroy(source.gameObject, clip.length / pitch);
    }

    public void PlayLooping(AudioClip clip, Transform parent = null, float? duration = null, float volume = 1f, float pitch = 1f)
    {
        if(clip == null)
            return;

        AudioSource source = InstantiateSource($"{LoopingNamePrefix}_{clip.name}", parent != null);
        if (parent != null)
            source.transform.SetParent(parent);
        else
            source.transform.SetParent(transform);

        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = true;
        source.Play();

        if(duration != null)
            Destroy(source.gameObject, duration.Value);
    }

    public AudioSource InstantiateSource(string name, bool use3D)
    {
        GameObject newSourceObject = new GameObject(name);
        AudioSource source = newSourceObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[0];

        if (use3D)
        {
            source.spatialBlend = 1f; // 3d sound
            source.minDistance = 5f;  // Set minimum distance for 3D audio
        }

        return source;
    }

    public float GetMasterVolume() => Utils.ConvertDecibelsToVolume(audioMixer.GetFloat(VolumeParameter, out float value) ? value : 0f);
    public void SetMasterVolume(float volume) => audioMixer.SetFloat(VolumeParameter, Utils.ConvertVolumeToDecibels(volume));
}
