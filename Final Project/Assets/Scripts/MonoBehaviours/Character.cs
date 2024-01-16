using UnityEngine;

// Make the class abstract as it will need to be inherited by a subclass
public abstract class Character : MonoBehaviour
{
    // Properties
    public float hitPoints;
    public float maxHitPoints;
    public float startingHitPoints;
    public int keys;

    public enum CharacterCategory
    {
        PLAYER
    }

    public CharacterCategory characterCategory;
}
