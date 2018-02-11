using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Unit : Photon.MonoBehaviour
{
    private Unit Instance;

    private Vector3 TargetPosition;
    private Quaternion TargetRotation;
    private string TargetSprite;
    private Vector3 TargetScale;
    private bool TargetIsTower;
    private int TargetMaxHp;
    private int TargetAtk;
    private int TargetDef;
    private int TargetMovement;
    private int TargetAtkRange;
    private int TargetDamage;
    private int TargetAtkBuff;
    private int TargetDefBuff;
    private int TargetMovementBuff;
    private int TargetAtkRangeBuff;
    private PhotonView PhotonView;

    public int atkbuff;
    private int atkbuffcountTurn;
    public int defbuff;
    private int defbuffcountTurn;
    public int movementbuff;
    private int movementbuffcountTurn;
    public int atkrangebuff;
    private int atkrangebuffcountTurn;


    public int[] cooldownSkill = { 0, 0 };

    public bool havePlayed = false;
    public bool haveMoved = false;

    private string _spritePath;
    public string SpritePath
    {
        get { return _spritePath; }
        set { _spritePath = value; }
    }

    public HexCoordinates coordinate;
    public bool isTower = false;
    public unitStructure structure;
    public int Damage;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        PhotonView = GetComponent<PhotonView>();
        Vector3 S = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().size = S;

        if (!PhotonView.isMine)
        {
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.0f, 8.0f, 1.0f);
        }
    }

    // Update is called once per frame
    void Update() {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "map")
        {
            if (PhotonView.isMine)
            {
                //code start here

                if (StatusControl.Instance.active == false)
                {
                    haveMoved = false;
                    havePlayed = false;
                }

                if (isTower) return;

                if (!UnitBar.Instance.isPlay)
                {
                    if (!this.Equals(UnitBar.Instance.selectUnit)) return;
                    if (PhotonView.isMine)
                    {
                        UnitBar.Instance.selectUnit = this;
                        Vector3 dist = Camera.main.WorldToScreenPoint(transform.position);
                        Vector3 curPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist.z);
                        Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
                        transform.position = worldPos;
                    }
                    return;
                }
                else
                {
                    //move code

                    if (!this.Equals(UnitBar.Instance.selectUnit)) return;

                    if (UnitBar.Instance.action == 1)
                    {
                        if (haveMoved == true || havePlayed == true) { print("move already"); return; }
                        if (Input.GetMouseButtonUp(0))
                        {
                            if (UnitBar.Instance.action == 1)
                            {
                                int dist = HexCoordinates.cubeDeistance(coordinate, UnitBar.Instance._hexGrid.getTouchCoordinate());
                                if (dist != 0 && dist <= structure.Movement + movementbuff)
                                {
                                    HexCoordinates touchCoordinate = UnitBar.Instance._hexGrid.getTouchCoordinate();
                                    if (!UnitBar.Instance.moveArea.Contains(touchCoordinate) || !canSetPosition(touchCoordinate)) return;
                                    coordinate = touchCoordinate;
                                    setUnitPosition(HexCoordinates.cubeToOffset(coordinate));
                                    UnitBar.Instance.clearSelectUnit();
                                    haveMoved = true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                transform.position = TargetPosition;
                coordinate = HexCoordinates.FromPosition(TargetPosition);
                transform.rotation = TargetRotation;
                SpritePath = TargetSprite;
                structure.SpritePath_img = TargetSprite;
                structure.Hp = TargetMaxHp;
                structure.Atk = TargetAtk;
                structure.Def = TargetDef;
                structure.Movement = TargetMovement;
                structure.Atkrange = TargetAtkRange;
                Damage = TargetDamage;
                atkbuff = TargetAtkBuff;
                defbuff = TargetDefBuff;
                movementbuff = TargetMovementBuff;
                atkrangebuff = TargetAtkRangeBuff;
                this.setUnitSprite(TargetSprite);
                if (UnitBar.Instance.isPlay)
                {
                    transform.localScale = TargetScale;
                    if (TargetIsTower)
                    {
                        if (!UnitBar.Instance.enemyTowers.Contains(this)) { UnitBar.Instance.enemyTowers.Add(this); }
                    }
                    else
                    {
                        if (!UnitBar.Instance.enemyUnits.Contains(this)) { UnitBar.Instance.enemyUnits.Add(this); }
                    }
                    this.transform.SetParent(UnitBar.Instance.transform);
                }
                else
                {
                    if (TargetIsTower)
                    {
                        isTower = TargetIsTower;
                        transform.localScale = TargetScale;
                    }
                }
            }
            if (StatusControl.Instance.turn == atkbuffcountTurn)
            {
                PhotonView.RPC("setBuff", PhotonTargets.All, 0, 0, 0, "atk");
            }
            if (StatusControl.Instance.turn == defbuffcountTurn)
            {
                PhotonView.RPC("setBuff", PhotonTargets.All, 0, 0, 0, "def");
            }
            if (StatusControl.Instance.turn == movementbuffcountTurn)
            {
                PhotonView.RPC("setBuff", PhotonTargets.All, 0, 0, 0, "movement");
            }
            if (StatusControl.Instance.turn == atkrangebuffcountTurn)
            {
                PhotonView.RPC("setBuff", PhotonTargets.All, 0, 0, 0, "attack range");
            }

            for (int i = 0; i < 2; i++) { 
                if (StatusControl.Instance.turn == cooldownSkill[i])
                {
                    cooldownSkill[i] = 0;
                }
            }
        }
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(SpritePath);
            stream.SendNext(isTower);
            stream.SendNext(transform.localScale);
            stream.SendNext(structure.Hp);
            stream.SendNext(structure.Atk);
            stream.SendNext(structure.Def);
            stream.SendNext(structure.Movement);
            stream.SendNext(structure.Atkrange);
            stream.SendNext(Damage);
            stream.SendNext(atkbuff);
            stream.SendNext(defbuff);
            stream.SendNext(movementbuff);
            stream.SendNext(atkrangebuff);

        }
        else
        {
            TargetPosition = (Vector3)stream.ReceiveNext();
            TargetRotation = (Quaternion)stream.ReceiveNext();
            TargetSprite = (string)stream.ReceiveNext();
            TargetIsTower = (bool)stream.ReceiveNext();
            TargetScale = (Vector3)stream.ReceiveNext();
            TargetMaxHp = (int)stream.ReceiveNext();
            TargetAtk = (int)stream.ReceiveNext();
            TargetDef = (int)stream.ReceiveNext();
            TargetMovement = (int)stream.ReceiveNext();
            TargetAtkRange = (int)stream.ReceiveNext();
            TargetDamage = (int)stream.ReceiveNext();
            TargetAtkBuff = (int)stream.ReceiveNext();
            TargetDefBuff = (int)stream.ReceiveNext();
            TargetMovementBuff = (int)stream.ReceiveNext();
            TargetAtkRangeBuff = (int)stream.ReceiveNext();
        }
    }

    private void OnMouseDown() {
        if (UnitBar.Instance.ready == 1) return;
        //if not select set this to select
        if (UnitBar.Instance.selectUnit == null) {
            UnitBar.Instance.selectUnit = this;
            return;
        }

        if (!UnitBar.Instance.isPlay) {
            if (isTower) return;
            if (PhotonView.isMine) {
                HexCoordinates hexPosition = UnitBar.Instance._hexGrid.getTouchCoordinate();

                if (!canSetPosition(hexPosition)) {
                    transform.position = new Vector3(-1920, -1920, -4);
                    coordinate = new HexCoordinates(0, 0);
                    SetUnitBar.Instance.cantSetUnit(UnitBar.Instance.Units.IndexOf(this));
                    UnitBar.Instance.selectUnit = null;
                    return;
                }
                coordinate = hexPosition;
                Vector3 position = HexCoordinates.cubeToOffset(hexPosition);
                setUnitPosition(position);
                UnitBar.Instance.selectUnit = null;
            }
        } else {
            if (PhotonView.isMine)
            {
                if (UnitBar.Instance.action == 4)
                {
                    int dist = HexCoordinates.cubeDeistance(UnitBar.Instance.selectUnit.coordinate, coordinate);
                    if (dist <= UnitBar.Instance.selectSkill.SkillRange)
                    {
                        activeBuffSkill();
                        UnitBar.Instance.selectUnit.havePlayed = true;
                        UnitBar.Instance.selectUnit.haveMoved = true;
                        StatusControl.Instance.ActionPoints -= 1;
                        UnitBar.Instance.clearSelectUnit();
                    }
                }
            }
            if (!PhotonView.isMine) {
                if (UnitBar.Instance.action == 2) {
                    int dist = HexCoordinates.cubeDeistance(UnitBar.Instance.selectUnit.coordinate, coordinate);
                    if (dist == UnitBar.Instance.selectUnit.structure.Atkrange + atkrangebuff) {
                        PhotonView.RPC("damaged", PhotonTargets.All, UnitBar.Instance.selectUnit.structure.Atk + UnitBar.Instance.selectUnit.atkbuff);
                        print("Damage : "+ Damage);
                        if (structure.Hp - Damage <= 0)desTroyUnit();
                        UnitBar.Instance.selectUnit.havePlayed = true;
                        UnitBar.Instance.selectUnit.haveMoved = true;
                        StatusControl.Instance.ActionPoints -= 1;
                        UnitBar.Instance.clearSelectUnit();
                    }
                }

                if (UnitBar.Instance.action == 3)
                {
                    int dist = HexCoordinates.cubeDeistance(UnitBar.Instance.selectUnit.coordinate, coordinate);
                    if (dist == UnitBar.Instance.selectSkill.SkillRange)
                    {
                        PhotonView.RPC("damaged", PhotonTargets.All, UnitBar.Instance.selectUnit.structure.Atk + UnitBar.Instance.selectUnit.atkbuff);
                        print("Damage : " + Damage);
                        if (structure.Hp - Damage <= 0) desTroyUnit();
                        UnitBar.Instance.selectUnit.havePlayed = true;
                        UnitBar.Instance.selectUnit.haveMoved = true;
                        StatusControl.Instance.ActionPoints -= 1;
                        UnitBar.Instance.clearSelectUnit();
                    }
                }
            }
        }
    }

    public void setUnitSprite(string path) {
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
    }

    //if can set unit return true else return false
    private bool canSetPosition(HexCoordinates coordinates) {
        if (coordinates.Equals(HexCoordinates.defaultCoordinate)) return false;

        if (!UnitBar.Instance.isPlay) {
            if (PhotonNetwork.isMasterClient) {
                if (!(coordinates.Y < HexGrid.mapDetail.area[0])) return false;
            } else {
                if (!(coordinates.Y > HexGrid.mapDetail.area[1])) return false;
            }
        }

        Vector3 position = HexCoordinates.cubeToOffset(coordinates);
        int type = UnitBar.Instance._hexGrid.cells[(int)(position.x + (position.z * UnitBar.Instance._hexGrid.width))].mapType;
        if (type == 1 || type == 2) return false;

        for (int i = 0; i < UnitBar.Instance.Units.Count; i++) {
            if (this.Equals(UnitBar.Instance.Units[i])) continue;
            if (coordinates.Equals(UnitBar.Instance.Units[i].coordinate)) return false;
        }

        for (int i = 0;i < UnitBar.Instance.Towers.Count; i++) {
            if (coordinates.Equals(UnitBar.Instance.Towers[i].coordinate)) return false;
        }

        for (int i = 0; i < UnitBar.Instance.enemyUnits.Count; i++) {
            if (coordinates.Equals(UnitBar.Instance.enemyUnits[i].coordinate)) return false;
        }

        for (int i = 0; i < UnitBar.Instance.enemyTowers.Count; i++) {
            if (coordinates.Equals(UnitBar.Instance.enemyTowers[i].coordinate)) return false;
        }

        return true;
    }

    public void setUnitPosition(Vector3 position) {
        coordinate = HexCoordinates.FromOffsetCoordinates((int)position.x, (int)position.z);
        position.x = position.x * (HexMetrics.outerRadius * 1.5f);
        position.y = (position.z + position.x * 0.5f - (int)position.x / 2) * (HexMetrics.innerRadius * 2f);
        if (isTower) position.z = -3;
        else position.z = transform.localPosition.z;
        transform.localPosition = position;
    }

    public void desTroyUnit() {
        if (isTower) {
            Unit tower = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "NewPlayer"), new Vector3(0, 0, 1), Quaternion.identity, 0).GetComponent<Unit>();
            UnitBar.Instance.Towers.Add(tower);
            tower.coordinate = coordinate;
            tower.setUnitPosition(HexCoordinates.cubeToOffset(coordinate));
            tower.transform.SetParent(UnitBar.Instance.transform);
            tower.SpritePath = SpritePath;
            tower.setUnitSprite(SpritePath.Replace("red/CR", "blue/CB"));
        }
        PhotonView.RPC("RPC_destroyUnit", PhotonTargets.All);
    }

    public void RPC_createTower() {

    }

    [PunRPC]
    public void RPC_destroyUnit() {
        if (PhotonView.isMine) {
            if (isTower) {
                UnitBar.Instance.Towers.Remove(this);
            }else
                UnitBar.Instance.Units.Remove(this);
        } else {
            if (isTower) {
                UnitBar.Instance.enemyTowers.Remove(this);
            } else
                UnitBar.Instance.enemyUnits.Remove(this);
        }
        Destroy(this.gameObject);
    }

    public void activeBuffSkill()
    {
        switch (UnitBar.Instance.selectSkill.SkillName)
        {
            case "Zoom":
                PhotonView.RPC("setBuff", PhotonTargets.All, 1, UnitBar.Instance.selectSkill.SkillDuration, StatusControl.Instance.turn, "attack range");
                UnitBar.Instance.selectUnit.cooldownSkill[1] = UnitBar.Instance.selectSkill.SkillCooldown + StatusControl.Instance.turn;
                break;
            case "Power Boost":
                PhotonView.RPC("setBuff", PhotonTargets.All, 5, UnitBar.Instance.selectSkill.SkillDuration, StatusControl.Instance.turn, "atk");
                UnitBar.Instance.selectUnit.cooldownSkill[1] = UnitBar.Instance.selectSkill.SkillCooldown + StatusControl.Instance.turn;
                break;
            case "Guard Boost":
                PhotonView.RPC("setBuff", PhotonTargets.All, 5, UnitBar.Instance.selectSkill.SkillDuration, StatusControl.Instance.turn, "def");
                UnitBar.Instance.selectUnit.cooldownSkill[1] = UnitBar.Instance.selectSkill.SkillCooldown + StatusControl.Instance.turn;
                break;
            case "Wind Boost":
                PhotonView.RPC("setBuff", PhotonTargets.All, 1, UnitBar.Instance.selectSkill.SkillDuration, StatusControl.Instance.turn, "movement");
                UnitBar.Instance.selectUnit.cooldownSkill[1] = UnitBar.Instance.selectSkill.SkillCooldown + StatusControl.Instance.turn;
                break;
            case "Heal":
                PhotonView.RPC("setBuff", PhotonTargets.All, 10, UnitBar.Instance.selectSkill.SkillDuration, StatusControl.Instance.turn, "hp");
                UnitBar.Instance.selectUnit.cooldownSkill[0] = UnitBar.Instance.selectSkill.SkillCooldown + StatusControl.Instance.turn;
                break;
        }
    }

    [PunRPC]
    public void damaged(int atk)
    {
        Damage += (atk * atk) / (atk + structure.Def);

    }

    [PunRPC]
    public void setBuff(int buff, int count, int turn, string type)
    {

        switch (type)
        {
            case "atk":
                atkbuff = buff;
                atkbuffcountTurn = turn + count;
                break;
            case "def":
                defbuff = buff;
                defbuffcountTurn = turn + count;
                break;
            case "movement":
                movementbuff = buff;
                movementbuffcountTurn = turn + count;
                break;
            case "attack range":
                atkrangebuff = buff;
                atkrangebuffcountTurn = turn + count;
                break;
            case "hp":
                Damage -= buff;
                break;
        }
    }

    [PunRPC]
    public void resetCooldown(int num)
    {
        cooldownSkill[num] = 0;
    }
}
