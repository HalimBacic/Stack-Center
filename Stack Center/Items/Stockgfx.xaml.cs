using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Stack_Center.Items
{
    /// <summary>
    /// Interaction logic for Stockgfx.xaml
    /// </summary>
    public partial class Stockgfx : UserControl
    {
        Double height;
        Double width;
        Double length;
        Double weight;
        int quan;
        Shape shape;

        public Stockgfx()
        {
            InitializeComponent();
        }

        public Stockgfx(Ellipse el,Double h,Double wi,Double l,Double we,int q)
        {
            InitializeComponent();
            height = h;
            width = wi;
            length = l;
            weight = we;
            quan = q;
            shape = el;
            uc.Height = shape.Height + 150;
            uc.Width = shape.Width + 150;
            gridUC.Height = gridUC.Height + 150;
            gridUC.Width = gridUC.Width + 150;
            canvasUC.Children.Add(shape);
        }

        public Stockgfx(Rectangle rect, Double h, Double wi, Double l, Double we, int q)
        {
            InitializeComponent();
            height = h;
            width = wi;
            length = l;
            weight = we;
            quan = q;
            shape = rect;
            uc.Height = shape.Height + 150;
            uc.Width = shape.Width + 150;
            gridUC.Height = gridUC.Height + 150;
            gridUC.Width = gridUC.Width + 150;
            canvasUC.Children.Add(shape);
        }
    }
}
