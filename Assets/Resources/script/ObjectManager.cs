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

    public void showSelectButton(Vector3 position) {
        GameObject obj = this.GetComponent<Transform>().Find("Phase2").gameObject.GetComponent<Transform>().Find("select button").gameObject;
        obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        position.x = ((position.x - 75) / 105 * 720) + 825;
        position.y += ((position.y - 21) / 30 * 160) + 271;
        obj.transform.position = position;
    }

    public void hideSelectButton() {
        this.GetComponent<Transform>().Find("Phase2").gameObject.GetComponent<Transform>().Find("select button").gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1);
    }
}
