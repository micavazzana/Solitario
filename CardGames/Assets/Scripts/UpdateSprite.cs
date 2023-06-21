using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    [Header("Sprites Carta")]
    public Sprite cartaFront;
    public Sprite cartaBack;

    private SpriteRenderer spriteRenderer;
    private Solitario solitario;

    public bool esVisible;
    private void Awake()
    {
        this.esVisible = false;
    }
    void Start()
    {
        Mazo mazo = new Mazo(false);
        this.solitario = FindObjectOfType<Solitario>();

        //Busco la carta correspondiente en el mazo y asigna el sprite adecuado a "cartaFront"
        for (int i = 0; i < mazo.cartas.Count; i++)
        {
            if (this.name == mazo.cartas[i].Nombre)
            {
                this.cartaFront = this.solitario.cartas[i];
                break;
            }
        }

        //Obtengo componente "SpriteRenderer" del objeto actual
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Actualiza el sprite según la visibilidad especificada
        this.spriteRenderer.sprite = this.esVisible ? this.cartaFront : this.cartaBack;
    }
}
