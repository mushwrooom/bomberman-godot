using Godot;
using GdUnit4;
using static GdUnit4.Assertions;
using System;

[TestSuite]
public class MonsterTests
{
    private Monster monster;

    [Before]
    public void SetUp()
    {
        monster = new Monster();
        monster.Speed = 1;
        monster._Ready(); 
    }

    [TestCase]
    public void Test_Init()
    {
        AssertThat(monster.Position).IsEqual(Vector3.Zero);
    }

    [TestCase]
    public void Test_Change_Direction()
    {
        monster.ChangeDirection();
        AssertThat(monster.Velocity.Length()).IsGreater(0);
    }

    [TestCase]
    public void Test_Move()
    {
        monster.Position = new Vector3(1,1,1);
        Vector3 initialPosition = monster.Position;
        monster._on_timer_timeout();
        monster._PhysicsProcess(1.0);

        AssertThat(monster.Position).IsNotEqual(initialPosition);
        
    }
  
 


    [TestCase]
    public void Test_Collision_Blast()
    {
        var blast = new Blast();
        monster.AddChild(blast);

        monster.Move(1.0);

        AssertThat(monster.IsQueuedForDeletion()).IsTrue();
    }

    [TestCase]
    public void Test_Direction_ChangeOn_Slow()
    {
        Vector3 initialDirection = monster.Velocity;
        monster.Position = new Vector3(0.01f, 0, 0);
        monster._PhysicsProcess(1.0);
        AssertThat(monster.Velocity).IsNotEqual(initialDirection);
    }
}
