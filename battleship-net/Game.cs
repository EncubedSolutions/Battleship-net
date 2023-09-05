namespace battleship_net 
{
    public class Game {
        private GameOptions _gameOptions;
        public Game(GameOptions options, Coordinate boardOffset) {

            _gameOptions = options;
            BoardOffset = boardOffset;

            Renderer = new Renderer(BoardWidth, BoardHeight, boardOffset);
            InputHandler = new InputHandler();
        }

        public int BoardWidth => _gameOptions.Width;
        public int BoardHeight => _gameOptions.Height;
        public Coordinate BoardOffset { get; }
        public Renderer Renderer {get;}
        public InputHandler InputHandler {get;}

        public IEnumerable<Ship> Ships =>_gameOptions.Ships;
        public Player Player1 {get; private set; }

        public Player Player2 {get; private set;}

        public void Play(Player p1, Player p2) {
            Player1 = p1;
            Player2 = p2;
            
            Renderer.RenderPlayerBoard(Player1);
            Player1.PlaceShips(Ships);
            Renderer.RenderPlayerBoard(Player2);
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
