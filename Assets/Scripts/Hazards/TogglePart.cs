using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePart : MonoBehaviour
{
    [SerializeField]
    private Sprite onSprite;
    [SerializeField]
    private Sprite offSprite;

    private SpriteRenderer sR;

    private void Awake()
    {
        sR = GetComponent<SpriteRenderer>();
        if (sR)
        {
            if (onSprite)
            {
                sR.sprite = onSprite;
            }
            else
            {
                onSprite = sR.sprite;
            }
        }
    }

    public void SetState(bool state)
    {
        if (sR)
        {
            if (state)
            {
                sR.sprite = onSprite;
                sR.enabled = true;
            }
            else
            {
                if (offSprite)
                {
                    sR.sprite = offSprite;
                }
                else
                {
                    sR.enabled = false;
                }
            }
        }
    }
}
