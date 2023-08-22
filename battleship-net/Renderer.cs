namespace battleship_net {
    public class Renderer {

        private const int colHeader = 3;
        private readonly int _width;
        private readonly int _height;
        private readonly Coordinate _boardOffset;
        private readonly int _promptY;

        public Renderer(int width, int height, Coordinate boardOffset)
        {
            _width =width;
            _height = height;
            _boardOffset = boardOffset;
            _promptY = height + 5;

            Console.CursorVisible = false;
        }
        public void RefreshBoard(IEnumerable<(Ship Ship, Position Pos)> ships ){
            RenderBoard();

            foreach(var ship in ships){
                RenderShip(ship.Ship, ship.Pos);
            }
        }

        public void RenderBoard()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(_boardOffset.X, _boardOffset.Y);
            for (int i=0; i< _height; i++)
            {
                RenderCellRow(i);
            }
            RenderHeaderRow();
        }

        private void RenderHeaderRow(){
            var cells = Enumerable.Range('a', _width).Select(c => (char)c);
            var row = string.Join(' ', cells);
            var yOffset = _boardOffset.Y + _height;
            Console.SetCursorPosition(_boardOffset.X + colHeader, yOffset);
            Console.Write($"{row}");
        }

        private void RenderCellRow(int rowNumber){
            var cells = Enumerable.Repeat('_', _width);
            var row = string.Join('|', cells);
            Console.SetCursorPosition(_boardOffset.X, _boardOffset.Y + rowNumber);
            Console.Write($"{rowNumber} |{row}|");
        }

        public void RenderShip(Ship ship, Position position, bool isValid = true) 
        {
            var cellX = _boardOffset.X + colHeader + (position.Col *2);
            var cellY = _boardOffset.Y + position.Row;

            Console.BackgroundColor = isValid ? ConsoleColor.DarkGreen : ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.White;

            for(int i= 0; i < ship.Size; i++) {

                if (position.Orientation == Orientation.Vertical) {
                    Console.SetCursorPosition(cellX, cellY+i);
                }
                else{ 
                    Console.SetCursorPosition(cellX + (i * 2), cellY);
                }
                Console.Write(ship.Name[0]);
            }

            Console.ResetColor();
        }

        public void RenderPrompt(string message)
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, _promptY);
            Console.Write(message.PadRight(80));
            Console.SetCursorPosition(message.Length + 1, _promptY);
        }
    }
}