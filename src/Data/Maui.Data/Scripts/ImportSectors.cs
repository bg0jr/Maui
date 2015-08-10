using System.Linq;
using System.Transactions;
using Blade;
using Maui.Entities;
using Maui;
using Maui.Logging;
using Maui.Shell;
using Blade.Logging;
using Blade.Shell;

namespace Maui.Data.Scripts
{
    public class ImportSectors : ScriptBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( ImportSectors ) );
        
        protected override void Run()
        {
            using ( var tom = Engine.ServiceProvider.CreateEntityRepository() )
            {
                if ( tom.Sectors.Any() )
                {
                    myLogger.Info( "Sectors already imported. Skipping ..." );
                    return;
                }

                using ( var trans = new TransactionScope() )
                {
                    tom.Sectors.AddObject( CreateSector( "Automobil", "Automobil/Flugzeugbau", "Autovermietung", "Autozulieferer", "Leasing" ) );
                    tom.Sectors.AddObject( CreateSector( "Bau", "Bauindustrie", "Baumaschinen", "Baustoffe" ) );
                    tom.Sectors.AddObject( CreateSector( "Bekleidung", "Beikleidung/Textil", "Textil/Bekleidung" ) );
                    tom.Sectors.AddObject( CreateSector( "Biotechnologie" ) );
                    tom.Sectors.AddObject( CreateSector( "Chemie", "Chemie/Schmierstoffe", "Chemische Industrie" ) );
                    tom.Sectors.AddObject( CreateSector( "Dienstleistung" ) );
                    tom.Sectors.AddObject( CreateSector( "Elektro", "Elektr. Bauelemente", "Elektro/Elektronik", "Elektroindustrie", "Elektronik" ) );
                    tom.Sectors.AddObject( CreateSector( "Erdöl/Erdgas" ) );
                    tom.Sectors.AddObject( CreateSector( "Finanzen", "Bank", "Banken/Finanzdienstleistungen", "Discount-Broker", "Finanzdienstleister", "Hypothekenbank" ) );
                    tom.Sectors.AddObject( CreateSector( "Handel", "Handel/Distribution" ) );
                    tom.Sectors.AddObject( CreateSector( "Holding", "Beteiligungen", "Holding/Beteiligungen" ) );
                    tom.Sectors.AddObject( CreateSector( "IT", "Computer", "Datenuebertragung", "EDV/Hardware", "Halbleiter", "Hardware/Software", "Internet", "Internetdienste", "IT-Dienstleister", "IT-Dienstleistungen", "IT-Sicherheit", "Netzwerkinfrastruktur", "Software" ) );
                    tom.Sectors.AddObject( CreateSector( "Immobilien", "Immobilienverwaltung" ) );
                    tom.Sectors.AddObject( CreateSector( "Index Börse" ) );
                    tom.Sectors.AddObject( CreateSector( "Konsum", "Konsumgüter" ) );
                    tom.Sectors.AddObject( CreateSector( "Logistik", "Verkehr/Logistik", "Verpackungen", "Versandhandel" ) );
                    tom.Sectors.AddObject( CreateSector( "Marktforschung" ) );
                    tom.Sectors.AddObject( CreateSector( "Maschinenbau", "CNC-Maschinenbau", "Druckmaschinen", "Spezial-/Maschinenbau", "Spezialmaschinen" ) );
                    tom.Sectors.AddObject( CreateSector( "Medien", "Medien/Verlag/Druck" ) );
                    tom.Sectors.AddObject( CreateSector( "Medizin", "Klinik/Kurbetrieb", "Kliniken/Pflegeeinrichtungen", "Medizin. Geräte", "Medizintechnik" ) );
                    tom.Sectors.AddObject( CreateSector( "Metalle", "Eisen/Stahl", "Stahl" ) );
                    tom.Sectors.AddObject( CreateSector( "Nahrungsmittel", "Nahrungs-/Genussmittel" ) );
                    tom.Sectors.AddObject( CreateSector( "Papier/Holz" ) );
                    tom.Sectors.AddObject( CreateSector( "Performanceindex" ) );
                    tom.Sectors.AddObject( CreateSector( "Pharma", "Pharma/Biotechnol.", "Pharma/Kosmetik", "Pharmahandel" ) );
                    tom.Sectors.AddObject( CreateSector( "Technologie", "Großküchentechnik", "Lackieranlagen", "Nanotechnologie", "Vakuumpumpen" ) );
                    tom.Sectors.AddObject( CreateSector( "Telekommunikation" ) );
                    tom.Sectors.AddObject( CreateSector( "Touristik", "Freizeit/Touristik" ) );
                    tom.Sectors.AddObject( CreateSector( "Umwelt", "Umwelttechnik" ) );
                    tom.Sectors.AddObject( CreateSector( "Verkehr", "Fluggesellschaft", "Flurförderzeuge", "Navigation" ) );
                    tom.Sectors.AddObject( CreateSector( "Versicherung", "Rückversicherung" ) );
                    tom.Sectors.AddObject( CreateSector( "Versorgung", "Energietechnik", "Industriegase" ) );
                   
                    tom.SaveChanges();
                    trans.Complete();
                }
            }
        }

        private Sector CreateSector( string name, params string[] aliases )
        {
            var sector = new Sector() { Name = name };

            foreach ( var alias in aliases )
            {
                sector.Aliases.Add( new SectorAlias() { Sector = sector, Name = alias } );
            }

            return sector;
        }
    }
}
