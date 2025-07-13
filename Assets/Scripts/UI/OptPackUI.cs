using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptPackUI : MonoBehaviour
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
        if(Hero != null && Hero.getOptPack) 
        {
            imageComponent.color = Color.white;
        }
    }
}
