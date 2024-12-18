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
    public float Speed = 8;     //ֱ���ٶ�
    public float TurnSpeed = 180;       //��ת�ٶ�
    public AudioSource MovementAudio;       //���������
    public AudioClip EngineIdle;        //̹�˴�����Ч
    public AudioClip EngineDriving;         //̹���ƶ���Ч
    public float PitchRange = 0.25f;       //����

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
        //W��S����ǰ���ƶ�
        MovementInputValue = Input.GetAxis(TurnAxisName);
        //A��D����������ת
        TurnInputValue = Input.GetAxis(MovementAxisName);

        //��������
        if (Mathf.Abs(MovementInputValue) < 0.2 && Mathf.Abs(TurnInputValue) < 0.2)
        {
            if (MovementAudio.clip == EngineDriving)     //���ƶ��л�����ֹ
            {
                MovementAudio.clip = EngineIdle;
                //ÿ��������0.75-1.25֮�両��
                MovementAudio.pitch = UnityEngine.Random.Range(OriginalPitch - PitchRange, OriginalPitch + PitchRange);
                MovementAudio.Play();
            } 

        }
        else
        {
            if (MovementAudio.clip == EngineIdle)        //�Ӿ�ֹ�л����ƶ�
            {
                MovementAudio.clip = EngineDriving;
                //ÿ��������0.75-1.26֮�両��
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
        //��Ԫ��
        Quaternion quaternion = Quaternion.Euler(0, turn, 0);       //��y����ת
        rb.MoveRotation(rb.rotation * quaternion);
    }
}
