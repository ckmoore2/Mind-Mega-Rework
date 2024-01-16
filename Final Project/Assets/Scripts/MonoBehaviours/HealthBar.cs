using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    // Reference to the current Player object to get hit point fields
    // Will be set programmatically, instead of through the Unity Editor, so it is hidden in the Inspector window
    [HideInInspector]
    public Player character;

    // For convenience, a direct reference to the health bar meter; set through the Unity Editor
    public Image meterImage;

    // For convenience, a direct reference to the text in the health bar and timer; set through the Unity Editor
    public TextMeshProUGUI hpText;

    // Update is called once per frame
    void Update()
    {
       
        if (character.hitPoints > 0 && character.keys < 5)
        {
            // set the meter's fill amount; must be a value between 0 and 1
            meterImage.fillAmount = character.hitPoints / character.maxHitPoints;

            // modify the text
            hpText.text = "HP:" + (meterImage.fillAmount * 100);

        }
        if (character.hitPoints <= 0)
        {
            //To make sure HP Text and HP bar matches when character dies
            hpText.text = "HP: 0";
            meterImage.fillAmount = 0;
        }
    }
}
