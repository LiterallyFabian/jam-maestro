using UnityEngine;

namespace JamMeistro.Game
{
    public class AudioManager
    {

        /// <summary>
        /// Plays a random AudioClip from an array
        /// </summary>
        /// <param name="clips">The clip pool to play from.</param>
        /// <param name="randomizePitch">Whether to randomize the pitch of the clip between 0.8-1.2.</param>
        public static AudioSource PlayAudio(AudioClip[] clips, bool randomizePitch = false)
        {
            if (clips.Length == 0)
                return null;
            return PlayAudio(clips[Random.Range(0, clips.Length)], randomizePitch);
        }

        public static AudioSource PlayAudio(AudioClip clip, bool randomizePitch = false)
        {
            if (clip == null) return null;
            GameObject obj = new GameObject();
            AudioSource source = obj.AddComponent<AudioSource>();

            if (randomizePitch)
                source.pitch = Random.Range(0.8f, 1.2f);

            source.clip = clip;
            source.Play();

            Object.Destroy(obj, clip.length + 0.5f);
            return source;
        }
    }
}