using TMPro;
using UnityEngine;

public class UI_Main : MonoBehaviour
{
    // Summon 버튼 안의 텍스트, 보유한 제화, 몬스터 카운트를 받아옴
    [SerializeField] private TextMeshProUGUI MonsterCount_T;
    [SerializeField] private TextMeshProUGUI Money_T;
    [SerializeField] private TextMeshProUGUI Summon_T;

    [SerializeField] private Animator MoneyAnimation;

    private void Start()
    {
        Game_mng.instance.OnMoneyUp += Money_Anim;
        // delegate 함수를 써서 만들었기 때문에 다른 곳에서 불러와서 함수를 실행시킬 수 있다
    }

    private void Update()
    {
        MonsterCount_T.text = Game_mng.instance.monsters.Count.ToString() + " / 100";
        // Game_mng 에 있는 monsters 의 count를 String 형변환하여 불러온다
        Money_T.text = Game_mng.instance.Money.ToString();
        Summon_T.text = Game_mng.instance.SummonCount.ToString();

        Summon_T.color = Game_mng.instance.Money >= Game_mng.instance.SummonCount ? Color.white : Color.red;
        // 가지고 있는 돈이, 소환에 필요한 돈보다 높다면은 색을 하얀색으로 표현하고, 아니면 빨간색으로 표현한다

    }

    private void Money_Anim()
    {
        MoneyAnimation.SetTrigger("GET");
        // Trigger의 "GET" 이 작동했을 때, MoneyAnimaiton 을 작동시킨다
    }
}
