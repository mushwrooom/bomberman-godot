using GdUnit4;
using static GdUnit4.Assertions;

[TestSuite]
public class ExampleTest
{
    Player player = new Player();
    [TestCase]
    public void test_speed()
    {
        AssertInt(player.speed).IsEqual(5);
    }


    [TestCase]
    public void test_playerID()
    {
        player.SetPlayerId(69);
        AssertInt(player.GetPlayerID()).IsEqual(69);
    }
}