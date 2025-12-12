using System;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    public int nbExp;
    private Singleton singleton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        singleton = Singleton.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "expPicker")
        {
            singleton.AddXP(nbExp);
            Destroy(gameObject);
        }
    }
}
