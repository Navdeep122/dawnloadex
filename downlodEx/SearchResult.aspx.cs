using ClosedXML.Excel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace downlodEx
{
    public partial class SearchResult : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Common common = new Common();
                string userId = common.Uid;
                string password = common.Pass;
            }

        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string uploadedFileName = FileUpload1.FileName;
                string fileExtension = Path.GetExtension(uploadedFileName).ToLower();

                if (fileExtension != ".xlsx")
                {
                    StatusLabel.Text = "Please upload an .xlsx file.";
                    return;
                }

                try
                {
                    
                    string filePath = Path.Combine(Server.MapPath("~/App_Data"), uploadedFileName);
                    FileUpload1.SaveAs(filePath);
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("UCC Code");
                    dataTable.Columns.Add("Allocation at EOD");

                    try
                    {
                        using (XLWorkbook workbook = new XLWorkbook(filePath))
                        {
                            var worksheet = workbook.Worksheet(1); 
                            var rows = worksheet.RowsUsed(); 

                            int rowIndex = 0;
                            foreach (var row in rows)
                            {
                                rowIndex++;
                                if (rowIndex <= 2)
                                {
                                    continue; 
                                }

                                DataRow dataRow = dataTable.NewRow();
                                dataRow["UCC Code"] = row.Cell(6).GetValue<string>(); 
                                dataRow["Allocation at EOD"] = row.Cell(13).GetValue<string>();
                                dataTable.Rows.Add(dataRow);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        StatusLabel.Text = "Error reading Excel file: " + ex.Message;
                        return;
                    }

                    
                    using (XLWorkbook workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add(dataTable, "Sheet1");


                       
                        foreach (var row in worksheet.RowsUsed())
                        {
                            foreach (var cell in row.Cells())
                            {
                                cell.Style.Fill.BackgroundColor = XLColor.White;
                                cell.Style.Font.FontColor = XLColor.Black;

                                cell.Style.Border.OutsideBorder = XLBorderStyleValues.None;                            }
                        }

                        var headerRow = worksheet.Row(1);
                        foreach (var cell in headerRow.Cells())
                        {

                            cell.Style.Fill.BackgroundColor = XLColor.White;
                            cell.Style.Font.FontColor = XLColor.Gray;
                        }

                        using (MemoryStream stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            byte[] fileBytes = stream.ToArray();

                            Response.Clear();
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", $"attachment; filename={Path.GetFileNameWithoutExtension(uploadedFileName)}_processed.xlsx");
                            Response.BinaryWrite(fileBytes);
                            Response.End();
                        }
                    }
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Error: " + ex.Message;
                }
            }
            else
            {
                StatusLabel.Text = "Please upload a file.";
            }
        }
    }
}


