using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Photon.MonoBehaviour {
    HexCoordinates coordinate;
 //   public HexGrid map;
    int attackRange = 1;
    bool move = false;
    private Vector3 TargetPosition;
    private Quaternion TargetRotation;
    private PhotonView PhotonView;
    public bool canDrag = true;

    // Use this for initialization
    void Awake () {
        PhotonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update() {
        if (PhotonView.isMine)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (move)
                {
                    int dist = HexCoordinates.cubeDeistance(coordinate, UnitManager.Instance.HexGrid.getTouchCoordinate());
                    if (dist != 0 && dist <= attackRange)
                        setUnitPosition(HexCoordinates.cubeToOffset(UnitManager.Instance.HexGrid.getTouchCoordinate()));
                }
                if (coordinate.Equals(UnitManager.Instance.HexGrid.getTouchCoordinate()))
                {
                    UnitManager.Instance.HexGrid.setHexFilter(coordinate, 1);
                    move = true;
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                UnitManager.Instance.HexGrid.clearHexFilter();
                move = false;
            }
        }
        
    }

    public void setUnitPosition(Vector3 position) {
        coordinate = HexCoordinates.FromOffsetCoordinates((int)position.x, (int)position.z);
        position.x = position.x * (HexMetrics.outerRadius * 1.5f);
        position.y = 2;
        position.z = (position.z + position.x * 0.5f - position.x / 2) * (HexMetrics.innerRadius * 2f);
        transform.localPosition = position;
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            TargetPosition = (Vector3)stream.ReceiveNext();
            TargetRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    public void asd() {
        Debug.Log( this.GetComponentInParent<UnitBar>().name);
    }

}
