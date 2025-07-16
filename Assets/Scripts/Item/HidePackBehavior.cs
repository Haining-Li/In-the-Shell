using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePackBehavior : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Delete()
    {
        AudioController.Instance.PlaySfx(AudioController.Instance.Item);
        Destroy(gameObject);
    }
}
