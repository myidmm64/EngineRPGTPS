using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightChanger : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;
    [SerializeField]
    private List<GameObject> lights = new List<GameObject>();
    private bool _isNight = false;

    private void Update()
    {
        transform.Rotate(Vector3.right, _speed * Time.deltaTime);

        if(transform.eulerAngles.x >= 170f)
            if(_isNight == false)
            {
                _isNight = true;
                for(int i =0; i<lights.Count; i++)
                {
                    lights[i].SetActive(true);
                }
            }

        if ((transform.eulerAngles.x >= 10f) && (transform.eulerAngles.x < 170f))
            if (_isNight == true)
            {
                _isNight = false;
                for (int i = 0; i < lights.Count; i++)
                {
                    lights[i].SetActive(false);
                }
            }
    }
}
