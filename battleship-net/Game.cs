namespace battleship_net 
{
    public class Game {

        
        public Game(int boardWidth, int boardHeight, Coordinate boardOffset) {

            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            BoardOffset = boardOffset;

            Renderer = new Renderer(boardWidth, boardHeight, boardOffset, boardOffset with {X = boardOffset.X + 26});
            InputHandler = new InputHandler();
            Player1 = new Player(this, "Mary");
            Player2 = new Player(this, "Bob");

            Ships = new List<Ship>{
                new Ship("Carrier", 5),
                new Ship("Battleship", 4),
                new Ship("Destroyer", 3),
                new Ship("Submarine", 3),
                new Ship("Cruiser", 2),
            };
        }


        public int BoardWidth { get; }
        public int BoardHeight { get; }
        public Coordinate BoardOffset { get; }
        public Renderer Renderer {get;}
        public InputHandler InputHandler {get;}

        public IEnumerable<Ship> Ships {get;}
        public Player Player1 {get;}

        public Player Player2 {get;}

        public void Play(){
            
            Renderer.RenderPlayerBoard();
            Player1.PlaceShips(Ships);
            Player2.PlaceShips(Ships);

            var attacker = Player1;
            var defender = Player2;

            while(defender.IsAlive)
            {
                var target = attacker.GetTarget();
                var didHit = defender.DidHit(target);
                attacker.UpdateHits(target, didHit);
            }

            Renderer.RenderPrompt($"Player {attacker.Name} WON!!!!!");
        }
    }
}
