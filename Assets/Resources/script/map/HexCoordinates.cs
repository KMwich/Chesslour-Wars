using UnityEngine;

[System.Serializable]
public struct HexCoordinates {
    [SerializeField]
    private int x, y;
    public static HexCoordinates defaultCoordinate = new HexCoordinates(0, 0);

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
        float y = position.y / (HexMetrics.innerRadius * 2f);
        float z = -y;

        float offset = position.x / (HexMetrics.outerRadius * 3f);
        y -= offset;
        z -= offset;

        int iX = Mathf.RoundToInt(-y - z);
        int iY = Mathf.RoundToInt(z);
        int iZ = Mathf.RoundToInt(y);

        if (iX + iY + iZ != 0) {
            float dX = Mathf.Abs(-y - z - iX);
            float dY = Mathf.Abs(z - iY);
            float dZ = Mathf.Abs(y - iZ);

            if (dZ > dY && dZ > dX) {
                iZ = -iY - iX;
            } else if (dX > dY) {
                iX = -iZ - iY;
            }
        }

        //Debug.Log(iX + "," + iZ + "," + iY);

        return new HexCoordinates(iX, iZ);
    }

    public static Vector3Int cubeToOffset (HexCoordinates coordinate) {
        Vector3Int vector = new Vector3Int();
        vector.x = coordinate.X;
        vector.y = 0;
        vector.z = coordinate.Y + coordinate.X / 2;

        return vector;
    }

    public static int cubeDeistance (HexCoordinates a, HexCoordinates b) {
        return Mathf.Max(Mathf.Abs(a.X - b.X), Mathf.Abs(a.Y - b.Y), Mathf.Abs(a.Z - b.Z));
    }

    public static HexCoordinates[] neighbor(HexCoordinates coordinate) {
        HexCoordinates[] n = new HexCoordinates[6];
        
        for (int i = 0, x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (y != x) {
                    n[i] = new HexCoordinates(x + coordinate.X, y + coordinate.Y);
                    i++;
                }
            }
        }
        return n;
    }
}
