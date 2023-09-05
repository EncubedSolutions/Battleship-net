using System.Text.Json;

namespace battleship_net 
{
public class Program
{
    private static void Main(string[] args)
    {   
        var options = GetOptions("./battleship-net/options.json");
        var game = new Game(options, new Coordinate(0,3));
        var p1 = new HumanPlayer(game, "Bob");
        var p2 = new ComputerPlayer(game, "Mary", 5);
        Console.Clear();
        Console.WriteLine("Hello, Battleship.net!");
        Console.WriteLine();

        game.Play(p1,p2);
    }   

    private static GameOptions GetOptions(string file){

        if (!File.Exists(file)) {
            var s = new List<Ship>{
                new Ship("Carrier", 5),
                new Ship("Battleship", 4),
                new Ship("Destroyer", 3),
                new Ship("Submarine", 3),
                new Ship("Cruiser", 2),
            };
            return new GameOptions(10,10,s);
        }

        using (var stream = File.Open(file, FileMode.Open)) {
            var result = JsonSerializer.Deserialize<GameOptions>(stream);
            return result;
        }
    }
}

public record Coordinate(int X, int Y);
public record Position(int Col, int Row, Orientation Orientation);
public record Ship (string Name, int Size, int Armour = 1);

public record GameOptions(int Width, int Height, IEnumerable<Ship> Ships);

public enum Orientation { Horizontal, Vertical};

}