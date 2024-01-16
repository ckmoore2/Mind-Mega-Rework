using TMPro;
using UnityEngine;

public class Player : Character
{
    // Used to get a reference to the prefab
    public HealthBar healthBarPrefab;

    // A copy of the health bar prefab
    HealthBar healthBar;

    //Text to screen
    public TextMeshProUGUI keysText;
    public TextMeshProUGUI gameConditionText;
    public TextMeshProUGUI timerText;

    // Number to keep track of how much time is left
    [SerializeField] public float remainingTime;
    public float elapsedTime;
    public float timeSS;

    // Start is called before the first frame update
    private void Start()
    {
        // Start the player off with the starting hit point value
        hitPoints = startingHitPoints;

        // Get a copy of the health bar prefab and store a reference to it
        healthBar = Instantiate(healthBarPrefab);

        // Set the healthBar's character property to this character so it can retrieve hit points & max hit points
        healthBar.character = this;
    }

    // Update is called once per frame
    private void Update()
    {
        //Timer for decreasing health
        if (hitPoints > 0 && keys < 5)
        {
            timeSS = Time.deltaTime;
            remainingTime -= timeSS;
            elapsedTime += timeSS;
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
            elapsedTime = 100;
        }

        //Print the time remaining to screen
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Decrease hit points every second
        if (elapsedTime >= 1.0f)
        {
            losingHitPoints(1);
            elapsedTime = 0; // Reset the timer
        }
        if (hitPoints <= 0)
        {
            gameConditionText.text = "YOU DIED!! :(\n" + "Press ESC to exit or Spacebar to replay";
            timerText.text = "0:00";
        }
        if (keys >= 5)
        {
            gameConditionText.text = "YOU WIN!! :)\n" + "Press ESC to exit or Spacebar to replay";
        }

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
