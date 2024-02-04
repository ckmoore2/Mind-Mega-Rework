using TMPro;
using UnityEngine;
 
public class Player : Character
{
    // UI elements
    public HealthBar healthBarPrefab;
    HealthBar healthBar;
    public TextMeshProUGUI keysText;
    public TextMeshProUGUI gameConditionText;

    // Initialize variables
    public int heartHPBoost = 10;
    public int keyHPBoost = 10;

    // Start is called before the first frame update
    public void Start()
    {
        // Set initial health
        hitPoints = startingHitPoints;

        // Instantiate the health bar and set its references
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;
        healthBar.player = this;
    }
 
    // Update is called once per frame
    private void Update()
    {
        hitPoints -= Time.deltaTime;
        if (hitPoints < 0)
        {
            hitPoints = 0;
        }

        UpdateGameConditionText();
    }
 
    private void UpdateGameConditionText()
    {
        if (hitPoints <= 0)
        {
            gameConditionText.text = "YOU DIED!! :(\nPress ESC to exit or Spacebar to replay";
        }
        else if (keys >= 5)
        {
            gameConditionText.text = "YOU WIN!! :)\nPress ESC to exit or Spacebar to replay";
        }
    }
 
    // Example method to handle picking up a heart
    public void IncreaseHealth(int healthAmount)
    {
        // Increase health by 10, ensuring it doesn't exceed maxHitPoints
        hitPoints += healthAmount;
    }
 
    // Called when player's collider touches an "Is Trigger" collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Retrieve the game object that the player collided with, and check the tag
        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {
            // Grab a reference to the Item (scriptable object) inside the Consumable class and assign it to hitObject
            // Note: at this point it is a coin, but later may be other types of CanBePickedUp objects
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;
 
            // Check for null to make sure it was successfully retrieved, and avoid potential errors
            if (hitObject != null)
            {
                // debugging
                print("it: " + hitObject.objectName);
 
                // indicates if the collision object should disappear
                bool shouldDisappear = false;
 
                switch (hitObject.itemType)
                {
                    case Item.ItemType.HEALTH:
                        // hearts should disappear if they adjust the player's hit points
                        shouldDisappear = true;
                        IncreaseHealth(heartHPBoost);
                        break;
 
                    case Item.ItemType.KEY:
                        // Keys should disappear and then increase the amount the player has
                        // Also prints amount of keys to screen
                        shouldDisappear = true;
                        IncreaseHealth(keyHPBoost);

                        if (shouldDisappear)
                        {
                            keys++;
                            keysText.text = "Keys: " + keys + "/5";
                        }
                        break;
 
                    default:
                        break;
                }
 
                // Hide the game object in the scene to give the illusion of picking up
                if (shouldDisappear)
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }
}