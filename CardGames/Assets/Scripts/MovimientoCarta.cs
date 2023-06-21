using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoCarta : MonoBehaviour
{
    private Vector3 posicionInicial;
    private bool seEstaMoviendo = false;
    private Renderer miRenderer;

    private void Start()
    {
        this.posicionInicial = transform.position;
        this.miRenderer = GetComponent<Renderer>();
    }

    private void OnMouseDown()
    {
        //Hago que siempre este al frente
        miRenderer.sortingOrder = 1;
        this.seEstaMoviendo = true;
    }

    private void OnMouseDrag()
    {
        if (this.seEstaMoviendo)
        {
            transform.position = ObtenerPosicionMouseEnMundo();
        }
    }

    private void OnMouseUp()
    {
        seEstaMoviendo = false;



        //Si colisiona con una carta que corresponde entonces que se posicione sobre ella
        //obtener posicion de la carta que esta colisionando
        //transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
        //Si no corresponde, devolverla a su posición inicial:
        //transform.position = this.posicionInicial;


        // Raycast para detectar colisiones con otras cartas
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        int numeroCartaSeleccionada = 0;
        if (hit)
        {
            // Obtener el script o componente de la carta con la que colisionó
            GameObject cartaSeleccionada = this.GetComponent<GameObject>();
            Carta cartaColisionada = hit.collider.GetComponent<Carta>();
            GameObject objetoCartaColisionada = hit.collider.GetComponent<GameObject>();

            Mazo mazo = new Mazo(false);
            foreach (Carta c in mazo.cartas)
            {
                if (cartaSeleccionada.name == c.Nombre)
                {
                    numeroCartaSeleccionada = (int)c.Numero;
                    break;
                }
            }

            if (cartaColisionada != null && (int)cartaColisionada.Numero == numeroCartaSeleccionada + 1)
            {
                // Si colisiona con una carta de valor 1 más, posicionarse sobre ella
                Vector3 posicionCartaColisionada = objetoCartaColisionada.transform.position;
                transform.position = new Vector3(posicionCartaColisionada.y + 0.2f, posicionCartaColisionada.x, posicionCartaColisionada.z);
            }
            else
            {
                // Si no corresponde, devolverla a su posición inicial
                transform.position = this.posicionInicial;
            }
        }
        else
        {
            // Si no hay colisión con otra carta, devolverla a su posición inicial
            transform.position = this.posicionInicial;
        }




        //Cuando suelto la carta que el orderLayer vuelva a 0
        miRenderer.sortingOrder = 0;        
    }

    private Vector3 ObtenerPosicionMouseEnMundo()
    {
        Vector3 posicionMouse = Input.mousePosition;
        posicionMouse.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(posicionMouse);
    }
}