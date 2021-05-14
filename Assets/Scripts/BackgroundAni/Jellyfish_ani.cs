using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Jellyfish_ani : MonoBehaviour
{
    public GameObject Jellfish1;
    public GameObject Jellfish2;
    // Start is called before the first frame updat
    void Start()
    {
        Jellfish1.GetComponent<GameObject>();
        Jellfish2.GetComponent<GameObject>();
        StartCoroutine(fishani());
    }

    IEnumerator fishani()
    {
        while(true)
        {
            Jellfish1.transform.position = new Vector2(-5.22f, 2.83f);
            Jellfish2.transform.position = new Vector2(9.99f, -3.58f);
            yield return new WaitForSeconds(1f);
            Jellfish1.transform.position = new Vector2(-5.22f, 2.63f);
            Jellfish2.transform.position = new Vector2(9.99f, -3.78f);
            yield return new WaitForSeconds(1f);
        }
    }
}
