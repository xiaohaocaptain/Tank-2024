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
    public float ExplosionRadius = 5f;  //��ը�뾶
    public float ExplosionForce = 1000f;    //��ը��
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
        //��ȡһ�������ڵ���������
        Collider[] targetCollider = Physics.OverlapSphere(transform.position, ExplosionRadius, TankMask);
        for(int i = 0; i < targetCollider.Length; i++)
        {
            Rigidbody target = targetCollider[i].GetComponent<Rigidbody>();
            if (target == null)
            {
                continue;
            }

            //ʩ��RigidBody�Դ��ı�ը��
            target.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);

            //��Ѫ
            float damage = CalulateDamage(target.gameObject.transform.position);
            MyTankHealth tankHealth = targetCollider[i].GetComponent<MyTankHealth>();
            tankHealth.TankDamage(damage);
        }

        //��ը��Ч
        ExplosionParticle.transform.parent = null;
        ExplosionParticle.Play();

        //��ը����
        ExplosionAudio.transform.parent = null;
        ExplosionAudio.Play();

        //����
        Destroy(gameObject);    //ɾ���ڵ�
        Destroy(ExplosionAudio.gameObject, ExplosionAudio.clip.length); //ɾ����Ƶ���
        Destroy(ExplosionParticle.gameObject,ExplosionParticle.main.duration);  //ɾ��������Ч
    }
    /// <summary>
    /// ���㱬ը�˺�
    /// </summary>
    /// <param name="targetPosition">��ը��Χ��̹�˵�λ��</param>
    /// <returns>0������ֵ����ֹ̹��Ѫ������</returns>
    private float CalulateDamage(Vector3 targetPosition)
    {
        //����̹���뱬ը��֮��ľ���
        Vector3 Vector3Distance = targetPosition - transform.position;
        float Distance = Vector3Distance.magnitude;

        //����ٷֱ��˺�
        return Mathf.Max(0,(ExplosionRadius - Distance) / ExplosionRadius * MaxDamage);
    }
}
