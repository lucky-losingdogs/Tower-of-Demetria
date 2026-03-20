using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    //override means this function is the child class's implementation
    //of an abstract function declared in the parent class.
    public override void AffectCharacter(GameObject player, float val)
    {
        HP health = player.GetComponent<HP>();
        if (health != null)
        {
            if (val > 0)
            {
                player.SendMessage("AddHealth", val, SendMessageOptions.DontRequireReceiver);
            }
            else if (val < 0)
            {
                player.SendMessage("TakeDamage", val, SendMessageOptions.DontRequireReceiver);
            }
        }
            
    }
}