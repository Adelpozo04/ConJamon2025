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

    public bool startClosed = true;
    
    private Collider2D _coll;
    public bool flag;
    private void Start()
    {
        _coll = GetComponent<Collider2D>();
        flag = startClosed;

        _coll.enabled = flag;
        firstHalf.sprite = flag ? closeSpriteFirstHalf : openSpriteFirstHalf;
        secondHalf.sprite = flag ? closeSpriteSecondHalf : openSpriteSecondHalf;
    }

    public override void Activar(bool state)
    {
        flag = !flag;
        _coll.enabled = flag;
        firstHalf.sprite = flag ? closeSpriteFirstHalf : openSpriteFirstHalf;
        secondHalf.sprite = flag ? closeSpriteSecondHalf : openSpriteSecondHalf;
    }
}