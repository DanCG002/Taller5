using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using infeccioso = NPC.Enemigo;
using zombie2 = NPC.Enemigo2;
namespace NPC
{
    namespace Amigo
    {
       
        public sealed class Pueblerino : MonoBehaviour
        {
            public Civitasinfo info_Aldeano = new Civitasinfo();
            public Estado estado_Ald;
            float tiempo;
            public float distancia;
            public float vel_Escape;
            public float años;
            int Accion;
            public bool Huida = false;
            bool observar = false;
            public Vector3 orientacion;
            GameObject Objetivo;
            GameObject[] Zombie,Otro_Zombie;

            public enum nombres
            {
                Fred, Carlos, Jeremy, Annie, Carmen, Andres, Canela, Robin,
                Arturo, Otoniel, Robert, Fausto, Javier, Hernan, Larry, Gregorio, Zac,
                Victor, Rosa, Daniela
            }

            
            public enum Estado
            {
                quieto, movimiento, Rotacion, escape
            }

           
            IEnumerator localizar_Zomb()
            {
                Otro_Zombie = GameObject.FindGameObjectsWithTag("Zombie2");             
                Zombie = GameObject.FindGameObjectsWithTag("Zombie");
                foreach (GameObject Elemento in Zombie)
                {
                    infeccioso.Zombie referentezombie = Elemento.GetComponent<infeccioso.Zombie>();
                    if (referentezombie != null)
                    {
                        distancia = Mathf.Sqrt(Mathf.Pow((Elemento.transform.position.x - transform.position.x), 2) + Mathf.Pow((Elemento.transform.position.y - transform.position.y), 2) + Mathf.Pow((Elemento.transform.position.z - transform.position.z), 2));
                        if (!Huida)
                        {
                            if (distancia < 5f)
                            {
                                estado_Ald = Estado.escape;
                                Objetivo = Elemento;
                                Huida = true;
                            }
                        }
                    }
                }

                foreach (GameObject Elemento2 in Otro_Zombie)
                {
                    zombie2.Zombie2 referentezombieesp = Elemento2.GetComponent<zombie2.Zombie2>();
                    if (referentezombieesp != null)
                    {
                       distancia = Mathf.Sqrt(Mathf.Pow((Elemento2.transform.position.x - transform.position.x), 2) + Mathf.Pow((Elemento2.transform.position.y - transform.position.y), 2) + Mathf.Pow((Elemento2.transform.position.z - transform.position.z), 2));
                        if (!Huida)
                        {
                            if(distancia < 5f)
                            {
                                estado_Ald = Estado.escape;
                                Objetivo = Elemento2;
                                Huida = true;
                            }
                        }
                    }    
                }

                if (Huida)
                {
                    if (distancia > 5f)
                    {
                        Huida = false;
                    }
                }

                yield return new WaitForSeconds(0.2f);
                StartCoroutine(localizar_Zomb());
            }
            
            void Start()
            {
                Rigidbody pueblo;
                gameObject.tag = "Aldeano";
                pueblo = gameObject.AddComponent<Rigidbody>();
                pueblo.constraints = RigidbodyConstraints.FreezeAll;
                pueblo.useGravity = false;
                nombres apodo;
                pueblo.GetComponent<Renderer>().material.color = Color.yellow;
                apodo = (nombres)Random.Range(0, 20);
                info_Aldeano.apodo = apodo.ToString();
                años = Random.Range(15, 101);
                info_Aldeano.años = (int)años;
                vel_Escape = 10 / años;
                gameObject.name = apodo.ToString();
                StartCoroutine(localizar_Zomb());
            }

   
            void Update()
            {
                tiempo += Time.deltaTime;
                if (!Huida)
                {
                    if (tiempo >= 3)
                    {
                        Accion = Random.Range(0, 3);
                        observar = true;
                        tiempo = 0;
                        if (Accion == 0)
                        {
                            estado_Ald = Estado.quieto;
                        }
                        else if (Accion == 1)
                        {
                            estado_Ald = Estado.movimiento;
                        }
                        else if (Accion == 2)
                        {
                            estado_Ald = Estado.Rotacion;

                        }
                    }
                }

                switch (estado_Ald)
                {
                    case Estado.quieto:
                        break;

                    case Estado.movimiento:
                        if (observar)
                        {
                            gameObject.transform.Rotate(0, Random.Range(0, 361), 0);
                        }
                        gameObject.transform.Translate(0, 0, 0.05f);
                        observar = false;
                        break;

                    case Estado.Rotacion:
                        gameObject.transform.Rotate(0, Random.Range(1, 50), 0);
                        break;

                    case Estado.escape:
                        orientacion = Vector3.Normalize(Objetivo.transform.position - transform.position);
                        transform.position -= orientacion * vel_Escape;
                        break;
                }
            }


            
        }
    }
}

