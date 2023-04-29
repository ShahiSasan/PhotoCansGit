using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace PhotoCansGit.UTL
{
    public static class  UTL
    {
          public static MeshGeometry3D CreateCylander(Point3D P3d,
   double Height, double Radius, int num_theta, bool peripheral)
    {
        return Task.Run(() =>
        {
            double cf = 2 * Math.PI / num_theta;
            var meshbody = new MeshGeometry3D();
            Point3DCollection PTS = new Point3DCollection();
            System.Windows.Media.PointCollection PTTX = new System.Windows.Media.PointCollection();

            Int32Collection Tring = new Int32Collection();

            for (double t = 0; t < 2 * Math.PI + cf; t += cf)
            {
                var pt = new Point3D(P3d.X + Radius * Math.Cos(t), P3d.Y + .5 * Height, P3d.Z + Radius * Math.Sin(t));
                PTS.Add(pt);
                var pt1 = new Point3D(P3d.X + Radius * Math.Cos(t), P3d.Y - .5 * Height, P3d.Z + Radius * Math.Sin(t));
                PTS.Add(pt1);
                PTTX.Add(new Point(t / (2 * Math.PI), 0));
                PTTX.Add(new Point(t / (2 * Math.PI), 1));
            }

            PTS.Add(new Point3D(P3d.X, P3d.Y + .5 * Height, P3d.Z));
            PTTX.Add(new Point(.5, 0));

            PTS.Add(new Point3D(P3d.X, P3d.Y - .5 * Height, P3d.Z));
            PTTX.Add(new Point(.5, 1));

            for (int k = 0; k < PTS.Count - 4; k += 1)
            {
                if (k % 2 == 0)
                {
                    Tring.Add(k);
                    Tring.Add(PTS.Count - 2);
                    Tring.Add(k + 2);
                    Tring.Add(k);
                    Tring.Add(k + 2);
                    Tring.Add(k + 1);
                    Tring.Add(k + 2);
                    Tring.Add(k + 3);
                    Tring.Add(k + 1);
                }
                if (k % 2 == 1)
                {
                    Tring.Add(k + 2);
                    Tring.Add(PTS.Count - 1);
                    Tring.Add(k);
                }

            }
            meshbody.Positions = PTS;
            meshbody.TextureCoordinates = PTTX;
            meshbody.TriangleIndices = Tring;
           
            meshbody.Freeze();
            return meshbody;
        }).GetAwaiter().GetResult();
    }
}
}
