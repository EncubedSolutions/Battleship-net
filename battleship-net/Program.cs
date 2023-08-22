namespace battleship_net 
{
public class Program
{
    private static void Main(string[] args)
    {

        var game = new Game(10,10,new Coordinate(0,3));
        
        Console.Clear();
        Console.WriteLine("Hello, Battleship.net!");
        Console.WriteLine();

        game.Play();
    }   
}

public record Coordinate(int X, int Y);
public record Position(int Col, int Row, Orientation Orientation);
public record Ship (string Name, int size);

public enum Orientation { Horizontal, Vertical};

}