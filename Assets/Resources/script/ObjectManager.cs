using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {
    public void hide() {
        this.transform.localScale = new Vector3(0, 1, 1);
    }

    public void show() {
        this.transform.localScale = new Vector3(1, 1, 1);
    }
}
