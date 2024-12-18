using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyTankShooting : MonoBehaviour
{
    public GameObject FireTransform;
    public GameObject Shell;
    public int PlayerNumber = 1;
    public Slider ChargingSlider;

    public AudioSource ShootingAudio;
    public AudioClip CharingClip;
    public AudioClip FireClip;

    public float MinLaunchForce = 10f;
    public float MaxLaunchForce = 20f;
    public float ChargeTime = 0.75f;        //����ʱ��

    private string FireButton;
    private float CurrentLaunchForce;
    private float ChargeSpeed;      //�����ٶ�

    private bool Fired;

    // Start is called before the first frame update
    void Start()
    {
        FireButton = "Fire" + PlayerNumber;
        ChargeSpeed = (MaxLaunchForce - MinLaunchForce) / ChargeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(FireButton))    //����
        {
            CurrentLaunchForce = MinLaunchForce;

            ShootingAudio.clip = CharingClip;
            ShootingAudio.Play();

            Fired = false;
        }
        else if (Input.GetButton(FireButton)&&!Fired)  //����
        {
            CurrentLaunchForce += ChargeSpeed * Time.deltaTime;
            ChargingSlider.value = CurrentLaunchForce;  //UI�任
            
            if (CurrentLaunchForce >= MaxLaunchForce)
            {
                CurrentLaunchForce = MaxLaunchForce;    //������������Ϊ20
                //Fire(); 
            }
        }else if (Input.GetButtonUp(FireButton)&&!Fired)    //����
        {
            Fire();
        }
    }
    void Fire()
    {
        ChargingSlider.value = MinLaunchForce;  //��������UI
        Fired = true;
        GameObject gameObjectInstantiate = Instantiate(Shell, FireTransform.transform.position, FireTransform.transform.rotation);
        Rigidbody rigidbodyShell = gameObjectInstantiate.GetComponent<Rigidbody>();
        rigidbodyShell.velocity = FireTransform.transform.forward * CurrentLaunchForce;

        ShootingAudio.clip = FireClip;
        ShootingAudio.Play();

        CurrentLaunchForce = MinLaunchForce;    //��ֹ���ܳ�������
    }
}
