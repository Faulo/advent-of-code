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
        var list = new List<Bounds>(cubes);

        for (var currentIndex = 0; currentIndex < list.Count; currentIndex++) {
            var wasDivided = false;
            do {
                for (var i = currentIndex + 1; i < list.Count; i++) {
                    if (Bounds.TryDivide(list, currentIndex, i)) {
                        wasDivided = true;
                        break;
                    }
                }
            } while (wasDivided);
        }

        // now, all bounds in list are not-overlapping, hopefully
        long count = 0;
        foreach (var bounds in list) {
            if (bounds.value) {
                count += bounds.positionCount;
            }
        }
        return count;
    }

    public long CountRebootCubesAutoDetectBounds() {
        Vector3 min = new();
        Vector3 max = new();
        foreach (var cube in cubes) {
            if (cube.value) {
                for (var j = 0; j < 3; j++) {
                    min[j] = Math.Min(min[j], cube.min[j]);
                    max[j] = Math.Min(max[j], cube.max[j]);
                }
            }
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

public struct Vector3 {
    public int this[int i] {
        get => i switch {
            0 => x,
            1 => y,
            2 => z,
            _ => throw new NotImplementedException(),
        };
        set {
            switch (i) {
                case 0:
                    x = value;
                    break;
                case 1:
                    y = value;
                    break;
                case 2:
                    z = value;
                    break;
            }
        }
    }

    public int x;
    public int y;
    public int z;

    public Vector3(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static Vector3 operator +(Vector3 lhs, Vector3 rhs) {
        return new(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
    }

    public static Vector3 operator -(Vector3 lhs, Vector3 rhs) {
        return new(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
    }

    public static Vector3 operator /(Vector3 lhs, int divisor) {
        return new(lhs.x / divisor, lhs.y / divisor, lhs.z / divisor);
    }
}

public readonly struct Bounds {
    public static bool TryDivide(List<Bounds> list, int i, int j) {
        var lhs = list[i];
        var rhs = list[j];

        if (!lhs.value) {
            // no need to divide if previous step is to turn OFF
            return false;
        }

        if (!lhs.OverlapsWith(rhs)) {
            // no need to divide if the bounds don't overlap
            return false;
        }

        return false;
    }

    public Bounds CreateIntersection(Bounds other, bool value) {
        Vector3 min = new();
        Vector3 max = new();
        for (var i = 0; i < 3; i++) {
            min[i] = Math.Max(other.min[i], this.min[i]);
            max[i] = Math.Min(other.max[i], this.max[i]);
        }
        return new Bounds(min, max, value);
    }

    private bool OverlapsWith(Bounds bounds) {
        if (Contains(bounds.center) || bounds.Contains(center)) {
            return true;
        }
        for (var i = 0; i < 8; i++) {
            if (Contains(bounds.vertices[i]) || bounds.Contains(vertices[i])) {
                return true;
            }
        }
        return false;
    }

    public readonly Vector3 min;
    public readonly Vector3 max;

    public readonly Vector3 center;
    public readonly Vector3 size;

    public readonly Vector3[] vertices;

    public readonly bool value;
    public readonly int positionCount;

    public IEnumerable<Vector3> AllPositionsWithin {
        get {
            for (var x = min.x; x <= max.x; x++) {
                for (var y = min.y; y <= max.y; y++) {
                    for (var z = min.z; z <= max.z; z++) {
                        yield return new(x, y, z);
                    }
                }
            }
        }
    }

    public bool Contains(Vector3 position) {
        return position.x >= min.x && position.x <= max.x
            && position.y >= min.y && position.y <= max.y
            && position.z >= min.z && position.z <= max.z;
    }

    public Bounds(int extends)
        : this(new(-extends, -extends, -extends), new(extends, extends, extends)) {
    }

    public Bounds(string minX, string maxX, string minY, string maxY, string minZ, string maxZ, string onOrOff)
        : this(new(int.Parse(minX), int.Parse(minY), int.Parse(minZ)), new(int.Parse(maxX), int.Parse(maxY), int.Parse(maxZ)), onOrOff == "on") {
    }

    public Bounds(Vector3 min, Vector3 max, bool value = false) {
        this.min = min;
        this.max = max;
        this.value = value;

        center = (min + max) / 2;
        size = max - min;
        positionCount = size.x * size.y * size.z;
        vertices = new[] {
            min + new Vector3(0, 0, 0),
            min + new Vector3(size.x, 0, 0),
            min + new Vector3(0, size.y, 0),
            min + new Vector3(size.x, size.y, 0),
            min + new Vector3(0, 0, size.z),
            min + new Vector3(size.x, 0, size.z),
            min + new Vector3(0, size.y, size.z),
            min + new Vector3(size.x, size.y, size.z),
        };
    }
}