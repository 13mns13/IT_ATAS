using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;

namespace Atlas_of_innovation
{
    public partial class MapPanel : UserControl
    {

        public MapPanel()
        {
            InitializeComponent();

            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.MinZoom = 5;
            gMapControl1.MaxZoom = 15;
            gMapControl1.Zoom = 15;
            gMapControl1.Position = new PointLatLng(47, 41);
            gMapControl1.OnMarkerClick += (a, b) =>
            {
                gMapControl1.Position = new PointLatLng(a.Position.Lat, a.Position.Lng);
                gMapControl1.Zoom = 15;
                if (onButtonClick == null)
                    return;
                onButtonClick(this,a.Tag.ToString());

            };
        }
        public void AddMarker(int index, double x, double y)
        {
            gMapControl1.Overlays.Add(new GMapOverlay());
            GMarkerGoogle gm = new GMarkerGoogle(new PointLatLng(x, y), GMarkerGoogleType.blue);
            gm.Tag = index;
            gMapControl1.Overlays[gMapControl1.Overlays.Count - 1].Markers.Add(gm);
            //gMapControl.Overlays[gMapControl.Overlays.Count - 1].Markers.Add(new GMarkerGoogle(gMapControl.FromLocalToLatLng((int)x, (int)y), GMarkerGoogleType.red));
        }//

        public delegate void CallFormMethod(object obj, string e);
        public event CallFormMethod onButtonClick;

    }
}
