using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotIcon : MonoBehaviour
{
    public enum IconType { NONE, COIN, MOVE_FORWARD, MOVE_BACKWARDS, ROTATE }

    public IconType type {  get; private set; }

    private Vector2 rotationDirection;


    public void RandomizeIconType()
    {
        type = (IconType)Random.Range(1, (int)IconType.ROTATE + 1);

        if (type == IconType.ROTATE)
        {
            rotationDirection.x = Random.Range(-1, 1);
            rotationDirection.y = Random.Range(-1, 1);

            transform.right = rotationDirection;
        }
    }

    public Vector2 GetRotationDirection()
    {
        return rotationDirection;
    }
}
