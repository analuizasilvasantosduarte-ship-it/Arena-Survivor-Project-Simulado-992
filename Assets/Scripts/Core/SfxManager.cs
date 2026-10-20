using System.Collections.Generic;
using UnityEngine;

namespace ArenaSurvivor.Core
{
    public class SfxManager : MonoBehaviour
    {
        public static SfxManager Instance { get; private set; }

        readonly Dictionary<string, AudioClip> _clips = new Dictionary<string, AudioClip>();
        AudioSource _source;

        public static SfxManager Create(Transform parent)
        {
            var go = new GameObject("SfxManager");
            go.transform.SetParent(parent, false);
            return go.AddComponent<SfxManager>();
        }

        void Awake()
        {
            Instance = this;
            _source = gameObject.AddComponent<AudioSource>();
            _source.playOnAwake = false;

            _clips["shoot"] = MakeTone(880f, 0.08f, 0.15f);
            _clips["hit"] = MakeTone(320f, 0.1f, 0.2f);
            _clips["enemyDie"] = MakeTone(180f, 0.18f, 0.25f);
            _clips["playerHurt"] = MakeTone(140f, 0.2f, 0.3f);
            _clips["victory"] = MakeArpeggio(new float[] { 523f, 659f, 784f, 1047f });
            _clips["gameOver"] = MakeArpeggio(new float[] { 400f, 320f, 240f, 160f });
            _clips["click"] = MakeTone(660f, 0.06f, 0.12f);
            _clips["wave"] = MakeTone(520f, 0.15f, 0.2f);
        }

        public void Play(string key)
        {
            if (_source != null && _clips.TryGetValue(key, out var clip))
                _source.PlayOneShot(clip);
        }

        AudioClip MakeTone(float frequency, float duration, float volume)
        {
            const int sampleRate = 44100;
            var samples = new float[(int)(sampleRate * duration)];
            for (var i = 0; i < samples.Length; i++)
            {
                var t = (float)i / sampleRate;
                var envelope = 1f - (float)i / samples.Length;
                samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * t) * volume * envelope;
            }

            var clip = AudioClip.Create("tone_" + frequency, samples.Length, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        AudioClip MakeArpeggio(float[] notes)
        {
            const int sampleRate = 44100;
            var noteDuration = 0.12f;
            var samples = new float[(int)(sampleRate * noteDuration * notes.Length)];
            var index = 0;
            foreach (var note in notes)
            {
                var count = (int)(sampleRate * noteDuration);
                for (var i = 0; i < count && index < samples.Length; i++, index++)
                {
                    var t = (float)i / sampleRate;
                    var envelope = 1f - (float)i / count;
                    samples[index] = Mathf.Sin(2f * Mathf.PI * note * t) * 0.2f * envelope;
                }
            }

            var clip = AudioClip.Create("arp", samples.Length, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }
    }
}
