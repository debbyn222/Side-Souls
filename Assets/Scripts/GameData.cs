using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// The GameData class is a centralized container for game related data and resources.
/// This class allows you to store and manage references to various assets, such as animation clips,
/// which can be accessed by other scripts. By using this class, you can avoid having multiple 
/// public serialized fields scattered across different scripts.
[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public AnimationClip rollAnimationClip;
    public PhysicsMaterial2D noFrictionMaterial;
    [NonSerialized] public Player player;
    [NonSerialized] public BoxCollider2D playerFeetCollider;
    [NonSerialized] public BoxCollider2D playerBodyCollider;

    public void Initialize()
    {
        player = FindAnyObjectByType<Player>();
        playerFeetCollider = player.transform.Find("FeetCollider").GetComponent<BoxCollider2D>();
        playerBodyCollider = player.transform.Find("BodyCollider").GetComponent<BoxCollider2D>();
    }
}