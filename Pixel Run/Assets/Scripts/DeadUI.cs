using UnityEngine;
using UnityEngine.UI;

public class DeadUI : MonoBehaviour {
    public static DeadUI instance;
    [SerializeField] private Button retry;

    void Awake() {
        instance = this;

        retry.onClick.AddListener(() => {
            StageGenerator.instance.StageSpawn(-1);
            gameObject.SetActive(false);
        });
    }
}