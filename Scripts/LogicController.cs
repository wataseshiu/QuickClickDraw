using System.Collections;
using System.Collections.Generic;
using naichilab.EasySoundPlayer.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LogicController : SingletonMonoBehaviour<LogicController>
{
    [SerializeField]
    private ITimeCounter _timeCounter = new TimeCounter();
    public float checkCount = 3.0f;

    public float inputStartSecondMin = 3.0f;
    public float inputStartSecondRandomAdd = 5.0f;
    public float inputStartSecondCurrent = 0.0f;

    Transform player,enemy;
    Animator playerAnim,enemyAnim;
    public Canvas cutin;

    public enum GAMESTATE
    {
        IDLE,
        PRE_INPUT,
        INPUT_WAIT,
        WIN,
        LOSE,
        RESET,
    }

    public GAMESTATE gAMESTATE = GAMESTATE.IDLE;
    public GAMESTATE resultState = GAMESTATE.IDLE;

    // Start is called before the first frame update
    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        playerAnim = player.GetComponent<Animator>();
        enemyAnim = enemy.GetComponent<Animator>();
        playerAnim.enabled = true;
        enemyAnim.enabled = true;
        inputStartSecondRandomAdd = Random.Range(0.0f,inputStartSecondRandomAdd);
    }

    // Update is called once per frame
    void Update()
    {
        switch (gAMESTATE)
        {
            case GAMESTATE.IDLE:
                {
                    UpdateIdle();
                    break;
                }
            case GAMESTATE.PRE_INPUT:
            {
                UpdatePreInput();
                break;
            }
            case GAMESTATE.INPUT_WAIT:
                {
                    UpdateInputWait();
                    break;
                }
            case GAMESTATE.WIN:
                {
                    UpdateWin();
                    break;
                }
            case GAMESTATE.LOSE:
                {
                    UpdateLose();
                    break;
                }
            case GAMESTATE.RESET:
                {
                    UpdateReset();
                    break;
                }
        }
    }

    void UpdateIdle()
    {
    }

    //撃っていいタイミングより前に撃ったら負け
    void UpdatePreInput()
    {
        //待機時間を加算
        inputStartSecondCurrent += Time.deltaTime;

        //待機時間中に撃ったら負け
        if (Input.GetButtonDown("Fire1"))
        {
            //負けに遷移
            resultState = GAMESTATE.LOSE; 
            gAMESTATE = GAMESTATE.LOSE;
        }

        //待機時間の経過していたら勝負開始のステートへ
        if (inputStartSecondCurrent >= inputStartSecondMin + inputStartSecondRandomAdd)
        {
            //カットイン表示を勝負の合図ということにする
            //一回だけ通したいのでここにかいた
            cutin.gameObject.SetActive(true);
            //SE再生
            SePlayer.Instance.Play(Random.Range(5,9));
            gAMESTATE = GAMESTATE.INPUT_WAIT;
        }
    }

    void UpdateInputWait()
    {
        //タイマーカウントアップ
        _timeCounter.CountUp();

        //敵（制限時間）より早く撃てた
        if (!_timeCounter.CheckTime(checkCount))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //勝ちに遷移
                resultState = GAMESTATE.WIN; 
                gAMESTATE = GAMESTATE.WIN;
            }
        }
        //撃てなかった
        else
        {
            //負けに遷移
            resultState = GAMESTATE.LOSE; 
            gAMESTATE = GAMESTATE.LOSE;
        }
    }

    void UpdateWin()
    {
        cutin.gameObject.SetActive(false);
        CanvasManager.Instance.HideSmokeCanvas();
        SwitchRendererFeature.Instance.SwitchFullScreenRendererFeatureOn();
        playerAnim.SetTrigger("Shooting");
        enemyAnim.enabled = false;
        gAMESTATE = GAMESTATE.IDLE;
        StartCoroutine(SetStateReset());
    }

    void UpdateLose()
    {
        cutin.gameObject.SetActive(false);
        CanvasManager.Instance.HideSmokeCanvas();
        SwitchRendererFeature.Instance.SwitchFullScreenRendererFeatureOn();
        enemyAnim.SetTrigger("Shooting");
        playerAnim.enabled = false;
        gAMESTATE = GAMESTATE.IDLE;
        StartCoroutine(SetStateReset());
    }

    IEnumerator SetStateReset()
    {
        yield return new WaitForSeconds(1);
        SwitchRendererFeature.Instance.SwitchFullScreenRendererFeatureOff();
        CanvasManager.Instance.ShowSmokeCanvas();
        yield return new WaitForSeconds(4);
        //勝ったときはタイムスコアを更新する
        if (resultState == GAMESTATE.WIN)
        {
            CanvasManager.Instance.SetTimeScoreText(_timeCounter.GetRoundCurrentCount());
        }
        CanvasManager.Instance.SetCanvas(CanvasManager.CANVAS_NAME.C_INGAME);
        CanvasManager.Instance.ShowResultCanvas(resultState == GAMESTATE.WIN);
    }

    public void UpdateReset()
    {
        playerAnim.enabled = true;
        enemyAnim.enabled = true;
        resultState = GAMESTATE.IDLE; 
        gAMESTATE = GAMESTATE.PRE_INPUT;
        cutin.gameObject.SetActive(false);
        _timeCounter.ResetCurrentCount();
        inputStartSecondCurrent = 0.0f;
        inputStartSecondRandomAdd = Random.Range(0.0f,inputStartSecondRandomAdd);
    }
}
