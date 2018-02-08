using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MapSelect : Photon.MonoBehaviour {

    [SerializeField]
    private GameObject _map;
    private GameObject Map
    {
        get { return _map; } set { _map = value; }
    }

    //[SerializeField]
    private MapLists  _mapLists;
    public MapLists  MapLists
    {
        get { return _mapLists; }
        set { _mapLists = value; }
    }


    private int index;
    private int indexC;
    private PhotonView PhotonView;
    public static MapSelect Instance;


    private void Awake()
    {

        string data = File.ReadAllText("Assets/Resources/database/mapSelect.json");
        //print(data);
        MapLists = JsonUtility.FromJson<MapLists>(data);
        index = 0;
        indexC = 0;
        Instance = this;
        PhotonView = GetComponent<PhotonView>();
        //if (PhotonNetwork.isMasterClient == false)
        //{
        //    Map.GetComponent<Transform>().Find("Next").gameObject.GetComponent<Image>().sprite = null;
        //    Map.GetComponent<Transform>().Find("Back").gameObject.GetComponent<Image>().sprite = null;

        //}
    }

    private void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
            MapRefresh(index);
        }
        else
        {
            MapRefresh(indexC);
        }
    }

    private void MapRefresh(int i)
    {
        Map.GetComponent<Transform>().Find("Text").gameObject.GetComponent<Text>().text = MapLists.Name[i];
        Map.GetComponent<Image>().sprite = Resources.Load<Sprite>(MapLists.SpritePath_img[i]);
        DDOL.mapIndex = MapLists.Index[i];
    }

    public void OnClickNext()
    {
        if (CurrentRoomCanvas.ready == 1 || CurrentRoomCanvas.readyState == 2) { return; }
        if (!PhotonNetwork.isMasterClient)
            return;
        index++;
        if(index >= MapLists.Index.Length)
        {
            index = 0;
        }
    }

    public void OnClickBack()
    {
        if (CurrentRoomCanvas.ready == 1 || CurrentRoomCanvas.readyState == 2) { return; }
        if (!PhotonNetwork.isMasterClient)
            return;
        index--;
        if (index < 0)
        {
            index = MapLists.Index.Length -1;
        }
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
                stream.SendNext(index);
        }
        else
        {
                indexC = (int)stream.ReceiveNext();
        }
    }
}