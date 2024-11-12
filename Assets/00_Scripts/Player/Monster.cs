using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;


public class Monster : Charactor
// Charactor 스크립트를 상속받겠다
{
    [SerializeField] private float m_Speed; // 속도를 제어할 m_Speed
    [SerializeField] private HitText hitText; // 대미지를 표현할 HitText
    [SerializeField] private Image m_Fill, m_Fill_Deco;

    // public Vector2 target; // Update에 자리지정을 해주기 위해서 사용함
    int target_Value = 0; // target은 스스로 찾을 수 있게 되었으니 위치값만 넣어주면 된다
    public int HP = 0, MaxHp = 0; // 몬스터의 체력을 관리

    bool isDead = false;

    public override void Start()
    // Charactor 부모 스크립트를 virtual 로 작성했으므로
    // override 로 데려올 수 있다(상속 받을 수 있다)
    {
        HP = MaxHp; // MaxHp를 지정해주면 자동적으로 HP 의 값도 바뀐다
        base.Start();
        // 부모인 Charactor의 Start를 받아온다
    }

    // 몬스터의 자리를 지정해줌
    private void Update()
    {
        m_Fill_Deco.fillAmount = Mathf.Lerp(m_Fill_Deco.fillAmount, m_Fill.fillAmount, Time.deltaTime * 2.0f);
        // 체력바의 빨간색이 줄어들면 시간차를 두고 하얀색이 줄어들도록 꾸미는 함수

        if (isDead) return; // DEAD 상태가 되면 더이상 Update문을 실행하지 않음
        // if (target == null) return; // 타겟이 없다면 해당 Update 를 실행하지 않음
        // Charactor 스크립트에서 move_list를 static으로 처리했기 때문에 target을 스스로 찾을 수 있으므로 삭제한다
        transform.position = Vector2.MoveTowards(transform.position, Charactor_Spawner.move_list[target_Value], Time.deltaTime * m_Speed);
        // 2번째 인자에 Charactor_Spawner~ 는 Charactor_Spawner 스크립트의 movce_list 를 받아와서,
        // 위에서 지정한 int target_Value 의 값을 찾아서 간다
        if (Vector2.Distance(transform.position, Charactor_Spawner.move_list[target_Value]) <= 0.0f)
        // Vector2.Distance 두 개의 위치값이 들어갔을 경우에, 그 값을 float 형태로 나눠준다 가까울수록 줄어듦
        {
            target_Value++;
            renderer.flipX = target_Value >= 3 ? true : false;
            // Charactor 스크립트에서 protected 로 지정해준 renderer 변수를 가져온다
            // 3항연산자. 3보다 높을 때는 true, 아니면 false
            // Spawner 에서 스폰위치가 3번일 때 x축을 뒤집는다
            if (target_Value >= 4)
            {
                target_Value = 0;
            }
        }
    }

    public void GetDamage(int dmg)
    {
        if (isDead) return;

        HP -= dmg;
        m_Fill.fillAmount = (float)HP / (float)MaxHp; // (float로 형변환)체력바의 체력을 image를 통해 시각화해준다
        // float 로 형변환을 해줘야 fillAmount가 정상적으로 작동한다. 유니티의 고질적인 오류
        Instantiate(hitText, transform.position, UnityEngine.Quaternion.identity).Initalize(dmg);

        if (HP < 0)
        {
            isDead = true;
            Game_mng.instance.GetMoney(1);
            // monster가 사망했을 때, 돈을 GetMoney 함수의 (1)만큼 추가함
            Game_mng.instance.RemoveMonster(this);
            // 싱글톤으로 작성된 Game_mng 스크립트의 RemoveMonster 함수를 가져와서 'this' monster를 제외한다
            gameObject.layer = LayerMask.NameToLayer("Default");
            // Monster가 체력이0이 되어도 Hero가 공격하면서 자꾸 애니매이션이 재생되기 때문에 아예 layer를 Default로 바꿔줌
            // 이미 Monster 라는 layer를 따로 지정을 해줬기 때문에 가능한 설정
            AnimatorChange("DAED", true);
            StartCoroutine(Dead_Coroutine());
            AnimatorChange("DEAD", true);
        }
    }

    IEnumerator Dead_Coroutine()
    // Monster 가 사망했는데 그 자리에 그대로 있는게 보기 싫으니까 없애주는 함수
    {
        float Alpha = 1.0f;

        while (renderer.color.a > 0.0f)
        {
            Alpha -= Time.deltaTime; // 초당 알파값이 감소한다
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, Alpha);
            // Alpha 변수가 0이 될 때 까지 반복됨

            yield return null;
            // 한 프레임을 대기하라
        }

        Destroy(this.gameObject);
    }
}
