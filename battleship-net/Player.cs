
namespace battleship_net {

public class Player {
        private readonly Game _game;
        private readonly Renderer _renderer;
        private readonly InputHandler _inputHandler;

        List<(Ship, Position)> placed = new List<(Ship, Position)>();


    public Player(Game game) {
            _game =game;
            _renderer = game.Renderer;
            _inputHandler = game.InputHandler;
        }

    public void PlaceShips(IEnumerable<Ship> ships) {
        
        foreach(var ship in ships) {
            Position pos = new Position(0,0,Orientation.Vertical);
            _renderer.RenderPrompt($"Please enter location for {ship.Name}: ");
            _renderer.RefreshBoard(placed);
            _renderer.RenderShip(ship, pos);
            //GetInput
            while (true) {
                var (command, data) = _inputHandler.GetInput();
                pos = GetPosition(pos, command, data, ship.size);
                //Validate Pos
                _renderer.RefreshBoard(placed);
                _renderer.RenderShip(ship, pos);
                if(command == InputCommand.Accept) {
                    placed.Add(new (ship, pos));
                    break;
                }
            }
        }
    }

     private Position GetPosition(Position prev, InputCommand command, int? data, int shipSize)=>
     command switch{
              InputCommand.MoveUp => HandleMove(prev with { Row = prev.Row - 1}, shipSize),  
              InputCommand.MoveDown => HandleMove(prev with { Row = prev.Row + 1}, shipSize),
              InputCommand.MoveLeft => HandleMove(prev with { Col = prev.Col - 1}, shipSize),
              InputCommand.MoveRight => HandleMove(prev with { Col = prev.Col + 1}, shipSize),
              InputCommand.SelectCol =>  HandleMove(prev with { Col = data.Value}, shipSize),
              InputCommand.SelectRow => HandleMove(prev with { Row = data.Value}, shipSize),
              InputCommand.Rotate => HandleMove(prev with { Orientation = prev.Orientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal}, shipSize),
              _ => prev
            };
    
    private Position HandleMove(Position desired, int shipSize) {
        var x = Math.Clamp(desired.Col, 0, _game.BoardWidth - (desired.Orientation == Orientation.Horizontal ? shipSize : 1));
        var y = Math.Clamp(desired.Row, 0, _game.BoardHeight - (desired.Orientation == Orientation.Vertical ? shipSize : 1));

        return desired with {Row = y, Col = x};
    }
}
}