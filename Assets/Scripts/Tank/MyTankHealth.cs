using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyTankHealth : MonoBehaviour
{
    public float StartHealth = 100f;    //初始血量
    public GameObject ExplosionPrefab;  //爆炸预制体
    public Slider HealthSlider;

    public Image FillImage;
    public Color HealthColor_66_100;
    public Color HealthColor_33_66;
    public Color HealthColor0_33;

    private float CurrentHealth;    //当前血量
    private ParticleSystem ExplosionParticle;   //爆炸粒子特效
    private AudioSource ExplosionAudio; //爆炸音效
    private bool Dead;  //true坦克死亡，false坦克存活

    private void Awake()
    {
        //从爆炸预制体中获取爆炸粒子特效
        ExplosionParticle = Instantiate(ExplosionPrefab).GetComponent<ParticleSystem>();
        //从爆炸预制体中获取爆炸音效
        ExplosionAudio = ExplosionParticle.GetComponent<AudioSource>();
        //暂时关闭组件
        ExplosionParticle.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        CurrentHealth = StartHealth;    //设置当前血量为初始血量
        Dead = false;   //设置初始状态
        setHealthUI();
    }

    public void TankDamage(float amount)
    {
        CurrentHealth -= amount;
        //将当前血量变化展现在UI中
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
    /// 坦克血量归零处理
    /// </summary>
    private void isDead()
    {
        Dead = true;
        //播放粒子特效
        ExplosionParticle.transform.position = transform.position;  //移动到坦克位置
        ExplosionParticle.gameObject.SetActive(true);   //打开组件
        ExplosionParticle.Play();
        //播放声音
        ExplosionAudio.Play();

        gameObject.SetActive(false);
    }
    private void setHealthUI()
    {
        HealthSlider.value = CurrentHealth;

        //UI颜色变更
        if (CurrentHealth >= 60f)
        {
            FillImage.color = HealthColor_66_100;   //绿
        }else if (CurrentHealth >= 30f && CurrentHealth < 60f)
        {
            FillImage.color = HealthColor_33_66;    //黄
        }else if (CurrentHealth < 30f)
        {
            FillImage.color = HealthColor0_33;      //红
        }
    }
}
