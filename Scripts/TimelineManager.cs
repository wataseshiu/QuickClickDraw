using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : SingletonMonoBehaviour<TimelineManager>
{
    public List<PlayableDirector> playableDirectors;

    public PlayableDirector PlayTimeline(string playableName)
    {
        foreach( PlayableDirector playable in playableDirectors)
        {
            if(playable.name == playableName)
            {
                Debug.Log("PlayTimeline");
                //最初から再生させる
                playable.time = 0;
                playable.Play();
                return playable;
            }
        }
        return null;
    }

    public async UniTask WaitTimeline(PlayableDirector playableDirector, CancellationToken cancellationToken)
    {
        try
        {
            await UniTask.WaitWhile((() => (playableDirector.time < playableDirector.duration)), cancellationToken: cancellationToken);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Cancelled");
        }
    }
}
