using GdUnit4;
using Godot;
using static GdUnit4.Assertions;
using System.Collections.Generic;

[TestSuite]
public class PlayerControlTest
{
    Player p1 = new Player();
    Player p2 = new Player();
    Map map = new Map();
    Main main = new Main();
    Global global = new Global();
    playerCustomize playerCustomize = new playerCustomize();

    [Before]
    public void SetUp()
    {
        map.players = new List<Player>();
        map.players.Add(p1);
        map.players.Add(p2);

        main.map = map;
        main.global = global;
    }

    [TestCase]
    public void InitialAttributes()
    {
        AssertInt(p1.speed).IsEqual(5);
        AssertThat(map.shrinkTime).IsEqual(30.0);
        map.players[1].SetPlayerId(7);
        AssertInt(map.players[1].GetPlayerID()).IsEqual(7);
    }

    [TestCase]
    public void playersDie()
    {
        AssertThat(main.IsEnd()).IsFalse();

        map.players[1].Die();
        AssertThat(main.IsEnd()).IsTrue();
        map.players[0].Die();
        AssertThat(main.IsDraw()).IsTrue();
    }
    
    [TestCase]
    public void TestMovementDirection()
    {
        p1.SetControlSchemes(new Dictionary<ControlScheme, Key>
        {
            { ControlScheme.MOVE_LEFT, Godot.Key.A },
            { ControlScheme.MOVE_RIGHT, Godot.Key.D },
            { ControlScheme.MOVE_UP, Godot.Key.W },
            { ControlScheme.MOVE_DOWN, Godot.Key.S }
        });

        // Simulate pressing the MOVE_RIGHT key
        Vector3 before=p1.Position;
        Input.ActionPress(Godot.Key.D.ToString()); //go right
        p1._PhysicsProcess(1.0); // 1 second frame
        AssertThat(p1.Position).IsNotEqual(before);
    }

    [TestCase]
    public void ControlButtonHandling()
    {
        // Simulate a button press and check if current struct is updated correctly
        Button testButton = playerCustomize.actionList[0].GetChild(0) as Button;
        testButton.EmitSignal("pressed");
        AssertThat(playerCustomize.cur.b).IsEqual(testButton);
        AssertInt(playerCustomize.cur.id).IsEqual(0);
    }

    [TestCase]
    public void NameLabelUpdate()
    {
        playerCustomize.UpdateButtonNames(0);

        Button button = playerCustomize.actionList[0].GetChild(0) as Button;
        Label label = button.GetChild(0) as Label;
        AssertThat(label.Text).IsEqual("W"); //default MOVE_UP key for player with ID 0
    }

}