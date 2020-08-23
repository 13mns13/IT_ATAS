using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace Atlas_of_innovation
{
    class MapClass
    {
        public GMapControl gMapControl { get; set; }
        public MapClass(GMapControl gMapControl)
        {
            this.gMapControl = gMapControl;
        }

        /// <summary>
        /// Загрузка карты
        /// </summary>
        /// <param name="gMapControl1"></param>
        public void LoadMap()
        {
            gMapControl.DragButton = MouseButtons.Left;
            gMapControl.MapProvider = GMapProviders.GoogleMap;
            gMapControl.MinZoom = 5;
            gMapControl.MaxZoom = 100;
            gMapControl.Zoom = 10;
            gMapControl.Position = new PointLatLng(47, 41);
            gMapControl.OnMarkerClick += (a, b) => {
                gMapControl.Position = new PointLatLng(a.Position.Lat, a.Position.Lng);
                gMapControl.Zoom = 15; };
        }

        /// <summary>
        /// Добавить точку на карте
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Add_Marker(double x, double y)
        {
            gMapControl.Overlays.Add(new GMapOverlay());
            GMarkerGoogle gm = new GMarkerGoogle(new PointLatLng(x, y), GMarkerGoogleType.blue);          
            gMapControl.Overlays[gMapControl.Overlays.Count - 1].Markers.Add(gm);
            //gMapControl.Overlays[gMapControl.Overlays.Count - 1].Markers.Add(new GMarkerGoogle(gMapControl.FromLocalToLatLng((int)x, (int)y), GMarkerGoogleType.red));
        }//готово



    }
}
