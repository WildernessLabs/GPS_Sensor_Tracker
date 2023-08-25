﻿using Meadow;
using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Logging;
using Meadow.Peripherals.Sensors.Location.Gnss;
using Meadow.Units;
using System;

namespace GnssTracker_Demo.Controllers
{
    public class DisplayController
    {
        protected int counter = 0;
        protected Logger Log { get => Resolver.Log; }
        protected DisplayScreen DisplayScreen { get; set; }

        protected Font12x20 LargeFont { get; set; }
        protected Font4x8 SmallFont { get; set; }

        protected DisplayLabel TemperatureLabel { get; set; }
        protected DisplayLabel HumidityLabel { get; set; }
        protected DisplayLabel PressureLabel { get; set; }
        protected DisplayLabel LatitudeLabel { get; set; }
        protected DisplayLabel LongitudeLabel { get; set; }
        protected DisplayLabel CounterLabel { get; set; }

        public DisplayController(IGraphicsDisplay display)
        {
            LargeFont = new Font12x20();
            SmallFont = new Font4x8();

            DisplayScreen = new DisplayScreen(display, RotationType._270Degrees);
        }

        public void ShowSplashScreen()
        {
            var image = Image.LoadFromResource("GnssTracker_Demo.gnss_tracker.bmp");

            var displayImage = new DisplayImage(0, 0, 250, 122, image)
            {
                BackColor = Color.FromHex("#23ABE3"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            DisplayScreen.Controls.Add(displayImage);
        }

        public void LoadDataScreen()
        {
            try
            {
                DisplayScreen.Controls.Clear();

                var box = new DisplayBox(0, 0, DisplayScreen.Width, DisplayScreen.Height)
                {
                    ForeColor = Color.White,
                    Filled = true
                };

                var frame = new DisplayBox(5, 5, 240, 112)
                {
                    ForeColor = Color.Black,
                    Filled = false
                };

                TemperatureLabel = new DisplayLabel(10, 10, DisplayScreen.Width - 20, LargeFont.Height)
                {
                    Text = $"Temp:     0.00°C",
                    TextColor = Color.Black,
                    BackColor = Color.White,
                    Font = LargeFont,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                HumidityLabel = new DisplayLabel(10, 30, DisplayScreen.Width - 20, LargeFont.Height)
                {
                    Text = $"Humidity: 0.00%",
                    TextColor = Color.Black,
                    BackColor = Color.White,
                    Font = LargeFont,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                PressureLabel = new DisplayLabel(10, 50, DisplayScreen.Width - 20, LargeFont.Height)
                {
                    Text = $"Pressure: 0.00atm",
                    TextColor = Color.Black,
                    BackColor = Color.White,
                    Font = LargeFont,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                LatitudeLabel = new DisplayLabel(10, 72, DisplayScreen.Width - 20, LargeFont.Height)
                {
                    Text = $"Lat: 0°0'0.0\"",
                    TextColor = Color.White,
                    BackColor = Color.Red,
                    Font = LargeFont,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                LongitudeLabel = new DisplayLabel(10, 92, DisplayScreen.Width - 20, LargeFont.Height)
                {
                    Text = $"Lon: 0°0'0.0\"",
                    TextColor = Color.White,
                    BackColor = Color.Red,
                    Font = LargeFont,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                counter++;
                CounterLabel = new DisplayLabel(222, 113, 20, 8)
                {
                    Text = $"{counter.ToString("D4")}",
                    TextColor = Color.Black,
                    BackColor = Color.White,
                    Font = SmallFont,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                DisplayScreen.Controls.Add(box, frame, TemperatureLabel, HumidityLabel, PressureLabel, LatitudeLabel, LongitudeLabel, CounterLabel);
            }
            catch (Exception e)
            {
                Log?.Error($"err while rendering: {e.Message}");
            }
        }

        public void UpdateDisplay((Temperature? Temperature, RelativeHumidity? Humidity, Pressure? Pressure, Resistance? GasResistance) conditions, GnssPositionInfo locationInfo)
        {
            TemperatureLabel.Text = $"Temp:     {conditions.Temperature?.Celsius:n2}°C";
            HumidityLabel.Text = $"Humidity: {conditions.Humidity?.Percent:n2}%";
            PressureLabel.Text = $"Pressure: {conditions.Pressure?.StandardAtmosphere:n2}atm";

            string lat = locationInfo == null
                ? $"Lat: 0°0'0.0\""
                : $"Lat: " +
                $"{locationInfo?.Position?.Latitude?.Degrees}°" +
                $"{locationInfo?.Position?.Latitude?.Minutes:n2}'" +
                $"{locationInfo?.Position?.Latitude?.seconds}\"";
            LatitudeLabel.Text = lat;

            string lon = locationInfo == null
                ? $"Lon: 0°0'0.0\""
                : $"Lon: " +
                $"{locationInfo?.Position?.Longitude?.Degrees}°" +
                $"{locationInfo?.Position?.Longitude?.Minutes:n2}'" +
                $"{locationInfo?.Position?.Longitude?.seconds}\"";
            LongitudeLabel.Text = lon;

            counter++;
            CounterLabel.Text = $"{counter.ToString("D4")}";
        }
    }
}