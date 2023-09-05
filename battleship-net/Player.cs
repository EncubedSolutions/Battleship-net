namespace battleship_net {

public abstract class Player {
        protected Game Game {get;}
        protected Renderer Renderer {get;}
        protected InputHandler InputHandler {get;}

        public IReadOnlyList<(Ship, Position)> Ships => _ships;
        public IReadOnlyList<(Coordinate, bool)> Targets => _targets; 
        protected IReadOnlyDictionary<Coordinate, bool> ShipCells => _shipCells;
        
        private Dictionary<Coordinate, bool> _shipCells = new Dictionary<Coordinate, bool>();

        private List<(Ship, Position)> _ships = new List<(Ship, Position)>();
        private List<(Coordinate, bool)> _targets = new List<(Coordinate, bool)>();

    public string Name {get;init;}

    public bool IsAlive => ShipCells.Any(c => c.Value == false);

    public Player(Game game, string name) {
            Game =game;
            Renderer = game.Renderer;
            InputHandler = game.InputHandler;
            Name =name;
        }

    public abstract void PlaceShips(IEnumerable<Ship> ships);

    public abstract Coordinate GetTarget();
    public bool DidHit(Coordinate coordinate){
        if (_shipCells.ContainsKey(coordinate)) {
            _shipCells[coordinate] = true;
            return true;
        }
        
        return false;
    }

    public void UpdateHits(Coordinate coordinate, bool didHit){
        _targets.Add((coordinate, didHit));
    }

    protected void AddShipPlacement(Ship ship, Position pos){
        _ships.Add(new(ship, pos));
        var cells = GetCellsForShip(ship, pos);
        foreach(var c in cells){
         _shipCells[c] = false;
        }
    }

    protected bool CanPlaceShip(Ship ship, Position pos)
    {
        var shipCells = GetCellsForShip(ship, pos);

        var existing = Ships.SelectMany(item => GetCellsForShip(item.Item1,item.Item2));

        var overlaps = existing.Intersect(shipCells);

        return !overlaps.Any();
    }

    protected IEnumerable<Coordinate> GetCellsForShip(Ship ship, Position pos)
    {
        if (pos.Orientation == Orientation.Horizontal){
            var x = Enumerable.Range(pos.Col, ship.Size);
            return x.Select(v => new Coordinate(v, pos.Row));
        } else {
            var y = Enumerable.Range(pos.Row, ship.Size);
            return y.Select(v => new Coordinate(pos.Col, v));
        }
    }
}

public class HumanPlayer : Player{
    public HumanPlayer(Game game, string name) :
     base(game, name)
     {

     }
        public override void PlaceShips(IEnumerable<Ship> ships)
        {
            foreach (var ship in ships) {
                PlaceShip(ship);
            }
        }
        private void PlaceShip(Ship ship)
        {
            Position pos = new Position(0, 0, Orientation.Vertical);
            Renderer.RenderPrompt($"Please enter location for {ship.Name}: ");
            Renderer.RefreshPlayerBoard(this);
            var initCanPlace = CanPlaceShip(ship, pos);
            Renderer.RenderShip(ship, pos,initCanPlace);

            while (true)
            {
                var (command, data) = InputHandler.GetInput();
                pos = GetPosition(pos, command, data, ship.Size);
                var canPlace = CanPlaceShip(ship, pos);
                Renderer.RefreshPlayerBoard(this);
                Renderer.RenderShip(ship, pos, canPlace);
                if ((command == InputCommand.Accept) && canPlace)
                {
                    AddShipPlacement(ship, pos);
                    break;
                }
            }
        }

        public override Coordinate GetTarget()
        {
     
        Renderer.RenderPrompt("Where are you Targetting?");

        var pos = new Position(0,0,Orientation.Horizontal); 
        while (true) {
            var (command, data) = InputHandler.GetInput();
            if  (command == InputCommand.Accept){
                break;
            }

            pos = GetPosition(pos, command, data, 1);
        }

        return new Coordinate(pos.Col, pos.Row);
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
        var x = Math.Clamp(desired.Col, 0, Game.BoardWidth - (desired.Orientation == Orientation.Horizontal ? shipSize : 1));
        var y = Math.Clamp(desired.Row, 0, Game.BoardHeight - (desired.Orientation == Orientation.Vertical ? shipSize : 1));

        return desired with {Row = y, Col = x};
    }
}

public class ComputerPlayer : Player {

    private readonly Random _rand;
     public ComputerPlayer(Game game, string name, int seed) :
     base(game, name)
     {
        _rand = new Random(seed);
     }

        public override void PlaceShips(IEnumerable<Ship> ships)
        {
            foreach(var ship in ships){
                while (true) {
                    var pos = GetRandomPos(ship.Size
                    );
                    if (CanPlaceShip(ship, pos)){
                        AddShipPlacement(ship, pos);
                        Renderer.RenderShip(ship, pos);
                        Thread.Sleep(1000);
                        break;
                    }
                }
            }
        }

        private Position GetRandomPos(int shipSize){
            var x = _rand.Next(0, Game.BoardWidth);
            var y = _rand.Next(0, Game.BoardHeight);
            var o = (Orientation)_rand.Next(0,2);

            x = Math.Clamp(x, 0, Game.BoardWidth - (o == Orientation.Horizontal ? shipSize : 1));
            y = Math.Clamp(y, 0, Game.BoardHeight - (o == Orientation.Vertical ? shipSize : 1));
            return new Position(x,y,o);
        }

        public override Coordinate GetTarget()
        {
            while (true){
                var p = GetRandomPos(1);
                var result = new Coordinate(p.Col, p.Row);

                if (!Targets.Any(c => c.Item1 == result)) {
                     Thread.Sleep(1000);
                    return result;
                }
            }
        }
    }
}

