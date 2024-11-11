using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public delegate void OnMoneyUpEventHandler();
// 이벤트 형식으로 작동시킴
// 안에 여러가지 함수를 넣을 수 있는데, 이번엔 이벤트 형식을 넣음

public class Game_mng : MonoBehaviour
{
    // 싱글톤 패턴으로 만듦
    public static Game_mng instance = null;
    // static 으로 만들어줬기 때문에 어느 스크립트에서도 접근이 가능하다

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // public int MonsterCount;
    public int Money = 50;
    public int SummonCount = 20;

    public event OnMoneyUpEventHandler OnMoneyUp;

    public List<Monster> monsters = new List<Monster>();
    // monster 를 add 로 list에 더해주고, remove로 해당 몬스터를 찾아서 제외한다

    #region 몬스터 추가/제거

    public void AddMonster(Monster monster)
    {
        monsters.Add(monster);
    }

    public void RemoveMonster(Monster monster)
    {
        monsters.Remove(monster);
    }
    #endregion

    #region Money

    public void GetMoney(int value)
    {
        Money += value;
        // Money 의 value 만큼 올려준다
        OnMoneyUp?.Invoke();
        // Money를 얻었을 때, Invoke 를 이용해서 해당 Evnet를 실행해준다    
    }

    #endregion
}
