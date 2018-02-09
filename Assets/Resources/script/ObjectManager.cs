using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    public static ObjectManager Instance;

    public void hide(int phase) {
        this.GetComponent<Transform>().Find("Phase" + phase).gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1);
        print("hide");
    }

    public void show(int phase) {
        this.GetComponent<Transform>().Find("Phase" + phase).gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        print("show");
    }

    private void Awake() {
        Instance = this;
    }


}
