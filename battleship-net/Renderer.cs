namespace battleship_net {
    public class Renderer {

        public void RenderBoard(int width, int height)
        {
            Console.Write(" ");

            for (var i = 0; i < width; i++)
            {
                var c = (int)'A' + i;
                Console.Write($"|{(char)c}");
            }

            Console.WriteLine("|");
            Console.Write(" ");
            
            for (var i = 0; i < width; i++)
            {
                Console.Write("--");
            }

            Console.WriteLine();
            for (var i = 0; i < height; i++)
            {
                Console.Write($"{i}");
                for (var j = 0; j < width; j++)
                {
                    Console.Write("|_");
                }
                Console.WriteLine("|");
            }
        }
        public void RenderShip(Ship ship, string coordinate) 
        {
            const int headerRows = 4;
            const int headerCols = 2;

            var location = Program.ParseCoordinates(coordinate);
            var cellX = headerCols + (location.col *2);
            var cellY = headerRows + location.row;

            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;

            for(int i= 0; i < ship.size; i++) {

                if (location.orientation == Orientation.Vertical) {
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
            Console.SetCursorPosition(0, 15);
            Console.Write(message.PadRight(80));
            Console.SetCursorPosition(message.Length +1, 15);
        }
    }
}