using UnityEngine;

[System.Serializable]
public class Character
{
    public Sprite characterName;
    public Sprite characterImage;
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;
    public PowerUpType powerUpType;
}

public enum PowerUpType
{
    None,
    PowerShot,
    Dash,
    Heal
}
