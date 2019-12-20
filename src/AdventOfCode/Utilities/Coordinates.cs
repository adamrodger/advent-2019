using System;
using System.Collections.Generic;

namespace AdventOfCode.Utilities
{
    public struct Point2D : IEquatable<Point2D>
    {
        public int X { get; }

        public int Y { get; }

        public Point2D(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool Equals(Point2D other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Point2D) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.X * 397) ^ this.Y;
            }
        }

        public static bool operator ==(Point2D left, Point2D right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Point2D left, Point2D right)
        {
            return !Equals(left, right);
        }

        public static implicit operator (int x, int y)(Point2D point)
        {
            return (point.X, point.Y);
        }

        public static implicit operator Point2D((int x, int y) coordinates)
        {
            return new Point2D(coordinates.x, coordinates.y);
        }

        public static Point2D operator +(Point2D a, Point2D b)
        {
            return new Point2D(a.X + b.X, a.Y + b.Y);
        }

        public IEnumerable<Point2D> Adjacent4()
        {
            yield return new Point2D(this.X, this.Y - 1);
            yield return new Point2D(this.X - 1, this.Y);
            yield return new Point2D(this.X + 1, this.Y);
            yield return new Point2D(this.X, this.Y + 1);
        }

        public override string ToString()
        {
            return $"{this.X}, {this.Y}";
        }
    }

    public class Point3D : IEquatable<Point3D>
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public Point3D()
        {
        }

        public Point3D(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public bool Equals(Point3D other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Point3D) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.X;
                hashCode = (hashCode * 397) ^ this.Y;
                hashCode = (hashCode * 397) ^ this.Z;
                return hashCode;
            }
        }

        public static bool operator ==(Point3D left, Point3D right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Point3D left, Point3D right)
        {
            return !Equals(left, right);
        }

        public static implicit operator (int x, int y, int z)(Point3D point)
        {
            return (point.X, point.Y, point.Z);
        }

        public static implicit operator Point3D((int x, int y, int z) coordinates)
        {
            return new Point3D(coordinates.x, coordinates.y, coordinates.z);
        }

        public static Point3D operator +(Point3D a, Point3D b)
        {
            return new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public override string ToString()
        {
            return $"{this.X}, {this.Y}, {this.Z}";
        }
    }
}
