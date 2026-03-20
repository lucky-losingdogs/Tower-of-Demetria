using UnityEngine;

public class SpawnCropDrop : MonoBehaviour
{
    [SerializeField] private GameObject m_cropPickUp;


    public void CropDrop()
    {
        Instantiate(m_cropPickUp, transform.position, Quaternion.identity);
    }
}
