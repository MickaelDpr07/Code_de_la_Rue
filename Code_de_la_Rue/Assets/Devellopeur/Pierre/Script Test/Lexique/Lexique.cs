using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lexique : MonoBehaviour
{
    public GameObject PanelDescription;
    public GameObject PanelLexique;
    public GameObject PanelButton;

    public TextMeshProUGUI titre;
    public TextMeshProUGUI contenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenText(string description)
    {
        PanelDescription.SetActive(true);
        contenu.text = description;

    }

    public void Retour()
    {
        PanelDescription.SetActive(false);
    }

    public void QuitterLexique()
    {
        PanelButton.SetActive(true);
        PanelLexique.SetActive(false);
        
    }
}
