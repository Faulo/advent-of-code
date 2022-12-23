using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AoC._22;

/**
 * <a href="https://adventofcode.com/2021/day/22">Day 22: Reactor Reboot</a>
 */

public sealed class ReactorReboot {
    private static readonly Regex lineSplitter = new(@"(on|off) x=(-?\d+)\.\.(-?\d+),y=(-?\d+)\.\.(-?\d+),z=(-?\d+)\.\.(-?\d+)");

    private readonly string[] inputLines;
    private readonly Bounds[] cubes;

    public ReactorReboot(params string[] inputLines) {
        this.inputLines = inputLines;

        cubes = inputLines
            .Where(line => lineSplitter.IsMatch(line))
            .Select(line => lineSplitter.Match(line))
            .Select(match => match.Groups)
            .Select(groups => new Bounds(groups[2].Value, groups[3].Value, groups[4].Value, groups[5].Value, groups[6].Value, groups[7].Value, groups[1].Value))
            .ToArray();

        Assert.AreEqual(inputLines.Length, cubes.Length);
    }

    public long CountRebootCubes() {
        // auto-detect bounds
        (int x, int y, int z) min = new();
        (int x, int y, int z) max = new();
        for (var i = 0; i < cubes.Length; i++) {
            var cube = cubes[i];
            min.x = Math.Min(min.x, cube.min.x);
            min.y = Math.Min(min.y, cube.min.y);
            min.z = Math.Min(min.z, cube.min.z);

            max.x = Math.Min(max.x, cube.max.x);
            max.y = Math.Min(max.y, cube.max.y);
            max.z = Math.Min(max.z, cube.max.z);
        }
        return Reboot(new Bounds(min, max));
    }

    public long CountRebootCubes(int extends) {
        // use bounds with this extends
        return Reboot(new Bounds(extends));
    }

    /// <summary>
    /// Count all cubes that are ON within <paramref name="bounds"/>.
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private long Reboot(Bounds bounds) {
        long count = 0;
        foreach (var position in bounds.AllPositionsWithin) {
            var value = false;
            for (var i = 0; i < cubes.Length; i++) {
                if (cubes[i].Contains(position)) {
                    value = cubes[i].value;
                }
            }
            if (value) {
                count++;
            }
        }
        return count;
    }
}

public readonly struct Bounds {
    public long CountRebootCubes() => throw new NotImplementedException();
    public Bounds Reboot(int size) => throw new NotImplementedException();

    public readonly (int x, int y, int z) min;
    public readonly (int x, int y, int z) max;
    public readonly bool value;

    public IEnumerable<(int x, int y, int z)> AllPositionsWithin {
        get {
            for (var x = min.x; x <= max.x; x++) {
                for (var y = min.y; y <= max.y; y++) {
                    for (var z = min.z; z <= max.z; z++) {
                        yield return (x, y, z);
                    }
                }
            }
        }
    }

    public bool Contains((int x, int y, int z) position) {
        return position.x >= min.x && position.x <= max.x
            && position.y >= min.y && position.y <= max.y
            && position.z >= min.z && position.z <= max.z;
    }

    public Bounds(int extends)
        : this((-extends, -extends, -extends), (extends, extends, extends)) {
    }

    public Bounds(string minX, string maxX, string minY, string maxY, string minZ, string maxZ, string onOrOff)
        : this((int.Parse(minX), int.Parse(minY), int.Parse(minZ)), (int.Parse(maxX), int.Parse(maxY), int.Parse(maxZ)), onOrOff == "on") {
    }

    public Bounds((int x, int y, int z) min, (int x, int y, int z) max, bool value = false) {
        this.min = min;
        this.max = max;
        this.value = value;
    }
}