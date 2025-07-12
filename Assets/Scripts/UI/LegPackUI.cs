using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LegPackUI : MonoBehaviour
{
    [SerializeField] HeroCollide Hero;
    private Image imageComponent;
    // Start is called before the first frame update
    void Start()
    {
        Hero = GameObject.Find("Hero").GetComponent<HeroCollide>();
        imageComponent = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Hero != null && Hero.getLegPack)
        {
            imageComponent.color = Color.white;
        }
    }
}
