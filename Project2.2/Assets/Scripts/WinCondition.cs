using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] GameObject winPopUp;

    // Start is called before the first frame update
    void Start()
    {
        winPopUp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        winPopUp.SetActive(true);
    }
}
