using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    private int m_damage = 10;

    private BoxCollider2D m_boxCollider;

    private void Awake()
    {
        GetCollider();
    }


    public override void Attack()
    {
        GetCollider();

        if (!m_boxCollider)
        {
            Debug.Log("Box collider is missing");
            return;
        }
        Slice();
    }

    private void Slice()
    {
        //get the collision overlaps in the box collision
        //and set all of the collisions in an array
        Vector2 center = m_boxCollider.bounds.center;
        Vector2 size = m_boxCollider.bounds.size;
        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f);

        foreach (Collider2D hit in hits)
        {
            if (hit.isTrigger)
                continue;

            //if the collision is an enemy, apply damage
            if (hit.gameObject.layer == LayerMask.NameToLayer("EnemyBody"))
            {
                ApplyDamage.ApplyDmg(hit.gameObject, m_damage);
            }

            //if the collision is a crop, destroy (harvest) it
            if (hit.CompareTag("Crop"))
            {
                SpawnCropDrop cropDrop = hit.gameObject.GetComponentInParent<SpawnCropDrop>();
                if (cropDrop != null)
                    cropDrop.CropDrop();

                Destroy(hit.gameObject.transform.parent.gameObject);
            }

            VFXManager.CreateSlash(center);
        }
    }

    //gets all the colliders on the player and returns the one that has IsTrigger ticked
    private BoxCollider2D GetCollider()
    {
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();

        foreach (BoxCollider2D col in colliders)
        {
            if (col.isTrigger)
            {
                m_boxCollider = col;
                return col;
            }
            else continue;
        }

        Debug.Log("Trigger BoxCollider2D is missing");
        return null;
    }
}
