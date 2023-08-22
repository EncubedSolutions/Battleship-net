
namespace battleship_net {

public class Player {
        private readonly Game _game;
        private readonly Renderer _renderer;
        private readonly InputHandler _inputHandler;

        List<(Ship, Position)> _placed = new List<(Ship, Position)>();
        List<(Coordinate, bool)> _shipCells = new List<(Coordinate, bool)>(); 

        List<(Coordinate, bool)> _targetCells = new List<(Coordinate, bool)>(); 

    public string Name {get;init;}

    public bool IsAlive => _shipCells.Any(c => c.Item2 == false);

    public Player(Game game, string name) {
            _game =game;
            _renderer = game.Renderer;
            _inputHandler = game.InputHandler;
            Name =name;

            _targetCells.Add((new Coordinate(0, 2), false));
            _targetCells.Add((new Coordinate(2, 2), true));
        }

    public void PlaceShips(IEnumerable<Ship> ships) {

            foreach (var ship in ships) {
                PlaceShip(ship);
            }
        }

    public Coordinate GetTarget() {
        _renderer.RefreshPlayerBoard(_placed);
        _renderer.RefreshTargetBoard(_targetCells);
        //TODO: Get cell from user
        return new Coordinate(0,0);
    }
    public bool DidHit(Coordinate coordinate){
        var cells = _placed.SelectMany(s => GetCellsForShip(s.Item1, s.Item2));
        return cells.Any(c => c == coordinate);
    }

    public void UpdateHits(Coordinate coordinate, bool didHit){
        _targetCells.Add((coordinate, didHit));
    }

        private void PlaceShip(Ship ship)
        {
            Position pos = new Position(0, 0, Orientation.Vertical);
            _renderer.RenderPrompt($"Please enter location for {ship.Name}: ");
            _renderer.RefreshPlayerBoard(_placed);
            _renderer.RenderShip(ship, pos);

            while (true)
            {
                var (command, data) = _inputHandler.GetInput();
                pos = GetPosition(pos, command, data, ship.Size);
                var canPlace = CanPlaceShip(ship, pos);
                _renderer.RefreshPlayerBoard(_placed);
                _renderer.RenderShip(ship, pos, canPlace);
                if ((command == InputCommand.Accept) && canPlace)
                {
                    _placed.Add(new(ship, pos));
                     var cells = GetCellsForShip(ship, pos);
                    _shipCells.AddRange(cells.Select(c => (c, false)));
                    break;
                }
            }
        }

        private bool CanPlaceShip(Ship ship, Position pos)
        {
            var shipCells = GetCellsForShip(ship, pos);

            var existing = _placed.SelectMany(item => GetCellsForShip(item.Item1,item.Item2));

            var overlaps = existing.Intersect(shipCells);

            return !overlaps.Any();
        }

        private IEnumerable<Coordinate> GetCellsForShip(Ship ship, Position pos)
        {
            if (pos.Orientation == Orientation.Horizontal){
                var x = Enumerable.Range(pos.Col, ship.Size);
                return x.Select(v => new Coordinate(v, pos.Row));
            } else {
                var y = Enumerable.Range(pos.Row, ship.Size);
                return y.Select(v => new Coordinate(pos.Col, v));
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