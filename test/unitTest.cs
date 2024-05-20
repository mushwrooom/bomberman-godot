using GdUnit4;
using Moq;
using static GdUnit4.Assertions;

[TestSuite]
public class MainTest
{
    Main main = Mock.Of<Main>();
    
    [TestCase]
    public void test_ready()
    {
        main.RequestReady();
        AssertThat(main.global).IsNotNull();
    }

    [TestCase]
    public void test_character_setup()
    {
        main.SetupCharacters();
        AssertThat(main.Characters).IsNotNull();
    }


    [TestCase]
    public void test_startgame()
    {
        main.StartGame();
        AssertThat(main.map).IsNotNull();
    }

    [TestCase]
    public void test_isEnd()
    {
        AssertThat(main.IsEnd()).IsFalse();
    }

    [TestCase]
    public void test_isDraw()
    {
        AssertThat(main.IsDraw()).IsFalse();
    }
}