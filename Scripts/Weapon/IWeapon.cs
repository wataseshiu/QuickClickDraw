using Cysharp.Threading.Tasks;

namespace Weapon
{
    public interface IWeapon
    {
        bool Shootable { get; }
        void Swap();
        UniTaskVoid Shoot();
    }
}
