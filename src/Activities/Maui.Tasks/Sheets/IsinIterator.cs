using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Maui.Logging;
using Maui.Data.Sheets;
using Blade.Logging;

namespace Maui.Tasks.Sheets
{
    /// <summary>
    /// Base class for isin based iterators.
    /// <remarks>
    /// Iterators allow to loop over a range of values in an excel sheet.
    /// Detects semantic of columns automatically by analysing the given
    /// header row. Derived classes can customize this behaviour.
    /// </remarks>
    /// </summary>
    public class IsinIterator : IEnumerable<string>
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( IsinIterator ) );
        
        /// <summary/>
        public IsinIterator( IWorksheet worksheet, int headerRow )
        {
            Worksheet = worksheet;
            HeaderRow = headerRow;

            CurrentDataRow = HeaderRow + 1;
        }

        /// <summary/>
        protected IWorksheet Worksheet { get; private set; }

        /// <summary>
        /// Current row of the iterator.
        /// </summary>
        protected int CurrentDataRow { get; private set; }

        /// <summary/>
        public string IsinColumn { get; private set; }

        /// <summary/>
        public string ErrorColumn { get; private set; }

        /// <summary/>
        public string StatusColumn { get; set; }

        /// <summary/>
        public int HeaderRow { get; private set; }

        /// <summary>
        /// Loops over the range returned by GetEnumerator(), gets some data
        /// with the provided getData() and sets it with setData(). Handles
        /// progress and errors.
        /// </summary>
        public void Loop<T>( Func<string, T> getData, Action<T> setData )
        {
            foreach ( var isin in this )
            {
                try
                {
                    MarkAsInProgress();
                    ResetError();

                    var data = getData( isin );

                    MarkAsSucceeded();
                    setData( data );
                }
                catch ( Exception ex )
                {
                    MarkAsFailed();
                    ReportError( ex );
                }
            }
        }

        /// <summary>
        /// Returns an enumerator for isins. Starts below the 
        /// header row and loops til the first empty row.
        /// </summary>
        public IEnumerator<string> GetEnumerator()
        {
            DetectHeaderColumns();

            var isinCell = GetCurrentIsinCell();

            while ( !IsEmptyCell( isinCell ) )
            {
                yield return isinCell.ToString().Trim();

                CurrentDataRow += 1;
                isinCell = GetCurrentIsinCell();
            }
        }

        /// <summary/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Scans the header row for the headers.
        /// Only checks the first 26 columns
        /// </summary>
        protected void DetectHeaderColumns()
        {
            for ( char col = 'A'; col <= 'Z'; col++ )
            {
                var cell = Worksheet.GetCell( col.ToString() + HeaderRow );
                if ( IsEmptyCell( cell ) )
                {
                    // emptly columns in between are not supported
                    break;
                }

                var header = cell.ToString().Trim();

                OnHeaderColumn( col, header );
            }

            OnHeaderDetected();
        }

        private bool IsEmptyCell( object cellValue )
        {
            return Worksheet.IsEmptyCell( cellValue ) ||
                // if we "calculate" from one sheet to the other in Excel
                // empty lines become "0"
                ( cellValue is int && (int)cellValue == 0 ) ||
                ( cellValue is double && (double)cellValue == 0d );
        }

        /// <summary>
        /// Called for each header column that should be analysed.
        /// <remarks>
        /// Base class already handles isin and error detection.
        /// </remarks>
        /// </summary>
        protected virtual void OnHeaderColumn( char column, string header )
        {
            if ( header == "Isin" )
            {
                IsinColumn = column.ToString();
            }
            else if ( header == "Error" )
            {
                ErrorColumn = column.ToString();
            }
        }

        /// <summary>
        /// Called when the header detection has finished.
        /// Could be used to e.g. validate the header detection
        /// (enforcing mandatory header columns).
        /// </summary>
        protected virtual void OnHeaderDetected()
        {
            if ( IsinColumn == null )
            {
                throw new ArgumentException( "Isin column not found" );
            }
        }

        private object GetCurrentIsinCell()
        {
            return Worksheet.GetCell( IsinColumn + CurrentDataRow );
        }

        /// <summary>
        /// Marks the current data row as "in progress" if a status columns has
        /// been defined during header analysis by derived class
        /// </summary>
        public void MarkAsInProgress()
        {
            if ( StatusColumn != null )
            {
                Worksheet.SetCell( StatusColumn + CurrentDataRow, "in progress ..." );
            }
        }

        /// <summary>
        /// Marks the current data row as "failed" if a status columns has
        /// been defined during header analysis by derived class
        /// </summary>
        public void MarkAsFailed()
        {
            if ( StatusColumn != null )
            {
                Worksheet.SetCell( StatusColumn + CurrentDataRow, "failed" );
            }
        }

        /// <summary>
        /// Marks the current data row as "succeeded" if a status columns has
        /// been defined during header analysis by derived class
        /// </summary>
        public void MarkAsSucceeded()
        {
            if ( StatusColumn != null )
            {
                Worksheet.SetCell( StatusColumn + CurrentDataRow, "succeeded" );
            }
        }

        /// <summary>
        /// Used to report errors related to current data row.
        /// If no error column could be detected a message box will be shown.
        /// </summary>
        public void ReportError( string message )
        {
            if ( ErrorColumn == null )
            {
                // TODO: this should be handled by logging
                MessageBox.Show( message );
            }
            else
            {
                Worksheet.SetCell( ErrorColumn + CurrentDataRow, message );
            }
        }

        /// <summary>
        /// <see cref="ReportError(string)"/>
        /// </summary>
        public void ReportError( Exception exception )
        {
            ReportError( exception.Message );
            myLogger.Error( exception, exception.Message );
        }

        /// <summary>
        /// Used to reset previous errors related to current data row.
        /// If no error column could be detected this method does nothing.
        /// </summary>
        public void ResetError()
        {
            if ( ErrorColumn != null )
            {
                Worksheet.SetCell( ErrorColumn + CurrentDataRow, null );
            }
        }
    }
}
