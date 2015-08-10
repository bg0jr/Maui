﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Blade;
using Blade.Collections;
using Maui.Data.Recognition.Html;
using Blade.Data;
using Maui.Data.Recognition.Html.WinForms;

namespace Maui.Tools.Studio.WebSpy
{
    public class MarkupDocument : IDisposable
    {
        private HtmlMarker myMarker = null;
        private HtmlDocumentAdapter myDocument = null;
        // holds the element which has been marked by the user "click"
        // (before extensions has been applied)
        private HtmlElementAdapter mySelectedElement = null;
        private HtmlTable myTable = null;
        private string myAnchor = null;
        private CellDimension myDimension = CellDimension.None;
        private int[] mySkipColumns = null;
        private int[] mySkipRows = null;
        private int myRowHeader = -1;
        private int myColumnHeader = -1;
        private string mySeriesName = null;

        public MarkupDocument()
        {
            myMarker = new HtmlMarker( myDocument );
            Reset();
        }

        public Action<bool> ValidationChanged = null;

        public HtmlElementAdapter SelectedElement
        {
            get { return mySelectedElement; }
            set
            {
                myMarker.UnmarkAll();

                mySelectedElement = value;
                if ( mySelectedElement == null )
                {
                    myTable = null;
                }
                else
                {
                    myTable = mySelectedElement.FindEmbeddingTable();

                    TemplateChanged();
                }
            }
        }

        public HtmlDocument Document
        {
            get { return myDocument != null ? myDocument.Document : null; }
            set
            {
                if ( myDocument != null )
                {
                    myDocument.Document.Click -= HtmlDocument_Click;
                }

                SelectedElement = null;

                myDocument = new HtmlDocumentAdapter( value );
                myDocument.Document.Click += HtmlDocument_Click;
            }
        }

        public string Anchor
        {
            get { return myAnchor; }
            set
            {
                SelectedElement = (HtmlElementAdapter)myDocument.GetElementByPath( HtmlPath.Parse( value ) );
            }
        }

        public void Dispose()
        {
            if ( myDocument != null )
            {
                myDocument.Document.Click -= HtmlDocument_Click;
            }
        }

        public void Reset()
        {
            myMarker.UnmarkAll();

            mySelectedElement = null;
            myTable = null;

            myDimension = CellDimension.None;

            mySkipColumns = null;
            mySkipRows = null;
            myRowHeader = -1;
            myColumnHeader = -1;
            mySeriesName = null;
        }

        public CellDimension Dimension
        {
            get { return myDimension; }
            set
            {
                myDimension = value; TemplateChanged();
            }
        }

        public int[] SkipRows
        {
            get { return mySkipRows; }
            set
            {
                if ( value.Length == 0 )
                {
                    value = null;
                }
                mySkipRows = value;
                TemplateChanged();
            }
        }

        public int[] SkipColumns
        {
            get { return mySkipColumns; }
            set
            {
                if ( value.Length == 0 )
                {
                    value = null;
                }
                mySkipColumns = value;
                TemplateChanged();
            }
        }

        public int RowHeader
        {
            get { return myRowHeader; }
            set
            {
                if ( value < 0 )
                {
                    value = -1;
                }
                myRowHeader = value;
                TemplateChanged();
            }
        }

        public int ColumnHeader
        {
            get { return myColumnHeader; }
            set
            {
                if ( value < 0 )
                {
                    value = -1;
                }
                myColumnHeader = value;
                TemplateChanged();
            }
        }

        public string SeriesName
        {
            get { return mySeriesName; }
            set
            {
                mySeriesName = value;
                ValidateSeriesName();
            }
        }

        private void HtmlDocument_Click( object sender, HtmlElementEventArgs e )
        {
            SelectedElement = myDocument.Create( myDocument.Document.GetElementFromPoint( e.ClientMousePosition ) );
        }

        private void TemplateChanged()
        {
            if ( mySelectedElement == null )
            {
                return;
            }

            // unmark all first
            myMarker.UnmarkAll();

            if ( myDimension == CellDimension.Row )
            {
                myMarker.MarkTableRow( mySelectedElement.Element );
                DoSkipColumns();
            }
            else if ( myDimension == CellDimension.Column )
            {
                myMarker.MarkTableColumn( mySelectedElement.Element );
                DoSkipRows();
            }
            else
            {
                myMarker.MarkElement( mySelectedElement.Element );
            }

            MarkRowHeader();
            MarkColumnHeader();

            ValidateSeriesName();
        }

        private void ValidateSeriesName()
        {
            if ( mySeriesName == null )
            {
                return;
            }

            IHtmlElement header = null;
            if ( myDimension == CellDimension.Column )
            {
                if ( myColumnHeader != -1 )
                {
                    header = FindColumnHeader( myColumnHeader )( mySelectedElement );
                }
            }
            else
            {
                // row or cell
                if ( myRowHeader != -1 )
                {
                    header = FindRowHeader( myRowHeader )( mySelectedElement );
                }
            }

            if ( header != null && !header.InnerText.Contains( mySeriesName ) )
            {
                ValidationChanged( false );
            }
            else
            {
                ValidationChanged( true );
            }
        }

        private void FireValidationChanged( bool isValid )
        {
            if ( ValidationChanged != null )
            {
                ValidationChanged( isValid );
            }
        }

        private void DoSkipRows()
        {
            int column = HtmlTable.GetColumnIndex( mySelectedElement );

            Func<int, IHtmlElement> GetCellAt = row => myTable.GetCellAt( row, column );

            SkipElements( mySkipRows, GetCellAt );
        }

        private void DoSkipColumns()
        {
            int row = HtmlTable.GetRowIndex( mySelectedElement );

            Func<int, IHtmlElement> GetCellAt = col => myTable.GetCellAt( row, col );

            SkipElements( mySkipColumns, GetCellAt );
        }

        private Func<IHtmlElement, IHtmlElement> FindRowHeader( int pos )
        {
            return e => myTable.GetCellAt( HtmlTable.GetRowIndex( e ), pos );
        }

        private Func<IHtmlElement, IHtmlElement> FindColumnHeader( int pos )
        {
            return e => myTable.GetCellAt( pos, HtmlTable.GetColumnIndex( e ) );
        }

        private void MarkRowHeader()
        {
            MarkHeader( myRowHeader, FindRowHeader );
        }

        private void MarkColumnHeader()
        {
            MarkHeader( myColumnHeader, FindColumnHeader );
        }

        private void MarkHeader( int pos, Func<int, Func<IHtmlElement, IHtmlElement>> FindHeaderCreator )
        {
            if ( pos == -1 )
            {
                return;
            }

            var FindHeader = FindHeaderCreator( pos );

            List<IHtmlElement> header = null;
            if ( myDimension == CellDimension.None )
            {
                // mark single column/row 
                header = new List<IHtmlElement>();
                header.Add( FindHeader( mySelectedElement ) );
            }
            else
            {
                // mark all columns/rows
                header = myMarker.MarkedElements
                    .Select( m => myDocument.Create( m ) )
                    .Select( m => FindHeader( m ) )
                    .ToList();
            }

            header
                .Cast<HtmlElementAdapter>()
                .Foreach( e => myMarker.MarkElement( e.Element, Color.SteelBlue ) );
        }

        private void SkipElements( int[] positions, Func<int, IHtmlElement> GetCellAt )
        {
            if ( positions == null )
            {
                return;
            }

            positions.Foreach( pos => myMarker.UnmarkElement( ( (HtmlElementAdapter)GetCellAt( pos ) ).Element ) );
        }
    }
}