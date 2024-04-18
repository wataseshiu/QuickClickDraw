using UnityEngine;

namespace Weapon
{
    public class WeaponSwapper : MonoBehaviour,IWeaponSwapper
    {
        [SerializeField] private Weapon idleWeapon;
        [SerializeField] private Weapon activeWeapon;

        public void SwapWeapon()
        {
            IWeapon idle = idleWeapon;
            IWeapon active = activeWeapon;
            idle.Swap();
            active.Swap();
        }
    }
}