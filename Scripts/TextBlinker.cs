using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace SequenceState
{
    public class TextBlinker : MonoBehaviour
    {
        //TextMeshProUGUIのテキストを点滅させる
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        private void Start()
        {
            //_textMeshProUGUIを点滅させる
            textMeshProUGUI.DOFade(0.0f, 0.6f)
                .SetEase(Ease.InCubic)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}