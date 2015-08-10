﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

using Blade;

namespace Maui.Data.Recognition.Html
{
    /// <summary>
    /// Adds helpful API to an <see cref="HtmlElement"/> which represents the root of a html table.
    /// </summary>
    public class HtmlTable
    {
        private IHtmlElement myBody = null;

        /// <summary>
        /// Creates an instance based on the given root element.
        /// </summary>
        /// <param name="root">html element pointing to the table</param>
        public HtmlTable( IHtmlElement root )
        {
            this.Require( x => root != null && root.TagName == "TABLE" );

            TableElement = root;
        }

        /// <summary>
        /// The HtmlElement representing the table.
        /// </summary>
        public IHtmlElement TableElement { get; private set; }

        /// <summary>
        /// Returns the HTML element representing the HTML table body.
        /// <remarks>
        /// Returns the TBODY element if one exists, <see cref="TableElement"/> otherwise.
        /// </remarks>
        /// </summary>
        public IHtmlElement TableBody
        {
            get
            {
                if ( myBody == null )
                {
                    var body = TableElement.Children.FirstOrDefault( e => e.TagName == "TBODY" );
                    myBody = (body == null ? TableElement : body);
                }

                return myBody;
            }
        }

        /// <summary>
        /// Returns the column index of the given HtmlElement or of its
        /// surrounding TD element.
        /// <seealso cref="GetEmbeddingTD"/>
        /// </summary>
        public static int GetColumnIndex( IHtmlElement e )
        {
            return GetEmbeddingTD( e ).GetChildPos();
        }

        /// <summary>
        /// Returns the TD element embedding the given element.
        /// If the given element itself is a TD, this one is returned.
        /// </summary>
        public static IHtmlElement GetEmbeddingTD( IHtmlElement e )
        {
            e.Require( x => e != null );

            if ( e.TagName == "TD" )
            {
                return e;
            }
            else
            {
                var parent = e.FindParent( p => p.TagName == "TD", p => e.IsTableOrTBody() );
                return (parent == null ? null : parent);
            }
        }

        /// <summary>
        /// Returns the row index of the given HtmlElement or of its
        /// surrounding TR element.
        /// <seealso cref="GetEmbeddingTR"/>
        /// </summary> 
        public static int GetRowIndex( IHtmlElement e )
        {
            return GetEmbeddingTR( e ).GetChildPos();
        }

        /// <summary>
        /// Returns the TR element embedding the given element.
        /// If the given element itself is a TR, this one is returned.
        /// </summary>
        public static IHtmlElement GetEmbeddingTR( IHtmlElement e )
        {
            e.Require( x => e != null );

            if ( e.TagName == "TR" )
            {
                return e;
            }
            else
            {
                var parent = e.FindParent( p => p.TagName == "TR", p => e.IsTableOrTBody() );
                return (parent == null ? null : parent);
            }
        }

        /// <summary>
        /// Returns the TD element at the specified position.
        /// </summary>
        /// <returns>the TD element found, null otherwise</returns>
        public IHtmlElement GetCellAt( int row, int column )
        {
            var r = TableBody.GetChildAt( "TR", row );
            if ( r == null )
            {
                return null;
            }

            return r.GetChildAt( "TD", column );
        }

        /// <summary>
        /// Returns all rows of the HTML table.
        /// Including potential header
        /// </summary>
        public IEnumerable<IHtmlElement> Rows
        {
            get
            {
                foreach ( var row in TableElement.Children )
                {
                    if ( row.TagName == "TR" )
                    {
                        yield return row;
                    }
                    if ( row.TagName == "THEAD" || row.TagName == "TBODY" )
                    {
                        foreach ( var innerRow in row.Children )
                        {
                            if ( innerRow.TagName == "TR" )
                            {
                                yield return innerRow;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the complete row of the given cell.
        /// </summary>
        public static IEnumerable<IHtmlElement> GetRow( IHtmlElement cell )
        {
            cell.Require( x => cell != null );

            var row = GetEmbeddingTR( cell );
            if ( row == null )
            {
                throw new ArgumentException( "Element does not point to cell inside table row" );
            }

            return row.Children;
        }

        /// <summary>
        /// Gets the complete column of the given cell.
        /// <remarks>Attention: Handling "colspan" is not implemented.
        /// A TR without any TD is skipped.</remarks>
        /// </summary>
        public static IEnumerable<IHtmlElement> GetColumn( IHtmlElement cell )
        {
            cell.Require( x => cell != null );

            HtmlTable table = cell.FindEmbeddingTable();
            if ( table == null )
            {
                throw new ArgumentException( "Element does not point to into table" );
            }

            int colIdx = cell.GetChildPos();

            foreach ( object row in table.TableBody.Children )
            {
                IHtmlElement e = ((IHtmlElement)row).GetChildAt( "TD", colIdx );
                if ( e == null )
                {
                    continue;
                }

                yield return e;
            }
        }
    }
}
