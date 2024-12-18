using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraControl : MonoBehaviour
{
    
    [HideInInspector]public Transform[] Targets; //所有坦克对象
    public float DampTime = 0.2f;   //镜头过渡时间
    public float ScreenEdgeBuffer = 4f;     //加边缘，防止坦克显示在边缘
    public float MinSize = 6.5f;    //最小显示画面

    private Camera Camera;
    private float ZoomSpeed;
    private Vector3 DesiredPosition;    //两个坦克的中心点
    private Vector3 MoveVelocity;    //镜头移动速度
    

    private void Awake()
    {
        Camera = GetComponentInChildren<Camera>();  //获取子组件Camera,只有一个时可以这样写
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
        //平滑过渡
        transform.position = Vector3.SmoothDamp(transform.position, DesiredPosition, ref MoveVelocity, DampTime);
    }
    private void FindAveragePosition()
    {
        Vector3 AveragePosition = new Vector3();
        int numTargets = 0;
        
        for(int i = 0; i < Targets.Length; i++)
        {
            if (!Targets[i].gameObject.activeSelf)  //判断坦克是否处于激活态
            {
                continue;
            }
            AveragePosition += Targets[i].position;
            numTargets++;
        }
        if (numTargets > 0) //存在两个坦克就平均，最后聚焦到胜者
        {
            AveragePosition /= numTargets;
        }

        AveragePosition.y = transform.position.y;   //防御bug,CaermaRig的y一般为0
        DesiredPosition = AveragePosition;
    }
    private void Zoom()
    {
        float requriedSize = FindRequiredSize();
        //平滑过渡,orthographicsize为系统定义组件名
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

        size += ScreenEdgeBuffer;   //防止坦克显示在边缘
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
