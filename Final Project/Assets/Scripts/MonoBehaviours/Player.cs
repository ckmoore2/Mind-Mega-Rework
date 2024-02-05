using TMPro;
using UnityEngine;
 
public class Player : Character
{
    // UI elements
    public HealthBar healthBarPrefab;
    HealthBar healthBar;
    public TextMeshProUGUI keysText;
    public TextMeshProUGUI gameConditionText;
    public TextMeshProUGUI timerText;
 
    // Timer and health fields
    public float remainingTime = 60f; // Start with 1 minute
    public float startingTime = 60f; // Starting time for reference
 
    private float elapsedTime;
 
    // Start is called before the first frame update
    private void Start()
    {
 
        // Set initial health and timer
        hitPoints = startingHitPoints = 100; // Assuming 100 is the full health
 
        // Instantiate the health bar and set its references
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;
        healthBar.player = this;
    }
 
    // Update is called once per frame
    private void Update()
    {
        hitPoints -= 3 * Time.deltaTime;

        if (hitPoints < 0)
        {
            hitPoints = 0;
        }

        UpdateGameConditionText();

    }


        // Timer and health decrement logic
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1.0f)
        {
            elapsedTime = 0;
            if (remainingTime > 0 && hitPoints > 0)
            {
                remainingTime -= 1;
                hitPoints -= 5; // Decrease hit points by 5 every second
                if (hitPoints < 0) hitPoints = 0; // Ensure hit points don't go below 0
            }
        }
 
        // Update the timer display
        UpdateTimerDisplay();
        UpdateGameConditionText();
    }
 
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
 
    private void UpdateGameConditionText()
    {
        if (hitPoints <= 0 || remainingTime <= 0)
        {
            gameConditionText.text = "YOU DIED!! :(\nPress ESC to exit or Spacebar to replay";
            timerText.text = "0:00";
        }
        else if (keys >= 5)
        {
            gameConditionText.text = "YOU WIN!! :)\nPress ESC to exit or Spacebar to replay";
        }
    }
 
    // Example method to handle picking up a heart
    public void PickupHeart(int healthAmount, float timeAmount)
    {
        // Maximum values
        float maxTime = 100f; // 1 minute and 40 seconds
        maxHitPoints = 100;
 
        // Increase health by 10, ensuring it doesn't exceed maxHitPoints
        hitPoints = Mathf.Min(hitPoints + 10, maxHitPoints);
 
        // Increase remaining time by 10 seconds, ensuring it doesn't exceed maxTime
        // Adding Mathf.Epsilon to account for any floating point imprecision
        remainingTime = Mathf.Min(remainingTime + 10 + Mathf.Epsilon, maxTime);
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
                    case Item.ItemType.COIN:
                        // coins should disappear by default and should be added to inventory
                        shouldDisappear = true;
                        break;
 
                    case Item.ItemType.HEALTH:
                        // hearts should disappear if they adjust the player's hit points
                        // when health meter is full, hearts aren't picked up and remain in the scene
                        shouldDisappear = increaseHitPoints(hitObject.quantity);
                        if (remainingTime + 25 < 100)
                        {
                            remainingTime = remainingTime + 25;
                        }
                        else
                        {
                            remainingTime = 101;
                        }
                        break;
 
                    case Item.ItemType.KEY:
                        // Keys should disappear and then increase the amount the player has
                        //Also prints amount of keys to screen
                        shouldDisappear = increaseHitPoints(hitObject.quantity);
                        remainingTime = 101;
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

 
    //Function to call to increase hitpoints
    public bool increaseHitPoints(int amount)
    {
        // Don't increase above the max amount
        if (hitPoints < maxHitPoints)
        {
            //Check to see if hitpoints + the amount does not go over the max amount
            //If it does go over then set to max hitpoints
            if (hitPoints + amount > maxHitPoints)
            {
                hitPoints = maxHitPoints;
                return true;
            }
            else
            {
                hitPoints = hitPoints + amount;
                print("Increase hitpoints by: " + amount + ". New value: " + hitPoints);
                return true;
            }
        }
 
        // Return false if hit points is at max and can't be adjusted
        return false;
    }
 
    //Function to decrease character health
    public bool losingHitPoints(int amount)
    {
        // Don't decrease below zero
        if (hitPoints > 0)
        {
            hitPoints = hitPoints - amount;
            print("Lost hitpoints by: " + amount + ". New value: " + hitPoints);
            return true;
        }
 
        // Return false if hit points is at max and can't be adjusted
        return false;
    }
}