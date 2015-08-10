﻿using System.Collections.Generic;
using System.Linq;
using Maui.Data.Recognition;
using Maui.Data.Recognition.DatumLocators;
using Maui;
using Maui.Data.Recognition.Spec;

namespace Maui.Tools.Studio.WebSpy.DatumLocatorValidation
{

    public class ValidationController
    {
        private IEnumerable<ParameterizedDatumLocator> myDatumLocators;
        private ServiceProvider myServiceProvider;

        public ValidationController()
        {
            myDatumLocators = BuildDatumLocators();
            myServiceProvider = new ServiceProvider();
        }

        private IEnumerable<ParameterizedDatumLocator> BuildDatumLocators()
        {
            return BuildStandingDatumLocators()
                .Concat( BuildValueDatumLocators() )
                .ToList();
        }

        private IEnumerable<ParameterizedDatumLocator> BuildStandingDatumLocators()
        {
            yield return CreateLocator( DatumLocatorDefinitions.Standing.Wpkn, 0, "DE0008404005" );

            yield return CreateLocator( DatumLocatorDefinitions.Standing.StockSymbol, 0, "US5017231003" );
            yield return CreateLocator( DatumLocatorDefinitions.Standing.StockSymbol, 1, "DE0008404005" );
            yield return CreateLocator( DatumLocatorDefinitions.Standing.StockSymbol, 2, "DE0008404005" );

            yield return CreateLocator( DatumLocatorDefinitions.Standing.CompanyName, 0, "DE0008404005" );

            yield return CreateLocator( DatumLocatorDefinitions.Standing.Sector, 0, "DE0008404005" );
        }

        private IEnumerable<ParameterizedDatumLocator> BuildValueDatumLocators()
        {
            yield return CreateLocator( DatumLocatorDefinitions.CurrentPrice, 0, "DE0008404005" );
            yield return CreateLocator( DatumLocatorDefinitions.CurrentPrice, 1, "DE0008404005" );
            yield return CreateLocator( DatumLocatorDefinitions.CurrentPrice, 2, "DE0008404005" );
        }

        private ParameterizedDatumLocator CreateLocator( DatumLocator locator, int siteIndex, string isin )
        {
            var validator = new ParameterizedDatumLocator( locator.Datum, locator.Sites[ siteIndex ] );
            validator.Parameters[ "stock.isin" ] = isin;
            return validator;
        }

        public IEnumerable<ParameterizedDatumLocator> GetDatumLocators()
        {
            return myDatumLocators;
        }

        public LocatorValidationResult Validate( ParameterizedDatumLocator locator )
        {
            var provider = new Validator( myServiceProvider.Browser() );
            return provider.Fetch( locator );
        }
    }
}
