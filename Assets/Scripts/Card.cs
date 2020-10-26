using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    #region Variables

    public UnityEvent action;

    public Sprite frontSprite;
    public Sprite backSprite;

    public new string name;
    public string description;

    #endregion
}
