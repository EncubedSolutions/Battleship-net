namespace battleship_net 
{
public class Program
{
    private static void Main(string[] args)
    {
        const int width = 10;
        const int height = 10;
        Coordinate boardOffset= new Coordinate(0,3);
        var renderer = new Renderer(width, height);
        var inputs = new InputHandler();

        Console.Clear();
        Console.WriteLine("Hello, Battleship.net!");
        Console.WriteLine();

        renderer.RenderBoard(boardOffset);

        List<Ship> ships = new List<Ship>{
                new Ship("Carrier", 5),
                new Ship("Battleship", 4),
                new Ship("Destroyer", 3),
                new Ship("Submarine", 3),
                new Ship("Cruiser", 2),
            };

        List<(Ship, Position)> placed = new List<(Ship, Position)>();

        foreach(var ship in ships) {
            var valid = false;
            Position pos = new Position(0,0,Orientation.Vertical);
            renderer.RenderPrompt($"Please enter location for {ship.Name}: ");

            //GetInput
            while (true) {
                renderer.RefreshBoard(boardOffset,placed);
                var (command, data) = inputs.GetInput();
                pos = GetPosition(pos, command, data);
                //Validate Pos
                renderer.RenderShip(ship, pos);
                if(command == InputCommand.Accept) {
                    placed.Add(new (ship, pos));
                    break;
                }
            }
        }
    }

    private static Position GetPosition(Position prev, InputCommand command, int? data)=>
     command switch{
              InputCommand.MoveUp => prev with { Row = prev.Row - 1},  
              InputCommand.MoveDown => prev with { Row = prev.Row + 1},
              InputCommand.MoveLeft => prev with {Col = prev.Col -1},
              InputCommand.MoveRight => prev with {Col = prev.Col +1},
              //InputCommand.Rotate => prev with {Orientation = Orientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal},
              InputCommand.SelectCol => prev with { Col = data.Value},
              InputCommand.SelectRow => prev with { Row = data.Value},
              _ => prev
            };
    
}

public record Coordinate(int X, int Y);
public record Position(int Col, int Row, Orientation Orientation);
public record Ship (string Name, int size);

public enum Orientation { Horizontal, Vertical};

}