using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LINQ
{
    public partial class Form1 : Form
    {
        List<Country> countries = new List<Country>();
        List<Ramen> ramens = new List<Ramen>();
        public Form1()
        {
            InitializeComponent();
            LoadData("ramen.csv");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void LoadData(string filename)
        {
            using (StreamReader sr = new StreamReader(filename, Encoding.Default))
            {
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');

                    var countryName = line[2];
                    Addcountry(countryName);

                    var ramen = new Ramen()
                    {
                        ID = ramens.Count + 1,
                        Brand = line[0],
                        Name = line[1],
                        Country = Country,
                        CountryFK = Country.ID,
                        Stars = Convert.ToDouble(line[3])
                    };
                    ramens.Add(ramen);

                    /* var currentCountry = (from c in countries where c.Name.Equals(countryName) select c).FirstOrDefault();

                     var ramen = new Ramen()
                     {
                         ID = ramens.Count + 1,
                         Brand = line[0],
                         Name = line[1],
                         Country = Country,
                         CountryFK = Country.ID,
                         Stars = Convert.ToDouble(line[3])
                     };
                     ramens.Add(ramen);

                     if (currentCountry == null)
                     {
                         currentCountry = new Country()
                         { 
                             ID = countries.Count + 1,
                             Name = countryName
                         };
                         countries.Add(currentCountry);
                     }*/

                }
            }
        }

        public Country Addcountry(string countryName)
        {
            var currentCountry = (from c in countries where c.Name.Contains(countryName) select c).FirstOrDefault();

            if (currentCountry == null)
            {
                currentCountry = new Country()
                {
                    ID= countries.Count + 1,
                    Name= countryName
                };
                countries.Add(currentCountry);
            }
            
            return currentCountry;
        }
    }
}
