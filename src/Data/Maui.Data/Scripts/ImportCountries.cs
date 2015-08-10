using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using Blade.Collections;
using Maui.Entities;
using Maui;
using Maui.Shell;
using Maui.Shell.Forms;
using Blade.Shell.Forms;
using Blade.Shell;

namespace Maui.Data.Scripts
{
    public class ImportCountries : ScriptBase
    {
        [Argument( Short = "-list", Long = "-list-fw-countries", Description = "List countries known by .NET framework" )]
        public bool ListFrameworkCountries
        {
            get;
            set;
        }

        protected override void Run()
        {
            if ( ListFrameworkCountries )
            {
                ListDotNetCountries();
            }
            else
            {
                ImportCountriesWithAliases();
            }
        }

        private void ImportCountriesWithAliases()
        {
            using ( var trans = new TransactionScope() )
            {
                using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
                {
                    foreach ( var record in CountryAliasRecords )
                    {
                        var lcid = record.Item1;
                        var alias = record.Item2;

                        var country = tom.Countries.FirstOrDefault( c => c.LCID == lcid );
                        if ( country == null )
                        {
                            country = new Country( lcid );

                            tom.Countries.AddObject( country );
                        }

                        if ( !tom.CountryAliases.Any( a => a.Name == alias ) )
                        {
                            var countryAlias = new CountryAlias( country, alias );

                            tom.CountryAliases.AddObject( countryAlias );
                        }
                    }

                    tom.SaveChanges();
                }

                trans.Complete();
            }
        }

        private void ListDotNetCountries()
        {
            foreach ( var lcid in GetLCIDOfAllCountries() )
            {
                var regionInfo = new RegionInfo( (int)lcid );

                var names = new List<string>();

                Action<string> AddIfNotExists = n => { if ( !names.Contains( n ) ) names.Add( n ); };

                AddIfNotExists( regionInfo.EnglishName );
                AddIfNotExists( regionInfo.DisplayName );

                Console.WriteLine( "{0,10} : {1}", lcid, names.ToHuman() );
            }
        }

        private IEnumerable<int> GetLCIDOfAllCountries()
        {
            var culturesGroupedByGeoId = CultureInfo.GetCultures( CultureTypes.SpecificCultures )
                .GroupBy( culture => new RegionInfo( culture.LCID ).GeoId );

            foreach ( var geoIdCulturePair in culturesGroupedByGeoId )
            {
                // we want to collect countries here, not cultures so lets just
                // take the first LCID for each GeoId
                yield return geoIdCulturePair.First().LCID;
            }
        }

        private static List<System.Tuple<int, string>> CountryAliasRecords = new List<System.Tuple<int, string>>()
        {
            new System.Tuple<int,string>(1065,"Iran"),
            new System.Tuple<int,string>(1063,"Litauen"),
            new System.Tuple<int,string>(1063,"Lithuania"),
            new System.Tuple<int,string>(1062,"Latvia"),
            new System.Tuple<int,string>(1062,"Lettland"),
            new System.Tuple<int,string>(1068,"Azerbaijan"),
            new System.Tuple<int,string>(1068,"Aserbaidschan"),
            new System.Tuple<int,string>(1067,"Armenien"),
            new System.Tuple<int,string>(1067,"Armenia"),
            new System.Tuple<int,string>(1066,"Vietnam"),
            new System.Tuple<int,string>(1061,"Estland"),
            new System.Tuple<int,string>(1061,"Estonia"),
            new System.Tuple<int,string>(1057,"Indonesia"),
            new System.Tuple<int,string>(1057,"Indonesien"),
            new System.Tuple<int,string>(1056,"Islamische Republik Pakistan"),
            new System.Tuple<int,string>(1056,"Islamic Republic of Pakistan"),
            new System.Tuple<int,string>(1055,"Turkey"),
            new System.Tuple<int,string>(1055,"Türkei"),
            new System.Tuple<int,string>(1060,"Slovenia"),
            new System.Tuple<int,string>(1060,"Slowenien"),
            new System.Tuple<int,string>(1059,"Belarus"),
            new System.Tuple<int,string>(1058,"Ukraine"),
            new System.Tuple<int,string>(1071,"Macedonia (FYROM)"),
            new System.Tuple<int,string>(1071,"Ehemalige jugoslawische Republik Mazedonien"),
            new System.Tuple<int,string>(1104,"Mongolia"),
            new System.Tuple<int,string>(1104,"Mongolei"),
            new System.Tuple<int,string>(1091,"Usbekistan"),
            new System.Tuple<int,string>(1091,"Uzbekistan"),
            new System.Tuple<int,string>(1089,"Kenya"),
            new System.Tuple<int,string>(1089,"Kenia"),
            new System.Tuple<int,string>(2049,"Iraq"),
            new System.Tuple<int,string>(2049,"Irak"),
            new System.Tuple<int,string>(1125,"Malediven"),
            new System.Tuple<int,string>(1125,"Maldives"),
            new System.Tuple<int,string>(1114,"Syrien"),
            new System.Tuple<int,string>(1114,"Syria"),
            new System.Tuple<int,string>(1088,"Kyrgyzstan"),
            new System.Tuple<int,string>(1088,"Kirgisistan"),
            new System.Tuple<int,string>(1080,"Färöer-Inseln"),
            new System.Tuple<int,string>(1080,"Faroe Islands"),
            new System.Tuple<int,string>(1079,"Georgia"),
            new System.Tuple<int,string>(1079,"Georgien"),
            new System.Tuple<int,string>(1078,"South Africa"),
            new System.Tuple<int,string>(1078,"Südafrika"),
            new System.Tuple<int,string>(1087,"Kasachstan"),
            new System.Tuple<int,string>(1087,"Kazakhstan"),
            new System.Tuple<int,string>(1086,"Malaysia"),
            new System.Tuple<int,string>(1081,"Indien"),
            new System.Tuple<int,string>(1081,"India"),
            new System.Tuple<int,string>(1054,"Thailand"),
            new System.Tuple<int,string>(1035,"Finnland"),
            new System.Tuple<int,string>(1035,"Finland"),
            new System.Tuple<int,string>(1033,"United States"),
            new System.Tuple<int,string>(1033,"Vereinigte Staaten von Amerika"),
            new System.Tuple<int,string>(1032,"Greece"),
            new System.Tuple<int,string>(1032,"Griechenland"),
            new System.Tuple<int,string>(1038,"Ungarn"),
            new System.Tuple<int,string>(1038,"Hungary"),
            new System.Tuple<int,string>(1037,"Israel"),
            new System.Tuple<int,string>(1036,"Frankreich"),
            new System.Tuple<int,string>(1036,"France"),
            new System.Tuple<int,string>(1031,"Deutschland"),
            new System.Tuple<int,string>(1031,"Germany"),
            new System.Tuple<int,string>(1027,"Spain"),
            new System.Tuple<int,string>(1027,"Spanien"),
            new System.Tuple<int,string>(1026,"Bulgaria"),
            new System.Tuple<int,string>(1026,"Bulgarien"),
            new System.Tuple<int,string>(1025,"Saudi-Arabien"),
            new System.Tuple<int,string>(1025,"Saudi Arabia"),
            new System.Tuple<int,string>(1030,"Denmark"),
            new System.Tuple<int,string>(1030,"Dänemark"),
            new System.Tuple<int,string>(1029,"Czech Republic"),
            new System.Tuple<int,string>(1029,"Tschechische Republik"),
            new System.Tuple<int,string>(1028,"Taiwan"),
            new System.Tuple<int,string>(1039,"Island"),
            new System.Tuple<int,string>(1039,"Iceland"),
            new System.Tuple<int,string>(1050,"Croatia"),
            new System.Tuple<int,string>(1050,"Kroatien"),
            new System.Tuple<int,string>(1049,"Russia"),
            new System.Tuple<int,string>(1049,"Russische Föderation"),
            new System.Tuple<int,string>(1048,"Rumänien"),
            new System.Tuple<int,string>(1048,"Romania"),
            new System.Tuple<int,string>(1053,"Schweden"),
            new System.Tuple<int,string>(1053,"Sweden"),
            new System.Tuple<int,string>(1052,"Albania"),
            new System.Tuple<int,string>(1052,"Albanien"),
            new System.Tuple<int,string>(1051,"Slowakische Republik"),
            new System.Tuple<int,string>(1051,"Slovakia"),
            new System.Tuple<int,string>(1046,"Brazil"),
            new System.Tuple<int,string>(1046,"Brasilien"),
            new System.Tuple<int,string>(1042,"Korea"),
            new System.Tuple<int,string>(1041,"Japan"),
            new System.Tuple<int,string>(1040,"Italien"),
            new System.Tuple<int,string>(1040,"Italy"),
            new System.Tuple<int,string>(1045,"Poland"),
            new System.Tuple<int,string>(1045,"Polen"),
            new System.Tuple<int,string>(1044,"Norwegen"),
            new System.Tuple<int,string>(1044,"Norway"),
            new System.Tuple<int,string>(1043,"Netherlands"),
            new System.Tuple<int,string>(1043,"Niederlande"),
            new System.Tuple<int,string>(2052,"People's Republic of China"),
            new System.Tuple<int,string>(2052,"Volksrepublik China"),
            new System.Tuple<int,string>(12289,"Libanon"),
            new System.Tuple<int,string>(12289,"Lebanon"),
            new System.Tuple<int,string>(11274,"Argentina"),
            new System.Tuple<int,string>(11274,"Argentinien"),
            new System.Tuple<int,string>(11273,"Trinidad und Tobago"),
            new System.Tuple<int,string>(11273,"Trinidad and Tobago"),
            new System.Tuple<int,string>(13313,"Kuwait"),
            new System.Tuple<int,string>(12298,"Ecuador"),
            new System.Tuple<int,string>(12297,"Zimbabwe"),
            new System.Tuple<int,string>(11265,"Jordan"),
            new System.Tuple<int,string>(11265,"Jordanien"),
            new System.Tuple<int,string>(9217,"Yemen"),
            new System.Tuple<int,string>(9217,"Jemen"),
            new System.Tuple<int,string>(8202,"Venezuela"),
            new System.Tuple<int,string>(8202,"Bolivarian Republic of Venezuela"),
            new System.Tuple<int,string>(8201,"Jamaica"),
            new System.Tuple<int,string>(8201,"Jamaika"),
            new System.Tuple<int,string>(10250,"Peru"),
            new System.Tuple<int,string>(10249,"Belize"),
            new System.Tuple<int,string>(9226,"Kolumbien"),
            new System.Tuple<int,string>(9226,"Colombia"),
            new System.Tuple<int,string>(13321,"Republic of the Philippines"),
            new System.Tuple<int,string>(13321,"Republik Philippinen"),
            new System.Tuple<int,string>(19466,"Nicaragua"),
            new System.Tuple<int,string>(18442,"Honduras"),
            new System.Tuple<int,string>(17418,"El Salvador"),
            new System.Tuple<int,string>(1082,"Malta"),
            new System.Tuple<int,string>(7194,"Bosnia and Herzegovina"),
            new System.Tuple<int,string>(7194,"Bosnien und Herzegowina"),
            new System.Tuple<int,string>(20490,"Puerto Rico"),
            new System.Tuple<int,string>(16394,"Bolivia"),
            new System.Tuple<int,string>(16394,"Bolivien"),
            new System.Tuple<int,string>(14346,"Uruguay"),
            new System.Tuple<int,string>(14337,"U.A.E."),
            new System.Tuple<int,string>(14337,"Vereinigte Arabische Emirate"),
            new System.Tuple<int,string>(13322,"Chile"),
            new System.Tuple<int,string>(16385,"Qatar"),
            new System.Tuple<int,string>(16385,"Katar"),
            new System.Tuple<int,string>(15370,"Paraguay"),
            new System.Tuple<int,string>(15361,"Bahrain"),
            new System.Tuple<int,string>(8193,"Oman"),
            new System.Tuple<int,string>(3079,"Österreich"),
            new System.Tuple<int,string>(3079,"Austria"),
            new System.Tuple<int,string>(3076,"Hongkong S.A.R."),
            new System.Tuple<int,string>(3076,"Hong Kong S.A.R."),
            new System.Tuple<int,string>(3073,"Egypt"),
            new System.Tuple<int,string>(3073,"Ägypten"),
            new System.Tuple<int,string>(4097,"Libya"),
            new System.Tuple<int,string>(4097,"Libyen"),
            new System.Tuple<int,string>(3084,"Kanada"),
            new System.Tuple<int,string>(3084,"Canada"),
            new System.Tuple<int,string>(3081,"Australia"),
            new System.Tuple<int,string>(3081,"Australien"),
            new System.Tuple<int,string>(2110,"Brunei Darussalam"),
            new System.Tuple<int,string>(2058,"Mexico"),
            new System.Tuple<int,string>(2058,"Mexiko"),
            new System.Tuple<int,string>(2057,"Großbritannien"),
            new System.Tuple<int,string>(2057,"United Kingdom"),
            new System.Tuple<int,string>(2055,"Switzerland"),
            new System.Tuple<int,string>(2055,"Schweiz"),
            new System.Tuple<int,string>(2074,"Serbia"),
            new System.Tuple<int,string>(2074,"Serbien und Montenegro"),
            new System.Tuple<int,string>(2074,"Serbia and Montenegro (Former)"),
            new System.Tuple<int,string>(2070,"Protugal"),
            new System.Tuple<int,string>(2070,"Portugal"),
            new System.Tuple<int,string>(2060,"Belgium"),
            new System.Tuple<int,string>(2060,"Belgien"),
            new System.Tuple<int,string>(4100,"Singapore"),
            new System.Tuple<int,string>(4100,"Singapur"),
            new System.Tuple<int,string>(6154,"Panama"),
            new System.Tuple<int,string>(6153,"Ireland"),
            new System.Tuple<int,string>(6153,"Irland"),
            new System.Tuple<int,string>(6145,"Morocco"),
            new System.Tuple<int,string>(6145,"Marokko"),
            new System.Tuple<int,string>(7178,"Dominikanische Republik"),
            new System.Tuple<int,string>(7178,"Dominican Republic"),
            new System.Tuple<int,string>(7169,"Tunisia"),
            new System.Tuple<int,string>(7169,"Tunesien"),
            new System.Tuple<int,string>(6156,"Fürstentum Monaco"),
            new System.Tuple<int,string>(6156,"Principality of Monaco"),
            new System.Tuple<int,string>(5130,"Costa Rica"),
            new System.Tuple<int,string>(5121,"Algerien"),
            new System.Tuple<int,string>(5121,"Algeria"),
            new System.Tuple<int,string>(4106,"Guatemala"),
            new System.Tuple<int,string>(4103,"Luxembourg"),
            new System.Tuple<int,string>(4103,"Luxemburg"),
            new System.Tuple<int,string>(5129,"Neuseeland"),
            new System.Tuple<int,string>(5129,"New Zealand"),
            new System.Tuple<int,string>(5127,"Liechtenstein"),
            new System.Tuple<int,string>(5124,"Macao S.A.R.")
        };
    }
}
