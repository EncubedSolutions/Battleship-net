namespace battleship_net 
{
    public class Game {

        
        public Game(int boardWidth, int boardHeight, Coordinate boardOffset) {

            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            BoardOffset = boardOffset;

            Renderer = new Renderer(boardWidth, boardHeight, boardOffset);
            InputHandler = new InputHandler();
            Player1 = new Player(this);
            Player2 = new Player(this);

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
            
            Renderer.RenderBoard();
            Player1.PlaceShips(Ships);
            Player2.PlaceShips(Ships);

            //todo: Implement game play loop

        }
    }
}
