namespace battleship_net {
    public class Renderer {

        private int _width;
        private int _height;
        private int _promptY;

        public Renderer(int width, int height)
        {
            _width =width;
            _height = height;

            _promptY = height + 5;
        }
        public void RefreshBoard(Coordinate offset, IEnumerable<(Ship Ship, Position Pos)> ships ){
            RenderBoard(offset);

            foreach(var ship in ships){
                RenderShip(ship.Ship, ship.Pos);
            }
        }

        public void RenderBoard(Coordinate offset)
        {
            Console.SetCursorPosition(offset.X, offset.Y);
            for (int i=0; i< _height; i++)
            {
                RenderCellRow(i, offset);
            }
            RenderHeaderRow(offset);
        }

        private void RenderHeaderRow(Coordinate offset){
            var cells = Enumerable.Range('a', _width).Select(c => (char)c);
            var row = string.Join(' ', cells);
            var yOffset = offset.Y + _height;
            Console.SetCursorPosition(offset.X+3, yOffset);
            Console.Write($"{row}");
        }

        private void RenderCellRow(int rowNumber, Coordinate offset){
            var cells = Enumerable.Repeat('_', _width);
            var row = string.Join('|', cells);
            Console.SetCursorPosition(offset.X, offset.Y + rowNumber);
            Console.Write($"{rowNumber} |{row}|");
        }

        public void RenderShip(Ship ship, Position position) 
        {
            //ToDo.. Look to tidy this up, using board offsets.
            const int headerRows = 4;
            const int headerCols = 2;

            var cellX = headerCols + (position.Col *2);
            var cellY = headerRows + position.Row;

            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;

            for(int i= 0; i < ship.size; i++) {

                if (position.Orientation == Orientation.Vertical) {
                    Console.SetCursorPosition(cellX, cellY+i);
                }
                else{ 
                    Console.SetCursorPosition(cellX +i, cellY);
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