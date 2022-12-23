using System;
using System.IO;
using NUnit.Framework;

namespace AoC._22;

public sealed class ReactorRebootTest {

    [Test]
    public void TestExample1Line1() {
        var reactorReboot = new ReactorReboot("on x=10..12,y=10..12,z=10..12");
        var result = reactorReboot.CountRebootCubes(50);
        Assert.AreEqual(27, result);
    }

    [Test]
    public void TestExample1Line2() {
        var reactorReboot = new ReactorReboot(
                "on x=10..12,y=10..12,z=10..12",
                "on x=11..13,y=11..13,z=11..13"
        );
        var result = reactorReboot.CountRebootCubes(50);

        Assert.AreEqual(46, result);
    }

    [Test]
    public void TestExample1Line3() {
        var reactorReboot = new ReactorReboot(
                "on x=10..12,y=10..12,z=10..12",
                "on x=11..13,y=11..13,z=11..13",
                "off x=9..11,y=9..11,z=9..11"
        );
        var result = reactorReboot.CountRebootCubes(50);

        Assert.AreEqual(38, result);
    }

    [Test]
    public void TestExample1A() {
        var reactorReboot = new ReactorReboot(ReadInput(@"22\exampleA.txt"));
        var result = reactorReboot.CountRebootCubes(50);

        Assert.AreEqual(39, result);
    }

    private string[] ReadInput(string fileName) {
        return File.ReadAllLines(fileName);
    }

    [Test]
    public void TestExample1B() {
        var reactorReboot = new ReactorReboot(ReadInput(@"22\exampleB.txt"));
        var result = reactorReboot.CountRebootCubes(50);

        Assert.AreEqual(590784, result);
    }

    [Test]
    public void TestPuzzle1() {
        var reactorReboot = new ReactorReboot(ReadInput(@"22\input.txt"));
        var result = reactorReboot.CountRebootCubes(50);

        Console.WriteLine("Puzzle 1: " + result);
        Assert.AreEqual(596598, result);
    }

    [Test]
    public void TestExample2() {
        var reactorReboot = new ReactorReboot(ReadInput(@"22\exampleC.txt"));

        var result = reactorReboot.CountRebootCubes();
        Assert.AreEqual(2758514936282235L, result);
    }

    [Test]
    public void TestExample2For50A() {
        var reactorReboot = new ReactorReboot(ReadInput(@"22\exampleA.txt"));
        var result = reactorReboot.CountRebootCubes(50);

        Assert.AreEqual(39, result);
    }

    [Test]
    public void TestExample2For50B() {
        var reactorReboot = new ReactorReboot(ReadInput(@"22\exampleB.txt"));
        var result = reactorReboot.CountRebootCubes(50);

        Assert.AreEqual(590784, result);
    }

    [Test]
    public void TestExample2For50C() {
        var reactorReboot = new ReactorReboot(ReadInput(@"22\exampleC.txt"));
        var result = reactorReboot.CountRebootCubes(50);

        Assert.AreEqual(474140, result);
    }

    [Test]
    public void TestPuzzle2() {
        var reactorReboot = new ReactorReboot(ReadInput(@"22\input.txt"));

        var result = reactorReboot.CountRebootCubes();
        Console.WriteLine("Puzzle 2: " + result);
        Assert.AreEqual(946529356520531L, result); // too low
    }
}
