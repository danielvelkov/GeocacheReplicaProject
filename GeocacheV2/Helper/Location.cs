using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GeocacheV2.Helper
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
                        latitude = data.results[0]["geometry"]["location"]["lat"];
                        longtitude = data.results[0]["geometry"]["location"]["lng"];
                    }
                }
            }

            // jeesus christ. i did that cuz json uses weird culture and just .replace doesnt work
            //StringBuilder sb = new StringBuilder(latitude);
            //sb[2] = ',';
            //latitude = sb.ToString();

            //StringBuilder sb2 = new StringBuilder(longtitude);
            //sb2[2] = ',';
            //longtitude = sb2.ToString();

            FixJobjectString(ref latitude);
            FixJobjectString(ref longtitude);
           
            Lat = double.Parse(latitude);
            Lon = double.Parse(longtitude);
        }
        public Location(double lat,double lon)
        {
            //make address from lat and lon
        }

        void FixJobjectString(ref string inputString)
        {
            StringBuilder sb = new StringBuilder(inputString);
            // TODO make it so it finds the dot not hard code replace it
            sb[2] = ',';
            inputString = sb.ToString();
        }

        public override string ToString()
        {
            return Lat.ToString() + " :" + Lon.ToString(); 
        }
    }
}
