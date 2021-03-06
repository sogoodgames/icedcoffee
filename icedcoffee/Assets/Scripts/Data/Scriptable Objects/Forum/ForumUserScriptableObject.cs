﻿using UnityEngine;

[CreateAssetMenu(fileName = "ForumUserData", menuName = "IcedCoffee/ScriptableObjects/ForumUser", order = 1)]
public class ForumUserScriptableObject : ScriptableObject 
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public string Username; // poster username
    public Friend UserID;
    public Sprite Icon; // icon file
}
