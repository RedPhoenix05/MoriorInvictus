using System.Data.Common;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AltarAudio : MonoBehaviour
{
    AudioListener listener;
    Player player;
    bool valid = false;
    AudioSource source;

    void Start()
    {
        listener = FindFirstObjectByType<AudioListener>();
        if (listener)
        {
            player = listener.gameObject.GetComponent<Player>();
            if (player) valid = true;
        }

        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (valid)
        {
            if (!player.living)
            {
                if (dist <= source.maxDistance)
                {
                    float width = source.maxDistance - source.minDistance;
                    source.volume = Mathf.Lerp(0.5f, 0.0f, (dist - source.minDistance) / width);
                    if (!source.isPlaying) source.Play();
                }
                else source.Stop();
            }
            else if (source.isPlaying) source.Stop();
        }
    }
}
