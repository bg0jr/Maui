﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Data.Recognition;
using Maui.Data.Recognition.Providers;
using Maui.Data.Recognition.Spec;

namespace Maui.Data.Recognition
{
    public class DatumProviderFactory : IDatumProviderFactory
    {
        private IDocumentBrowser myWebScrapSC;

        public DatumProviderFactory( IDocumentBrowser webScrapSC, IFetchPolicy fetchPolicy )
            : this( webScrapSC, fetchPolicy, null )
        {
        }

        public DatumProviderFactory( IDocumentBrowser webScrapSC, IFetchPolicy fetchPolicy, IResultPolicy resultPolicy )
        {
            myWebScrapSC = webScrapSC;
            FetchPolicy = fetchPolicy;
            ResultPolicy = resultPolicy;

            LocatorRepository = new DatumLocatorRepository();
        }

        public IFetchPolicy FetchPolicy
        {
            get;
            private set;
        }

        public IResultPolicy ResultPolicy
        {
            get;
            private set;
        }

        public IDatumProvider Create( DatumLocator datumLocator )
        {
            return new GenericDatumProvider( myWebScrapSC, datumLocator, FetchPolicy, ResultPolicy );
        }

        public IDatumProvider Create( string datum )
        {
            var locator = LocatorRepository.Get( datum );
            if ( locator == null )
            {
                return null;
            }

            return Create( locator );
        }

        public DatumLocatorRepository LocatorRepository
        {
            get;
            private set;
        }
    }
}
