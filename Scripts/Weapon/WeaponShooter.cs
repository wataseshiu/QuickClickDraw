using UnityEngine;
using UnityEngine.Serialization;

namespace Weapon
{
    public class WeaponShooter : MonoBehaviour, IWeaponShooter
    {
        [SerializeField] private Weapon weapon;
        public void Shoot()
        {
            IWeapon iWeapon = this.weapon;
            iWeapon.Shoot();
            //test
            Debug.Log("test");
        }
    }
}