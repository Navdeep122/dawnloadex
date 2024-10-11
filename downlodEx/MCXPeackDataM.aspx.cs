using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace downlodEx
{
    public partial class MCXPeackDataM : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFiles)
            {
                string uploadPath = Server.MapPath("~/UploadedFiles1");
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
                    string[] headers = { "Date", "Time", "CM ID", "TM ID", "Client Code", "Span", "ELM", "Additional Margin", "Total Margin", "Cash Colleteral", "Colleteral", "Shortage", "Excess" };

                    for (int col = 0; col < headers.Length; col++)
                    {
                        sheet.Cells[currentRow, col + 1].Value = headers[col];
                        sheet.Cells[currentRow, col + 1].Style.Font.Bold = true;
                        sheet.Cells[currentRow, col + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    }
                    currentRow++;

                    string[] filePaths = Directory.GetFiles(Server.MapPath("~/UploadedFiles1"));

                    foreach (string filePath in filePaths)
                    {
                        try
                        {
                            if (Path.GetExtension(filePath).Equals(".CSV", StringComparison.OrdinalIgnoreCase))
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
                                        for (int col = 0; col < headers.Length - 1; col++)
                                        {
                                            if (col < values.Length)
                                            {
                                                sheet.Cells[currentRow, col + 1].Value = values[col];
                                            }
                                            else
                                            {
                                                sheet.Cells[currentRow, col + 1].Value = string.Empty;
                                            }

                                            sheet.Cells[currentRow, col + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                        }

                                      
                                        string timeValue = values[1]; 
                                        int excessValue = 0;

                                        if (DateTime.TryParse(timeValue, out DateTime time))
                                        {
                                            if (time >= new DateTime(time.Year, time.Month, time.Day, 10, 0, 0) && time < new DateTime(time.Year, time.Month, time.Day, 11, 0, 0))
                                            {
                                                excessValue = 1;
                                            }
                                            else if (time >= new DateTime(time.Year, time.Month, time.Day, 11, 0, 0) && time < new DateTime(time.Year, time.Month, time.Day, 12, 0, 0))
                                            {
                                                excessValue = 2;
                                            }
                                            else if (time >= new DateTime(time.Year, time.Month, time.Day, 13, 0, 0) && time < new DateTime(time.Year, time.Month, time.Day, 14, 0, 0))
                                            {
                                                excessValue = 3;
                                            }
                                            else if (time >= new DateTime(time.Year, time.Month, time.Day, 15, 0, 0) && time < new DateTime(time.Year, time.Month, time.Day, 17, 0, 0))
                                            {
                                                excessValue = 4;
                                            }
                                            else if (time >= new DateTime(time.Year, time.Month, time.Day, 18, 0, 0) && time < new DateTime(time.Year, time.Month, time.Day, 19, 0, 0))
                                            {
                                                excessValue = 6;
                                            }
                                            else if (time >= new DateTime(time.Year, time.Month, time.Day, 20, 0, 0) && time < new DateTime(time.Year, time.Month, time.Day, 21, 0, 0))
                                            {
                                                excessValue = 7;
                                            }
                                            else
                                            {
                                                excessValue = 8;
                                            }
                                        }

                                        sheet.Cells[currentRow, headers.Length].Value = excessValue;
                                        sheet.Cells[currentRow, headers.Length].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                        currentRow++;
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

                
                    string[] headers2 = { "Date", "Time", "CM ID", "TM ID", "Client Code", "Span", "ELM", "Additional Margin", "Total Margin", "Cash Colleteral", "Colleteral", "Shortage", "Excess","" };

                
                    for (int col = 0; col < headers2.Length; col++)
                    {
                        sheet.Cells[currentRow, col + 1].Value = headers2[col];
                        sheet.Cells[currentRow, col + 1].Style.Font.Bold = true;
                        sheet.Cells[currentRow, col + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    }
                    currentRow++;

                  
                    string[] filePaths2 = Directory.GetFiles(Server.MapPath("~/UploadedFiles1"));

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

                                        if (values.Length > 11 && !string.IsNullOrEmpty(values[11]) && decimal.TryParse(values[11], out decimal columnLValue) && columnLValue != 0)
                                        {
                                            for (int col = 0; col < headers2.Length - 1; col++)
                                            {
                                                if (col < values.Length)
                                                {
                                                    sheet.Cells[currentRow, col + 1].Value = values[col];
                                                }
                                                else
                                                {
                                                    sheet.Cells[currentRow, col + 1].Value = string.Empty;
                                                }

                                                sheet.Cells[currentRow, col + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                            }

                                            string timeValue = values[1];
                                            int excessValue = 0;

                                            if (DateTime.TryParse(timeValue, out DateTime time))
                                            {
                                                if (time >= new DateTime(time.Year, time.Month, time.Day, 10, 0, 0) && time < new DateTime(time.Year, time.Month, time.Day, 11, 0, 0))
                                                {
                                                    excessValue = 1;
                                                }
                                                else if (time >= new DateTime(time.Year, time.Month, time.Day, 11, 0, 0) && time < new DateTime(time.Year, time.Month, time.Day, 12, 0, 0))
                                                {
                                                    excessValue = 2;
                                                }
                                                else if (time >= new DateTime(time.Year, time.Month, time.Day, 13, 0, 0) && time < new DateTime(time.Year, time.Month, time.Day, 14, 0, 0))
                                                {
                                                    excessValue = 3;
                                                }
                                                else if (time >= new DateTime(time.Year, time.Month, time.Day, 15, 0, 0) && time < new DateTime(time.Year, time.Month, time.Day, 17, 0, 0))
                                                {
                                                    excessValue = 4;
                                                }
                                                else if (time >= new DateTime(time.Year, time.Month, time.Day, 18, 0, 0) && time < new DateTime(time.Year, time.Month, time.Day, 19, 0, 0))
                                                {
                                                    excessValue = 6;
                                                }
                                                else if (time >= new DateTime(time.Year, time.Month, time.Day, 20, 0, 0) && time < new DateTime(time.Year, time.Month, time.Day, 21, 0, 0))
                                                {
                                                    excessValue = 7;
                                                }
                                                else
                                                {
                                                    excessValue = 8;
                                                }
                                            }

                                            sheet.Cells[currentRow, headers2.Length - 1].Value = excessValue;
                                            sheet.Cells[currentRow, headers2.Length - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                            currentRow++;
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
                    string excelFileName = "CombinedReport.xlsx";
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + excelFileName);
                    HttpContext.Current.Response.BinaryWrite(excel.GetAsByteArray());
                    HttpContext.Current.Response.End();
                }
            }
        }
    }
}
