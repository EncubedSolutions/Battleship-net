namespace battleship_net_tests;

public class UnitTest1
{
    [Theory]
    [InlineData("VA0", 5, true)]
    [InlineData("VA4", 5, true)]
    [InlineData("VA5", 5, true)]    
    [InlineData("VA6", 5, false)]
    [InlineData("VA9", 5, false)]

    [InlineData("HA0", 5, true)]
    [InlineData("HF0", 5, true)]
    [InlineData("HG0", 5, false)]    
    [InlineData("HH0", 5, false)]
    [InlineData("HI0", 5, false)]
    [InlineData("HJ0", 5, false)]

    [InlineData("VJ0", 5, true)]
    [InlineData("VK0", 5, false)]
    [InlineData("VAA", 5, false)]
    [InlineData("V99", 5, false)]
    [InlineData("VA10", 5, false)]
    [InlineData("V10A", 5, false)]
    public void ValidateInput(string input, int shipSize, bool expected)
    {
        //Arrange
        var ship = new Ship("test", shipSize);

        //Act
        var result = Program.ValidateCoordinates(input, ship);

        //ASSERT
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("HA0", 0,0, Orientation.Horizontal)]
    [InlineData("VD6", 6,3, Orientation.Vertical)]
    public void ParseCoordinates(string input, int row, int col, Orientation orientation) 
    {
        //Arrange && Act
        var results = Program.ParseCoordinates(input);

        //Assert
        Assert.Equal(row, results.row);
        Assert.Equal(col, results.col);
        Assert.Equal(orientation, results.orientation);
    }
}