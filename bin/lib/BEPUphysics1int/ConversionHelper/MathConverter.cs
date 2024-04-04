using FixMath.NET;
using Microsoft.Xna.Framework;

namespace ConversionHelper
{
    /// <summary>
    /// Helps convert between XNA math types and the BEPUphysics replacement math types.
    /// A version of this converter could be created for other platforms to ease the integration of the engine.
    /// </summary>
    public static class MathConverter
    {
        //Vector2
        public static Vector2 Convert(BEPUutilities.Vector2 bepuVector)
        {
            Vector2 toReturn;
            toReturn.X = (float)bepuVector.X;
            toReturn.Y = (float)bepuVector.Y;
            return toReturn;
        }

        public static void Convert(ref BEPUutilities.Vector2 bepuVector, out Vector2 xnaVector)
        {
            xnaVector.X = (float)bepuVector.X;
            xnaVector.Y = (float)bepuVector.Y;
        }

        public static BEPUutilities.Vector2 Convert(Vector2 xnaVector)
        {
            BEPUutilities.Vector2 toReturn;
            toReturn.X = (Fix64)xnaVector.X;
            toReturn.Y = (Fix64)xnaVector.Y;
            return toReturn;
        }

        public static void Convert(ref Vector2 xnaVector, out BEPUutilities.Vector2 bepuVector)
        {
            bepuVector.X = (Fix64)xnaVector.X;
            bepuVector.Y = (Fix64)xnaVector.Y;
        }

        //Vector3
        public static Vector3 Convert(BEPUutilities.Vector3 bepuVector)
        {
            Vector3 toReturn;
            toReturn.X = (float)bepuVector.X;
            toReturn.Y = (float)bepuVector.Y;
            toReturn.Z = (float)bepuVector.Z;
            return toReturn;
        }

        public static void Convert(ref BEPUutilities.Vector3 bepuVector, out Vector3 xnaVector)
        {
            xnaVector.X = (float)bepuVector.X;
            xnaVector.Y = (float)bepuVector.Y;
            xnaVector.Z = (float)bepuVector.Z;
        }

        public static BEPUutilities.Vector3 Convert(Vector3 xnaVector)
        {
            BEPUutilities.Vector3 toReturn;
            toReturn.X = (Fix64)xnaVector.X;
            toReturn.Y = (Fix64)xnaVector.Y;
            toReturn.Z = (Fix64)xnaVector.Z;
            return toReturn;
        }

        public static void Convert(ref Vector3 xnaVector, out BEPUutilities.Vector3 bepuVector)
        {
            bepuVector.X = (Fix64)xnaVector.X;
            bepuVector.Y = (Fix64)xnaVector.Y;
            bepuVector.Z = (Fix64)xnaVector.Z;
        }

        public static Vector3[] Convert(BEPUutilities.Vector3[] bepuVectors)
        {
            Vector3[] xnaVectors = new Vector3[bepuVectors.Length];
            for (int i = 0; i < bepuVectors.Length; i++)
            {
                Convert(ref bepuVectors[i], out xnaVectors[i]);
            }
            return xnaVectors;

        }

        public static BEPUutilities.Vector3[] Convert(Vector3[] xnaVectors)
        {
            var bepuVectors = new BEPUutilities.Vector3[xnaVectors.Length];
            for (int i = 0; i < xnaVectors.Length; i++)
            {
                Convert(ref xnaVectors[i], out bepuVectors[i]);
            }
            return bepuVectors;

        }

        //Matrix
        public static Matrix Convert(BEPUutilities.Matrix matrix)
        {
            Matrix toReturn;
            Convert(ref matrix, out toReturn);
            return toReturn;
        }

        public static BEPUutilities.Matrix Convert(Matrix matrix)
        {
            BEPUutilities.Matrix toReturn;
            Convert(ref matrix, out toReturn);
            return toReturn;
        }

        public static void Convert(ref BEPUutilities.Matrix matrix, out Matrix xnaMatrix)
        {
            xnaMatrix.M11 = (float)matrix.M11;
            xnaMatrix.M12 = (float)matrix.M12;
            xnaMatrix.M13 = (float)matrix.M13;
            xnaMatrix.M14 = (float)matrix.M14;

            xnaMatrix.M21 = (float)matrix.M21;
            xnaMatrix.M22 = (float)matrix.M22;
            xnaMatrix.M23 = (float)matrix.M23;
            xnaMatrix.M24 = (float)matrix.M24;

            xnaMatrix.M31 = (float)matrix.M31;
            xnaMatrix.M32 = (float)matrix.M32;
            xnaMatrix.M33 = (float)matrix.M33;
            xnaMatrix.M34 = (float)matrix.M34;

            xnaMatrix.M41 = (float)matrix.M41;
            xnaMatrix.M42 = (float)matrix.M42;
            xnaMatrix.M43 = (float)matrix.M43;
            xnaMatrix.M44 = (float)matrix.M44;

        }

        public static void Convert(ref Matrix matrix, out BEPUutilities.Matrix bepuMatrix)
        {
            bepuMatrix.M11 = (Fix64)matrix.M11;
            bepuMatrix.M12 = (Fix64)matrix.M12;
            bepuMatrix.M13 = (Fix64)matrix.M13;
            bepuMatrix.M14 = (Fix64)matrix.M14;

            bepuMatrix.M21 = (Fix64)matrix.M21;
            bepuMatrix.M22 = (Fix64)matrix.M22;
            bepuMatrix.M23 = (Fix64)matrix.M23;
            bepuMatrix.M24 = (Fix64)matrix.M24;

            bepuMatrix.M31 = (Fix64)matrix.M31;
            bepuMatrix.M32 = (Fix64)matrix.M32;
            bepuMatrix.M33 = (Fix64)matrix.M33;
            bepuMatrix.M34 = (Fix64)matrix.M34;

            bepuMatrix.M41 = (Fix64)matrix.M41;
            bepuMatrix.M42 = (Fix64)matrix.M42;
            bepuMatrix.M43 = (Fix64)matrix.M43;
            bepuMatrix.M44 = (Fix64)matrix.M44;

        }

        public static Matrix Convert(BEPUutilities.Matrix3x3 matrix)
        {
            Matrix toReturn;
            Convert(ref matrix, out toReturn);
            return toReturn;
        }

        public static void Convert(ref BEPUutilities.Matrix3x3 matrix, out Matrix xnaMatrix)
        {
            xnaMatrix.M11 = (float)matrix.M11;
            xnaMatrix.M12 = (float)matrix.M12;
            xnaMatrix.M13 = (float)matrix.M13;
            xnaMatrix.M14 = 0;

            xnaMatrix.M21 = (float)matrix.M21;
            xnaMatrix.M22 = (float)matrix.M22;
            xnaMatrix.M23 = (float)matrix.M23;
            xnaMatrix.M24 = 0;

            xnaMatrix.M31 = (float)matrix.M31;
            xnaMatrix.M32 = (float)matrix.M32;
            xnaMatrix.M33 = (float)matrix.M33;
            xnaMatrix.M34 = 0;

            xnaMatrix.M41 = 0;
            xnaMatrix.M42 = 0;
            xnaMatrix.M43 = 0;
            xnaMatrix.M44 = 1;
        }

        public static void Convert(ref Matrix matrix, out BEPUutilities.Matrix3x3 bepuMatrix)
        {
            bepuMatrix.M11 = (Fix64)matrix.M11;
            bepuMatrix.M12 = (Fix64)matrix.M12;
            bepuMatrix.M13 = (Fix64)matrix.M13;

            bepuMatrix.M21 = (Fix64)matrix.M21;
            bepuMatrix.M22 = (Fix64)matrix.M22;
            bepuMatrix.M23 = (Fix64)matrix.M23;

            bepuMatrix.M31 = (Fix64)matrix.M31;
            bepuMatrix.M32 = (Fix64)matrix.M32;
            bepuMatrix.M33 = (Fix64)matrix.M33;

        }

        //Quaternion
        public static Quaternion Convert(BEPUutilities.Quaternion quaternion)
        {
            Quaternion toReturn;
            toReturn.X = (float)quaternion.X;
            toReturn.Y = (float)quaternion.Y;
            toReturn.Z = (float)quaternion.Z;
            toReturn.W = (float)quaternion.W;
            return toReturn;
        }

        public static BEPUutilities.Quaternion Convert(Quaternion quaternion)
        {
            BEPUutilities.Quaternion toReturn;
            toReturn.X = (Fix64)quaternion.X;
            toReturn.Y = (Fix64)quaternion.Y;
            toReturn.Z = (Fix64)quaternion.Z;
            toReturn.W = (Fix64)quaternion.W;
            return toReturn;
        }

        public static void Convert(ref BEPUutilities.Quaternion bepuQuaternion, out Quaternion quaternion)
        {
            quaternion.X = (float)bepuQuaternion.X;
            quaternion.Y = (float)bepuQuaternion.Y;
            quaternion.Z = (float)bepuQuaternion.Z;
            quaternion.W = (float)bepuQuaternion.W;
        }

        public static void Convert(ref Quaternion quaternion, out  BEPUutilities.Quaternion bepuQuaternion)
        {
            bepuQuaternion.X = (Fix64)quaternion.X;
            bepuQuaternion.Y = (Fix64)quaternion.Y;
            bepuQuaternion.Z = (Fix64)quaternion.Z;
            bepuQuaternion.W = (Fix64)quaternion.W;
        }

        //Ray
        public static BEPUutilities.Ray Convert(Ray ray)
        {
            BEPUutilities.Ray toReturn;
            Convert(ref ray.Position, out toReturn.Position);
            Convert(ref ray.Direction, out toReturn.Direction);
            return toReturn;
        }

        public static void Convert(ref Ray ray, out BEPUutilities.Ray bepuRay)
        {
            Convert(ref ray.Position, out bepuRay.Position);
            Convert(ref ray.Direction, out bepuRay.Direction);
        }

        public static Ray Convert(BEPUutilities.Ray ray)
        {
            Ray toReturn;
            Convert(ref ray.Position, out toReturn.Position);
            Convert(ref ray.Direction, out toReturn.Direction);
            return toReturn;
        }

        public static void Convert(ref BEPUutilities.Ray ray, out Ray xnaRay)
        {
            Convert(ref ray.Position, out xnaRay.Position);
            Convert(ref ray.Direction, out xnaRay.Direction);
        }

        //BoundingBox
        public static BoundingBox Convert(BEPUutilities.BoundingBox boundingBox)
        {
            BoundingBox toReturn;
            Convert(ref boundingBox.Min, out toReturn.Min);
            Convert(ref boundingBox.Max, out toReturn.Max);
            return toReturn;
        }

        public static BEPUutilities.BoundingBox Convert(BoundingBox boundingBox)
        {
            BEPUutilities.BoundingBox toReturn;
            Convert(ref boundingBox.Min, out toReturn.Min);
            Convert(ref boundingBox.Max, out toReturn.Max);
            return toReturn;
        }

        public static void Convert(ref BEPUutilities.BoundingBox boundingBox, out BoundingBox xnaBoundingBox)
        {
            Convert(ref boundingBox.Min, out xnaBoundingBox.Min);
            Convert(ref boundingBox.Max, out xnaBoundingBox.Max);
        }

        public static void Convert(ref BoundingBox boundingBox, out BEPUutilities.BoundingBox bepuBoundingBox)
        {
            Convert(ref boundingBox.Min, out bepuBoundingBox.Min);
            Convert(ref boundingBox.Max, out bepuBoundingBox.Max);
        }

        //BoundingSphere
        public static BoundingSphere Convert(BEPUutilities.BoundingSphere boundingSphere)
        {
            BoundingSphere toReturn;
            Convert(ref boundingSphere.Center, out toReturn.Center);
            toReturn.Radius = (float)boundingSphere.Radius;
            return toReturn;
        }

        public static BEPUutilities.BoundingSphere Convert(BoundingSphere boundingSphere)
        {
            BEPUutilities.BoundingSphere toReturn;
            Convert(ref boundingSphere.Center, out toReturn.Center);
            toReturn.Radius = (Fix64)boundingSphere.Radius;
            return toReturn;
        }

        public static void Convert(ref BEPUutilities.BoundingSphere boundingSphere, out BoundingSphere xnaBoundingSphere)
        {
            Convert(ref boundingSphere.Center, out xnaBoundingSphere.Center);
            xnaBoundingSphere.Radius = (float)boundingSphere.Radius;
        }

        public static void Convert(ref BoundingSphere boundingSphere, out BEPUutilities.BoundingSphere bepuBoundingSphere)
        {
            Convert(ref boundingSphere.Center, out bepuBoundingSphere.Center);
            bepuBoundingSphere.Radius = (Fix64)boundingSphere.Radius;
        }

        //Plane
        public static Plane Convert(BEPUutilities.Plane plane)
        {
            Plane toReturn;
            Convert(ref plane.Normal, out toReturn.Normal);
            toReturn.D = (float)plane.D;
            return toReturn;
        }

        public static BEPUutilities.Plane Convert(Plane plane)
        {
            BEPUutilities.Plane toReturn;
            Convert(ref plane.Normal, out toReturn.Normal);
            toReturn.D = (Fix64)plane.D;
            return toReturn;
        }

        public static void Convert(ref BEPUutilities.Plane plane, out Plane xnaPlane)
        {
            Convert(ref plane.Normal, out xnaPlane.Normal);
            xnaPlane.D = (float)plane.D;
        }

        public static void Convert(ref Plane plane, out BEPUutilities.Plane bepuPlane)
        {
            Convert(ref plane.Normal, out bepuPlane.Normal);
            bepuPlane.D = (Fix64)plane.D;
        }
    }
}
