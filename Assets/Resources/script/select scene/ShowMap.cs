using UnityEngine.UI;
using UnityEngine;

public class ShowMap : MonoBehaviour {

    [SerializeField]
    private GameObject _map;
    private GameObject Map
    {
        get { return _map; } set { _map = value; }
    }


    private void Awake()
    {
        Map.GetComponent<Image>().sprite = Resources.Load<Sprite>(MapSelect.Instance.MapLists.SpritePath_img[DDOL.mapIndex - 1]);
        //print(DDOL.mapIndex - 1);
        //print(MapSelect.Instance.MapLists.SpritePath_img[DDOL.mapIndex - 1]);
    }
}
