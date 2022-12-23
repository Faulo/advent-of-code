using System;
using System.IO;
using NUnit.Framework;

namespace AoC._22;

public sealed class ReactorRebootTest {

    [Test]
    public void testExample1Line1() {
        var reactorReboot = new ReactorReboot("on x=10..12,y=10..12,z=10..12");
        var cube = reactorReboot.reboot(50);

        var result = cube.countCubes(true);
        Assert.AreEqual(27, result);
    }

    [Test]
    public void testExample1Line2() {
        var reactorReboot = new ReactorReboot(
                "on x=10..12,y=10..12,z=10..12",
                "on x=11..13,y=11..13,z=11..13"
        );
        var cube = reactorReboot.reboot(50);

        var result = cube.countCubes(true);
        Assert.AreEqual(46, result);
    }

    [Test]
    public void testExample1Line3() {
        var reactorReboot = new ReactorReboot(
                "on x=10..12,y=10..12,z=10..12",
                "on x=11..13,y=11..13,z=11..13",
                "off x=9..11,y=9..11,z=9..11"
        );
        var cube = reactorReboot.reboot(50);

        var result = cube.countCubes(true);
        Assert.AreEqual(38, result);
    }

    [Test]
    public void testExample1A() {
        var reactorReboot = new ReactorReboot(readInput("exampleA.txt"));
        var cube = reactorReboot.reboot(50);

        var result = cube.countCubes(true);
        Assert.AreEqual(39, result);
    }

    private string[] readInput(string fileName) {
        return File.ReadAllLines(fileName);
    }

    [Test]
    public void testExample1B() {
        var reactorReboot = new ReactorReboot(readInput("exampleB.txt"));
        var cube = reactorReboot.reboot(50);

        var result = cube.countCubes(true);
        Assert.AreEqual(590784, result);
    }

    [Test]
    public void testPuzzle1() {
        var reactorReboot = new ReactorReboot(readInput("input.txt"));
        var cube = reactorReboot.reboot(50);

        var result = cube.countCubes(true);
        Console.WriteLine("Puzzle 1: " + result);
        Assert.AreEqual(596598, result);
    }

    [Test]
    public void testExample2BoxTrue() {
        var box = new ReactorReboot.Box(
                new int[] { 1, 2, 3 },
                new int[] { 2, 4, 6 },
                true
        );

        Assert.AreEqual(2 * 3 * 4, box.calculateArea());
        Assert.AreEqual(2 * 3 * 4, box.countWithValue(true));
        Assert.AreEqual(0, box.countWithValue(false));
    }

    [Test]
    public void testExample2BoxFalse() {
        var box = new ReactorReboot.Box(
                new int[] { 1, 2, 3 },
                new int[] { 2, 4, 6 },
                false
        );

        Assert.AreEqual(2 * 3 * 4, box.calculateArea());
        Assert.AreEqual(0, box.countWithValue(true));
        Assert.AreEqual(2 * 3 * 4, box.countWithValue(false));
    }

    [Test]
    public void testExample2BoxTrueWithFalseChild() {
        var box = new ReactorReboot.Box(
                new int[] { 1, 1, 1 },
                new int[] { 10, 10, 10 },
                true
        );
        Assert.AreEqual(1000, box.calculateArea());
        Assert.AreEqual(1000, box.countWithValue(true));
        Assert.AreEqual(0, box.countWithValue(false));

        box.children.Add(new ReactorReboot.Box(
                new int[] { 1, 2, 3 },
                new int[] { 2, 4, 6 },
                false
        ));

        Assert.AreEqual(1000, box.calculateArea());
        Assert.AreEqual(1000 - (2 * 3 * 4), box.countWithValue(true));
        Assert.AreEqual(2 * 3 * 4, box.countWithValue(false));
    }

    [Test]
    public void testExample2BoxTrueWithTrueChild() {
        var box = new ReactorReboot.Box(
                new int[] { 1, 1, 1 },
                new int[] { 10, 10, 10 },
                true
        );
        Assert.AreEqual(1000, box.calculateArea());
        Assert.AreEqual(1000, box.countWithValue(true));
        Assert.AreEqual(0, box.countWithValue(false));

        box.children.Add(new ReactorReboot.Box(
                new int[] { 1, 2, 3 },
                new int[] { 2, 4, 6 },
                true
        ));

        Assert.AreEqual(1000, box.calculateArea());
        Assert.AreEqual(1000, box.countWithValue(true));
        Assert.AreEqual(0, box.countWithValue(false));
    }

    [Test]
    public void testExample2BoxFalseWithTrueChild() {
        var box = new ReactorReboot.Box(
                new int[] { 1, 1, 1 },
                new int[] { 10, 10, 10 },
                false
        );
        Assert.AreEqual(1000, box.calculateArea());
        Assert.AreEqual(0, box.countWithValue(true));
        Assert.AreEqual(1000, box.countWithValue(false));

        box.children.Add(new ReactorReboot.Box(
                new int[] { 1, 2, 3 },
                new int[] { 2, 4, 6 },
                true
        ));

        Assert.AreEqual(1000, box.calculateArea());
        Assert.AreEqual(2 * 3 * 4, box.countWithValue(true));
        Assert.AreEqual(1000 - (2 * 3 * 4), box.countWithValue(false));
    }

    [Test]
    public void testExample2BoxFalseWithFalseChild() {
        var box = new ReactorReboot.Box(
                new int[] { 1, 1, 1 },
                new int[] { 10, 10, 10 },
                false
        );
        Assert.AreEqual(1000, box.calculateArea());
        Assert.AreEqual(0, box.countWithValue(true));
        Assert.AreEqual(1000, box.countWithValue(false));

        box.children.Add(new ReactorReboot.Box(
                new int[] { 1, 2, 3 },
                new int[] { 2, 4, 6 },
                false
        ));

        Assert.AreEqual(1000, box.calculateArea());
        Assert.AreEqual(0, box.countWithValue(true));
        Assert.AreEqual(1000, box.countWithValue(false));
    }

    [Test]
    public void testExample2() {
        var reactorReboot = new ReactorReboot(readInput("exampleC.txt"));

        var result = reactorReboot.countRebootCubes(int.MinValue, int.MaxValue);
        Assert.AreEqual(2758514936282235L, result);
    }

    [Test]
    public void testExample2For50A() {
        var reactorReboot = new ReactorReboot(readInput("exampleA.txt"));
        var result = reactorReboot.countRebootCubes(-50, 50);

        Assert.AreEqual(39, result);
    }

    [Test]
    public void testExample2For50B() {
        var reactorReboot = new ReactorReboot(readInput("exampleB.txt"));
        var result = reactorReboot.countRebootCubes(-50, 50);

        Assert.AreEqual(590784, result);
    }

    [Test]
    public void testExample2For50C() {
        var reactorReboot = new ReactorReboot(readInput("exampleC.txt"));
        var result = reactorReboot.countRebootCubes(-50, 50);

        Assert.AreEqual(474140, result);
    }

    [Test]
    public void testPuzzle2() {
        var reactorReboot = new ReactorReboot(readInput("input.txt"));

        var result = reactorReboot.countRebootCubes(int.MinValue, int.MaxValue);
        Console.WriteLine("Puzzle 2: " + result);
        Assert.AreEqual(946529356520531L, result); // too low
    }
}
