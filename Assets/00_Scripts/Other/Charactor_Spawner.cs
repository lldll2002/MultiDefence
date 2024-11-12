using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactor_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawn_Prefab;
    [SerializeField] private Monster _spawn_Monster_Prefab; // 캐릭터, 몬스터의 스폰을 한 스크립트에서 관리함
    // Monster 라는 스크립트가 있으므로 사용할 수 있다


    List<Vector2> spawn_list = new List<Vector2>();
    // 스폰될 리스트를 저장하는 함수를 만듦
    // 처음엔 public 으로 보이게끔 만들어놓고, 나중에 지워서 inspector 가 깔끔하게 보이게 해준다
    List<bool> spawn_list_Array = new List<bool>();
    // 스폰된 위치의 리스트를 따로 만들어준다. Summon에서 위치값에 따라서 생성해야하니까
    // <bool>인 이유? 그 자리에 이미 생성 되어있는지 판단해서 true, false 로 구분해줘야 하니까
    public static List<Vector2> move_list = new List<Vector2>();
    // 몬스터가 각 모서리마다 이동하는 것을 리스트로 만들어서 관리
    // static 으로 설정 되었다면 Monster 가 타겟을 스스로 찾아낼 수 있다

    private void Start()
    {
        Grid_Start();

        for (int i = 0; i < transform.childCount; i++)
        // 해당스크립트를 가지고 있는 GameObject 의 자식 Object 숫자 만큼 하나씩 가져와서
        {
            move_list.Add(transform.GetChild(i).position); // 그 Child의 position 을 get해서 move_list 에 Add 한다
        }

        StartCoroutine(Spawn_Monster_Coroutine());
    }

    #region Make Grid
    private void Grid_Start()
    {
        SpriteRenderer parentSprite = GetComponent<SpriteRenderer>();
        // 부모 스프라이트의 스프라이트렌더러를 가져와서 x,y 값을 구할 수 있다 
        float parentwidth = parentSprite.bounds.size.x;
        float parentheight = parentSprite.bounds.size.y;

        float xCount = transform.localScale.x / 6;
        float yCount = transform.localScale.y / 3;
        for (int row = 0; row < 3; row++) // 상하 = 3개
        {
            for (int col = 0; col < 6; col++) // 좌우 = 6개
            {
                // // 하나의 오브젝트를 만들어줌
                // var go = new GameObject(string.Format("Grid{0}:{1}", row, col));
                // // {0}에 x, {1}에 y 만약에 {2} 가 추가되면 y, z 로 추가하면됨
                // go.transform.localScale = new Vector3(xCount, yCount, 1.0f);
                // 위의 내용들을 모두 정리해서 List로 만들거다

                float xPos = (-parentwidth / 2) + (col * xCount) + (xCount / 2);
                // x값 몇 번째 위치인지 나온다
                // parentwidth에 -를 곱한 이유는 게임뷰 상에서 오브젝트를 생성할 때
                // 0으로 하면 정가운데서 시작하는데, 그보다 왼쪽에서부터 소환해야 하기 때문에
                // -를 곱해서 거기부터 계산해준다
                float yPos = (parentheight / 2) - (row * yCount) + (yCount / 2);
                // y값 몇 번째 위치인지 나온다

                //go.transform.localPosition = 
                spawn_list.Add(new Vector2(xPos, yPos + transform.localPosition.y - yCount));
                // Position 값은 Scale 값에 더해주면 되는데 원래 값을 알아야함
                // Sprite Renderer 에 접근해서 값을 받아올 수 있음
                // yPos에 y의 로컬포지션값을 더해줘야 높이가 맞는다(Spawner의 위치가 -로 되어있으니까)
                // But transform.localPosition.y 값만 더해주게 되면 원래 가지고있던 Scale.y 값만큼 또 빼줘야함.
                spawn_list_Array.Add(false);
                // 현재 위치에 오브젝트가 들어와있는지 파악하는 함수
                // 만약에 false? 이 위치에 캐릭터 없으니까 소환 가능하다. 라는 뜻
            }
        }
    }
    #endregion

    #region 캐릭터 소환
    public void Summon()
    {
        if (Game_mng.instance.Money < Game_mng.instance.SummonCount)
        // Game_mng 에 갖고있는 Money 값이 SummonCount 값 보다 낮다면,
        {
            return; // 리턴하고,
        }

        Game_mng.instance.Money -= Game_mng.instance.SummonCount;
        // 아니라면 Game_mng 에 갖고있는 SummonCount 수 만큼 빼준다
        Game_mng.instance.SummonCount += 2;
        // 소환 할 때 마다 SummonCount 의 값을 2씩 올려준다

        int position_value = -1; // 변수값을 하나 만든다
        var go = Instantiate(_spawn_Prefab); // 프리펩을 생성(instantiate) 한다.
        for (int i = 0; i < spawn_list_Array.Count; i++)
        // for 문을 통해서 spawn_list_Array를 하.나.씩 (Count) 파악을 한다
        // i 를 0부터 스폰리스트의 숫자(Count) 만큼 증가시킨다
        {
            if (spawn_list_Array[i] == false)
            // 파악하던 도중에 만약에 spawn_list_Array중에 하나가 false 라면,
            {
                position_value = i;
                // 그 값을 position_value 안에 넣어서 반환하고
                spawn_list_Array[i] = true;
                // 채워진 위치는 true 로 바꿔준다
                break;
            }
        }
        go.transform.position = spawn_list[position_value];
        // 그렇게 들어간 값을 spawn_list안에서 해당 값의 위치를 가져오고,
        // 그 위치를 생성된 오브젝트의 position에 넣어준다.
    }
    #endregion

    #region 몬스터 소환

    // 특정시간마다 계속 생성 되어야 한다 -> 코루틴 사용
    IEnumerator Spawn_Monster_Coroutine()
    {
        var go = Instantiate(_spawn_Monster_Prefab, move_list[0], Quaternion.identity);
        // go.target = move_list[1]; // Monster 스크립트에서 target과 move_list를 받아올 수 있어서 쓸 수 있다
        // List 를 static 으로 작성해서 타겟을 스스로 찾아낼 수 있으므로 삭제한다

        Game_mng.instance.AddMonster(go);
        // go 라는 이름의 몬스터를 넣어준다, 임의로
        yield return new WaitForSeconds(1.0f);
        // cpu에 점유를 해서 0.5초의 대기시간을 갖고 이후에 다시 스타트코루틴을 실행함
        // 0.3초에 한 번씩 소환하는 구문을 반복하는 것

        StartCoroutine(Spawn_Monster_Coroutine());
    }


    #endregion


}
