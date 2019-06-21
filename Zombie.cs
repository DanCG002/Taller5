using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using zombie = NPC.Amigo;

namespace NPC
{
    namespace Enemigo
    {

        public  class Zombie : MonoBehaviour
        {
            public Info_Zomb info_Zomb;
            public Hero Daño;
            bool infectado = false;
            int asigna;
            public string deseo;
            public int Accion = 0;
            public float años = 0;
            public float tiempo = 0;
            public bool observar = false;
            public float vel_Persecucion;
            public Estado estado_Zombie;
            public Vector3 orientacion;
            float trecho1;
            float trecho2;
            public bool siguiendo = false;
            GameObject objetivo, heroe;
            GameObject[] aldeanos;
 
            IEnumerator localizar_Ald()
            {
                heroe = GameObject.FindGameObjectWithTag("Hero");
                aldeanos = GameObject.FindGameObjectsWithTag("Aldeano");
                foreach (GameObject objeto in aldeanos)
                {
                    yield return new WaitForEndOfFrame();
                    zombie.Pueblerino componente_Aledeano = objeto.GetComponent<zombie.Pueblerino>();
                    if (componente_Aledeano != null)
                    {
                        trecho2 = Mathf.Sqrt(Mathf.Pow((heroe.transform.position.x - transform.position.x), 2) + Mathf.Pow((heroe.transform.position.y - transform.position.y), 2) + Mathf.Pow((heroe.transform.position.z - transform.position.z), 2));
                        trecho1 = Mathf.Sqrt(Mathf.Pow((objeto.transform.position.x - transform.position.x), 2) + Mathf.Pow((objeto.transform.position.y - transform.position.y), 2) + Mathf.Pow((objeto.transform.position.z - transform.position.z), 2));
                        if (!siguiendo)
                        {

                            if(trecho1 < 5f)
                            {
                                estado_Zombie = Estado.persecucion;
                                objetivo = objeto;
                                siguiendo = true;
                            }
                            else if (trecho2 < 5f)
                            {
                                estado_Zombie = Estado.persecucion;
                                objetivo = heroe;
                                siguiendo = true;
                            }
                        }
                        if (trecho1 < 5f && trecho2 < 5f)
                        {
                            objetivo = objeto;
                        }
                    }
                }

                if (siguiendo)
                {
                    if (trecho1 > 5f && trecho2 > 5f)
                    {
                        siguiendo = false;
                    }
                }
                
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(localizar_Ald());
            }

          
            public enum Extremidades
            {
                cabeza, torso, brazos, cuello, piernas
            }

            
            public enum Estado
            {
                movimiento, quieto, Rotacion, persecucion
            }

           
            void Start()
            {
                if (!infectado)
                {
                    años = Random.Range(15, 101);
                    info_Zomb = new Info_Zomb();
                    asigna = Random.Range(0, 3);
                    Rigidbody Zom;
                    Zom = gameObject.AddComponent<Rigidbody>();
                    Zom.constraints = RigidbodyConstraints.FreezeAll;
                    Zom.useGravity = false;
                    gameObject.name = "Zombie";
                }
                else
                {
                    años = info_Zomb.años;
                    gameObject.name = info_Zomb.apodo;
                }
                
                StartCoroutine(localizar_Ald());
                Daño = GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>();
                vel_Persecucion = 10 / años;
                this.gameObject.tag = "Zombie";
                Extremidades bodyparts;
                bodyparts = (Extremidades)Random.Range(0, 5);
                deseo = bodyparts.ToString();
                info_Zomb.gusto = deseo;
                this.gameObject.GetComponent<Renderer>().material.color = Color.magenta;
                
            }
            
            void Update()
            {
                tiempo += Time.deltaTime;
                if (!siguiendo)
                {
                    if (tiempo >= 3)
                    {
                        Accion = Random.Range(0, 3);
                        observar = true;
                        tiempo = 0;
                        if (Accion == 0)
                        {
                            estado_Zombie = Estado.quieto;
                        }
                        else if (Accion == 1)
                        {
                            estado_Zombie = Estado.movimiento;
                        }
                        else if (Accion == 2)
                        {
                           estado_Zombie = Estado.Rotacion;
                        }
                    }
                }
                
               
                
                switch (estado_Zombie)
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
                        gameObject.transform.Rotate(0, Random.Range(0.5f,2f), 0);
                        break;

                    case Estado.persecucion:
                        orientacion = Vector3.Normalize(objetivo.transform.position - transform.position);
                        transform.position += orientacion * vel_Persecucion;
                        break;
                }
            }

            
             void OnCollisionEnter(Collision collision)
            {
                if (collision.gameObject.tag == "Aldeano")
                {
                    Destroy(collision.gameObject.GetComponent<NPC.Amigo.Pueblerino>());
                    collision.gameObject.AddComponent<Zombie>().info_Zomb = collision.gameObject.GetComponent<NPC.Amigo.Pueblerino>().info_Aldeano;
                    collision.gameObject.GetComponent<Zombie>().infectado = true;
                   
                }

                if (collision.gameObject.tag == "Hero")
                {
                    Daño.salud -= 10;
                }
                if (collision.gameObject.tag == "Bala")
                {
                    gameObject.tag = "Muerto";
                    gameObject.SetActive(false);
                    collision.gameObject.SetActive(false);
                }
            }

           
        }
    }
}
