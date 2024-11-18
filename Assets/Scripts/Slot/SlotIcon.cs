using UnityEngine;

public class SlotIcon : MonoBehaviour
{
    public enum IconType { NONE, COIN, MOVE_FORWARD, MOVE_BACKWARDS, ROTATE }

    public IconType type {  get; private set; }

    private Vector2Int rotationDirection;

  

    public void RandomizeIconType(int _x, int _y)
    {
        int randomLastNum = 
            _x <= 0 || _x >= GameManager.Instance.slotWidth - 1
            ? (int)IconType.MOVE_FORWARD + 1
            : (int)IconType.ROTATE + 1;

        type = (IconType)Random.Range(1, randomLastNum);

        if (type == IconType.ROTATE)
        {
            int randomX = Random.Range(0, 2) == 0 ? -1 : 1;
            int randomY = Random.Range(0, 2) == 0 ? -1 : 1;

            if(_y >= GameManager.Instance.slotHeight - 1 && randomY == 1)
                randomY = -1;
            else if (_y <= 0 && randomY == -1)
                randomY = 1;
            rotationDirection.x = randomX;
            rotationDirection.y = randomY;

            transform.right = new Vector3(rotationDirection.x, -rotationDirection.y);
        }
        else
            transform.right = transform.parent.right;
    }

    public Vector2Int GetRotationDirection()
    {
        return rotationDirection;
    }
}
