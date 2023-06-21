using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputJugador : MonoBehaviour
{
    public GameObject slot1;
    private Solitario solitario;

    private void Start()
    {
        this.solitario = FindObjectOfType<Solitario>();
        this.slot1 = this.gameObject;
    }
    void Update()
    {
        GetMouseClick();
    }
   
    void GetMouseClick()
    {
        if (Input.GetMouseButton(0))
        {
            //Obtengo la posicion del mouse
            Vector3 posicionMouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));

            //Hago un Raycast para fijarme si el mouse le pega a algo
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit)
            {
                if (hit.collider.CompareTag("Mazo"))
                {
                    solitario.RepartirCartasMazo();
                }
                else if (hit.collider.CompareTag("Carta"))
                {
                    //CartaSeleccionada(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Pila"))
                {

                }
                else if (hit.collider.CompareTag("Fila"))
                {

                }
            }
        }
    }

    void CartaSeleccionada(GameObject carta)
    {

        //if la carta esta boca abajo
        //if la carta no esta bloqueada
        //darla vuelta

        //if la carta clickeada esta en el mazo
        //if la carta no esta bloqueada
        //seleccionarla

        //if la carta esta boca arriba
        //if no esta seleccionada
        //seleccionarla
        //if ya hay una carta seleccionada y no es la misma carta
        //if la carta es elegible para ponerla sobre la anterior
        //ponerla ahi
        //sino
        //seleccionar la nueva carta
        //else if ya hay una carta seleccionada y es la misma carta
        //if si hace doble click
        //if la carta es elegible para ponerla ensima entonces ponerla

        if (slot1 = this.gameObject)
        {
            slot1 = carta;
        }
    }
}
