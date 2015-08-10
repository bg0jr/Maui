using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maui.Data.Sheets
{
    /// <summary>
    /// Interprets the given sheet as ini-like config.
    /// Section titles need to be in the first column and sourrounded 
    /// with "[]". Section content are key-value-pairs. The key needs to be 
    /// in the first column, the value in the second. Empty rows are ignored.
    /// </summary>
    public class WorkbookConfig
    {
        /// <summary/>
        public static readonly string DefaultSheetName = "Maui-Config";

        private const string UnassignedSectionName = "@@@unassigned@@@";
        private IWorksheet mySheet;

        /// <summary/>
        public WorkbookConfig( IWorksheet sheet )
        {
            mySheet = sheet;
        }

        /// <summary/>
        public ConfigSection GetSection( string name )
        {
            return ReadConfig().SingleOrDefault( section => section.Name == name );
        }

        private IList<ConfigSection> ReadConfig()
        {
            var sections = new List<ConfigSection>();
            ConfigSection currentSection = null;

            int rowCount = mySheet.GetRowCount();
            for ( int row = 1; row <= rowCount; ++row )
            {
                if ( IsEmptyRow( row ) )
                {
                    continue;
                }

                var sectionName = TryGetSectionTitle( row );

                if ( sectionName != null )
                {
                    currentSection = GetOrCreateSection( sections, sectionName );
                }
                else
                {
                    if ( currentSection == null )
                    {
                        // found properties outside any section
                        // -> lets create a placeholder for those properties
                        currentSection = GetOrCreateSection( sections, UnassignedSectionName );
                    }

                    currentSection.Properties.Add( ReadProperty( row ) );
                }
            }

            return sections;
        }

        // in this context a cell is treated as empty if there is now key defined
        private bool IsEmptyRow( int row )
        {
            return mySheet.IsEmptyCell( mySheet.GetCell( "A" + row ) );
        }

        private ConfigSection GetOrCreateSection( IList<ConfigSection> sections, string sectionName )
        {
            var section = sections.SingleOrDefault( sec => sec.Name == sectionName );
            if ( section == null )
            {
                section = new ConfigSection( sectionName );
                sections.Add( section );
            }

            return section;
        }

        private KeyValuePair<string, string> ReadProperty( int row )
        {
            var key = mySheet.GetCell( "A" + row ).ToString();
            var value = mySheet.GetCell( "B" + row );

            return new KeyValuePair<string, string>( key,
                ( value != null ? value.ToString() : null ) );
        }

        /// <summary>
        /// Tries to interpret the first column of the given row as section
        /// title. returns null if it is no section title.
        /// </summary>
        private string TryGetSectionTitle( int row )
        {
            var cell = mySheet.GetCell( "A" + row ).ToString();

            if ( cell.StartsWith( "[" ) && cell.EndsWith( "]" ) )
            {
                return cell.Substring( 1, cell.Length - 2 );
            }

            return null;
        }

        /// <summary>
        /// Writes the given section into the config.
        /// If the section already exists it will be overwritten. If not
        /// the new section will be added.
        /// </summary>
        public void SetSection( ConfigSection section )
        {
            var sections = ReadConfig();

            AddOrOverwrite( sections, section );

            WriteConfig( sections );
        }

        private void AddOrOverwrite( IList<ConfigSection> sections, ConfigSection section )
        {
            var existingSection = sections.SingleOrDefault( sec => sec.Name == section.Name );
            if ( existingSection != null )
            {
                existingSection.Properties.Clear();
                existingSection.Properties.AddRange( section.Properties );
            }
            else
            {
                sections.Add( section );
            }
        }

        private void WriteConfig( IEnumerable<ConfigSection> sections )
        {
            int initialRowCoung = mySheet.GetRowCount();

            int row = 1;
            foreach ( var section in sections )
            {
                WriteSection( ref row, section );

                // add emtpy row between sections
                ClearRow( row );
                row++;
            }

            // clear old config values
            for ( int i = row; i < initialRowCoung; ++i )
            {
                ClearRow( i );
            }
        }

        private void WriteSection( ref int row, ConfigSection section )
        {
            ClearRow( row );
            mySheet.SetCell( "A" + row, "[" + section.Name + "]" );
            row++;

            foreach ( var pair in section.Properties )
            {
                WriteProperty( row++, pair );
            }
        }

        private void ClearRow( int row )
        {
            mySheet.SetCell( "A" + row, null );
            mySheet.SetCell( "B" + row, null );
        }

        private void WriteProperty( int row, KeyValuePair<string, string> pair )
        {
            mySheet.SetCell( "A" + row, pair.Key );
            mySheet.SetCell( "B" + row, pair.Value );
        }
    }
}
