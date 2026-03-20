using UnityEngine;

public class ApplyDamage : MonoBehaviour
{
    //apply damage to the target's hp component
    static public void ApplyDmg(GameObject target, float dmg)
    {
        if (target != null)
        {
            //SendMessage calls TakeDamage function in the target
            target.SendMessage("TakeDamage", dmg, SendMessageOptions.DontRequireReceiver);
        }
    }

    public static bool CooldownFinished(float timeout)
    {
        return Time.time > timeout;
    }

    public static float StartCooldown(float cooldown)
    {
        return Time.time + cooldown;
    }
}
