using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine("Hello, Battleship.net!");
        Console.WriteLine();

        Console.WriteLine(" |A|B|C|D|E|F|G|H|I|J|K|");
        Console.WriteLine(" -----------------------");

        for (var i = 0; i < 10; i++)
        {
            Console.WriteLine($"{i}|_|_|_|_|_|_|_|_|_|_|_|");
        }

        List<Ship> ships = new List<Ship>{
                new Ship("Carrier", 5),
                new Ship("Battleship", 4),
                new Ship("Destroyer", 3),
                new Ship("Submarine", 3),
                new Ship("Cruiser", 2),
            };

        foreach(var ship in ships) {
            var valid = false;
            while (!valid){
                Console.Write($"Please enter location for {ship.Name}: ");
                var coordinate = Console.ReadLine();
                valid = ValidateCoordinates(coordinate, ship);
            }
        }
    }

    private static bool ValidateCoordinates(string value, Ship ship){
        //Assume vertical
        var expression = "^[A-K][0-9]$";

        if (!Regex.IsMatch(value, expression, RegexOptions.IgnoreCase)) {
                return false;
        }

        var coordinates = ParseCoordinates(value);

        if (coordinates.row + 1 < ship.size) {
            return false;
        }
        return true;
    }

    private static (int row, int col) ParseCoordinates(string value){
        var v = value.ToUpper();
        var col = (int)('A' - v[0]);
        var row = int.Parse(value[1].ToString());

        return (row, col);
    }
}

record Ship (string Name, int size);
