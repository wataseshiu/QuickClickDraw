using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ScreenFader
{
    public class ScreenFader: SingletonMonoBehaviour<ScreenFader>
    {
        [SerializeField] Image fadeImage;

        public async UniTask FadeOut(float duration = 1.0f, CancellationToken cancellationToken = default)
        {
            await fadeImage.DOFade(1.0f, duration).ToUniTask(cancellationToken: cancellationToken);
        }

        public async UniTask FadeIn(float duration = 1.0f, CancellationToken cancellationToken = default)
        {
            await fadeImage.DOFade(0.0f, duration).ToUniTask(cancellationToken: cancellationToken);
        }
    }
}