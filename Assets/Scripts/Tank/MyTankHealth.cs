using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyTankHealth : MonoBehaviour
{
    public float StartHealth = 100f;    //��ʼѪ��
    public GameObject ExplosionPrefab;  //��ըԤ����
    public Slider HealthSlider;

    public Image FillImage;
    public Color HealthColor_66_100;
    public Color HealthColor_33_66;
    public Color HealthColor0_33;

    private float CurrentHealth;    //��ǰѪ��
    private ParticleSystem ExplosionParticle;   //��ը������Ч
    private AudioSource ExplosionAudio; //��ը��Ч
    private bool Dead;  //true̹��������false̹�˴��

    private void Awake()
    {
        //�ӱ�ըԤ�����л�ȡ��ը������Ч
        ExplosionParticle = Instantiate(ExplosionPrefab).GetComponent<ParticleSystem>();
        //�ӱ�ըԤ�����л�ȡ��ը��Ч
        ExplosionAudio = ExplosionParticle.GetComponent<AudioSource>();
        //��ʱ�ر����
        ExplosionParticle.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        CurrentHealth = StartHealth;    //���õ�ǰѪ��Ϊ��ʼѪ��
        Dead = false;   //���ó�ʼ״̬
        setHealthUI();
    }

    public void TankDamage(float amount)
    {
        CurrentHealth -= amount;
        //����ǰѪ���仯չ����UI��
        setHealthUI();

        if (CurrentHealth <= 0f && !Dead)
        {
            isDead();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// ̹��Ѫ�����㴦��
    /// </summary>
    private void isDead()
    {
        Dead = true;
        //����������Ч
        ExplosionParticle.transform.position = transform.position;  //�ƶ���̹��λ��
        ExplosionParticle.gameObject.SetActive(true);   //�����
        ExplosionParticle.Play();
        //��������
        ExplosionAudio.Play();

        gameObject.SetActive(false);
    }
    private void setHealthUI()
    {
        HealthSlider.value = CurrentHealth;

        //UI��ɫ���
        if (CurrentHealth >= 60f)
        {
            FillImage.color = HealthColor_66_100;   //��
        }else if (CurrentHealth >= 30f && CurrentHealth < 60f)
        {
            FillImage.color = HealthColor_33_66;    //��
        }else if (CurrentHealth < 30f)
        {
            FillImage.color = HealthColor0_33;      //��
        }
    }
}
