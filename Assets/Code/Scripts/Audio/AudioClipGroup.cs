﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ShootingRangeGame.Audio
{
    [Serializable]
    public class AudioClipGroup
    {
        [SerializeField] private List<AudioClip> list;
        [SerializeField] private Mode mode;
        [SerializeField] private Vector2 volumeRange = Vector2.one;
        [SerializeField] private Vector2 pitchRange = Vector2.one;

        private int index;

        private static AudioListener listener;

        public void Play(Action<AudioClip> callback)
        {
            switch (mode)
            {
                default:
                case Mode.First:
                    callback(list[0]);
                    break;
                case Mode.Sequential:
                    PlaySequential(callback);
                    break;
                case Mode.Random:
                    PlayRandom(callback);
                    break;
            }

            index++;
        }

        public void Play(AudioSource source) => Play(clipEntry =>
        {
            float random(Vector2 range) => Random.Range(range.x, range.y);

            source.volume = random(volumeRange);
            source.pitch = random(pitchRange);
            source.PlayOneShot(clipEntry);
        });

        public void Play(Vector3 position) => Play(clipEntry =>
        {
            var source = new GameObject("[TEMP] Audio Source").AddComponent<AudioSource>();
            source.transform.position = position;
            Object.Destroy(source.gameObject, clipEntry.length + 0.5f);

            Play(source);
        });

        public void Play(AudioSource source, Vector3 position)
        {
            if (source) Play(source);
            else Play(position);
        }

        public void Play()
        {
            if (!listener)
            {
                listener = Object.FindObjectOfType<AudioListener>();
                if (!listener) return;
            }

            Play(listener.transform.position);
        }

        private void PlaySequential(Action<AudioClip> callback)
        {
            callback(list[index++ % list.Count]);
        }

        private void PlayRandom(Action<AudioClip> callback)
        {
            callback(list[Random.Range(0, list.Count)]);
        }

        public enum Mode
        {
            First,
            Random,
            Sequential,
        }
    }
}