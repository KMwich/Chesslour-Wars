using UnityEngine;

[System.Serializable]
public struct HexCoordinates {
    [SerializeField]
    private int x, y;

    public int X {
        get {
            return x;
        }
    }

    public int Y {
        get {
            return y;
        }
    }

    public int Z {
        get {
            return -X - Y;
        }
    }

    public HexCoordinates(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int y) {
        return new HexCoordinates(x, y - x / 2);
    }

    public override string ToString() {
        return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines() {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }

    public static HexCoordinates FromPosition(Vector3 position) {
        float z = position.z / (HexMetrics.innerRadius * 2f);
        float y = -z;

        float offset = position.x / (HexMetrics.outerRadius * 3f);
        z -= offset;
        y -= offset;

        int iX = Mathf.RoundToInt(-z - y);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(z);

        if (iX + iY + iZ != 0) {
            float dX = Mathf.Abs(-z - y - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(z - iZ);

            if (dZ > dY && dZ > dX) {
                iZ = -iY - iX;
            } else if (dX > dY) {
                iX = -iZ - iY;
            }
        }

        Debug.Log(iX + "," + iZ + "," + iY);

        return new HexCoordinates(iX, iZ);
    }
}
