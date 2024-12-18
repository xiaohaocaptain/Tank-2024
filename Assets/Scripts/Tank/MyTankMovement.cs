using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class MyTankMovement : MonoBehaviour
{
    private Rigidbody rb = null;
    private string MovementAxisName;
    private string TurnAxisName;
    private float MovementInputValue;
    private float TurnInputValue;
    private float OriginalPitch;

    public int PlayerNumber = 1;
    public float Speed = 8;     //直线速度
    public float TurnSpeed = 180;       //旋转速度
    public AudioSource MovementAudio;       //播放器组件
    public AudioClip EngineIdle;        //坦克待机音效
    public AudioClip EngineDriving;         //坦克移动音效
    public float PitchRange = 0.25f;       //音调

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        MovementAxisName = "Horizontal" + PlayerNumber;
        TurnAxisName = "Vertical" + PlayerNumber;
        OriginalPitch = MovementAudio.pitch;
    }

    // Update is called once per frame
    void Update()
    {
        //W和S进行前后移动
        MovementInputValue = Input.GetAxis(TurnAxisName);
        //A和D进行左右旋转
        TurnInputValue = Input.GetAxis(MovementAxisName);

        //播放声音
        if (Mathf.Abs(MovementInputValue) < 0.2 && Mathf.Abs(TurnInputValue) < 0.2)
        {
            if (MovementAudio.clip == EngineDriving)     //从移动切换到静止
            {
                MovementAudio.clip = EngineIdle;
                //每次音调在0.75-1.25之间浮动
                MovementAudio.pitch = UnityEngine.Random.Range(OriginalPitch - PitchRange, OriginalPitch + PitchRange);
                MovementAudio.Play();
            } 

        }
        else
        {
            if (MovementAudio.clip == EngineIdle)        //从静止切换到移动
            {
                MovementAudio.clip = EngineDriving;
                //每次音调在0.75-1.26之间浮动
                MovementAudio.pitch = UnityEngine.Random.Range(OriginalPitch - PitchRange, OriginalPitch + PitchRange);
                MovementAudio.Play();
            }
        }

    }
 

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    void Move()
    {
        Vector3 MovementV3 = transform.forward * MovementInputValue * Time.deltaTime * Speed;
        rb.MovePosition(rb.position + MovementV3);
    }

    void Turn()
    {
        float turn = TurnInputValue * Time.deltaTime * TurnSpeed;
        //四元数
        Quaternion quaternion = Quaternion.Euler(0, turn, 0);       //绕y轴旋转
        rb.MoveRotation(rb.rotation * quaternion);
    }
}
