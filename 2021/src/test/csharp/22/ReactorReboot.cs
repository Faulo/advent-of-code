using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AoC._22;
/**
 * <a href="https://adventofcode.com/2021/day/22">Day 22: Reactor Reboot</a>
 */
public sealed class ReactorReboot {

    public sealed class Cube {

        private readonly int minX;
        private readonly int maxX;
        private readonly int minY;
        private readonly int maxY;
        private readonly int minZ;
        private readonly int maxZ;
        private readonly bool[][][] elements;

        public Cube(int minX, int maxX, int minY, int maxY, int minZ, int maxZ) {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
            this.minZ = minZ;
            this.maxZ = maxZ;

            elements = new bool[maxX - minX + 1][][];
            for (var x = 0; x < elements.Length; x++) {
                elements[x] = new bool[maxY - minY + 1][];
                for (var y = 0; y < elements[x].Length; y++) {
                    elements[x][y] = new bool[maxZ - minZ + 1];
                }
            }
        }

        public void setValues(int x1, int x2, int y1, int y2, int z1, int z2, bool value) {
            // don't do anything if values are completely outside the cube
            if ((x1 < minX && x2 < minX) || (x1 > maxX && x2 > maxX)) {
                return;
            }

            if ((y1 < minY && y2 < minY) || (y1 > maxY && y2 > maxY)) {
                return;
            }

            if ((z1 < minZ && z2 < minZ) || (z1 > maxZ && z2 > maxZ)) {
                return;
            }

            // make sure the coordinates are inside the cube
            var sanitizeX1 = sanitize(x1, minX, maxX);
            var sanitizeX2 = sanitize(x2, minX, maxX);
            var sanitizeY1 = sanitize(y1, minY, maxY);
            var sanitizeY2 = sanitize(y2, minY, maxY);
            var sanitizeZ1 = sanitize(z1, minZ, maxZ);
            var sanitizeZ2 = sanitize(z2, minZ, maxZ);

            // now set the correct elements to the value
            for (var x = sanitizeX1; x <= sanitizeX2; x++) {
                for (var y = sanitizeY1; y <= sanitizeY2; y++) {
                    for (var z = sanitizeZ1; z <= sanitizeZ2; z++) {
                        elements[x - minX][y - minY][z - minZ] = value;
                    }
                }
            }
        }

        private int sanitize(int value, int min, int max) {
            return Math.Min(max, Math.Max(min, value));
        }

        public long countCubes(bool value) {
            long result = 0;
            for (var x = 0; x < elements.Length; x++) {
                for (var y = 0; y < elements[x].Length; y++) {
                    for (var z = 0; z < elements[x][y].Length; z++) {
                        if (elements[x][y][z] == value) {
                            result++;
                        }
                    }
                }
            }
            return result;
        }
    }

    private readonly string[] inputLines;

    public ReactorReboot(params string[] inputLines) {
        this.inputLines = inputLines;
    }

    // NOTE: this method does not work for the big files
    public Cube rebootToMaxSize() {
        var minX = int.MaxValue;
        var minY = int.MaxValue;
        var minZ = int.MaxValue;
        var maxX = 0;
        var maxY = 0;
        var maxZ = 0;

        foreach (var line in inputLines) {
            // on x=-20..26,y=-36..17,z=-47..7
            var split = line.Split("(on x=|off x=|\\.\\.|,y=|,z=)");

            minX = Math.Min(minX, Math.Min(int.Parse(split[1]), int.Parse(split[2])));
            minY = Math.Min(minY, Math.Min(int.Parse(split[3]), int.Parse(split[4])));
            minZ = Math.Min(minZ, Math.Min(int.Parse(split[5]), int.Parse(split[6])));

            maxX = Math.Max(maxX, Math.Max(int.Parse(split[1]), int.Parse(split[2])));
            maxY = Math.Max(maxY, Math.Max(int.Parse(split[3]), int.Parse(split[4])));
            maxZ = Math.Max(maxZ, Math.Max(int.Parse(split[5]), int.Parse(split[6])));
        }

        return reboot(minX, maxX, minY, maxY, minZ, maxZ);
    }

    public Cube reboot(int size) {
        return reboot(-size, size, -size, size, -size, size);
    }

    private Cube reboot(int minX, int maxX, int minY, int maxY, int minZ, int maxZ) {
        var result = new Cube(minX, maxX, minY, maxY, minZ, maxZ);
        foreach (var line in inputLines) {
            // on x=-20..26,y=-36..17,z=-47..7
            var split = line.Split("(on x=|off x=|\\.\\.|,y=|,z=)");
            result.setValues(
                    int.Parse(split[1]), int.Parse(split[2]),
                    int.Parse(split[3]), int.Parse(split[4]),
                    int.Parse(split[5]), int.Parse(split[6]),
                    line[1] == 'n');
        }
        return result;
    }

    public long countRebootCubes(int minValue, int maxValue) {
        var result = new Box(minValue, maxValue, false);
        foreach (var line in inputLines) {
            // on x=-20..26,y=-36..17,z=-47..7
            var split = line.Split("(on x=|off x=|\\.\\.|,y=|,z=)");

            var minCoordinates = new int[] { int.Parse(split[1]), int.Parse(split[3]), int.Parse(split[5]) };
            var maxCoordinates = new int[] { int.Parse(split[2]), int.Parse(split[4]), int.Parse(split[6]) };
            var possibleChild = new Box(minCoordinates, maxCoordinates, line[1] == 'n');

            result.traverse(box => box.createIntersectionChildren(possibleChild));
        }
        return result.countWithValue(true);
    }


    public class Box {

        private readonly int[] minCoordinates;
        private readonly int[] maxCoordinates;
        private readonly bool value;
        public List<Box> children = new();

        public Box(int minCoordinate, int maxCoordinate, bool value) : this(new int[] { minCoordinate, minCoordinate, minCoordinate }, new int[] { maxCoordinate, maxCoordinate, maxCoordinate }, value) {
        }

        public Box(int[] minCoordinates, int[] maxCoordinates, bool value) {
            this.minCoordinates = minCoordinates;
            this.maxCoordinates = maxCoordinates;
            this.value = value;
        }

        public long calculateArea() {
            long result = 1;
            for (var i = 0; i < minCoordinates.Length; i++) {
                result *= maxCoordinates[i] - minCoordinates[i] + 1;
            }
            return result;
        }

        public void traverse(Action<Box> consumer) {
            var childrenArray = children.ToArray();
            consumer(this);
            foreach (var child in childrenArray) {
                child.traverse(consumer);
            }
        }

        public long countWithValue(bool value) {
            if (this.value == value) {
                return calculateArea() - calculateChildrenWithValue(!value);
            }
            return calculateChildrenWithValue(value);
        }

        private long calculateChildrenWithValue(bool value) {
            long result = 0;
            var childrenArray = children.ToArray();

            foreach (var child in childrenArray) {
                result += child.countWithValue(value);
            }

            // FIXME: this works so far, but the current problem is that two intersecting children have their intersection
            // counted twice. So for the A example, the 8 cubes between the first and second child are counted double
            for (var i = 0; i < childrenArray.Length; i++) {
                var child1 = childrenArray[i];

                for (var j = i + 1; j < childrenArray.Length; j++) {
                    var child2 = childrenArray[j];

                    // the problem only persists for children with the same value
                    if (child1.value != child2.value) {
                        continue;
                    }

                    // and if they don't intersect, we don't have a problem
                    if (!child1.intersects(child2)) {
                        continue;
                    }

                    // so now we have an intersection, but this intersection might again be changed by the
                    // parents' children (they have the same children for the intersection part)
                    var intersection = child1.createIntersection(child2, child2.value);
                    intersection.createIntersectionChildren(child2.children);
                    var intersectionValue = intersection.countWithValue(child2.value);
                    Assert.IsTrue(intersectionValue <= result, "Intersection value should not be bigger than result " + intersectionValue + " < " + result);
                    result -= intersectionValue;
                }
            }

            return result;
        }

        public void createIntersectionChildren(List<Box> possibleChildren) {
            foreach (var possibleChild in possibleChildren) {
                createIntersectionChildren(possibleChild);
            }
        }

        public void createIntersectionChildren(Box possibleChild) {
            // if the boxes do not intersect we can ignore it (the box WILL intersect the result box anyway)
            if (!intersects(possibleChild)) {
                return;
            }

            // if the box has the same value, we do not need to check this box
            if (value == possibleChild.value) {
                return;
            }

            var intersection = createIntersection(possibleChild, possibleChild.value);
            children.Add(intersection);
        }

        public bool intersects(Box other) {
            for (var i = 0; i < minCoordinates.Length; i++) {
                if (other.minCoordinates[i] > maxCoordinates[i]) {
                    return false;
                }
                if (minCoordinates[i] > other.maxCoordinates[i]) {
                    return false;
                }
            }
            return true;
        }

        public Box createIntersection(Box other, bool newValue) {
            var newMinCoordinates = new int[3];
            var newMaxCoordinates = new int[3];

            for (var i = 0; i < newMinCoordinates.Length; i++) {
                newMinCoordinates[i] = Math.Max(other.minCoordinates[i], minCoordinates[i]);
                newMaxCoordinates[i] = Math.Min(other.maxCoordinates[i], maxCoordinates[i]);
            }
            return new Box(newMinCoordinates, newMaxCoordinates, newValue);
        }
    }
}
