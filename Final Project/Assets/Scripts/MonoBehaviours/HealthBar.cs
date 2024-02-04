//[6:17 PM] Phillips, Evan Joseph
using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;

using TMPro;
 
public class HealthBar : MonoBehaviour

{

    [HideInInspector]

    public Player character; // Reference to the Player object
 
    public Image meterImage; // Reference to the health bar meter

    public TextMeshProUGUI hpText; // Reference to the text in the health bar

    public Player player; // Additional reference to the player for timer access
 
    void Update()

    {

        // Ensure the player and character references are set

        if (player == null || character == null)

        {

            Debug.LogError("Player or Character reference not set in HealthBar");

            return;

        }
 
        if (player.hitPoints > 0 && player.keys < 5)

        {

            // Calculate health percentage and time percentage

            float displayPercentage = Mathf.Clamp(Mathf.Min(player.hitPoints, 100) / 100, 0f, 1f);

            // Set the health bar's fill amount

            meterImage.fillAmount = displayPercentage;
 
            // Modify the text to show the current HP value

            if (player.hitPoints > 100) {
                hpText.text = "HP: 100 (+" + (Mathf.FloorToInt(player.hitPoints) - 100) + ")";
            } else
            {
                hpText.text = "HP: " + Mathf.FloorToInt(player.hitPoints);
            }

        }
 
        if (player.hitPoints <= 0)

        {

            // Match HP Text and HP bar when character dies

            hpText.text = "HP: 0";

            meterImage.fillAmount = 0;

        }

    }

}
