using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CanvasManager : SingletonMonoBehaviour<CanvasManager>
{
    public Canvas canvasTitle;
    public Canvas canvasMenu;
    public Canvas canvasInGame;
    public RectTransform inGameWin;
    public RectTransform inGameLose;
    public Canvas canvasReady;
    public Canvas canvasSmoke;

    [SerializeField] private TextMeshProUGUI timeScoreText;
    [SerializeField] private Button[] titleButton;
    public Button[] TitleButton => titleButton;
    [SerializeField] private Button[] menuButton;
    public Button[] MenuButton => menuButton;
    [SerializeField] private Button[] inGameButton;
    public Button[] InGameButton => inGameButton;
    [SerializeField] private Button[] restartButton;
    public Button[] RestartButton => restartButton;
    [SerializeField] private Button[] quitButton;
    public Button[] QuitButton => quitButton;

    public enum CANVAS_NAME
    {
        C_TITLE,
        C_MENU,
        C_INGAME,
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    public void SetCanvas(CANVAS_NAME canvasName, bool isShow = true)
    {
        switch(canvasName)
        {
            case CANVAS_NAME.C_TITLE:
                {
                    canvasTitle.gameObject.SetActive(isShow);
                    canvasMenu.gameObject.SetActive(!isShow);
                    canvasInGame.gameObject.SetActive(!isShow);
                    canvasSmoke.gameObject.SetActive(isShow);
                    break;
                }
            case CANVAS_NAME.C_MENU:
                {
                    canvasTitle.gameObject.SetActive(!isShow);
                    canvasMenu.gameObject.SetActive(isShow);
                    canvasInGame.gameObject.SetActive(!isShow);
                    canvasSmoke.gameObject.SetActive(isShow);
                    break;
                }
            case CANVAS_NAME.C_INGAME:
                {
                    canvasTitle.gameObject.SetActive(!isShow);
                    canvasMenu.gameObject.SetActive(!isShow);
                    canvasInGame.gameObject.SetActive(isShow);
                    canvasSmoke.gameObject.SetActive(isShow);
                    break;
                }
        }
    }
    
    public void ShowReadyCanvas()
    {
        canvasReady.gameObject.SetActive(true);
    }
    public void HideReadyCanvas()
    {
        canvasReady.gameObject.SetActive(false);
    }
    public void ShowSmokeCanvas()
    {
        canvasSmoke.gameObject.SetActive(true);
    }
    public void HideSmokeCanvas()
    {
        canvasSmoke.gameObject.SetActive(false);
    }
    public void ShowResultCanvas(bool isWin)
    { 
        inGameWin.gameObject.SetActive(isWin);
        inGameLose.gameObject.SetActive(!isWin);
    }
    
    public void ResetAllCanvas()
    {
        canvasTitle.gameObject.SetActive(false);
        canvasMenu.gameObject.SetActive(false);
        canvasInGame.gameObject.SetActive(false);
    }
    
    public void SetTimeScoreText(float time)
    {
        timeScoreText.text = "Time :" + time;
    }
}
