using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class MyShellExplosion : MonoBehaviour
{
    public LayerMask TankMask;
    public float MaxLifeTime = 2f;
    public ParticleSystem ExplosionParticle;
    public AudioSource ExplosionAudio;
    public float ExplosionRadius = 5f;  //爆炸半径
    public float ExplosionForce = 1000f;    //爆炸力
    public float MaxDamage = 100f;

    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject,MaxLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //获取一定球域内的所有物体
        Collider[] targetCollider = Physics.OverlapSphere(transform.position, ExplosionRadius, TankMask);
        for(int i = 0; i < targetCollider.Length; i++)
        {
            Rigidbody target = targetCollider[i].GetComponent<Rigidbody>();
            if (target == null)
            {
                continue;
            }

            //施加RigidBody自带的爆炸力
            target.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);

            //扣血
            float damage = CalulateDamage(target.gameObject.transform.position);
            MyTankHealth tankHealth = targetCollider[i].GetComponent<MyTankHealth>();
            tankHealth.TankDamage(damage);
        }

        //爆炸特效
        ExplosionParticle.transform.parent = null;
        ExplosionParticle.Play();

        //爆炸声音
        ExplosionAudio.transform.parent = null;
        ExplosionAudio.Play();

        //销毁
        Destroy(gameObject);    //删除炮弹
        Destroy(ExplosionAudio.gameObject, ExplosionAudio.clip.length); //删除音频组件
        Destroy(ExplosionParticle.gameObject,ExplosionParticle.main.duration);  //删除粒子特效
    }
    /// <summary>
    /// 计算爆炸伤害
    /// </summary>
    /// <param name="targetPosition">爆炸范围内坦克的位置</param>
    /// <returns>0到计算值，防止坦克血量倒增</returns>
    private float CalulateDamage(Vector3 targetPosition)
    {
        //计算坦克与爆炸点之间的距离
        Vector3 Vector3Distance = targetPosition - transform.position;
        float Distance = Vector3Distance.magnitude;

        //计算百分比伤害
        return Mathf.Max(0,(ExplosionRadius - Distance) / ExplosionRadius * MaxDamage);
    }
}
