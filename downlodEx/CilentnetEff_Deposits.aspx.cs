using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace downlodEx
{
    public partial class CilentnetEff_Deposits : System.Web.UI.Page
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
                try
                {
                    // Check file extension
                    string fileExtension = Path.GetExtension(FileUpload1.FileName).ToLower();
                    if (fileExtension != ".csv")
                    {
                        StatusLabel.Text = "Please upload a valid .csv file.";
                        return;
                    }

                    // Save the uploaded file to a temporary location
                    string filePath = Path.Combine(Server.MapPath("~/App_Data"), FileUpload1.FileName);
                    FileUpload1.SaveAs(filePath);

                    // Load the CSV file
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Client/CP Code");
                    dataTable.Columns.Add("Total Cash Component");

                    try
                    {
                        using (StreamReader sr = new StreamReader(filePath))
                        {
                            string line;
                            bool isFirstLine = true;

                            while ((line = sr.ReadLine()) != null)
                            {
                                if (isFirstLine)
                                {
                                    isFirstLine = false; // Skip header line
                                    continue;
                                }

                                string[] values = line.Split(',');
                                DataRow dataRow = dataTable.NewRow();
                                dataRow["Client/CP Code"] = values[2]; // Column A
                                dataRow["Total Cash Component"] = values[5]; // Column B
                                dataTable.Rows.Add(dataRow);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        StatusLabel.Text = "Error reading CSV file: " + ex.Message;
                        return;
                    }

                    // Convert DataTable to CSV
                    string csv = DataTableToCsv(dataTable);

                    // Provide the CSV file for download
                    Response.Clear();
                    Response.ContentType = "text/csv";
                    Response.AddHeader("content-disposition", $"attachment; filename={uploadedFileName}");
                    Response.Write(csv);
                    Response.End();
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

        private string DataTableToCsv(DataTable dataTable)
        {
            using (StringWriter csvString = new StringWriter())
            {
                // Write the column headers
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    csvString.Write(dataTable.Columns[i]);
                    if (i < dataTable.Columns.Count - 1)
                    {
                        csvString.Write(",");
                    }
                }
                csvString.WriteLine();

                // Write the data rows
                foreach (DataRow row in dataTable.Rows)
                {
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        csvString.Write(row[i].ToString());
                        if (i < dataTable.Columns.Count - 1)
                        {
                            csvString.Write(",");
                        }
                    }
                    csvString.WriteLine();
                }

                return csvString.ToString();
            }
        }
    }
}
    
