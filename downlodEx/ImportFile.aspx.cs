using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;

namespace downlodEx
{
    public partial class ImportFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFiles)
            {
                string uploadPath = Server.MapPath("~/UploadedFiles");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                foreach (HttpPostedFile uploadedFile in FileUpload1.PostedFiles)
                {
                    string filePath = Path.Combine(uploadPath, uploadedFile.FileName);
                    uploadedFile.SaveAs(filePath);
                }
                ViewState["FilesUploaded"] = true;
            }
        }


        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (ViewState["FilesUploaded"] != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var sheet = excel.Workbook.Worksheets.Add("CombinedData");
                    int currentRow = 1;

                    string[] filePaths = Directory.GetFiles(Server.MapPath("~/UploadedFiles"));
                    var mcxPeakData = new Dictionary<string, Dictionary<string, decimal>>();

                    foreach (string filePath in filePaths)
                    {
                        if (Path.GetFileName(filePath).StartsWith("MCX_PeakMargin"))
                        {
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                            string[] fileNameParts = fileNameWithoutExtension.Split('0');
                            if (fileNameParts.Length >= 3)
                            {
                                string indicator = fileNameParts[4]; 
                                var peakData = new Dictionary<string, decimal>();

                                using (var reader = new StreamReader(filePath))
                                {
                                    bool isFirstRow = true;
                                    while (!reader.EndOfStream)
                                    {
                                        var line = reader.ReadLine();
                                        var values = line.Split(',');

                                        if (isFirstRow)
                                        {
                                            isFirstRow = false;
                                            continue;
                                        }

                                        if (values.Length > 2) 
                                        {
                                            string uccCode = values[4];
                                            if (!peakData.ContainsKey(uccCode))
                                            {
                                                
                                                if (decimal.TryParse(values[8], out decimal i) &&
                                                    decimal.TryParse(values[9], out decimal j) &&
                                                    decimal.TryParse(values[10], out decimal k))
                                                {
                                                    peakData[uccCode] = (j + k) - i;
                                                }
                                            }
                                        }
                                    }
                                }

                                mcxPeakData[indicator] = peakData;
                            }
                        }
                    }



                    foreach (string filePath in filePaths)
                    {
                        try
                        {
                            if (Path.GetExtension(filePath).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                            {
                                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                                string header = fileNameWithoutExtension.Length >= 6
                                    ? fileNameWithoutExtension.Substring(0, 6)
                                    : "Unknown";

                                System.Diagnostics.Debug.WriteLine($"Processing file: {filePath}, Header: {header}");

                                string[] headers = null;

                                if (header == "F_SA04")
                                {
                                    headers = new[] { "Date", "Indicator", "UCC Code", "Min.Margin", "Total Cash Collateral", "Total Non Cash Collateral", "Excess Collateral of Other Segment", "Total Value of Collateral", "Short Allocation", "MCX Peak" };
                                    sheet.Cells[currentRow, 1].Value = "F_SA04";
                                    sheet.Cells[currentRow, 1].Style.Font.Bold = true;
                                    sheet.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                    currentRow++;
                                   
                                    for (int col = 0; col < headers.Length; col++)
                                    {
                                        sheet.Cells[currentRow, col + 1].Value = headers[col];
                                        sheet.Cells[currentRow, col + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                    }
                                    currentRow++;
                                }
                                else if (header == "C_SA04" || header == "F_SA06" || header == "MCX_EO")
                                {
                                    if (header == "C_SA04")
                                    {
                                        headers = new[] { "Date", "Indicator", "UCC Code", "Min.Margin", "Total Cash Collateral", "Total Non Cash Collateral", "Excess Collateral of Other Segment", "Total Value of Collateral", "Short Allocation" };
                                        sheet.Cells[currentRow, 1].Value = "C_SA04";
                                        sheet.Cells[currentRow, 1].Style.Font.Bold = true;
                                        sheet.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                        currentRow++;
                                   
                                        for (int col = 0; col < headers.Length; col++)
                                        {
                                            sheet.Cells[currentRow, col + 1].Value = headers[col];
                                            sheet.Cells[currentRow, col + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                        }
                                        currentRow++;
                                    }
                                    else if (header == "F_SA06")
                                    {
                                        sheet.Cells[currentRow, 1].Value = "F_SA06";
                                        sheet.Cells[currentRow, 1].Style.Font.Bold = true;
                                        sheet.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                        currentRow++;
                                    }
                                    else if (header == "MCX_EO")
                                    {
                                        headers = new[] { "Date", "Time", "CM ID", "Client Code", "Shortage" };
                                        sheet.Cells[currentRow, 1].Value = "MCX Peak1";
                                        sheet.Cells[currentRow, 1].Style.Font.Bold = true;
                                        sheet.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                        currentRow++;
                                       
                                        for (int col = 0; col < headers.Length; col++)
                                        {
                                            sheet.Cells[currentRow, col + 1].Value = headers[col];
                                            sheet.Cells[currentRow, col + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                        }
                                        currentRow++;
                                    }
                                }

                                if (headers != null)
                                {
                                    using (var reader = new StreamReader(filePath))
                                    {
                                        bool isFirstRow = true;
                                        while (!reader.EndOfStream)
                                        {
                                            var line = reader.ReadLine();
                                            var values = line.Split(',');

                                            if (isFirstRow)
                                            {
                                                isFirstRow = false;
                                            }

                                            bool includeRow = false;

                                            if (header == "F_SA06" && values.Length > 9 && decimal.TryParse(values[9], out decimal columnJValue) && columnJValue != 0)
                                            {
                                                includeRow = true;
                                            }
                                            else if (header == "C_SA04" && values.Length > 8 && decimal.TryParse(values[8], out decimal columnIValue) && columnIValue != 0)
                                            {
                                                includeRow = true;
                                            }
                                            else if (header == "F_SA04" && values.Length > 8 && decimal.TryParse(values[8], out decimal columnI0alue) && columnI0alue != 0)
                                            {
                                                includeRow = true;
                                            }
                                            else if (header == "MCX_EO")
                                            {
                                                includeRow = true;
                                            }

                                            if (includeRow)
                                            {
                                              
                                                if (header == "F_SA04")
                                                {
                                                    string uccCode = values[2]; 
                                                    string indicator = values[1]; 
                                                    if (mcxPeakData.ContainsKey(indicator) && mcxPeakData[indicator].ContainsKey(uccCode))
                                                    {
                                                        decimal mcxPeakValue = mcxPeakData[indicator][uccCode];
                                                        sheet.Cells[currentRow, headers.Length].Value = mcxPeakValue;
                                                    }
                                                    else
                                                    {
                                                        sheet.Cells[currentRow, headers.Length].Value = 0;
                                                    }
                                                }


                                                for (int col = 0; col < values.Length; col++)
                                                {
                                                    sheet.Cells[currentRow, col + 1].Value = values[col];
                                                    sheet.Cells[currentRow, col + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                                }


                                                currentRow++;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error processing file {filePath}: {ex.Message}");
                        }
                    }
                


                        currentRow++;

                        string[] headers3 = { "MCX Peak2" };
                        string[] headers2 = { "Date", "Time", "CM ID", "TM ID", "Client Code", "Span", "ELM", "Additional Margin", "Total Margin", "Cash Collateral", "Collateral", "Shortage", "Excess", "C_SA03", "F_SA03", "C_RMS", "F_RMS", "C_CC02", "F_CC02" };

                        sheet.Cells[currentRow, 1, currentRow, headers2.Length].Merge = true;
                        sheet.Cells[currentRow, 1].Value = headers3[0];
                        sheet.Cells[currentRow, 1].Style.Font.Bold = true;
                        sheet.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        currentRow++;

                        for (int col = 0; col < headers2.Length; col++)
                        {
                            sheet.Cells[currentRow, col + 1].Value = headers2[col];
                            sheet.Cells[currentRow, col + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        }
                        currentRow++;
                        var csa03Data = new List<Dictionary<string, decimal>>();
                        var fsa03Data = new List<Dictionary<string, decimal>>();
                        string[] csa03Files = Directory.GetFiles(Server.MapPath("~/UploadedFiles"), "C_SA03_P_14697_*.csv");
                        foreach (string csa03File in csa03Files)
                        {
                            var fileData = new Dictionary<string, decimal>();
                            try
                            {
                                using (var reader = new StreamReader(csa03File))
                                {
                                    bool isFirstRow = true;
                                    while (!reader.EndOfStream)
                                    {
                                        var line = reader.ReadLine();
                                        var values = line.Split(',');
                                        if (isFirstRow)
                                        {
                                            isFirstRow = false;
                                            continue;
                                        }
                                        if (values.Length > 7 && decimal.TryParse(values[6], out decimal indexGValue) && decimal.TryParse(values[3], out decimal indexDValue))
                                        {
                                            var clientCode = values[2];
                                            fileData[clientCode] = indexGValue - indexDValue;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error processing C_SA03 file {csa03File}: {ex.Message}");
                            }
                            csa03Data.Add(fileData);
                        }
                        string[] fsa03Files = Directory.GetFiles(Server.MapPath("~/UploadedFiles"), "F_SA03*.csv");
                        foreach (string fsa03File in fsa03Files)
                        {
                            var fileData = new Dictionary<string, decimal>();
                            try
                            {
                                using (var reader = new StreamReader(fsa03File))
                                {
                                    bool isFirstRow = true;
                                    while (!reader.EndOfStream)
                                    {
                                        var line = reader.ReadLine();
                                        var values = line.Split(',');
                                        if (isFirstRow)
                                        {
                                            isFirstRow = false;
                                            continue;
                                        }
                                        if (values.Length > 7 && decimal.TryParse(values[6], out decimal indexGValue) && decimal.TryParse(values[3], out decimal indexDValue))
                                        {
                                            var clientCode = values[2];
                                            fileData[clientCode] = indexGValue - indexDValue;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error processing F_SA03 file {fsa03File}: {ex.Message}");
                            }
                            fsa03Data.Add(fileData);
                        }
                        var clientMaxShortage = new Dictionary<string, decimal>();
                        var clientMaxShortageRow = new Dictionary<string, string[]>();

                        string[] filePaths2 = Directory.GetFiles(Server.MapPath("~/UploadedFiles"));
                        foreach (string filePath in filePaths2)
                        {
                            try
                            {
                                if (Path.GetExtension(filePath).Equals(".CSV", StringComparison.OrdinalIgnoreCase))
                                {
                                    using (var reader = new StreamReader(filePath))
                                    {
                                        bool isFirstRow = true;
                                        int rowIndex = 0;

                                        while (!reader.EndOfStream)
                                        {
                                            var line = reader.ReadLine();
                                            var values = line.Split(',');

                                            if (isFirstRow)
                                            {
                                                isFirstRow = false;
                                            }

                                            if (rowIndex < 2)
                                            {
                                                rowIndex++;
                                                continue;
                                            }

                                            if (values.Length > 11 && decimal.TryParse(values[11], out decimal columnLValue) && columnLValue != 0)
                                            {
                                                var clientCode = values[4];
                                                if (!clientMaxShortage.ContainsKey(clientCode) || columnLValue > clientMaxShortage[clientCode])
                                                {
                                                    clientMaxShortage[clientCode] = columnLValue;
                                                    clientMaxShortageRow[clientCode] = values;
                                                }
                                            }

                                            rowIndex++;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error processing file {filePath}: {ex.Message}");
                            }
                        }




                        string[] cmCollateralFiles = Directory.GetFiles(Server.MapPath("~/UploadedFiles"), "CM_ClientLevelCollaterals_*.csv");
                        var cmData = new Dictionary<string, decimal>();

                        foreach (string cmFile in cmCollateralFiles)
                        {
                            try
                            {
                                string fileName = Path.GetFileNameWithoutExtension(cmFile);
                                string dateTimePart = fileName.Replace("CM_ClientLevelCollaterals_", "");

                                if (dateTimePart.Length == 14)
                                {
                                    string datePart = dateTimePart.Substring(0, 8);
                                    string timePart = dateTimePart.Substring(8, 6);

                                    DateTime fileDate = DateTime.ParseExact(datePart, "ddMMyyyy", CultureInfo.InvariantCulture);
                                    TimeSpan fileTime = TimeSpan.ParseExact(timePart, "hhmmss", CultureInfo.InvariantCulture);

                                    if ((fileTime >= new TimeSpan(18, 0, 0) && fileTime < new TimeSpan(21, 0, 0)) ||
                                        (fileTime >= new TimeSpan(22, 0, 0) && fileTime < new TimeSpan(23, 0, 0)))
                                    {
                                        using (var reader = new StreamReader(cmFile))
                                        {
                                            bool isFirstRow = true;
                                            while (!reader.EndOfStream)
                                            {
                                                var line = reader.ReadLine();
                                                var values = line.Split(',');

                                                if (isFirstRow)
                                                {
                                                    isFirstRow = false;
                                                    continue;
                                                }

                                                if (values.Length > 7 &&

                                                    decimal.TryParse(values[4], out decimal indexEValue) &&
                                                    decimal.TryParse(values[5], out decimal indexFValue) &&
                                                    decimal.TryParse(values[6], out decimal indexGValue) &&
                                                    decimal.TryParse(values[3], out decimal indexDValue))
                                                {
                                                    var clientCode = values[2];
                                                    decimal formulaValue = (indexEValue + indexFValue + indexGValue) - indexDValue;
                                                    if (clientMaxShortageRow.ContainsKey(clientCode))
                                                    {
                                                        cmData[clientCode] = formulaValue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error processing CM_ClientLevelCollaterals file {cmFile}: {ex.Message}");
                            }
                        }
                        string[] cmCollateralFiles1 = Directory.GetFiles(Server.MapPath("~/UploadedFiles"), "FO_ClientLevelCollaterals_*.csv");
                        var cmData1 = new Dictionary<string, decimal>();


                        foreach (string cmFile1 in cmCollateralFiles1)
                        {
                            try
                            {
                                string fileName = Path.GetFileNameWithoutExtension(cmFile1);
                                string dateTimePart = fileName.Replace("FO_ClientLevelCollaterals_", "");

                                if (dateTimePart.Length == 14)
                                {
                                    string datePart = dateTimePart.Substring(0, 8);
                                    string timePart = dateTimePart.Substring(8, 6);

                                    DateTime fileDate = DateTime.ParseExact(datePart, "ddMMyyyy", CultureInfo.InvariantCulture);
                                    TimeSpan fileTime = TimeSpan.ParseExact(timePart, "hhmmss", CultureInfo.InvariantCulture);


                                    if ((fileTime >= new TimeSpan(18, 0, 0) && fileTime < new TimeSpan(21, 0, 0)) ||
                                        (fileTime >= new TimeSpan(22, 0, 0) && fileTime < new TimeSpan(23, 0, 0)))
                                    {
                                        using (var reader = new StreamReader(cmFile1))
                                        {
                                            bool isFirstRow = true;
                                            while (!reader.EndOfStream)
                                            {
                                                var line = reader.ReadLine();
                                                var values = line.Split(',');

                                                if (isFirstRow)
                                                {
                                                    isFirstRow = false;
                                                    continue;
                                                }

                                                if (values.Length > 7 &&
                                                    decimal.TryParse(values[4], out decimal indexEValue) &&
                                                    decimal.TryParse(values[5], out decimal indexFValue) &&
                                                    decimal.TryParse(values[6], out decimal indexGValue) &&
                                                    decimal.TryParse(values[3], out decimal indexDValue))
                                                {
                                                    var clientCode = values[2];
                                                    decimal formulaValue = (indexEValue + indexFValue + indexGValue) - indexDValue;
                                                    if (clientMaxShortageRow.ContainsKey(clientCode))
                                                    {
                                                        cmData1[clientCode] = formulaValue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error processing FO_ClientLevelCollaterals file {cmFile1}: {ex.Message}");
                            }
                        }
                        var fc02Data = new Dictionary<string, decimal>();
                        string[] fc02Files = Directory.GetFiles(Server.MapPath("~/UploadedFiles"), "F_CC02_14697_*.csv");

                        foreach (string fc02Data1 in fc02Files)
                        {
                            try
                            {
                                using (var reader = new StreamReader(fc02Data1))
                                {
                                    bool isFirstRow = true;
                                    while (!reader.EndOfStream)
                                    {
                                        var line = reader.ReadLine();
                                        var values = line.Split(',');

                                        if (isFirstRow)
                                        {
                                            isFirstRow = false;
                                            continue;
                                        }

                                        if (values.Length > 6 &&
                                            decimal.TryParse(values[5], out decimal indexFValue) &&
                                            decimal.TryParse(values[6], out decimal indexGValue))
                                        {
                                            var clientCode = values[1];
                                            decimal cc02Value = indexFValue - indexGValue;

                                            if (!fc02Data.ContainsKey(clientCode))
                                            {
                                                fc02Data[clientCode] = cc02Value;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error processing C_CC02_14697 file {fc02Data}: {ex.Message}");
                            }
                        }


                        var cc02Data = new Dictionary<string, decimal>();
                        string[] cc02Files = Directory.GetFiles(Server.MapPath("~/UploadedFiles"), "C_CC02_14697_*.csv");

                        foreach (string cc02File in cc02Files)
                        {
                            try
                            {
                                using (var reader = new StreamReader(cc02File))
                                {
                                    bool isFirstRow = true;
                                    while (!reader.EndOfStream)
                                    {
                                        var line = reader.ReadLine();
                                        var values = line.Split(',');

                                        if (isFirstRow)
                                        {
                                            isFirstRow = false;
                                            continue;
                                        }

                                        if (values.Length > 6 &&
                                            decimal.TryParse(values[5], out decimal indexFValue) &&
                                            decimal.TryParse(values[6], out decimal indexGValue))
                                        {
                                            var clientCode = values[1];
                                            decimal cc02Value = indexFValue - indexGValue;
                                            if (!cc02Data.ContainsKey(clientCode))
                                            {
                                                cc02Data[clientCode] = cc02Value;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error processing C_CC02_14697 file {cc02File}: {ex.Message}");
                            }
                        }
                    foreach (var clientCode in clientMaxShortageRow.Keys)
                    {
                        if (decimal.TryParse(clientCode, out _))
                        {
                            continue;
                        }

                        if (clientMaxShortageRow.TryGetValue(clientCode, out var maxShortageRow))
                        {
                            for (int col = 0; col < headers2.Length - 5; col++)
                            {
                                if (col < maxShortageRow.Length)
                                {
                                    sheet.Cells[currentRow, col + 1].Value = maxShortageRow[col];
                                    sheet.Cells[currentRow, col + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                }
                            }

                            string timeValue = maxShortageRow.Length > 1 ? maxShortageRow[1] : string.Empty;
                            int excessValue = CalculateExcessValue(timeValue);

                            sheet.Cells[currentRow, headers2.Length - 6].Value = excessValue;
                            sheet.Cells[currentRow, headers2.Length - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                            int fileIndex = excessValue - 1;
                            if (fileIndex >= 0 && fileIndex < csa03Data.Count)
                            {
                                var csa03FileData = csa03Data[fileIndex];
                                if (csa03FileData.TryGetValue(clientCode, out decimal csa03Value))
                                {
                                    sheet.Cells[currentRow, headers2.Length - 5].Value = csa03Value;
                                    sheet.Cells[currentRow, headers2.Length - 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                }

                                var fsa03FileData = fsa03Data[fileIndex];
                                if (fsa03FileData.TryGetValue(clientCode, out decimal fsa03Value))
                                {
                                    sheet.Cells[currentRow, headers2.Length - 4].Value = fsa03Value;
                                    sheet.Cells[currentRow, headers2.Length - 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                }
                            }
                            if (cmData.TryGetValue(clientCode, out decimal cmValue))
                            {
                                sheet.Cells[currentRow, headers2.Length - 3].Value = cmValue;
                                sheet.Cells[currentRow, headers2.Length - 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            }


                            if (cmData1.TryGetValue(clientCode, out decimal cmValue1))
                            {

                                sheet.Cells[currentRow, headers2.Length - 2].Value = cmValue1;
                                sheet.Cells[currentRow, headers2.Length - 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            }


                            if (cc02Data.TryGetValue(clientCode, out decimal cc02Value))
                            {

                                sheet.Cells[currentRow, headers2.Length - 1].Value = cc02Value;
                                sheet.Cells[currentRow, headers2.Length - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            }

                            if (fc02Data.TryGetValue(clientCode, out decimal Fc02Value))
                            {

                                sheet.Cells[currentRow, headers2.Length].Value = Fc02Value;
                                sheet.Cells[currentRow, headers2.Length].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            }


                            currentRow++;
                        }
                      
                    }




                    int CalculateExcessValue(string timeValue)
                        {
                            if (DateTime.TryParse(timeValue, out DateTime time))
                            {
                                if (time >= new DateTime(time.Year, time.Month, time.Day, 10, 0, 0) && time < new DateTime(time.Year, time.Month, time.Day, 11, 0, 0))
                                {
                                    return 1;
                                }
                                else if (time >= new DateTime(time.Year, time.Month, time.Day, 11, 0, 0) && time < new DateTime(time.Year, time.Month, time.Day, 13, 0, 0))
                                {
                                    return 2;
                                }
                                else if (time >= new DateTime(time.Year, time.Month, time.Day, 13, 0, 0) && time < new DateTime(time.Year, time.Month, time.Day, 15, 0, 0))
                                {
                                    return 3;
                                }
                                else if (time >= new DateTime(time.Year, time.Month, time.Day, 15, 0, 0) && time < new DateTime(time.Year, time.Month, time.Day, 18, 0, 0))
                                {
                                    return 4;
                                }
                                else if (time >= new DateTime(time.Year, time.Month, time.Day, 18, 0, 0) && time <= new DateTime(time.Year, time.Month, time.Day, 20, 0, 0))
                                {
                                    return 6;
                                }
                                else if (time >= new DateTime(time.Year, time.Month, time.Day, 20, 0, 0) && time <= new DateTime(time.Year, time.Month, time.Day, 21, 59, 0))
                                {
                                    return 7;
                                }
                                else
                                {
                                    return 8;
                                }
                            }
                            return 0;
                        }


                        string excelFileName = "CombinedReport.xlsx";
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + excelFileName);
                        HttpContext.Current.Response.BinaryWrite(excel.GetAsByteArray());

                        try
                        {
                            string[] allFilePaths = Directory.GetFiles(Server.MapPath("~/UploadedFiles"));
                            foreach (string filePath in allFilePaths)
                            {
                                try
                                {
                                    File.Delete(filePath);
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Error deleting file {filePath}: {ex.Message}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error accessing files for deletion: {ex.Message}");
                        }

                        HttpContext.Current.Response.End();
                    }
                }
            }
        }
    }
    



 





