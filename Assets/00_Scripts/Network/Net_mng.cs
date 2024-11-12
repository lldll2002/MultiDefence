using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class Net_mng : MonoBehaviour
{
    // 방, Lobby 가 있어야함 -> 플레이어가 원하는 게임을 찾거나, 새 게임을 만들고 대기할 수 있는 기능
    // Relay -> 매칭된 플레이어들의 Relay의 Join Code 로 연결되어, 호스트-클라이언트 방식으로 실시간 멀티플레이 환경을 유지
    private Lobby currentLobby;

    private async void Start()
    // async 비동기 -> 동시에 일어나지 않는다
    // 요청이 하나 진행이 되고, 그에 대한 결과가 하나 나오는데 그게 동시에 일어나지 않음
    // 요청이 완료될 때 까지 결과값이 나오지 않는다
    // 웹에 요청을 했을 때, 결과값을 찾고 다시 돌아오는 딜레이가 있을 테니 그 때 사용함
    {
        await UnityServices.InitializeAsync(); // 초기화
        if (!AuthenticationService.Instance.IsSignedIn)
        // 서비스 내부에 로그인이 되어있지 않다면
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            // 서비스에 로그인 해라
            // await 니까, 결과값이 나올 때 까지 계속해서 요청
        }
    }

    public async void StartMatchmaking()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        // 로그인이 되어있지 않다면
        {
            Debug.Log("로그인되지 않았습니다.");
            return;
            // 디버그 처리하고 되돌림
        }

        // currentLobby = await FindAvailableLobby();
    }

    // private async Task<Lobby> FindAvailableLobby()
    // {
    //     // try,catch 예외처리
    //     try
    //     // try 안쪽에서 실행되는 해당 내용들이 실행되고, 만약 실행이 안되면 catch안쪽에 있는 것이 실행된다
    //     {
    //         var queryResponse = await LobbyService.Instance.QueryLobbiesAsync();
    //         if (queryResponse.Results.Count > 0)
    //         {
    //             return queryResponse.Results[0];
    //         }
    //     }
    //     catch
    //     {

    //     }
    // }
}
