using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemControler : MonoBehaviour
{
    public Direction direction;
    public Image imgDirec; 
    public Sprite left,right,up,down;
    public bool isSelect;


    public void Init()
    {
        imgDirec = GetComponent<Image>();
        RandomDirection();
        switch (direction)
        {
            case Direction.Left:
                imgDirec.sprite = left;
                break;
            case Direction.Right:
                imgDirec.sprite = right;
                break;
            case Direction.Up:
                imgDirec.sprite = up;
                 break;
            case Direction.Down:
                imgDirec.sprite = down;
                 break;
            default:
                break;
        }

        isSelect = false;
    }
    public void OnSelectItem()
    {
        if (!isSelect)
        {
            GameControler.Instance.ChooseItemTarget();
            imgDirec.color = Color.red;
            isSelect = true;
            Debug.Log(direction.ToString());
        }
       
    }
    public void ResetItem()
    {
        imgDirec.color = Color.white;
        isSelect = false;
    }
    public Direction RandomDirection()
    {
        return direction = (Direction)Random.Range(0, 3);
    }
}
