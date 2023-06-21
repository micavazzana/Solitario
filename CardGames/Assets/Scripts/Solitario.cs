using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Dificultad
{
    Facil, Dificil
}
public class Solitario : MonoBehaviour
{
    public Mazo mazo;

    [Header("Objetos")]
    public GameObject prefabCarta;
    public GameObject objMazo;

    [Header("Arrays")]
    public Sprite[] cartas;
    public GameObject[] filas;
    public GameObject[] pilas;

    [Header("Dificultad")]
    public Dificultad modo;
    private int cantidadDeCartasARepartir;

    [Header("Variables FILAS")]
    private List<Carta>[] filasCartas;
    private List<Carta> fila0;
    private List<Carta> fila1;
    private List<Carta> fila2;
    private List<Carta> fila3;
    private List<Carta> fila4;
    private List<Carta> fila5;
    private List<Carta> fila6;

    [Header("Variables MAZO")]
    private List<Carta> gruposMostrados;
    private List<List<Carta>> listaDeCartasDescartadas;
    private List<Carta> cartasUsadas;
    private int grupoDeCartas;
    private int cartasRestantes;
    private int posicionMazo;    

    [Header("Variables PILAS")]
    private List<Carta>[] pilasCartas;


   

    void Start()
    {
        this.mazo = new Mazo(false);
        this.mazo.MezclarCartas();
        SetDificultad();
       


        //Mazo
        this.gruposMostrados = new List<Carta>();
        this.listaDeCartasDescartadas = new List<List<Carta>>();
        this.cartasUsadas = new List<Carta>();
        OrdenarMazoEnGrupos();

        //Filas
        this.fila0 = new List<Carta>();
        this.fila1 = new List<Carta>();
        this.fila2 = new List<Carta>();
        this.fila3 = new List<Carta>();
        this.fila4 = new List<Carta>();
        this.fila5 = new List<Carta>();
        this.fila6 = new List<Carta>();
        OrdenarCartasEnFilas();
        StartCoroutine(RepartirCartasEnFilas());
    }

    /// <summary>
    /// Establece la cantidad de cartas a repartir segun la dificultad elegida
    /// </summary>
    void SetDificultad()
    {
        this.cantidadDeCartasARepartir = this.modo == Dificultad.Facil ? 1 : 3;
    }

    /// <summary>
    /// Se encarga de distribuir las cartas en las filas del juego y quita las cartas correspondientes del mazo
    /// </summary>
    void OrdenarCartasEnFilas()
    {
        this.filasCartas = new List<Carta>[] { fila0, fila1, fila2, fila3, fila4, fila5, fila6 };

        //Itero las 7 filas
        for (int i = 0; i < 7; i++)
        {
            //Itero cada fila
            for (int j = i; j < 7; j++)
            {
                //Agrego la ultima carta del mazo a la lista de cartas de la fila j
                //Luego, se elimina esa carta del mazo para evitar repeticiones.
                this.filasCartas[j].Add(this.mazo.cartas.Last<Carta>());
                this.mazo.cartas.RemoveAt(this.mazo.cartas.Count - 1);
            }
        }
    }

    /// <summary>
    /// Crea Representaciones visuales de las cartas en las filas del juego
    /// utilizando posiciones adecuadas y visibilidad segun corresponda.
    /// Se crean gradualmente con una pequeña pausa para simular la reparticion.
    /// </summary>
    /// <returns></returns>
    IEnumerator RepartirCartasEnFilas()
    {
        //Recorro las filas
        for (int i = 0; i < 7; i++)
        {
            float yPos = 0;
            float zPos = 0.03f;

            //Recorro cada fila
            foreach (Carta carta in this.filasCartas[i])
            {
                //Determino la posicion en la que va a aparecer cada carta siendo filas la posicion parent
                Vector3 posicion = new Vector3(this.filas[i].transform.position.x, this.filas[i].transform.position.y - yPos, this.filas[i].transform.position.z - zPos);
                yPos = yPos + 0.2f;
                zPos = zPos + 0.03f;

                //Instancio un nuevo GameObject para cada carta que debe aparecer
                GameObject nuevaCarta = Instantiate(this.prefabCarta, posicion, Quaternion.identity, this.filas[i].transform);
                nuevaCarta.name = $"{carta.Nombre}";

                //Solo si la carta actual es la ultima carta de la fila la hago visible
                if (carta == this.filasCartas[i][this.filasCartas[i].Count - 1])
                    nuevaCarta.GetComponent<UpdateSprite>().esVisible = true;

                this.cartasUsadas.Add(carta);
                yield return new WaitForSeconds(0.01f);
            }

            //Elimino las cartas repartidas del mazo
            foreach (Carta carta in this.cartasUsadas)
            {
                if (this.mazo.cartas.Contains(carta))
                {
                    this.mazo.cartas.Remove(carta);
                }
            }
            this.cartasUsadas.Clear();
        }
    }

    /// <summary>
    /// Se encarga de organizar las cartas restantes del mazo en grupos
    /// Cada grupo se forma seleccionando las cartas consecutivas en el mazo
    /// </summary>
    void OrdenarMazoEnGrupos()
    {
        //Calculo cuantos grupos puedo formar a partir de las cartas que me quedaron en el mazo
        //Si se eligio modo Dificil, se dividira por 3. Este resultado me dara cuantos grupos puedo formar
        //Tambien calculo el residuo de la division para determinar cuantas cartas no puedo agrupar.
        this.grupoDeCartas = this.mazo.cartas.Count / this.cantidadDeCartasARepartir;
        this.cartasRestantes = this.mazo.cartas.Count % this.cantidadDeCartasARepartir;

        //Limpio el listado para que siempre empiece vacia y poder almacenar un nuevo grupo de cartas
        this.listaDeCartasDescartadas.Clear();

        //Itero sobre los grupos de cartas del mazo
        int modificador = 0;
        for (int i = 0; i < this.grupoDeCartas; i++)
        {
            //Creo una lista para almacenar las cartas del grupo actual y luego itero sobre la cantidad de cartas de cada grupo
            List<Carta> miGrupo = new List<Carta>();
            for (int j = 0; j < this.cantidadDeCartasARepartir; j++)
            {
                //Agrego la carta a miGrupo y desplazo la posicion de inicio de cartas de cada grupo con modificador
                miGrupo.Add(this.mazo.cartas[j + modificador]);
            }
            //Una vez agregadas las cartas del grupo actual, agrego miGrupo a las cartas ya descartadas
            this.listaDeCartasDescartadas.Add(miGrupo);

            //Incremento el valor del modificador para avanzar a la proxima posicion de inicio
            modificador = modificador + this.cantidadDeCartasARepartir;
        }

        //En caso de quitar cartas para utilizarlas es necesario reorganizar los grupos ya que no puedo agrupar segun las cartas a repartir
        if (this.cartasRestantes != 0)
        {
            List<Carta> restantes = new List<Carta>();
            modificador = 0;
            for (int k = 0; k < this.cartasRestantes; k++)
            {
                restantes.Add(this.mazo.cartas[this.mazo.cartas.Count - this.cartasRestantes + modificador]);
                modificador++;
            }
            this.listaDeCartasDescartadas.Add(restantes);
            this.grupoDeCartas++;
        }
        this.posicionMazo = 0;
    }

    /// <summary>
    /// Reparte las cartas del mazo en grupos visibles
    /// </summary>
    public void RepartirCartasMazo()
    {
        //Destruyo todos los objetos hijos del mazo
        foreach (Transform child in this.objMazo.transform)
        {
            if (child.CompareTag("Carta"))
            {
                ActualizarMazo(child.name);
                Destroy(child.gameObject);
            }
        }
        //Si todavia  hay cartas en el mazo para repartir
        if (this.posicionMazo < this.grupoDeCartas)
        {
            this.gruposMostrados.Clear();
            float xPos = 0.75f;
            float zPos = -0.03f;

            //Itero sobre las cartas del grupo actual
            foreach (Carta carta in this.listaDeCartasDescartadas[this.posicionMazo])
            {
                //Determino la posicion en la que va a aparecer cada carta tomando como referencia la posicion del mazo
                Vector3 pos = new Vector3(this.objMazo.transform.position.x + xPos, this.objMazo.transform.position.y, this.objMazo.transform.position.z + zPos);
                xPos = xPos + 0.2f;
                zPos = zPos - 0.1f;

                //Instancio un nuevo GameObject para cada carta que debe aparecer
                GameObject nuevaCarta = Instantiate(this.prefabCarta, pos, Quaternion.identity, this.objMazo.transform);
                nuevaCarta.name = carta.Nombre;

                //Hago visible la/las cartas
                nuevaCarta.GetComponent<UpdateSprite>().esVisible = true;

                this.gruposMostrados.Add(carta);
            }
            this.posicionMazo++;
        }
        else //Si ya no quedan cartas entonces lo vuelvo a reapilar
        {
            ReapilarMazo();
        }
    }

    /// <summary>
    /// Se encarga de actualizar el estado del mazo al remover una carta en especifico
    /// y la agrega a las cartas utilizadas
    /// </summary>
    /// <param name="nombreCarta">La carta a buscar</param>
    void ActualizarMazo(string nombreCarta)
    {
        for (int i = 0; i < this.mazo.cartas.Count; i++)
        {
            if (this.mazo.cartas[i].Nombre == nombreCarta)
            {
                this.cartasUsadas.Add(this.mazo.cartas[i]);
                this.mazo.cartas.Remove(this.mazo.cartas[i]);
                break;
            }
        }
    }

    /// <summary>
    /// Se encarga de volver a apilar las cartas usadas en el mazo
    /// </summary>
    void ReapilarMazo()
    {
        foreach (Carta carta in this.cartasUsadas)
        {
            this.mazo.cartas.Add(carta);
        }
        this.cartasUsadas.Clear();

        //Vuelvo a organizar las cartas restantes en grupos
        OrdenarMazoEnGrupos();
    }
}