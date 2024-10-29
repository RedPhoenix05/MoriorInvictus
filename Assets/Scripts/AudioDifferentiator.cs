using UnityEngine;

public class AudioDifferentiator : MonoBehaviour
{
    AudioListener listener;
    Player player;
    bool valid = false;

    [SerializeField] AudioSource sourceLiving;
    [SerializeField] AudioSource sourceDead;

    void Start()
    {
        listener = FindFirstObjectByType<AudioListener>();
        if (listener)
        {
            player = listener.gameObject.GetComponent<Player>();
            if (player) valid = true;
        }
    }

    private void Update()
    {
        if (valid)
        {
            if (sourceLiving) sourceLiving.mute = !player.living;
            if (sourceDead) sourceDead.mute = player.living;
        }
    }
}
