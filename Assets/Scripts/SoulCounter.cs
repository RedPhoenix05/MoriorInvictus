using TMPro;
using UnityEngine;

public class SoulCounter : MonoBehaviour
{
    Player player;
    TextMeshPro text;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        text = GetComponentInChildren<TextMeshPro>();
    }

    private void Update()
    {
        text.text = player.souls.ToString();
    }
}
