public enum Numero
{
    Joker, As, Dos, Tres, Cuatro, Cinco, Seis, Siete, Ocho, Nueve, Diez, J, Q, K
}
public enum Palo
{
    Joker, Corazon, Diamante, Trebol, Picas
}

public class Carta
{
    private Numero numero;
    private Palo palo;
    private string nombre;

    public Carta(Numero numero, Palo palo)
    {
        this.Numero = numero;
        this.Palo = palo;
    }

    public Numero Numero { get => numero; set => numero = value; } //validar que este entre el rango
    public Palo Palo { get => palo; set => palo = value; }
    public string Nombre { get => nombre; set => nombre = value; }
}