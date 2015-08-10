using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using Maui.Trading.Model;
using System.IO;
using Maui.Trading.Utils;
using Maui.Trading.Indicators;
using System.Windows.Forms;

namespace Maui.Trading.Reporting.Charting
{
    public class GenericStockChart
    {
        private class RenderingContext : Chart
        {
            private GenericStockChart myHandle;

            public RenderingContext( GenericStockChart handle )
            {
                myHandle = handle;

                SetupRenderingArea();
            }

            public ChartArea MainArea
            {
                get
                {
                    return ChartAreas.First();
                }
            }

            public GenericStockChartViewModel Model
            {
                get
                {
                    return myHandle.Model;
                }
            }

            private void SetupRenderingArea()
            {
                Width = myHandle.Width;
                Height = myHandle.Height;

                var area = new ChartArea( "main" );
                ChartAreas.Add( area );
            }

            public void Render()
            {
                AddSeries();
                AddSignals();

                SetupAxis();
                SetupLegend();
            }

            private void AddSeries()
            {
                foreach ( var curve in Model.Curves )
                {
                    var series = curve.CreateSeries();
                    AddSeries( series );
                }
            }

            private void AddSeries( Series series )
            {
                series.ChartArea = ChartAreas.First().Name;

                Series.Add( series );
            }

            private void AddSignals()
            {
                if ( Model.Signals == SignalCurve.Empty )
                {
                    return;
                }

                var series = Model.Signals.CreateSeries();
                AddSeries( series );
            }

            private void SetupAxis()
            {
                MainArea.AxisX = new XAxis( Model.MinTime, Model.MaxTime );

                double min = Model.MinValue * 0.90;
                double max = Model.MaxValue * 1.10;

                MainArea.AxisY = new PriceAxis( min, max );
                MainArea.AxisY2 = new PriceAxis( min, max );

                MainArea.AxisY.LabelStyle.Enabled = false;
            }

            private void SetupLegend()
            {
                if ( Series.Count <= 1 )
                {
                    return;
                }

                var legend = new Legend();
                legend.LegendStyle = LegendStyle.Table;
                legend.TableStyle = LegendTableStyle.Auto;
                legend.Docking = Docking.Bottom;

                Legends.Add( legend );
            }

            public Image GetImage()
            {
                using ( var stream = new MemoryStream() )
                {
                    SaveImage( stream, ChartImageFormat.Png );
                    return Bitmap.FromStream( stream );
                }
            }
        }

        public GenericStockChart( GenericStockChartViewModel viewModel )
        {
            Model = viewModel;

            SetDefaults();
        }

        public GenericStockChartViewModel Model
        {
            get;
            private set;
        }

        private void SetDefaults()
        {
            Width = 800;
            Height = 450;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public Image Render()
        {
            var renderingContext = new RenderingContext( this );
            renderingContext.Render();

            return renderingContext.GetImage();
        }
    }
}
