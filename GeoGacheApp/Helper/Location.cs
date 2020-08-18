using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Geocache.Helper
{
    public class Location
    {
        double lat;
        double lon;
        string address;

        public double Lat { get => lat; set => lat = value; }
        public double Lon { get => lon; set => lon = value; }
        public string Address { get => address; set => address = value; }

        // this is the geocode api so you need to add it to the documentation
        public Location(string address)
        {
            Address = address;
            string latitude;
            string longtitude;
            string url = "https://maps.googleapis.com/maps/api/geocode/json?address="+address+"&key=AIzaSyAPcLmG95S4gidIK7ixsqzIqf0oIcNDFFs";
            WebRequest request = WebRequest.Create(url);

            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {

                    using (var jsonTextReader = new JsonTextReader(reader))
                    {
                        var serialize = new JsonSerializer();
                        dynamic data = (JObject)serialize.Deserialize(jsonTextReader);
                        // the request returns OK when it found the address and NO_RESULTS when it didn't
                        if (data.status == "OK")
                        {
                            latitude = data.results[0]["geometry"]["location"]["lat"];
                            longtitude = data.results[0]["geometry"]["location"]["lng"];
                            FixJobjectString(ref latitude);
                            FixJobjectString(ref longtitude);
                        }
                        else
                        {
                            //set it to zeros
                            latitude = "0";
                            longtitude = "0";
                            MessageBox.Show(String.Format("Couldn't find location with this address:{0}", address));
                        }
                    }
                }
            }

           
           
            Lat = double.Parse(latitude);
            Lon = double.Parse(longtitude);
        }
        public Location(double lat,double lon)
        {
            Lat = lat;
            Lon = lon;
            //make address from lat and lon
        }

        void FixJobjectString(ref string inputString)
        {
            if (!Char.IsNumber(inputString[0])) //check if it starts with minus
            {
                foreach (var character in inputString.Substring(1, 3))
                    if (!Char.IsNumber(character))
                        inputString = inputString.Replace(character, ',');//json returns dots so we replace with commas
            }
            else
            {
                foreach (var character in inputString.Substring(0, 4))
                    if (!Char.IsNumber(character))
                        inputString =inputString.Replace(character, ',');
            }
        }

        public override string ToString()
        {
            if (Lat == 0)
                return "INVALID_ADDRESS";
            return Lat.ToString().Substring(0, 6) + " :" + Lon.ToString().Substring(0, 6); 
        }
    }
}
