using System.Collections;
using UnityEngine;

public class Hero : Charactor
// Charactor 를 상속받음
{
    public float attackRange = 1.0f; // 공격범위
    public float attackSpeed = 1.0f; // 공격속도
    public Monster target; // 타겟 중 가장 가까운 것을 공격하게끔
    public LayerMask enemyLayer; // Monster 의 Layer만 추적해야하기 때문에 따로 넣음
    // Monster 프리펩에 콜라이더를 넣어서 근처에 다가오면 공격하게끔 만듦
    // Hero 에 넣는 방법도 있지만 여기서는 그렇게 하지 않음

    private void Update() // 매 프레임마다 콜라이더를 추적하기 위해 Update를 사용함
    {
        CheckForEnemies();
    }


    void CheckForEnemies()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        attackSpeed += Time.deltaTime; // 1초에 1만큼의 값을 넣어줌
        // 2D 콜라이더를 추적함
        // Physics2D 2D 충돌체를 분별을 함, OverlapCircleAll 보이지 않는 원형의 Circle을 생성함
        // 그 Circle은 현재 위치를 기준으로 공격범위만큼, Enemy라는 특정 Layer를 판별함

        if (enemiesInRange.Length > 0)
        // 
        {
            target = enemiesInRange[0].GetComponent<Monster>();
            if (attackSpeed >= 1.0f)
            // 공격속도가 1.0f 보다 클 경우
            {
                attackSpeed = 0.0f;
                // 공격속도를 0으로 만들고
                AttackEnemy(target);
                // target 이 있을 경우에 AttackEnemy를 실행하는 함수
                // 0보다 클 경우 범위 안에 있는  target 중 제일 가까운 것을 넣는다
                // 1초마다 로그를 남긴다

            }
        }
        else
        {
            target = null;
            // 아니라면, target을 null로
        }
    }

    void AttackEnemy(Monster enemy)
    {
        AnimatorChange("ATTACK", true);
        // 범위 안에 들어오면 공격 하는지를 확인하기 위해서 넣었음
        // Debug.Log(enemy.name + "를(을) 공격합니다.");
        enemy.GetDamage(10);
        // Monster 스크립트에서 GetDamage함수를 가져온다
    }

    private void OnDrawGizmosSelected() // 눈에 가시적으로 보이게끔 만들어주기
    // 기즈모는 유니티 내부에서만 보이고 빌드하고서는 보이지 않음
    // 개발자들이 확인하기 위해서 사용함
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
