using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackEvents : MonoBehaviour
{
    public GameObject attackHitbox; 

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();

        DisableDamage();
    }

    public void EnableDamage()
    {
        if (spriteRenderer != null && attackHitbox != null)
        {
            Vector3 hitboxScale = attackHitbox.transform.localScale;
            if (spriteRenderer.flipX)
            {
                hitboxScale.x = -1f;
            }
            else
            {
                hitboxScale.x = 1f;
            }
            attackHitbox.transform.localScale = hitboxScale;

            attackHitbox.SetActive(true);
        }
    }

    public void DisableDamage()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }
}