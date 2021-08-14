using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform _tr;
    void Awake()
    {
        _tr = transform;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetProgressBar(float in_value)
    {
        _tr.localScale = new Vector3(in_value, _tr.localScale.y, _tr.localScale.z);
    }


}
