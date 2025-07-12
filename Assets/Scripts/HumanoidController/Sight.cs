using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    // Start is called before the first frame update
    private bool mAlert = false;
    private Vector3 mPos = Vector3.zero;
    public bool isInSight
    {
        get { return mAlert; }
    }

    public Vector3 mTargetPosition
    {
        get { return mPos; }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject e = collision.gameObject;
        if (e.CompareTag("Player"))
        {
            mAlert = true;
            mPos = e.transform.localPosition;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        GameObject e = other.gameObject;
        if (e.CompareTag("Player"))
        {
            mPos = e.transform.localPosition;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        GameObject e = collision.gameObject;
        if (e.CompareTag("Player"))
        {
            mAlert = false;
            mPos = e.transform.localPosition;
        }
    }
}
