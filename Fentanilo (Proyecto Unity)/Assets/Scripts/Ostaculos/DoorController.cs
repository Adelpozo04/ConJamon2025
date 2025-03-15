using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class DoorController : Activable
{
    [SerializeField] private SpriteRenderer firstHalf;
    [SerializeField] private SpriteRenderer secondHalf;

    [SerializeField] private Sprite openSpriteFirstHalf;
    [SerializeField] private Sprite openSpriteSecondHalf;
    [SerializeField] private Sprite closeSpriteFirstHalf;
    [SerializeField] private Sprite closeSpriteSecondHalf;

    private Collider2D _coll;

    private void Start()
    {
        _coll = GetComponent<Collider2D>();
    }

    public override void Activar(bool state)
    {
        _coll.enabled = state;
        firstHalf.sprite = state ? openSpriteFirstHalf : closeSpriteFirstHalf;
        secondHalf.sprite = state ? openSpriteSecondHalf : closeSpriteSecondHalf;
    }
}