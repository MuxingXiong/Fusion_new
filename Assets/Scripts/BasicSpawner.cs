using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField]
    private NetworkRunner networkRunner = null;

    [SerializeField]
    private NetworkPrefabRef playerPrefab;

    private Dictionary<PlayerRef, NetworkObject> playerList = new Dictionary<PlayerRef, NetworkObject>();

    private void Start()
    {
        StartGame(GameMode.AutoHostOrClient);
    }
    async void StartGame(GameMode mode)
    {
        networkRunner.ProvideInput = true;

        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "Fusion Room",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnConnectedToServer(NetworkRunner runner){}

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){}

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token){}

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){ }

    public void OnDisconnectedFromServer(NetworkRunner runner){}

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){}

    public void OnInput(NetworkRunner runner, NetworkInput input){
        var data = new NetworkInputData();
        data.mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        data.buttons.Set(InputButtons.W,Input.GetKey(KeyCode.W));
        data.buttons.Set(InputButtons.A, Input.GetKey(KeyCode.A));
        data.buttons.Set(InputButtons.S, Input.GetKey(KeyCode.S));
        data.buttons.Set(InputButtons.D, Input.GetKey(KeyCode.D));
        data.buttons.Set(InputButtons.Mouse0, Input.GetKey(KeyCode.Mouse0));
        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){}

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Vector3 spawnPosition = new Vector3(0,0,0);
        NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
        playerList.Add(player, networkPlayerObject);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (playerList.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            playerList.Remove(player);
        }
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data){}

    public void OnSceneLoadDone(NetworkRunner runner){}

    public void OnSceneLoadStart(NetworkRunner runner){}

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){}

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason){}

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){}
}
