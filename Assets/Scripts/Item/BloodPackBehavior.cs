using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPackage : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    AudioController audioController;
    void Start()
    {
        animator = GetComponent<Animator>();
        audioController = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Delete()
    {
        audioController.PlaySfx(audioController.BloodPack);
        animator.SetTrigger("IsDead");
        Destroy(gameObject);
    }
}
