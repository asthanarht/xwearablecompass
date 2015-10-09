using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.Wearable.Views;
using Android.Views.Animations;
using Android.Hardware;
using Android.Hardware.Location;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Java.Interop;

namespace Compass
{
    [Activity(Label = "Compass", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity,ISensorEventListener
    {
        //Initialize sensor manager for compass direction
        private SensorManager sensorManager;
        private float currentDegree = 0f;

        //Initilaize global variable for UI view
        ImageView outerDialImage;
        ImageView dialImage;
        TextView headingDirection;
        TextView headingDegree;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Initilize system sensor service 
            sensorManager = (SensorManager)GetSystemService(SensorService);

            // WatchViewStub determine wearable type and render view accordinglr from specfic xml defination 
            var v = FindViewById<WatchViewStub>(Resource.Id.watch_view_stub);

            //Infalte watchviewstub
            v.LayoutInflated += delegate
            {

                // Get elemnt from the layout resource,
                outerDialImage = FindViewById<ImageView>(Resource.Id.imageViewCompass);
                headingDirection = FindViewById<TextView>(Resource.Id.headingDirection);
                headingDegree = FindViewById<TextView>(Resource.Id.headingDegree);
                dialImage = FindViewById<ImageView>(Resource.Id.imageViewDial);
               
            };
        }

        protected override void OnResume()
        {
            base.OnResume();
            // Listen sensor manger on resume activity of app
            sensorManager.RegisterListener(this, sensorManager.GetDefaultSensor(SensorType.Orientation),SensorDelay.Game);
        }


        protected override void OnPause()
        {
            base.OnPause();
            // stop listening if app is no longer used. This is importent step otherwise it will drain your watch battery.
            sensorManager.UnregisterListener(this);
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {

        }

        public void OnSensorChanged(SensorEvent e)
        {
            //Get sensor degree
            float degree = (float)Math.Round(e.Values[0]);

            //values[0]: Angle between the magnetic north direction and the y-axis around the z-axis (0 to 359), where 0=North, 90=East, 180=South, 270=West.
            //values[1]: Rotation around x-axis (-180 to 180), with positive values when the z-axis moves towards the y-axis.
            //values[2]: Rotation around x-axis, (-90 to 90), increasing as the device moves clockwise.

            //calculate direction coordinal
            headingDirection.Text = CalculateDirection(degree);

            // Set text for degree
            headingDegree.Text = Convert.ToString(degree) + "\u00B0";
            outerDialImage.Rotation = degree;

            //currentDegree is updated with the value of -degree so that the next animation will start from the new position.
            currentDegree = -degree;
        }


        private string CalculateDirection(float degree)
        {
            if (degree > 322 && degree < 54)
                return "NE";
            if (degree > 235 && degree < 322)
                return "NW";
            if (degree > 148 && degree < 235)
                return "SW";
            if (degree > 54 && degree < 148)
                return "SE";
            if (degree == 322)
                return "N";
            if (degree == 235)
                return "W";
            if (degree == 54)
                return "E";
            if (degree == 148)
                return "S";

            return "NE"; //Default :)

        }
    }
}


