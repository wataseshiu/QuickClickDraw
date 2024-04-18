using UnityEngine;
using System.Collections;

//キューブオブジェクトのステート
namespace SequenceState
{

    //ステートの実行を管理するクラス
    public class StateProcessor
    {
        //ステート本体
        private SequenceState _State;
        public SequenceState State
        {
            set { _State = value; }
            get { return _State; }
        }

        // 実行ブリッジ
        public void Execute()
        {
            State.Execute();
        }

    }

    //ステートのクラス
    public abstract class SequenceState
    {
        //デリゲート
        public delegate void executeState();
        public executeState execDelegate;

        //実行処理
        public virtual void Execute()
        {
            if (execDelegate != null)
            {
                execDelegate();
            }
        }

        //ステート名を取得するメソッド
        public abstract string getStateName();
        public abstract void setupState();
    }

    // 以下状態クラス

    //  DefaultPosition
    public class SequenceStateTitle : SequenceState
    {
        public override string getStateName()
        {
            return "State:Title";
        }

        public override void setupState()
        {
            LogicController.Instance.enabled = false;
        }
    }

    //  StateA
    public class SequenceStateMenu : SequenceState
    {
        public override string getStateName()
        {
            return "State:Menu";
        }
        public override void setupState()
        {
            LogicController.Instance.enabled = false;
        }
    }

    //  StateB
    public class SequenceStateInGame : SequenceState
    {
        public override string getStateName()
        {
            return "State:InGame";
        }
        public override void setupState()
        {
            LogicController.Instance.enabled = true;
        }
/*
        public override void Execute()
        {
            Debug.Log("特別な処理がある場合は子が処理してもよい");
            if (execDelegate != null)
            {
                execDelegate();
            }
        }*/
    }
}