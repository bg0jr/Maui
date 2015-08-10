﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Maui.Data.Recognition.Core;
using System.Windows.Forms;
using System.Threading;
using Blade;
using System.IO;
using System.Web;
using Maui.Data.Recognition.Html;
using Maui.Data.Recognition.Spec;

namespace Maui.Data.Recognition.Core
{
    public class Navigator : INavigator
    {
        public Uri Navigate( Navigation navigation )
        {
            var uri = TryNavigateWithWildcards( navigation );
            if ( uri != null )
            {
                return uri;
            }

            return NavigateToFinalSite( navigation.Uris );
        }

        private Uri TryNavigateWithWildcards( Navigation navi )
        {
            if ( navi.Uris.Count != 1 )
            {
                // we can only handle single urls
                return null;
            }

            var url = navi.Uris[ 0 ];
            Uri uri = new Uri( url.UrlString );
            if ( !uri.IsFile && !uri.IsUnc )
            {
                // we cannot handle e.g. http now
                return null;
            }

            // currently we only handle "/xyz/*/file.txt"
            int pos = url.UrlString.IndexOf( "/*/" );
            if ( pos <= 0 )
            {
                // no pattern found
                return null;
            }

            string root = url.UrlString.Substring( 0, pos );
            string file = url.UrlString.Substring( pos + 3 );
            string[] dirs = Directory.GetDirectories( root, "*" );

            // now try everything with "or" 
            // first path which returns s.th. wins
            foreach ( string dir in dirs )
            {
                string tmpUri = Path.Combine( dir, file );
                if ( !File.Exists( tmpUri ) )
                {
                    continue;
                }

                return new Uri( tmpUri );
            }

            // so in this case we got a pattern navigation url but we were not able
            // to navigate to that url --> throw an exception
            throw new Exception( "Failed to navigate to the document" );
        }

        /// <summary>
        /// Navigates to the final site specified by the user steps and
        /// returns the Uri of the last site.
        /// Regular expressions in response URLs (embedded in {}) are matched. The
        /// resulting parameter is set to the next request URL using 
        /// string.Format() at placeholder {0}.
        /// </summary>
        private Uri NavigateToFinalSite( IEnumerable<NavigatorUrl> navigationSteps )
        {
            string param = null;

            Uri currentUrl = null;
            foreach ( NavigatorUrl navUrl in navigationSteps )
            {
                this.Require( x => navUrl.UrlType != UriType.None );

                if ( navUrl.UrlType == UriType.Request )
                {
                    string url = navUrl.UrlString;
                    if ( param != null )
                    {
                        url = string.Format( url, param );
                    }
                    else if ( HasPlaceHolder( url ) )
                    {
                        var ex = new ApplicationException( "Did not find a parameter for placeholder" );
                        ex.Data[ "Url" ] = url;

                        throw ex;
                    }

                    currentUrl = new Uri( url );

                    if ( navUrl != navigationSteps.Last() )
                    {
                        // dont sent the request - we reached the end and we are not interested in the response url
                        currentUrl = SendRequest( currentUrl );
                    }
                }
                else if ( navUrl.UrlType == UriType.Response )
                {
                    // get param from response url if any
                    param = PatternMatching.MatchEmbeddedRegex( navUrl.UrlString, currentUrl.ToString() );
                }
                else if ( navUrl.UrlType == UriType.SubmitFormular )
                {
                    currentUrl = SubmitFormular( currentUrl, navUrl.Formular );
                }
                else
                {
                    throw new NotSupportedException( "UrlType: " + navUrl.UrlType );
                }
            }

            return currentUrl;
        }

        private bool HasPlaceHolder( string url )
        {
            int begin = url.IndexOf( '{' );
            int end = url.IndexOf( '}' );

            if ( begin < 0 || end < 0 )
            {
                return false;
            }

            return Math.Abs( end - begin ) < 3;
        }

        private Uri SendRequest( Uri input )
        {
            WebResponse response = null;
            try
            {
                var request = WebRequest.Create( input );
                response = request.GetResponse();

                return response.ResponseUri;
            }
            finally
            {
                if ( response != null )
                {
                    response.Close();
                }
            }
        }

        /// <summary>
        /// 1. Get the document
        /// 2. find the formular specified by formular.Name (currently only Html supported)
        /// 3. fill the formular from the document with the additional/overwrite parameters
        /// 3. submit the formular
        /// </summary>
        private Uri SubmitFormular( Uri url, Formular formular )
        {
            using ( var loader = new WinFormHtmlDocumentLoader() )
            {
                var document = loader.LoadHtmlDocument( url );
                var htmlForm = new Html.WinForms.HtmlDocumentAdapter( document ).GetFormByName( formular.Name );
                if ( htmlForm == null )
                {
                    return null;
                }

                var formSubmitUrl = htmlForm.CreateSubmitUrl( formular );

                return SendRequest( formSubmitUrl );
            }
        }
    }
}
