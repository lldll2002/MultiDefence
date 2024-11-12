using System.Collections;
using TMPro;
using UnityEngine;

public class HitText : MonoBehaviour
{
    [SerializeField] private float floatSpeed; // Text 가 올라가는 속도
    [SerializeField] private float riseDuration = 1.0f; // Text 가 유지되는 시간
    [SerializeField] private float fadeDuration = 1.0f; // Text 가 투명해지는 데 걸리는 시간
    public Vector3 offset = new Vector3(0, 2, 0); // Text 가 올라가는 거리

    public TextMeshPro damageText;
    private Color textColor;

    public void Initalize(int dmg)
    {
        damageText.text = dmg.ToString();
        textColor = damageText.color;
        StartCoroutine(MoveAndFaed());
    }


    IEnumerator MoveAndFaed()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + offset; // 시작된 위치로부터 offset 위치까지

        float elapsedTime = 0; // 시간을 저장할 변수

        while (elapsedTime < riseDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / riseDuration);
            // .Lerp 선형보간법, start~ 부터 end~ 까지 천천히 이동을 함, 걸리는 시간동안
            // .Slerp 회전에 대한 선형보간법

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            textColor.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            // a 값을 .Lerp를 이용해서 1에서 0으로 옮겨줌, 걸리는 시간동안
            damageText.color = textColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
