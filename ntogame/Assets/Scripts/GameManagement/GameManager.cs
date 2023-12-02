using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public static bool KeyObtained = false;
    public static int MaxHealth = 5;
    public static int Health = MaxHealth;
    public static bool InputAllowed = true;
    public static bool IsDead = false;

    public static void ResetVariables()
    {
        Health = MaxHealth;
        InputAllowed = true;
        KeyObtained = false;
        IsDead = false;
    }
}
