using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyGameManager : MonoBehaviour
{
    public int NumOfRoundsToWin=5;
    public float StartDelay = 3f;
    public float EndDelay = 3f;
    public MyCameraControl cameraControl;
    public Text MessageText;
    public GameObject PrefabTank;
    public MyTankManager[] Tanks;

    private WaitForSeconds StartWait;
    private WaitForSeconds EndWait;
    private int RoundNumber=0;

    private MyTankManager RoundWinner;
    private MyTankManager GameWinner;

    // Start is called before the first frame update
    void Start()
    {
        //设置开始与结束等待时间
        StartWait = new WaitForSeconds(StartDelay);
        EndWait = new WaitForSeconds(EndDelay);

        SetAllTanks();
        SetCameraTargets();

        StartCoroutine(GameLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 设置双方坦克
    /// </summary>
    private void SetAllTanks()
    {
        for(int i = 0; i < Tanks.Length; i++)
        {
            Tanks[i].instance = Instantiate(PrefabTank, Tanks[i].BirthPoint.position, Tanks[i].BirthPoint.rotation) as GameObject;
            Tanks[i].PlayerNumber = i + 1;
            Tanks[i].Setup();
        }
    }

    private void SetCameraTargets()
    {
        Transform[] Targets = new Transform[Tanks.Length];
        for(int i = 0; i < Targets.Length; i++)
        {
            Targets[i] = Tanks[i].instance.transform;
        }
        cameraControl.Targets = Targets;    //数组形式赋给MyCameraControl
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (GameWinner != null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }
    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();

        cameraControl.SetStartPositionAndSize();

        RoundNumber++;
        if (RoundNumber == 1)
        {
            MessageText.text = "Tank Battle!\n"+"ROUND_" + RoundNumber;
        }
        else
        {
            MessageText.text = "ROUND_" + RoundNumber;
        }
        yield return StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        EnableTankControl();
        MessageText.text = "";

        while (!IfOneTankLeft())
        {
            yield return null;
        }
        
    }


    private IEnumerator RoundEnding()
    {
        DisableTankControl();

        RoundWinner = null;
        RoundWinner = GetRoundWinner();
        if (RoundWinner != null)
        {
            RoundWinner.wins++;
        }

        GameWinner =GetGameManner();

        string message = EndMessage();

        MessageText.text = message;
        yield return EndWait;
    }

    /// <summary>
    /// 判断是否还存在至少一个坦克
    /// </summary>
    /// <returns>判断结果</returns>
    private bool IfOneTankLeft()
    {
        int NumOfTankLeft = 0;
        for(int i = 0; i < Tanks.Length; i++)
        {
            if (Tanks[i].instance.activeSelf)
            {
                NumOfTankLeft++;
            }
        }
        return NumOfTankLeft <= 1;
    }

    private MyTankManager GetRoundWinner()
    {
        for (int i = 0; i < Tanks.Length; i++)
        {
            if (Tanks[i].instance.activeSelf)
                return Tanks[i];
        }

        return null;
    }
    private MyTankManager GetGameManner()
    {
        for (int i = 0; i < Tanks.Length; i++)
        {
            if (Tanks[i].wins == NumOfRoundsToWin)
                return Tanks[i];
        }

        return null;
    }

    /// <summary>
    /// 在一局游戏结束后更新UI上的message
    /// </summary>
    /// <returns>更新后展示的message</returns>
    private string EndMessage()
    {
        string message = "DRAW!";

        if (RoundWinner != null)
            message = RoundWinner.ColoredPlayerText + " WINS THIS ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < Tanks.Length; i++)
        {
            message += Tanks[i].ColoredPlayerText + ": " + Tanks[i].wins + " WINS\n";
        }

        if (GameWinner != null)
        {
            message = GameWinner.ColoredPlayerText + " WINS THE GAME!";
        }
            
        return message;
    }

    /// <summary>
    /// 重置所有坦克
    /// </summary>
    private void ResetAllTanks()
    {
        for (int i = 0; i < Tanks.Length; i++)
        {
            Tanks[i].Reset();
        }
    }
    private void EnableTankControl()
    {
        for (int i = 0; i < Tanks.Length; i++)
        {
            Tanks[i].EnableControl();
        }
    }
    private void DisableTankControl()
    {
        for (int i = 0; i < Tanks.Length; i++)
        {
            Tanks[i].DisableControl();
        }
    }
}
