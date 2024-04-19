using System;
using UnityEngine;

[Serializable
    ]
public class CharacterKeybinds
{
    [SerializeField] private KeyCode moveUp = KeyCode.W;
    [SerializeField] private KeyCode moveDown = KeyCode.S;
    [SerializeField] private KeyCode moveLeft = KeyCode.A;
    [SerializeField] private KeyCode moveRight = KeyCode.D;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;

    public KeyCode MoveUp { get => moveUp; set => moveUp = value; }
    public KeyCode MoveDown { get => moveDown; set => moveDown = value; }
    public KeyCode MoveLeft { get => moveLeft; set => moveLeft = value; }
    public KeyCode MoveRight { get => moveRight; set => moveRight = value; }
    public KeyCode RunKey { get => runKey; set => runKey = value; }

    private float GetAxisWithKey(KeyCode negative, KeyCode positive)
    {
        float axis = 0.0f;
        if (Input.GetKey(negative))
        {
            axis -= 1.0f;
        }
        if (Input.GetKey(positive))
        {
            axis += 1.0f;
        }
        return axis;
    }

    public float GetHorizontalAxisInput()
    {
        return GetAxisWithKey(MoveLeft, MoveRight);
    }

    public float GetVerticalAxisInput()
    {
        return GetAxisWithKey(MoveDown, MoveUp);
    }
}
