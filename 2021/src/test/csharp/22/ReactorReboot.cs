using System;
using System.Text.RegularExpressions;

namespace AoC._22;

/**
 * <a href="https://adventofcode.com/2021/day/22">Day 22: Reactor Reboot</a>
 */

public sealed class ReactorReboot {
    private static readonly Regex lineSplitter = new("(on x=|off x=|\\.\\.|,y=|,z=)");

    private readonly string[] inputLines;
    private readonly Bounds[] cubes;

    public ReactorReboot(params string[] inputLines) {
        this.inputLines = inputLines;

        CreateCubes();
    }

    private void CreateCubes() {
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
        throw new Exception();
    }
}

public readonly struct Bounds {
    public long CountRebootCubes() => throw new NotImplementedException();
    public Bounds Reboot(int size) => throw new NotImplementedException();

    public readonly (int x, int y, int z) min;
    public readonly (int x, int y, int z) max;
    public readonly bool value;

    public Bounds(int extends) : this((-extends, -extends, -extends), (extends, extends, extends)) {
    }

    public Bounds((int x, int y, int z) min, (int x, int y, int z) max, bool value = false) {
        this.min = min;
        this.max = max;
        this.value = value;
    }
}