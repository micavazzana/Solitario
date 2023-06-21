using System.Collections.Generic;

public class Mazo
{
    public List<Carta> cartas = new List<Carta>();

    public Mazo(bool tieneJoker)
    {
        CrearMazo(tieneJoker);
    }

    private void CrearMazo(bool tieneJoker)
    {
        CrearPalo(13, Palo.Corazon);
        CrearPalo(13, Palo.Diamante);
        CrearPalo(13, Palo.Trebol);
        CrearPalo(13, Palo.Picas);
        if(tieneJoker)
            CrearPalo(2, Palo.Joker);
    }

    private void CrearPalo(int iteraciones, Palo palo)
    {
        Numero num;
        for (int i = 1; i <= iteraciones; i++)
        {
            num = palo != Palo.Joker ? (Numero)i : Numero.Joker;
            Carta carta = new Carta(num, palo);
            carta.Nombre = $"{num}{palo}";
            this.cartas.Add(carta);
        }
    }

    public void MostrarCartasMazo()
    {
        foreach (Carta card in this.cartas)
        {
            UnityEngine.Debug.Log(card.Nombre);
        }
        UnityEngine.Debug.Log($"Cantidad de cartas: {this.cartas.Count}");
    }

    public void MezclarCartas()
    {
        System.Random random = new System.Random();

        for (int i = 0; i < this.cartas.Count; i++)
        {
            int j = random.Next(i, this.cartas.Count);
            Carta temp = cartas[i];
            cartas[i] = cartas[j];
            cartas[j] = temp;
        }
    }
}
