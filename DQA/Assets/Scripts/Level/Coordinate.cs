using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Coordinate : object{
    public int X;
    public int Z;

    public Coordinate(int x, int z) {
        X = x;
        Z = z;
    }

    public void Add(int x, int z) {
        X += x;
        Z += z;
    }

    public Vector3 GetWorldPosition() {
        return new Vector3(X * GameplayConstants.Instance.RoomSize, 0, Z * GameplayConstants.Instance.RoomSize);
    }

    public int Distance(Coordinate other) {
        int deltaX = Math.Abs(X - other.X);
        int deltaZ = Math.Abs(Z - other.Z);
        return deltaX + deltaZ;
    }

    public override int GetHashCode()
    {
        return X ^ Z;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public bool Equals(Coordinate p)
    {
        return X == p.X && Z == p.Z;
    }

    /// <summary>
    /// Returns the delta between another coord. EG. this=(1,0) other=(2,1). this.Delta(other) return (-1, -1)
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Coordinate Delta(Coordinate other) {
        return new Coordinate(other.X - this.X, other.Z - this.Z);
    }
}
