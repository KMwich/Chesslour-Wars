using UnityEngine;
using UnityEngine.UI;


public class ChangeToScene : MonoBehaviour {
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
        if (readyState == 2)
        {
            this.GetComponent<Transform>().Find("background").gameObject.GetComponent<Transform>().Find("go to play map").gameObject.GetComponent<Transform>().Find("Text").gameObject.GetComponent<Text>().text = "Start Game";
            ready = 0;
        }
        //this.GetComponent<Transform>().Find("StartMatch").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(MapLists.SpritePath_img[i]);
    }

    public void LoadToScene(int indexScene)
    {
        if (ready == 1) { return; }
        if (readyState != 2)
        {
            this.GetComponent<Transform>().Find("background").gameObject.GetComponent<Transform>().Find("go to play map").gameObject.GetComponent<Transform>().Find("Text").gameObject.GetComponent<Text>().text = "Waiting";
            ready++;
            PhotonView.RPC("updateRS", PhotonTargets.MasterClient);
        }
        else
        {
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.room.IsOpen = false;
                PhotonNetwork.room.IsVisible = false;
                PhotonNetwork.LoadLevel(indexScene);
            }
        }
    }

    [PunRPC]
    private void updateRS()
    {
        readyState++;
    }



}
