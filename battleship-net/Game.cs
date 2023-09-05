namespace battleship_net 
{
    public class Game {

        public Game(int boardWidth, int boardHeight, Coordinate boardOffset) {

            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            BoardOffset = boardOffset;

            Renderer = new Renderer(boardWidth, boardHeight, boardOffset, boardOffset with {X = boardOffset.X + 26});
            InputHandler = new InputHandler();
         

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
        public Player Player1 {get; private set; }

        public Player Player2 {get; private set;}

        public void Play(Player p1, Player p2) {
            Player1 = p1;
            Player2 = p2;
            
            Renderer.RenderPlayerBoard();
            Player1.PlaceShips(Ships);
            Renderer.RenderPlayerBoard();
            Player2.PlaceShips(Ships);

            var attacker = Player1;
            var defender = Player2;

            while (true){
                Renderer.RefreshPlayerBoard(attacker);
                Renderer.RefreshTargetBoard(attacker);

               TakeTurn(attacker, defender);
               if (!defender.IsAlive) {
                break;
               }
               var temp = attacker;
               attacker = defender;
               defender = temp;
            }
            
            Renderer.RenderPrompt($"Player {attacker.Name} WON!!!!!");
        }

        private void TakeTurn(Player attacker, Player defender){

            var target = attacker.GetTarget();
            var didHit = defender.DidHit(target);
            attacker.UpdateHits(target, didHit);
        }
    }
}
