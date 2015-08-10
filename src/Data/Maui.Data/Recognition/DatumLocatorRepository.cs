﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blade.Binding;
using Maui.Data.Recognition.Spec;
using Blade.Collections;

namespace Maui.Data.Recognition
{
    public class DatumLocatorRepository
    {
        public IList<DatumLocator> myLocators;

        public DatumLocatorRepository()
        {
            myLocators = new List<DatumLocator>();
        }

        public IEnumerable<DatumLocator> Locators
        {
            get { return myLocators; }
        }

        /// <summary>
        /// If locator with same name already exists it will be overwritten.
        /// </summary>
        public void Add( DatumLocator locator )
        {
            var existingLocator = myLocators.FirstOrDefault( l => l.Datum == locator.Datum );
            if ( existingLocator != null )
            {
                myLocators.Remove( existingLocator );
            }

            myLocators.Add( locator );
        }

        public DatumLocator Get( string datum )
        {
            return myLocators.FirstOrDefault( l => l.Datum == datum );
        }

        public void Load( string directory )
        {
            var reader = new ValidatingXamlReader();

            var datumLocatorsFromXaml = Directory.GetFiles( directory, "*.xaml", SearchOption.AllDirectories )
                .Select( file => reader.Read<Document>( file ) )
                .SelectMany( doc => doc.DatumLocators )
                .ToList();

            datumLocatorsFromXaml.Foreach( Add );
        }
    }
}
