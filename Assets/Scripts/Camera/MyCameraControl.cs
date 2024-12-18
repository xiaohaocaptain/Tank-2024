using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraControl : MonoBehaviour
{
    
    [HideInInspector]public Transform[] Targets; //����̹�˶���
    public float DampTime = 0.2f;   //��ͷ����ʱ��
    public float ScreenEdgeBuffer = 4f;     //�ӱ�Ե����ֹ̹����ʾ�ڱ�Ե
    public float MinSize = 6.5f;    //��С��ʾ����

    private Camera Camera;
    private float ZoomSpeed;
    private Vector3 DesiredPosition;    //����̹�˵����ĵ�
    private Vector3 MoveVelocity;    //��ͷ�ƶ��ٶ�
    

    private void Awake()
    {
        Camera = GetComponentInChildren<Camera>();  //��ȡ�����Camera,ֻ��һ��ʱ��������д
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
        Zoom();
        Zoom();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void Move()
    {
        FindAveragePosition();
        //ƽ������
        transform.position = Vector3.SmoothDamp(transform.position, DesiredPosition, ref MoveVelocity, DampTime);
    }
    private void FindAveragePosition()
    {
        Vector3 AveragePosition = new Vector3();
        int numTargets = 0;
        
        for(int i = 0; i < Targets.Length; i++)
        {
            if (!Targets[i].gameObject.activeSelf)  //�ж�̹���Ƿ��ڼ���̬
            {
                continue;
            }
            AveragePosition += Targets[i].position;
            numTargets++;
        }
        if (numTargets > 0) //��������̹�˾�ƽ�������۽���ʤ��
        {
            AveragePosition /= numTargets;
        }

        AveragePosition.y = transform.position.y;   //����bug,CaermaRig��yһ��Ϊ0
        DesiredPosition = AveragePosition;
    }
    private void Zoom()
    {
        float requriedSize = FindRequiredSize();
        //ƽ������,orthographicsizeΪϵͳ���������
        Camera.orthographicSize = Mathf.SmoothDamp(Camera.orthographicSize, requriedSize,ref ZoomSpeed, DampTime);
    }
    private float FindRequiredSize()
    {
        Vector3 desiredLocalPosition = transform.InverseTransformPoint(DesiredPosition);

        float size = 0f;

        for(int i = 0; i < Targets.Length; i++)
        {
            if (!Targets[i].gameObject.activeSelf)
            {
                continue;
            }
            Vector3 TargetLocalPosition = transform.InverseTransformPoint(Targets[i].position);
            Vector3 desiredPosToTarget = TargetLocalPosition - desiredLocalPosition;
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / Camera.aspect);
        }

        size += ScreenEdgeBuffer;   //��ֹ̹����ʾ�ڱ�Ե
        size = Mathf.Max(size, MinSize);

        return size;
    }
    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = DesiredPosition;

        Camera.orthographicSize = FindRequiredSize();
    }
}
