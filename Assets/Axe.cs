using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    private bool isColliding = false;
    [SerializeField] private Transform head;
    [SerializeField] private Transform tip;
    
    private void Update() 
    {   
        Vector3 diff = tip.position - head.position; 
        Debug.DrawRay(head.position, diff * 100, Color.red, 0.1f);
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (isColliding) return;
        // return if not player is attacking
        
        isColliding = true;

        Vector3 axeVector = head.transform.position - tip.transform.position;
        axeVector = axeVector.normalized;

        float damage = 0f;
        int cnt = 0;

        // 충돌한 모든 접촉점(Contact Point)을 반복
        foreach (ContactPoint contact in collision.contacts)
        {
            // 충돌한 지점의 위치
            Vector3 collisionPoint = contact.point;
            // 충돌한 지점의 법선
            Vector3 collisionNormal = contact.normal;

            // 디버그용으로 충돌 지점과 법선을 시각적으로 표시
            Debug.DrawRay(collisionPoint, collisionNormal * 100, Color.red, 10f);
            
            // 법선 정보를 로그로 출력
            Debug.Log("Collision Point: " + collisionPoint);
            Debug.Log("Collision Normal: " + collisionNormal);

            damage += Vector3.Dot(axeVector, contact.normal);
            cnt++;
        }

        damage /= (float) cnt;
        Debug.Log(damage);

        isColliding = false;
    }
}
