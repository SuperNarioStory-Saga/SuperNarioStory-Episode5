using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScript : MonoBehaviour
{
    public Sprite end1;
    public Sprite end2;
    public Sprite end3;
    public Sprite keySprite;

    public GameObject endWrapper;
    public GameObject keyWrapper;

    private SpriteRenderer keyRend;
    private SpriteRenderer endRend;
    private Text keyText;
    private Text endText;
    void Start()
    {
        keyRend = keyWrapper.transform.GetChild(0).GetComponent<SpriteRenderer>();
        endRend = endWrapper.transform.GetChild(0).GetComponent<SpriteRenderer>();

        keyText = keyWrapper.transform.GetChild(1).GetComponent<Text>();
        endText = endWrapper.transform.GetChild(1).GetComponent<Text>();

        if (GlobalScript.keyOwned)
        {
            keyRend.sprite = keySprite;
        }
        keyText.text = GlobalScript.keyOwned ? "Une clé !" : "Une clé ?";
        switch (GlobalScript.choice)
        {
            case 1:
                endText.text = "Tout cela n'était qu'un jeu depuis le début !";
                endRend.sprite = end1;
                break;
            case 2:
                endText.text = "Tu es parti avec la princesse";
                endRend.sprite = end2;
                break;
            case 3:
                endText.text = "Tu as rencontré le professeur";
                endRend.sprite = end3;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
