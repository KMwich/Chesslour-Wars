using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour {


    public void OnClickStartSync()
    {
        PhotonNetwork.LoadLevel(2);
    }

    public void OnClickStartDelayed()
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.room.IsOpen = false;
            PhotonNetwork.room.IsVisible = false;
            PhotonNetwork.LoadLevel(2);
        }
        
    }
}
