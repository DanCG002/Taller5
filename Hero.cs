using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;



public sealed class Hero : MonoBehaviour
{
    public Creacion creacion;
    public float tiempo;
    public int salud;
    public TextMeshProUGUI TextZombie;
    public TextMeshProUGUI TextDemon;
    public TextMeshProUGUI Textcitizen;
    public TextMeshProUGUI Textvida;
    GameObject[] citizens, zombies, Demons;
    public GameObject weapon;
    Civitasinfo civinfo = new Civitasinfo();
    Info_Zomb zombieinfo = new Info_Zomb();

    void Start()
    {      
        weapon = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        weapon.transform.position = new Vector3(this.gameObject.transform.position.x + .314f, this.gameObject.transform.position.y + .314f, this.gameObject.transform.position.z + .314f);
        weapon.transform.localScale = new Vector3(.3f, .3f, .3f);
        weapon.transform.Rotate(90, 0, 0);
        weapon.transform.SetParent(gameObject.transform);
        weapon.AddComponent<Arma>();
        salud = 100;
        gameObject.tag = "Hero";
        gameObject.name = "Hero";
        TextZombie = GameObject.FindGameObjectWithTag("TextZombie").GetComponent<TextMeshProUGUI>();
        TextDemon = GameObject.FindGameObjectWithTag("TextDemon").GetComponent<TextMeshProUGUI>();
        Textcitizen = GameObject.FindGameObjectWithTag("TextCitizen").GetComponent<TextMeshProUGUI>();
        Textvida = GameObject.FindGameObjectWithTag("Textvida").GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {
        tiempo += Time.fixedDeltaTime;
        Textvida.text = salud.ToString();

        if (salud <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
     void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Zombie")
        {
            creacion = FindObjectOfType <Creacion>();

            creacion.mensajezombie.text = "Waaaarrrr quiero comer ";
        }
        if (collision.gameObject.tag == "Zombie2")
        {
            creacion = FindObjectOfType<Creacion>();
            
            creacion.mensajezombie.text = "Waaaarrrr quiero comer " + zombieinfo.gusto;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Medicine")
        {
            salud += 5;
            if(salud >= 100)
            {
                salud = 100;
                other.gameObject.SetActive(false);
            }

        }
    }

    public sealed class MoverH : MonoBehaviour
    {
        Velocidad velocidad;
        public readonly float MovX;
        private void Start()
        {
            velocidad = new Velocidad(Random.Range(0.25f, 2f));
        }

        private void Update()
        {
            float MovX = Input.GetAxisRaw("Horizontal");
            float MovY = Input.GetAxisRaw("Vertical");
            transform.Translate(0f, 0f, MovY * velocidad.velo);
            transform.Rotate(0f, MovX * 2f, 0);
        }
    }       
}
public sealed class Velocidad
{
    public readonly float velo;
    public Velocidad(float vel)
    {
        velo = vel;
    }
}
