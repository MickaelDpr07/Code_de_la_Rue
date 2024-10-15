using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quitter_PopUp : MonoBehaviour
{
    [SerializeField] GameObject PopUp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Quitter()
    {
        PopUp.SetActive(false);
    }
}
