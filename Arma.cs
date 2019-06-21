using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public sealed class Arma : MonoBehaviour
{
    public Hero lugar;
    public GameObject[] municion;
    public GameObject balas;
    public int contador = 0;
    public bool cantidad = true;

    void Start()
    {
        lugar = FindObjectOfType<Hero>();
        municion = new GameObject[100];

        for(int i = 0; i < municion.Length; i++)
        {
       
            balas = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            balas.AddComponent<Rigidbody>();
            balas.GetComponent<Rigidbody>().useGravity = false;
            balas.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            
            balas.GetComponent<Collider>().isTrigger = false;
            balas.transform.position = new Vector3(10000, 10000, 10000);
            balas.transform.localScale = new Vector3(.3f, .3f, .3f);
            balas.tag = "Bala";
            balas.SetActive(false);
            municion[i] = balas;
        }
    }

    void Update()
    {
        if (cantidad)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                municion[contador].transform.position = new Vector3(lugar.weapon.transform.position.x, lugar.weapon.transform.position.y, lugar.weapon.transform.position.z);
                municion[contador].transform.rotation = lugar.weapon.transform.rotation;
                municion[contador].SetActive(true);
                municion[contador].GetComponent<Rigidbody>().AddForce(transform.up * 500f);
                contador += 1;
            }
            
        }
        if (contador == 100)
            cantidad = false;
    }
}
