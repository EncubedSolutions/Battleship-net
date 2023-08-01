using System.Text.RegularExpressions;
namespace battleship_net 
{
public class Program
{
    private static void Main(string[] args)
    {
        var renderer = new Renderer();
        const int width = 10;
        const int height = 10;

        Console.Clear();
        Console.WriteLine("Hello, Battleship.net!");
        Console.WriteLine();

        renderer.RenderBoard(width, height);

        List<Ship> ships = new List<Ship>{
                new Ship("Carrier", 5),
                new Ship("Battleship", 4),
                new Ship("Destroyer", 3),
                new Ship("Submarine", 3),
                new Ship("Cruiser", 2),
            };

        foreach(var ship in ships) {
            var valid = false;
            string coordinate = null;
            while (!valid){
                renderer.RenderPrompt($"Please enter location for {ship.Name}: ");
                coordinate = Console.ReadLine();
                valid = ValidateCoordinates(coordinate, ship);
            }
            renderer.RenderShip(ship, coordinate);
        }
    }

    public static bool ValidateCoordinates(string value, Ship ship){
        
        var expression = "^[H,V][A-J][0-9]$";
        const int boardSize = 10;

        if (!Regex.IsMatch(value, expression, RegexOptions.IgnoreCase)) {
                return false;
        }

        var coordinates = ParseCoordinates(value);

        if (coordinates.orientation == Orientation.Vertical) {
            if (coordinates.row + ship.size > boardSize) {
                return false;
            }
        } else {
            if (coordinates.col + ship.size > boardSize) {
                return false;
            }
        }

        return true;
    }

    public static (int row, int col, Orientation orientation) ParseCoordinates(string value){
        var v = value.ToUpper();
        var orientation = v[0] == 'H' ? Orientation.Horizontal : Orientation.Vertical;
        var col = (int)(v[1] - 'A');
        var row = int.Parse(v[2].ToString());
        
        return (row, col, orientation);
    }

    
}

public record Ship (string Name, int size);

public enum Orientation { Horizontal, Vertical};
}