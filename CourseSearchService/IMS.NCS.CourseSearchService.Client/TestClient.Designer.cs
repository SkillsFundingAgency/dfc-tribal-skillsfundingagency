namespace IMS.NCS.CourseSearchService.Client
{
    partial class TestClient
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtResult = new System.Windows.Forms.RichTextBox();
            this.btnCourseList = new System.Windows.Forms.Button();
            this.btnCourseDetail = new System.Windows.Forms.Button();
            this.btnProviderSearch = new System.Windows.Forms.Button();
            this.btnProviderDetails = new System.Windows.Forms.Button();
            this.btnGetCategories = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(24, 32);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(531, 201);
            this.txtResult.TabIndex = 0;
            this.txtResult.Text = "";
            // 
            // btnCourseList
            // 
            this.btnCourseList.Location = new System.Drawing.Point(24, 249);
            this.btnCourseList.Name = "btnCourseList";
            this.btnCourseList.Size = new System.Drawing.Size(111, 23);
            this.btnCourseList.TabIndex = 1;
            this.btnCourseList.Text = "Get Course List";
            this.btnCourseList.UseVisualStyleBackColor = true;
            this.btnCourseList.Click += new System.EventHandler(this.btnCourseList_Click);
            // 
            // btnCourseDetail
            // 
            this.btnCourseDetail.Location = new System.Drawing.Point(24, 289);
            this.btnCourseDetail.Name = "btnCourseDetail";
            this.btnCourseDetail.Size = new System.Drawing.Size(111, 23);
            this.btnCourseDetail.TabIndex = 2;
            this.btnCourseDetail.Text = "Get Course Detail";
            this.btnCourseDetail.UseVisualStyleBackColor = true;
            this.btnCourseDetail.Click += new System.EventHandler(this.btnCourseDetail_Click);
            // 
            // btnProviderSearch
            // 
            this.btnProviderSearch.Location = new System.Drawing.Point(186, 249);
            this.btnProviderSearch.Name = "btnProviderSearch";
            this.btnProviderSearch.Size = new System.Drawing.Size(117, 23);
            this.btnProviderSearch.TabIndex = 3;
            this.btnProviderSearch.Text = "Get Providers";
            this.btnProviderSearch.UseVisualStyleBackColor = true;
            this.btnProviderSearch.Click += new System.EventHandler(this.btnProviderSearch_Click);
            // 
            // btnProviderDetails
            // 
            this.btnProviderDetails.Location = new System.Drawing.Point(186, 289);
            this.btnProviderDetails.Name = "btnProviderDetails";
            this.btnProviderDetails.Size = new System.Drawing.Size(117, 23);
            this.btnProviderDetails.TabIndex = 4;
            this.btnProviderDetails.Text = "Get Provider Details";
            this.btnProviderDetails.UseVisualStyleBackColor = true;
            this.btnProviderDetails.Click += new System.EventHandler(this.btnProviderDetails_Click);
            // 
            // btnGetCategories
            // 
            this.btnGetCategories.Location = new System.Drawing.Point(360, 249);
            this.btnGetCategories.Name = "btnGetCategories";
            this.btnGetCategories.Size = new System.Drawing.Size(108, 23);
            this.btnGetCategories.TabIndex = 5;
            this.btnGetCategories.Text = "Get Category List";
            this.btnGetCategories.UseVisualStyleBackColor = true;
            this.btnGetCategories.Click += new System.EventHandler(this.btnGetCategories_Click);
            // 
            // TestClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 411);
            this.Controls.Add(this.btnGetCategories);
            this.Controls.Add(this.btnProviderDetails);
            this.Controls.Add(this.btnProviderSearch);
            this.Controls.Add(this.btnCourseDetail);
            this.Controls.Add(this.btnCourseList);
            this.Controls.Add(this.txtResult);
            this.Name = "TestClient";
            this.Text = "TestClient";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtResult;
        private System.Windows.Forms.Button btnCourseList;
        private System.Windows.Forms.Button btnCourseDetail;
        private System.Windows.Forms.Button btnProviderSearch;
        private System.Windows.Forms.Button btnProviderDetails;
        private System.Windows.Forms.Button btnGetCategories;
    }
}