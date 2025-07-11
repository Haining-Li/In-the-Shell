using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPackage : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Delete()
    {
        animator.SetTrigger("IsDead");
        Destroy(gameObject);
    }
}
