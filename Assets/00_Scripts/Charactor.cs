using UnityEngine;

public class Charactor : MonoBehaviour
{
    protected Animator animator;
    protected SpriteRenderer renderer;
    // protected는 상속하는 부모 스크립트에서 하위 자식 스크립트가 사용할 수 있게끔 처리

    public virtual void Start()
    // virtual 로 작성하면 다른 스크립트에서 override 로 상속시킬 수 있다
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        // Hero와 Monster 프리펩들이 첫 번째 Child 에 Animator를 가지고 있으므로,
        // 그 child의 Animator Component를 가져온다
        // 이후 Hero 와 Monster 에 각각 상속시켜준다
        renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        // 위와 동일하게 Child에 SpriteRenderer 있으므로 가져온다
    }

    protected void AnimatorChange(string temp, bool Trigger)
    // 몬스터와 캐릭터에 모두 사용할 것이기 때문에 protected로 작성
    {
        if (Trigger)
        {
            animator.SetTrigger(temp);
        }
        else animator.SetBool(temp, true);
    }

    // void Update()
    // {

    // }
}
