using UnityEngine;

public class PlayerAttacker :MonoBehaviour
{
    playerAnimatorManager animatorHandler;
    InputHandler inputHandler;
    string lastAttack;

    private void Awake()
    {
        animatorHandler = GetComponentInChildren<playerAnimatorManager>();
        inputHandler = GetComponent<InputHandler>();
    }

    public void HandleComboAttack(WeaponItem weapon)
    {
        if (!inputHandler.comboFlag) return;

        animatorHandler.anim.SetBool("canDoCombo", false);

        if (lastAttack == weapon.OH_Light_Attack_1)
        {
            animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, false);
            //animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, false);
        //animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
        lastAttack = weapon.OH_Light_Attack_1;
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, false);
        //animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
        lastAttack = weapon.OH_Heavy_Attack_1;
    }
}


