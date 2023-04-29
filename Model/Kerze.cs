using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;


namespace PhotoCansGit.Model
{
    public class Kerze 
    {
      
        public double RadiusKZ { get; set; }
        public Brush BadaneBrush
        {
            get; set;
        }
        public bool AnimateShole { get; set; }

       
        List<MeshGeometry3D> KZMS;
        public Task Initialize()
        {// create meshgeometry for all component
            return Task.Run(() =>  {
               
                    var ls = new List<MeshGeometry3D>();
                    var mss3 = PhotoCansGit.UTL.UTL.CreateCylander(new Point3D(0, .35, 0), .7, .05, 20, true);
                    mss3.Freeze();
                    ls.Add(mss3);
                    var zarib = .085;
                    var ghubmesh = new MeshGeometry3D();
                    var teta = 2 * (double)Math.PI / 10;
                    double decrease = .25;
                    Point3DCollection PTS = new Point3DCollection();
                    System.Windows.Media.PointCollection PTTX = new System.Windows.Media.PointCollection();

                    Int32Collection Tring = new Int32Collection();
                    for (double yy = mss3.Positions.Max(c => c.Y) + .05; decrease > 0; yy += .12 * (double)zarib, decrease -= .016)
                        for (double t = 0; t <= 2 * Math.PI; t += teta)
                        {
                            PTS.Add(new Point3D(decrease * zarib * Math.Cos(t), yy, -decrease * zarib * Math.Sin(t)));
                            PTTX.Add(new Point((double)t / (2 * Math.PI), yy / .6));
                        }

                    for (int t = 0; t < PTS.Count; t++)
                    {
                        Tring.Add(t);
                        Tring.Add(t + 1);
                        Tring.Add(t + 10 + 1);
                        Tring.Add(t + 10 + 1);
                        Tring.Add(t + 10);
                        Tring.Add(t);
                    }
                    ghubmesh.Positions = PTS;
                    ghubmesh.TextureCoordinates = PTTX;
                    ghubmesh.TriangleIndices = Tring;
                    ghubmesh.Freeze();
                    ls.Add(ghubmesh);
                    var nakhmesh = PhotoCansGit.UTL.UTL.CreateCylander(new Point3D(0, .73, 0), .1, .007, 10, true);
                    nakhmesh.Freeze();
                    ls.Add(nakhmesh);
                     KZMS = ls;
                

            });
        }
        public  Visual3D CreateModel(Point3D P3d, double Height)
        {
            var md3 = new ModelVisual3D() { };
            Model3DGroup vd3 = null;
            //For Simple Candle We need tree GeometryModel3D Create MeshGeometry For each then group them to vd3

            GeometryModel3D ms3 = null;
            GeometryModel3D nakhmd = null;
            GeometryModel3D ghub = null;

            Task.Run(() =>
            {
                vd3 = new Model3DGroup();
               
                int resol = 50;
                var cf = 2 * (double)Math.PI / resol;
                var zarib = (double)Height / 10;
                

               Task.Run(() =>
                {
                    ms3 = new GeometryModel3D();
                    
                    
                        var mat3d = new MaterialGroup();
                        mat3d.Children.Add(new DiffuseMaterial() { Brush = BadaneBrush != null ? BadaneBrush : Brushes.MintCream });
                        mat3d.Freeze();
                        ms3.Material = mat3d;
                   
                    
                    ms3.Geometry = KZMS[0];
                    var tr = new Transform3DGroup();
                    var scxz = RadiusKZ != 0 ? RadiusKZ * Height : Height;
                    tr.Children.Add(new ScaleTransform3D() { ScaleX = scxz, ScaleY = Height, ScaleZ = scxz });
                    tr.Children.Add(new TranslateTransform3D() { OffsetX = P3d.X, OffsetY = P3d.Y, OffsetZ = P3d.Z });
                    ms3.Transform = tr;
                    ms3.Freeze();
                    ghub = new GeometryModel3D();
                   
                    ghub.Geometry = KZMS[1];
                    var trr = new Transform3DGroup();
                    trr.Children.Add(new ScaleTransform3D() { ScaleX = Height, ScaleY = Height, ScaleZ = Height });
                    trr.Children.Add(new TranslateTransform3D() { OffsetX = P3d.X, OffsetY = P3d.Y, OffsetZ = P3d.Z });
                    ghub.Transform = trr;
                    var brs = new LinearGradientBrush() { StartPoint = new Point(.5, 1), EndPoint = new Point(.5, 0) };
                    brs.GradientStops.Add(new GradientStop() { Color = Color.FromRgb(255, 69, 0), Offset = .2 });
                    brs.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(200, 184, 175, 125), Offset = .5 });
                    brs.GradientStops.Add(new GradientStop() { Color = Color.FromRgb(255, 165, 0), Offset = .7 });

                    brs.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(70, 20, 36, 61), Offset = .9 });
                    brs.GradientStops.Add(new GradientStop() { Color = Color.FromArgb(100, 20, 36, 61), Offset = 1 });
                    brs.Freeze();
                    var grpmat = new MaterialGroup();
                    grpmat.Children.Add(new DiffuseMaterial() { Brush = brs });
                    grpmat.Children.Add(new EmissiveMaterial() { Brush = brs });
                    grpmat.Freeze();
                    ghub.Material = ghub.BackMaterial = grpmat;
                    ghub.Freeze();
                
                    nakhmd = new GeometryModel3D();
                    nakhmd.Geometry = KZMS[2];
                    var trd = new Transform3DGroup();

                    trd.Children.Add(new ScaleTransform3D() { ScaleX = Height, ScaleY = Height, ScaleZ = Height });
                    trd.Children.Add(new TranslateTransform3D() { OffsetX = P3d.X, OffsetY = P3d.Y, OffsetZ = P3d.Z });
                    nakhmd.Transform = trd;
                    nakhmd.Material = new DiffuseMaterial() { Brush = Brushes.Gray };
                    nakhmd.Freeze();
                }).GetAwaiter().GetResult();

             
                vd3.Children.Add(ms3);
                vd3.Children.Add(nakhmd);

                vd3.Freeze();
            }).GetAwaiter().GetResult();
            
            var ghobul = new ModelVisual3D();


            ghobul.Content = ghub;
            if (AnimateShole)// if want Flame is Animated we can do so.
            {
                var trt = new Transform3DGroup();
                var sci = new ScaleTransform3D() { CenterX = P3d.X, CenterZ = P3d.Z, CenterY = ((MeshGeometry3D)KZMS[1]).Positions.Min(c => c.Y) * Height + P3d.Y };
                var rot = new RotateTransform3D() { CenterX = P3d.X, CenterZ = P3d.Z, CenterY = ((MeshGeometry3D)KZMS[1]).Positions.Min(c => c.Y) * Height + P3d.Y };
                var ax = new AxisAngleRotation3D() { Axis = new Vector3D(-.3, 0, -1) };
                rot.Rotation = ax;
                Task.Run(() =>
                {
                    DoubleAnimation scx = null;
                    DoubleAnimation scy = null;
                    DoubleAnimation scz = null;

                    DoubleAnimation rcz = null;
                  
                        scx = new DoubleAnimation(.9, 1.6, TimeSpan.FromSeconds(2)) { RepeatBehavior = RepeatBehavior.Forever, AutoReverse = true, EasingFunction = new SineEase() };
                        scx.Freeze();
                        scy = new DoubleAnimation(.9, 1.7, TimeSpan.FromSeconds(2)) { RepeatBehavior = RepeatBehavior.Forever, BeginTime = TimeSpan.FromSeconds(.5), AutoReverse = true, EasingFunction = new BackEase() };
                        scy.Freeze();
                        scz = new DoubleAnimation(.9, 1.1, TimeSpan.FromSeconds(2)) { RepeatBehavior = RepeatBehavior.Forever, BeginTime = TimeSpan.FromSeconds(.7), AutoReverse = true };
                        scz.Freeze();
                        rcz = new DoubleAnimation(-5, 5, TimeSpan.FromSeconds(2)) { RepeatBehavior = RepeatBehavior.Forever, BeginTime = TimeSpan.FromSeconds(.5), AutoReverse = true };
                        rcz.Freeze();
                  
                    return new AnimationTimeline[] { scx, scy, scz, rcz };
                }).ContinueWith(async (res) => await App.Current.Dispatcher.InvokeAsync(() => 
                {


                    sci.BeginAnimation(ScaleTransform3D.ScaleXProperty, res.Result[0]);
                    sci.BeginAnimation(ScaleTransform3D.ScaleYProperty, res.Result[1]);
                    sci.BeginAnimation(ScaleTransform3D.ScaleZProperty, res.Result[2]);
                    ax.BeginAnimation(AxisAngleRotation3D.AngleProperty, res.Result[3]);
                }, System.Windows.Threading.DispatcherPriority.Input));
                trt.Children.Add(sci);
                trt.Children.Add(rot);
                ghobul.Transform = trt;



            }


            md3.Children.Add(ghobul);


            md3.Content = vd3;



            return md3;
        }
      
    }
}
