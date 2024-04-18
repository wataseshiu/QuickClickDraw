using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using naichilab.EasySoundPlayer.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapon
{
    public class Weapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private bool shootable = false;
        public bool Shootable => shootable;

        //Shoot時に再生するエフェクト
        [SerializeField] private GameObject shootEffect;
        
        public void Swap()
        {
            //gameobjectのactiveを切り替える
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }

        public async UniTaskVoid Shoot()
        {
            //Shootableなら打つ
            if (!Shootable) return;
            Debug.Log("Shoot");

            //エフェクトが存在するなら再生する
            if (shootEffect == null) return;

            //SE再生
            SePlayer.Instance.Play(Random.Range(0,4));
 
            //Activeを切り替えるだけ
            shootEffect.SetActive(!shootEffect.activeInHierarchy);

            //パーティクルの再生終了したらActiveを切り替える
            var particleSystem = shootEffect.transform.GetChild(0).GetComponent<ParticleSystem>();
            if (particleSystem == null) return;

            await particleSystem.GetAsyncParticleSystemStoppedTrigger().OnParticleSystemStoppedAsync();

            shootEffect.SetActive(false);
            Debug.Log("await");
        }
    }
}