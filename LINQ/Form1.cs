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
        List<Brand> brands = new List<Brand>();
        public Form1()
        {
            InitializeComponent();
            LoadData("ramen.csv");
            getcountries();
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

                    Country ActCountry = Addcountry(line[2]);
                    Brand ActBrand = Addbrand(line[0]);

                    var ramen = new Ramen()
                    {
                        ID = ramens.Count + 1,
                        Brand = ActBrand,
                        Name = line[1],
                        Country = ActCountry,
                        CountryFK = ActCountry.ID,
                        Stars = Convert.ToDouble(line[3])
                    };
                    ramens.Add(ramen);
                }
                sr.Close();
            }

            /*public Country Addcountry(string countryName)
            {
                var currentCountry = (from c in countries where c.Name.Equals(countryName) select c).FirstOrDefault();

                if (currentCountry == null)
                {
                    currentCountry = new Country()
                    {
                        ID = countries.Count + 1,
                        Name = countryName
                    };
                    countries.Add(currentCountry);
                }

                return currentCountry;
            }*/
        }

        private Country Addcountry(string countryName)
        {
            var currentCountry = (from c in countries where c.Name.Equals(countryName) select c).FirstOrDefault();

            if (currentCountry == null)
            {
                currentCountry = new Country()
                {
                    ID = countries.Count + 1,
                    Name = countryName
                };
                countries.Add(currentCountry);
            }
            return currentCountry;
        }

        public Brand Addbrand(string brand)
        {
            var e = (from c in brands where c.Name.Equals(brand) select c).FirstOrDefault();

            if (e == null)
            {
                e = new Brand
                {
                    ID = brands.Count,
                    Name = brand
                };
                brands.Add(e);
            }
            return e;
        }

        public void getcountries()
        {
            var countriesList = from c in countries where c.Name.Contains(txtSearch.Text) orderby c.Name select c;
            listCountries.DataSource = countriesList.ToList();
            listCountries.DisplayMember = "Name";
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            getcountries();
        }

        private void listCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            Country c = (Country)listCountries.SelectedItem; 

            if (c==null)
            {
                return;
            }

            var countryRamens = from r in ramens 
                                where r.CountryFK == c.ID
                                select r;

            var groupedRamens = from r in countryRamens
                                group r.Stars by r.Brand.Name into f
                                select new
                                {
                                    brandname = f.Key,
                                    AverageRating = Math.Round(f.Average(),2)
                                };

            var orderedGroups = from f in groupedRamens
                                orderby f.AverageRating descending
                                select f;

            dataGridView1.DataSource = orderedGroups.ToList();
        }
    }
}
