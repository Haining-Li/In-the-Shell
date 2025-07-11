using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiderTest : MonoBehaviour
{
    Rigidbody2D mBody = null;
    // Start is called before the first frame update
    void Start()
    {
        mBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            mBody.AddForce(new Vector3(10, 0, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            mBody.AddForce(new Vector3(-10, 0, 0));
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Dash();
        }
    }

    public void Move()
    {
        mBody.AddForce(new Vector3(10, 0, 0));
    }

    public void Dash() {
        mBody.AddForce(new Vector3(2, 0, 0), ForceMode2D.Impulse);
    }
}
