using System;
using System.Linq;
using UnityEngine;

namespace CharacterDirector
{
    public class CharacterDirector: SingletonMonoBehaviour<CharacterDirector>
    {
        [SerializeField] private GameObject[] characters;
        private Animator[] _animators;
        private static readonly int OutGameSetup = Animator.StringToHash("OutGameSetup");

        private void Start()
        {
            if(characters.Length == 0) return;
            _animators = characters.Select( character => character.GetComponent<Animator>()).ToArray();
            
            //0と1と3を表示OFFにする
            characters[0].SetActive(false);
            characters[1].SetActive(false);
            characters[3].SetActive(false);
            
            //2のみ表示ONにする
            characters[2].SetActive(true);
            //2のAnimatorのOutGameSetupをtrueにする
            _animators[2].SetBool(OutGameSetup, true);
        }

        public void SetupMenuToInGame()
        {
            characters[3].SetActive(true);
        }

        public void SetupInGame()
        {
            characters[2].SetActive(false);
            characters[3].SetActive(false);
            
            characters[0].SetActive(true);
            characters[1].SetActive(true);
        }
    }
}