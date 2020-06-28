using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravitiy : MonoBehaviour
{
    private CharacterController cc;
    private float gravity = 9.82f;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        cc.Move(Vector3.down * 1.0f * Time.deltaTime);
    }
}
