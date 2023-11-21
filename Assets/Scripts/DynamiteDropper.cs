using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamiteDropper : MonoBehaviour
{
    public static int dynamiteCount = 3;
    public TextMeshProUGUI dynCountTxt;
    public Button dynBtn;

    public GameObject dynamite;

    // Start is called before the first frame update
    void Start()
    {
        dynCountTxt.text = dynamiteCount.ToString();
        if (dynamiteCount == 0)
        {
            dynBtn.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4) && dynamiteCount > 0)
        {
            dropDynamite();
        }
    }

    public void dropDynamite()
    {
        dynamiteCount--;
        dynCountTxt.text = dynamiteCount.ToString();

        GameObject newDyn = Instantiate(dynamite, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.identity);
        newDyn.transform.Rotate(new Vector3(90, 0, 0));

        if (dynamiteCount == 0)
        {
            dynBtn.interactable = false;
        }
    }
}
