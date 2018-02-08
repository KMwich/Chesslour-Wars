using UnityEngine;
using UnityEngine.UI;

public class CurrentRoomCanvas : MonoBehaviour {

    public static int readyState = 0;
    public static int ready = 0;
    PhotonView PhotonView;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        //print(readyState);
        if(readyState == 2)
        {
            this.GetComponent<Transform>().Find("StartMatch").gameObject.GetComponent<Transform>().Find("Text").gameObject.GetComponent<Text>().text = "Start Game";
            ready = 0;
        }
        //this.GetComponent<Transform>().Find("StartMatch").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(MapLists.SpritePath_img[i]);
    }

    public void OnClickStartSync()
    {
        PhotonNetwork.LoadLevel(2);
    }

    public void OnClickStartDelayed()
    {
        if (ready == 1) { return; }
        if (readyState != 2)
        {
            this.GetComponent<Transform>().Find("StartMatch").gameObject.GetComponent<Transform>().Find("Text").gameObject.GetComponent<Text>().text = "Waiting";
            ready++;
            PhotonView.RPC("updateRS", PhotonTargets.MasterClient);
        }
        else
        {
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.room.IsOpen = false;
                PhotonNetwork.room.IsVisible = false;
                PhotonNetwork.LoadLevel(2);
            }
        }
    }

    [PunRPC]
    private void updateRS()
    {
        readyState++;
    }
}
