using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Maui.Trading.Model;
using System.Drawing.Imaging;
using Maui.Trading.Reporting.Charting;
using Maui.Trading.Utils;
using Maui.Trading.Data;

namespace Maui.Trading.Reporting.Rendering
{
    public class HtmlGenericChartRenderAction : IRenderAction<GenericChartSection>
    {
        private class RenderAction
        {
            private const int ChartWidth = 800;

            private IRenderingContext myContext;
            private GenericChartSection mySection;
            private GenericStockChart myChart;
            private GenericStockChartViewModel.Settings myModelSettings;

            public RenderAction( IRenderingContext context, GenericChartSection section )
            {
                myContext = context;
                mySection = section;

                myModelSettings = new GenericStockChartViewModel.Settings();
                myModelSettings.CurvesPreRenderingOperators.Add( new ReducePointsOperator( ChartWidth / 4 ) );
            }

            public void Render()
            {
                myContext.Document.WriteLine( "<h2>{0}</h2>", mySection.Name );

                myChart = CreateChart();

                myContext.Document.WriteLine( "<div class=\"tabber\" style=\"width:" + myChart.Width + "px\">" );

                var max = mySection.Chart.Prices.Max( p => p.Time );

                RenderTab( "5 days", new TimeRange( mySection.Chart.Prices[ mySection.Chart.Prices.Count - 5 ].Time, max ) );
                RenderTab( "1 month", new TimeRange( max.AddMonths( -1 ), max ) );
                RenderTab( "3 month", new TimeRange( max.AddMonths( -3 ), max ) );
                RenderTab( "1 year", new TimeRange( max.AddYears( -1 ), max ), isActive: true );
                RenderTab( "3 year", new TimeRange( max.AddYears( -3 ), max ) );
                RenderTab( "all", new TimeRange( DateTime.MinValue, DateTime.MaxValue ) );

                myContext.Document.WriteLine( "</div>" );
            }

            private GenericStockChart CreateChart()
            {
                var model = new GenericStockChartViewModel( myModelSettings );
                model.Add( mySection.Chart );

                var chart = new GenericStockChart( model );
                chart.Width = ChartWidth;
                chart.Height = 450;

                return chart;
            }

            private void RenderTab( string tabTitle, TimeRange viewPort, bool isActive = false )
            {
                var imgSrc = RenderImage( tabTitle, viewPort );

                AddTab( tabTitle, imgSrc, isActive );
            }

            private string RenderImage( string subTitle, TimeRange viewPort )
            {
                myChart.Model.ViewPort = viewPort;
                var img = myChart.Render();

                var imgSrc = mySection.Chart.Name + "." + subTitle.Replace( " ", string.Empty ) + ".png";
                var imgFile = Path.Combine( myContext.DocumentUrlRoot, imgSrc );
                img.Save( imgFile, ImageFormat.Png );

                return imgSrc;
            }

            private void AddTab( string tabTitle, string imgSrc, bool isActive )
            {
                var cssClass = isActive ? "tabbertab tabbertabdefault" : "tabbertab";

                myContext.Document.WriteLine( "  <div class=\"{0}\">", cssClass );
                myContext.Document.WriteLine( "    <h2>{0}</h2>", tabTitle );
                myContext.Document.WriteLine( "    <img src=\"{0}\">", imgSrc );
                myContext.Document.WriteLine( "  </div>" );
            }
        }

        public void Render( GenericChartSection section, IRenderingContext context )
        {
            if ( section.Chart.IsEmpty )
            {
                return;
            }

            var renderAction = new RenderAction( context, section );
            renderAction.Render();
        }

    }
}
