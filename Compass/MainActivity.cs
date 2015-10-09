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
        // declare global variable
        private SensorManager sensorManager;

        //set current degree rotation 
        private float currentDegree = 0f;

        // initialize refrence of UI element 
        ImageView compassBgImage;
        ImageView dialImage;
        TextView headingDirection;
        TextView headingDegree;

        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //Get instance of Sensor Manger 
            sensorManager = (SensorManager)GetSystemService(SensorService);

            // WatchViewStub calculate wearable type and render view from appropriate UI xml defination
            var v = FindViewById<WatchViewStub>(Resource.Id.watch_view_stub);

            //Infalte watchviewstub
            v.LayoutInflated += delegate
            {

                // Get elemnt from the layout resource,
                compassBgImage = FindViewById<ImageView>(Resource.Id.imageViewCompass);
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
            //calculate degree
            float degree = (float)Math.Round(e.Values[0]);

            //calculate direction coordinal 
            headingDirection.Text = CalculateDirection(degree);

            // Set text for degree
            headingDegree.Text = Convert.ToString(degree) + "\u00B0";
            var rotation = new RotateAnimation(currentDegree, -degree, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f);
            rotation.Duration = 200;
            rotation.FillAfter = true;
            compassBgImage.Animation = rotation;
            currentDegree = -degree;
        }
        private string CalculateDirection(float degree)
        {
            if (degree > 304 && degree < 35)
                return "NE";
            if (degree > 35 && degree < 134)
                return "NW";
            if (degree > 134 && degree < 213)
                return "SW";
            if (degree > 213 && degree < 304)
                return "SE";
            if (degree > 35 && degree < 134)
                return "NE";
            if (degree == 35)
                return "N";
            if (degree == 134)
                return "W";
            if (degree == 304)
                return "E";
            if (degree == 213)
                return "S";

            return "NE"; //Default :)

        }
    }
}


