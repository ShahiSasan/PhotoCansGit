using PhotoCansGit.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace PhotoCansGit.ViewModel
{
   public class MainVM:ObservableObject
    {public Viewport3D Vws;
        Visual3D _model3D;
        public Visual3D model3D
        {
            get { return _model3D; }
            set { _model3D = value;
                OnPropertyChanged("model3D");
            }
        }
        Point3D _Center;
        public Point3D Center
        {
            get { return _Center; }
            set { _Center = value;
                OnPropertyChanged("Center");
            }
        }
        double _height;
        public double height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged("height");
            }
        }
        public MainVM(Viewport3D vw3d)
        {
            Vws = vw3d;
            Center = new Point3D(.5 * Vws.ActualWidth, .5 * Vws.ActualHeight, 0);
            height = .05 * vw3d.ActualHeight;
            var kz = new Kerze() { AnimateShole=true};
            kz.Initialize().GetAwaiter().GetResult();
            model3D = kz.CreateModel(Center,height);
            Vws.Children.Add(model3D);
        }
    }
}
