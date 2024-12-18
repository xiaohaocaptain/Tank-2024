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
    public float ChargeTime = 0.75f;        //充能时间

    private string FireButton;
    private float CurrentLaunchForce;
    private float ChargeSpeed;      //充能速度

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
        if (Input.GetButtonDown(FireButton))    //按下
        {
            CurrentLaunchForce = MinLaunchForce;

            ShootingAudio.clip = CharingClip;
            ShootingAudio.Play();

            Fired = false;
        }
        else if (Input.GetButton(FireButton)&&!Fired)  //持续
        {
            CurrentLaunchForce += ChargeSpeed * Time.deltaTime;
            ChargingSlider.value = CurrentLaunchForce;  //UI变换
            
            if (CurrentLaunchForce >= MaxLaunchForce)
            {
                CurrentLaunchForce = MaxLaunchForce;    //限制最大充能力为20
                //Fire(); 
            }
        }else if (Input.GetButtonUp(FireButton)&&!Fired)    //弹起
        {
            Fire();
        }
    }
    void Fire()
    {
        ChargingSlider.value = MinLaunchForce;  //重置蓄力UI
        Fired = true;
        GameObject gameObjectInstantiate = Instantiate(Shell, FireTransform.transform.position, FireTransform.transform.rotation);
        Rigidbody rigidbodyShell = gameObjectInstantiate.GetComponent<Rigidbody>();
        rigidbodyShell.velocity = FireTransform.transform.forward * CurrentLaunchForce;

        ShootingAudio.clip = FireClip;
        ShootingAudio.Play();

        CurrentLaunchForce = MinLaunchForce;    //防止充能超过上限
    }
}
