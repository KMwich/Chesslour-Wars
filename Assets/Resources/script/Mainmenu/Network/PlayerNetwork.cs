using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class PlayerNetwork : MonoBehaviour {

    public static PlayerNetwork Instance;
    public string PlayerName { get; private set; }
    private PhotonView PhotonView;
    private int PlayersInGame = 0;


    private void Awake()
    {
        Instance = this;
        PhotonView = GetComponent<PhotonView>();

        PlayerName = "Player" + Random.Range(1000, 9999);

        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 30;

        SceneManager.sceneLoaded += OnSceneFinishedLoading;

    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "select scene")
        {
            if (PhotonNetwork.isMasterClient)
                MasterLoadedGame(2);
            else
                NonMasterLoadedGame();
        }
        if (scene.name == "map")
        {
            if (PhotonNetwork.isMasterClient)
                MasterLoadedGame(3);
            else
                NonMasterLoadedGame();
        }
    }

    private void MasterLoadedGame(int i)
    {
        PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient);
        PhotonView.RPC("RPC_LoadGameOthers", PhotonTargets.Others,i);
    }

    private void NonMasterLoadedGame()
    {
        PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient);
    }

    [PunRPC]
    private void RPC_LoadGameOthers(int i)
    {
        PhotonNetwork.LoadLevel(i);
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        PlayersInGame++;
        if (PlayersInGame == PhotonNetwork.playerList.Length)
        {
            print("All players are in the game scene.");
            //PhotonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
        }
    }



    [PunRPC]
    private void RPC_CreatePlayer()
    {
        //float randomValue = Random.Range(0f, 5f);
        Quaternion euler = Quaternion.Euler(90, 0, 0);
        print("OK");
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "NewPlayer"), new Vector3(0, 0, 1), euler, 0);
    }
}
