using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePackBehavior : MonoBehaviour
{
    AudioController audioController;

    // Start is called before the first frame update
    void Start()
    {
        audioController = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Delete()
    {
        audioController.PlaySfx(audioController.Item);
        Destroy(gameObject);
    }
}
