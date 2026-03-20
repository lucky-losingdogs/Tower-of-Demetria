using UnityEngine;

public class BeeSpecialAttack : MonoBehaviour
{
    private CropController m_cropController;

    private void OnTriggerStay2D(Collider2D collision)
    {
        //so it's not colliding with the crop's trigger collider, and only its actual body
        if (collision.isTrigger)
            return;

        //if the thing that's colliding has the tag "Crop"
        if (collision.CompareTag("Crop"))
        {
            m_cropController = GetCropComponent(collision);
            ChangeCropActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //so it's not colliding with the crop's trigger collider, and only its actual body
        if (collision.isTrigger)
            return;

        //if the thing that's colliding has the tag "Crop"
        if (collision.CompareTag("Crop"))
        {
            m_cropController = GetCropComponent(collision);
            ChangeCropActive(true);
        }  
    }

    //sets the crop and crop controller variables
    private CropController GetCropComponent(Collider2D collision)
    {
        return collision.GetComponentInParent<CropController>(); 
    }

    private void ChangeCropActive(bool enable)
    {
        if (m_cropController == null)
            return;

        //if the controller script is active/inactive
        //change it to whatever the caller has passed as the bool
        m_cropController.enabled = enable;
    }
}
