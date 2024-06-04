using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
    //摄像机要看向的目标对象
    private Transform target;
    //偏移位置，摄像机相对目标对象的偏移
    public Vector3 offsetPos;
    //看向位置的y偏移值
    public float bodyHeight;
    //移动和旋转速度
    public float moveSpeed;
    public float rotateSpeed;
    //摄像机位置
    private Vector3 targetPos;
    //摄像机旋转角度
    private Quaternion targerAngle;
    void Update() {
        //根据目标对象计算摄像机位置和角度
        if(target == null)
            return;
        //z偏移
        targetPos = target.position + target.forward * offsetPos.z;
        //y
        targetPos += Vector3.up * offsetPos.y;
        //x
        targetPos += target.right * offsetPos.x;
        //插值运算，让摄像机不停的向目标点靠拢
        this.transform.position = Vector3.Lerp(this.transform.position,targetPos,moveSpeed * Time.deltaTime);
        //旋转计算
        targerAngle = Quaternion.LookRotation(target.position + Vector3.up * bodyHeight - this.transform.position);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,targerAngle,rotateSpeed * Time.deltaTime);
    }
    public void SetTarget(Transform player) {
        target = player;
    }
}
