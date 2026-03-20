using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager s_instance;
    private void Awake()
    {
        //Set the instance to thisif it's null
        if (s_instance == null)
            s_instance = this;
    }

    [SerializeField] private GameObject m_explosionPrefab;
    [SerializeField] private GameObject m_explosionPrefab02;
    [SerializeField] private GameObject m_slashPrefab;

    public static GameObject CreateExplosion(Vector3 position, float destroyAfter = 0.5f)
    {
        //instance not set error check
        if (s_instance == null)
        {
            Debug.LogError("Tried to spawn explosion but instance hasn't been set.");
            return null;
        }

        //spawn explosion
        GameObject explosion = Instantiate(s_instance.m_explosionPrefab, position, Quaternion.identity);

        //make explosion bigger
        explosion.transform.localScale = new Vector3(1.5f,1.5f,1.5f);

        //destroy explosion after a certain amount of time
        Destroy(explosion, destroyAfter);

        //return a reference to the explosion
        return explosion;
    }

    public static GameObject CreateExplosion02(Vector3 position, float destroyAfter = 0.5f)
    {
        //instance not set error check
        if (s_instance == null)
        {
            Debug.LogError("Tried to spawn explosion but instance hasn't been set.");
            return null;
        }

        //spawn explosion
        GameObject explosion = Instantiate(s_instance.m_explosionPrefab02, position, Quaternion.identity);

        //make explosion bigger
        explosion.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        //destroy explosion after a certain amount of time
        Destroy(explosion, destroyAfter);

        //return a reference to the explosion
        return explosion;
    }

    public static GameObject CreateSlash(Vector3 position, float destroyAfter = 0.5f)
    {
        if (s_instance == null)
        {
            Debug.LogError("Tried to spawn slash but instance hasn't been set.");
            return null;
        }

        GameObject slash = Instantiate(s_instance.m_slashPrefab, position, Quaternion.identity);

        //destroy explosion after a certain amount of time
        Destroy(slash, destroyAfter);

        return slash;
    }
}
