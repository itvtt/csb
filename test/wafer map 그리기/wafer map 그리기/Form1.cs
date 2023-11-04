using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using System.Windows.Forms.DataVisualization.Charting;
using OxyPlot;
using ClosedXML.Excel;
using OxyDataPoint = OxyPlot.DataPoint;
using OxyPlot.Series;

namespace wafer_map_그리기
{
    public partial class Form1 : Form
    {
        public class DataPointEx
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Value { get; set; }

            public DataPointEx(double x, double y)
            {
                X = x;
                Y = y;
            }
        }

        public Form1()
        {
            InitializeComponent();

            List<DataPointEx> points = ReadDataFromExcel("c:/121.xlsx");

 

            var plotModel = new PlotModel();

            foreach (var point in points)
            {
                AddPointToPlot(plotModel, point);
            }

            // PlotModel을 OxyPlot 컨트롤에 할당합니다.
            plotView1.Model = plotModel;
        }
        private void AddPointToPlot(PlotModel plotModel, DataPointEx point)
        {
            var series = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerFill = GetColorForValue(point.Value)
            };

            series.Points.Add(new ScatterPoint(point.X, point.Y));
            plotModel.Series.Add(series);
        }

        private OxyColor GetColorForValue(double value)
        {
            if (value < 0.3)
                return OxyColors.Red;
            else if (value < 0.6)
                return OxyColors.Yellow;
            else
                return OxyColors.Green;
        }

        public static List<DataPointEx> ReadDataFromExcel(string filePath)
        {
            var dataPoints = new List<DataPointEx>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed();

                foreach (var row in rows)
                {
                    string cellValue = row.Cell("x").GetString(); // 먼저 셀의 값을 문자열로 가져옵니다.

                    if (double.TryParse(cellValue, out double x))
                    {
                        // 변환에 성공한 경우 x에 값이 저장됩니다.
                    }
                    else
                    {
                        // 변환에 실패한 경우, 적절한 예외 처리 또는 기본값 설정을 합니다.
                    }
                    string cellValuey = row.Cell("y").GetString(); // 먼저 셀의 값을 문자열로 가져옵니다.

                    if (double.TryParse(cellValuey, out double y))
                    {
                        // 변환에 성공한 경우 x에 값이 저장됩니다.
                    }
                    else
                    {
                        // 변환에 실패한 경우, 적절한 예외 처리 또는 기본값 설정을 합니다.
                    }

                    //double x = row.Cell("x").GetDouble();
                    //double y = row.Cell("y").GetDouble();
                    //double value = row.Cell(2).GetDouble(); // 여기서 26은 "Z" 컬럼의 열 번호입니다. 실제 열 번호로 교체해주세요.
                    string cellValueForValue = row.Cell(2).GetString();

                    if (double.TryParse(cellValueForValue, out double value))
                    {
                        // 변환에 성공한 경우 value에 값이 저장됩니다.
                    }
                    else
                    {
                        // 변환에 실패한 경우, 적절한 예외 처리 또는 기본값 설정을 합니다.
                        value = 0;  // 예시로 0을 설정하였습니다. 필요에 따라 다르게 설정하실 수 있습니다.
                    }


                    var dataPoint = new DataPointEx(x, y) { Value = value };
                    dataPoints.Add(dataPoint);
                }
            }

            return dataPoints;
        }
    }
}