using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomAppear : MonoBehaviour
{
    public bool isRevealed = false;
    public DoorController linkedDoor;
    private Tilemap[] tilemaps;

    void Start()
    {
        tilemaps = GetComponentsInChildren<Tilemap>();
        SetTilemapsColor(Color.clear);
    }

    void Update()
    {
        if (!isRevealed && linkedDoor != null && linkedDoor.IsOpen)
        {
            isRevealed = true;
            StartCoroutine(RevealEffect());
        }
    }

    private IEnumerator RevealEffect()
    {
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            SetTilemapsColor(Color.Lerp(Color.clear, Color.white, t));
            yield return null;
        }

        SetTilemapsColor(Color.white);
    }

    private void SetTilemapsColor(Color color)
    {
        foreach (var tilemap in tilemaps)
        {
            tilemap.color = color;
        }
    }
}