using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionDisable : MonoBehaviour
{
    public GameObject instructionObject;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisableInstruction());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DisableInstruction()
    {
        yield return new WaitForSeconds(2);
        instructionObject.SetActive(false);
    }
}
