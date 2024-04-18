using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Playables;
using SequenceState;
using Cysharp.Threading.Tasks;
using naichilab.EasySoundPlayer.Scripts;
using Substance.Game;
using ScreenFader;
using UnityEngine.UI;
using VirtualCameraDirector;
using Random = UnityEngine.Random;

public class SequenceManager : SingletonMonoBehaviour<SequenceManager>, IDisposable
{
    //変更前のステート名
    private string _beforeStateName;

    //ステート
    private StateProcessor _stateProcessor = new StateProcessor();           //プロセッサー
    private SequenceStateTitle _stateTitle = new SequenceStateTitle();
    private SequenceStateMenu _stateMenu = new SequenceStateMenu();
    private SequenceStateInGame _stateInGame = new SequenceStateInGame();

    private CancellationTokenSource _cancellationTokenSource;

    // Start is called before the first frame update
    private async UniTaskVoid Start()
    {
        _stateProcessor.State = _stateTitle;
        _stateTitle.execDelegate = Title;
        _stateMenu.execDelegate = Menu;
        _stateInGame.execDelegate = InGame;
        
        _cancellationTokenSource = new();
        CancellationToken cancellationToken= _cancellationTokenSource.Token;

        var playable = TimelineManager.Instance.PlayTimeline("Title");
        await TimelineManager.Instance.WaitTimeline(playable, cancellationToken);
        Debug.Log("await passed");
        CanvasManager.Instance.SetCanvas(CanvasManager.CANVAS_NAME.C_TITLE);

        UniTask menuButtonTask = UniTask.WhenAny(CanvasManager.Instance.MenuButton.Select(b => b.OnClickAsync(cancellationToken)));
        UniTask quitButtonTask = UniTask.WhenAny(CanvasManager.Instance.QuitButton.Select(b => b.OnClickAsync(cancellationToken)));
        var result = await UniTask.WhenAny(menuButtonTask, quitButtonTask);

        //SE再生
        SePlayer.Instance.Play(Random.Range(5,9));
        
        if (result == 0)
        {
            _cancellationTokenSource.Cancel();
            ChangeState("Menu").Forget();

            return;
        }

        if (result == 1)
        {
            _cancellationTokenSource.Cancel();
            Quit();
            
            return;
        }
    }

    // Update is called once per frame
    private async void Update()
    {
        if(_stateProcessor.State == null)
        {
            return;
        }

        if(_stateProcessor.State.getStateName() != _beforeStateName)
        {
            Debug.Log("Now State;" + _stateProcessor.State.getStateName());
            _beforeStateName = _stateProcessor.State.getStateName();
            _stateProcessor.State.setupState();
        }
        _stateProcessor.Execute();

    }

    private async void Title()
    {
    }

    private async void Menu()
    {
    }

    private async void InGame()
    {
    }

    public async UniTask ChangeState(string state)
    {
        CanvasManager.Instance.ResetAllCanvas();
        _cancellationTokenSource = new();
        CancellationToken cancellationToken= _cancellationTokenSource.Token;
        
        switch(state)
        {
            case "Title":
            {
                _stateProcessor.State = _stateTitle;
                var playable = TimelineManager.Instance.PlayTimeline("MenuToTitle");
                await TimelineManager.Instance.WaitTimeline(playable, cancellationToken);
                Debug.Log("await passed");
                CanvasManager.Instance.SetCanvas(CanvasManager.CANVAS_NAME.C_TITLE);
                
                UniTask menuButtonTask = WaitForButtonClickTask(CanvasManager.Instance.MenuButton, cancellationToken);
                UniTask quitButtonTask = WaitForButtonClickTask(CanvasManager.Instance.QuitButton, cancellationToken);
                var result = await UniTask.WhenAny(menuButtonTask, quitButtonTask);

                //SE再生
                SePlayer.Instance.Play(Random.Range(5,9));
                
                if (result == 0)
                {
                    _cancellationTokenSource.Cancel();
                    playable.Stop();
                    ChangeState("Menu").Forget();
                }

                if (result == 1)
                {
                    _cancellationTokenSource.Cancel();
                    Quit();
                }
                break;
            }
            case "Menu":
            {
                _stateProcessor.State = _stateMenu;
                var playable = TimelineManager.Instance.PlayTimeline("TitleToMenu");
                await TimelineManager.Instance.WaitTimeline(playable, cancellationToken);
                Debug.Log("await passed");
                CanvasManager.Instance.SetCanvas(CanvasManager.CANVAS_NAME.C_MENU);

                UniTask titleButtonTask = WaitForButtonClickTask(CanvasManager.Instance.TitleButton, cancellationToken);
                UniTask inGameButtonTask = WaitForButtonClickTask(CanvasManager.Instance.InGameButton, cancellationToken);
                var result = await UniTask.WhenAny(titleButtonTask, inGameButtonTask);

                //SE再生
                SePlayer.Instance.Play(Random.Range(5,9));
                
                if (result == 0)
                {
                    _cancellationTokenSource.Cancel();
                    playable.Stop();
                    ChangeState("Title").Forget();
                }

                if (result == 1)
                {
                    _cancellationTokenSource.Cancel();
                    playable.Stop();
                    ChangeState("InGame").Forget();
                }
                break;
            }
            case "InGame":
            {
                CharacterDirector.CharacterDirector.Instance.SetupMenuToInGame();
                var playable = TimelineManager.Instance.PlayTimeline("MenuToInGame");
                await TimelineManager.Instance.WaitTimeline(playable, cancellationToken);
                await ScreenFader.ScreenFader.Instance.FadeOut(1.0f, cancellationToken: cancellationToken);
                CanvasManager.Instance.ResetAllCanvas();
                CharacterDirector.CharacterDirector.Instance.SetupInGame();
                //MainCameraをVirtualCamera_1に切り替える
                playable.Stop();
                VirtualCameraDirector.VirtualCameraDirector.Instance.SetActiveVirtualCamera("Virtual Camera_1");

                LogicController.Instance.Initialize();
                await ScreenFader.ScreenFader.Instance.FadeIn(1.0f, cancellationToken: cancellationToken);
                
                //READY演出をここに作る
                CanvasManager.Instance.ShowReadyCanvas();
                await UniTask.Delay(3000, cancellationToken: cancellationToken);
                CanvasManager.Instance.HideReadyCanvas();
                
                LogicController.Instance.gAMESTATE = LogicController.GAMESTATE.PRE_INPUT;
                _stateProcessor.State = _stateInGame;

                UniTask menuButtonTask = WaitForButtonClickTask(CanvasManager.Instance.MenuButton, cancellationToken);
                UniTask restartButtonTask = WaitForButtonClickTask(CanvasManager.Instance.RestartButton, cancellationToken);
                UniTask quitButtonTask = WaitForButtonClickTask(CanvasManager.Instance.QuitButton, cancellationToken);
                var result = await UniTask.WhenAny(menuButtonTask, restartButtonTask,quitButtonTask);

                //SE再生
                SePlayer.Instance.Play(Random.Range(5,9));
                
                if (result == 0)
                {
                    _cancellationTokenSource.Cancel();
                    playable.Stop();
                    ChangeState("Menu").Forget();
                }

                if (result == 1)
                {
                    _cancellationTokenSource.Cancel();
                    playable.Stop();
                    ChangeState("Restart").Forget();
                }

                if (result == 2)
                {
                    _cancellationTokenSource.Cancel();
                    Quit();
                }
                break;
            }
            case "Restart":
            {
                await ScreenFader.ScreenFader.Instance.FadeOut(1.0f, cancellationToken: cancellationToken);
                CanvasManager.Instance.ResetAllCanvas();
                LogicController.Instance.gAMESTATE = LogicController.GAMESTATE.RESET;

                await ScreenFader.ScreenFader.Instance.FadeIn(1.0f, cancellationToken: cancellationToken);

                UniTask menuButtonTask = WaitForButtonClickTask(CanvasManager.Instance.MenuButton, cancellationToken);
                UniTask restartButtonTask = WaitForButtonClickTask(CanvasManager.Instance.RestartButton, cancellationToken);
                UniTask quitButtonTask = WaitForButtonClickTask(CanvasManager.Instance.QuitButton, cancellationToken);
                var result = await UniTask.WhenAny(menuButtonTask, restartButtonTask,quitButtonTask);

                //SE再生
                SePlayer.Instance.Play(Random.Range(5,9));

                if (result == 0)
                {
                    _cancellationTokenSource.Cancel();
                    ChangeState("Menu").Forget();
                }

                if (result == 1)
                {
                    _cancellationTokenSource.Cancel();
                    ChangeState("Restart").Forget();
                }

                if (result == 2)
                {
                    _cancellationTokenSource.Cancel();
                    Quit();
                }
                break;
            }
        }
    }
    private UniTask WaitForButtonClickTask(Button[] buttons, CancellationToken cancellationToken)
    {
        return UniTask.WhenAny(buttons.Select(b => b.OnClickAsync(cancellationToken)));
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        _cancellationTokenSource.Cancel();
    }

    public void Dispose()
    {
        _cancellationTokenSource?.Dispose();
    }
}
