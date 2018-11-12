IF NOT EXISTS (SELECT TOP 1 1 FROM Content)
BEGIN

-- Default content for new pages
INSERT INTO [dbo].[Content]
           ([Version]
           ,[Path]
           ,[Title]
           ,[Body]
           ,[Scripts]
           ,[Styles]
           ,[Summary]
           ,[UserContext]
           ,[Embed]
           ,[RecordStatusId]
           ,[LanguageId]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]
           ,[ModifiedByUserId]
           ,[ModifiedDateTimeUtc])
     VALUES
           ( 1
           , 'DefaultContent'
           , 'Add a new page'
           , '<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. In tempus leo vitae nibh rutrum, et auctor nulla blandit. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nullam suscipit tincidunt sapien a consequat. Maecenas maximus consectetur lacus, ut molestie nunc dignissim vel. Interdum et malesuada fames ac ante ipsum primis in faucibus. Donec nulla nunc, facilisis sed iaculis nec, volutpat quis lectus. Praesent congue vehicula gravida.</p>
<p>Nam mattis diam at purus pellentesque tincidunt. Proin quis libero ultrices, placerat nibh quis, accumsan neque. Duis nec molestie diam. Mauris congue cursus egestas. Fusce rutrum varius turpis vel faucibus. Aliquam cursus luctus turpis, et consectetur augue. Nunc pulvinar nisl sit amet dui sodales tempus. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Ut est est, sodales sed dignissim et, porta et dui. Nunc rhoncus tortor sagittis ante congue, ut malesuada lorem lacinia. Sed eleifend, nunc a placerat interdum, nisi mauris malesuada massa, at maximus elit elit ac quam.</p>
<p>Phasellus in nibh non quam consectetur convallis non eu nisl. Integer eu dolor ex. Curabitur pellentesque arcu ut risus dignissim bibendum. Praesent viverra accumsan ex, eu interdum dolor. Nam elementum, diam eget finibus ornare, tellus eros pellentesque erat, ac pellentesque orci nulla ut leo. Interdum et malesuada fames ac ante ipsum primis in faucibus. Etiam ac molestie nibh.</p>
<p>Sed lobortis, neque ac gravida rhoncus, urna purus dapibus est, sed fermentum nulla purus quis mi. Nam hendrerit metus non ligula condimentum, viverra pharetra justo tempor. Ut in elementum massa, accumsan vulputate eros. Nam quis porta lorem. Ut vulputate risus est, quis laoreet risus vestibulum id. Nulla in tempor ex. Vivamus sed feugiat nunc. Etiam viverra nunc eget risus fermentum facilisis. Curabitur bibendum odio quis tempus elementum. Quisque sed diam sit amet tortor blandit scelerisque a sed orci.</p>
<p>Sed id nunc orci. Sed quis sagittis ligula. Nulla auctor erat sit amet orci congue venenatis. Quisque id lacus semper, consequat ipsum eget, mollis enim. Cras molestie, velit nec accumsan scelerisque, nisi orci tristique nunc, vitae tristique velit leo in mi. Nam vitae faucibus lorem. Mauris ut vehicula libero, eu efficitur sem. Sed finibus elementum nunc, nec semper sapien eleifend in. Integer tellus metus, bibendum vel diam a, porttitor maximus odio. Quisque placerat volutpat suscipit. Mauris ac lorem vitae est tristique pharetra molestie sed velit. Quisque massa nisi, euismod non lacus ac, feugiat dapibus ipsum. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Vestibulum ac nulla id mauris tempor egestas. Integer tincidunt placerat eros, et venenatis massa congue id.</p>'
           , null
           , null
           , 'Default content for new pages'
           , 127 /* All user contexts */
           , 1 /* Embedded */
           , 2 /* Live */
           , 1 /* English */
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
		   )

-- HomeSA
INSERT INTO [dbo].[Content]
           ([Version]
           ,[Path]
           ,[Title]
           ,[Body]
           ,[Scripts]
           ,[Styles]
           ,[Summary]
           ,[UserContext]
           ,[Embed]
           ,[RecordStatusId]
           ,[LanguageId]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]
           ,[ModifiedByUserId]
           ,[ModifiedDateTimeUtc])
     VALUES
           ( 1
           , 'Home'
           , null
           , '<div class="row">
    <div class="col-md-12">
        <h1>Welcome to the Course Directory Provider Portal</h1>
        <p class="lead">The Course Directory Provider Portal is provided by the Skills Funding Agency for providers to view and update their Course Directory information. The Course Directory stores both 16-18 and 19+ English provisions. The data uploaded to the Portal can be viewed on the
            <a title="National Careers Service website" target="_blank" href="https://nationalcareersservice.direct.gov.uk/advice/courses/Pages/default.aspx">National Careers Service web site</a>.</p>
    </div>
</div>

<div class="row">
    <div class="col-md-12">
<a class="btn btn-primary btn-lg center btn-gap" href="/Account/Login">Log in</a>    </div>
</div>

<div class="row">
    <div class="col-md-4"></div>
    <div class="col-md-4"></div>
    <div class="col-md-4"></div>
</div>

<div class="row">
    <div class="col-md-12"></div>    
</div>'
           , null
           , null
           , 'Site home page without Secure Access log in'
           , 16 /* Unauthenticated */
           , 1 /* Embedded */
           , 2 /* Live */
           , 1 /* English */
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
		   )

-- HomeSA
INSERT INTO [dbo].[Content]
           ([Version]
           ,[Path]
           ,[Title]
           ,[Body]
           ,[Scripts]
           ,[Styles]
           ,[Summary]
           ,[UserContext]
           ,[Embed]
           ,[RecordStatusId]
           ,[LanguageId]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]
           ,[ModifiedByUserId]
           ,[ModifiedDateTimeUtc])
     VALUES
           ( 1
           , 'HomeSA'
           , null
           , '<div class="row">
    <div class="col-md-12">
        <h1>Welcome to the Course Directory Provider Portal</h1>
        <p class="lead">The Course Directory Provider Portal is provided by the Skills Funding Agency for providers to view and update their Course Directory information. The Course Directory stores both 16-18 and 19+ English provisions. The data uploaded to the Portal can be viewed on the
            <a title="National Careers Service website" target="_blank" href="https://nationalcareersservice.direct.gov.uk/advice/courses/Pages/default.aspx">National Careers Service web site</a>.</p>
    </div>
</div>

<div class="row">
    <div class="col-md-12">
				<a class="btn btn-primary btn-lg center btn-gap" href="/Account/Login">Log in using a Portal Account</a><a class="btn btn-primary btn-lg center btn-gap" href="/SA/Login" id="loginLink">Log in with DfE Secure Access</a>            <div id="login-help">
                <div id="login-help-title">
                    <h3>Not sure which login method to use?</h3>
                </div>
                <div id="login-help-body">
                    <h4>DfE Secure Access</h4>
                    <p>If you are a Secure Access user click on the button labelled &quot;Log in with DfE Secure Access&quot;. This will take you to the Secure Access site where you can log in and access the Provider Portal.  In Secure Access the Provider Portal will be called &quot;Post-16 Portal&quot;. Clicking on the link for the Post-16 Portal will take you to your Portal homepage.</p>
                    <h4>Portal Account</h4>
                    <p>If you are a SFA or an EFA provider not using Secure Access click on the button labelled &quot;Log in using a Portal Account&quot;. Enter your email address and password to be taken to your Portal homepage.</p>
                </div>
            </div>
    </div>
</div>

<div class="row">
    <div class="col-md-4"></div>
    <div class="col-md-4"></div>
    <div class="col-md-4"></div>
</div>

<div class="row">
    <div class="col-md-12"></div>    
</div>'
           , null
           , null
           , 'Site home page with Secure Access log in'
           , 16 /* Unauthenticated */
           , 1 /* Embedded */
           , 2 /* Live */
           , 1 /* English */
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
		   )


-- HomeInvalidUser
INSERT INTO [dbo].[Content]
           ([Version]
           ,[Path]
           ,[Title]
           ,[Body]
           ,[Scripts]
           ,[Styles]
           ,[Summary]
           ,[UserContext]
           ,[Embed]
           ,[RecordStatusId]
           ,[LanguageId]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]
           ,[ModifiedByUserId]
           ,[ModifiedDateTimeUtc])
     VALUES
           ( 1
           , 'HomeInvalidUser'
           , null
           , '<div class="row">
<div class="col-md-12">
<h1>Welcome to the Course Directory Provider Portal</h1>
<p>Your account is not currently linked to a Provider or an Organisation, please contact the Course Directory Support Team on <a href="tel:08448115073">0844 811 5073</a> or <a href="mailto:support@coursedirectoryproviderportal.org.uk" target="_blank">support@coursedirectoryproviderportal.org.uk</a>.</p>
</div>
</div>'
           , null
           , null
           , 'Site home page for users with invalid accounts'
           , 8 /* AuthenticatedNoAccess */
           , 1 /* Embedded */
           , 2 /* Live */
           , 1 /* English */
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
		   )


-- Footer

INSERT INTO [dbo].[Content]
           ([Version]
           ,[Path]
           ,[Title]
           ,[Body]
           ,[Scripts]
           ,[Styles]
           ,[Summary]
           ,[UserContext]
           ,[Embed]
           ,[RecordStatusId]
           ,[LanguageId]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]
           ,[ModifiedByUserId]
           ,[ModifiedDateTimeUtc])
     VALUES
           ( 1
           , 'Footer'
           , null
           , '<p>
    <a href="/Help">Help</a> |
    <a href="/Help/TermsAndConditions">Terms and Conditions</a> |
	<a href="/Help/Cookies">Cookie Information</a>
</p>'
           , null
           , null
           , 'Initial content'
           , 127 /* All user contexts */
           , 1 /* Embedded */
           , 2 /* Live */
           , 1 /* English */
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
		   )

-- /Help and /Help/Index

INSERT INTO [dbo].[Content]
           ([Version]
           ,[Path]
           ,[Title]
           ,[Body]
           ,[Scripts]
           ,[Styles]
           ,[Summary]
           ,[UserContext]
           ,[Embed]
           ,[RecordStatusId]
           ,[LanguageId]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]
           ,[ModifiedByUserId]
           ,[ModifiedDateTimeUtc])
     VALUES
           ( 1
           , 'Help'
           , 'Help'
           , '<p><strong>This page contains information to help use the Provider Portal and Bulk Upload. Click the name of the document to download it.</strong> Documents are in PDF format, which can be viewed using the free <a target="_blank" href="http://get.adobe.com/reader/" >Adobe Acrobat Reader</a>.</p>
<p>If you need further help, please contact the Course Directory Support Team on <a href="tel:08448115073">0844 811 5073</a> or <a href="mailto:support@coursedirectoryproviderportal.org.uk">support@coursedirectoryproviderportal.org.uk</a>,<br />
 or the DfE Support Team on <a href="tel:08448115028">0844 811 5028</a> or <a href="mailto:dfe.support@coursedirectoryproviderportal.org.uk">dfe.support@coursedirectoryproviderportal.org.uk</a></p>

<p><strong><a target="_blank" href="/Content/Help/Introduction to the Course Directory.pdf" >Introduction to the Course Directory</a></strong></p>
<p><strong><a target="_blank" href="http://skillsfundingagency.bis.gov.uk/providers/allthelatest/providerupdate/" >Skills Funding Agency Update</a></strong> - a weekly newsletter for providers (external link, you will be taken to the Skills Funding Agency website)</p>
<p><strong><a target="_blank" href="/Content/Help/Course Directory Provider Portal Help Guide 2015.pdf" >Help Guide for SFA Funded Providers</a></strong> - this document gives instructions on how to use each part of the Provider Portal and Bulk Upload.</p>
<p><strong><a target="_blank" href="/Content/Help/Course Directory Provider Portal EFA Funded Providers Help Guide 2015.pdf" >Help Guide for EFA Funded Providers</a></strong> - this document gives instructions on how to use each part of the Provider Portal and Bulk Upload.</p>
<p><strong><a target="_blank" href="/Content/Help/Course Directory Provider Portal EFA Funded Providers Short Help Guide.pdf" >Short Help Guide for EFA Funded Providers</a></strong> - this document gives instructions on how to use each part of the Provider Portal and Bulk Upload.</p>
<p><strong><a target="_blank" href="/Content/Help/DfE Portal guide for new users.pdf" >Quick Guide for Users New To Providing Data</a></strong> - this document gives instructions on how to use each part of the Provider Portal and Bulk Upload.</p>
<p><strong><a target="_blank" href="/Content/Help/Course Directory Provider Portal Data Standards.pdf" >Provider Data Standards</a></strong> - this document has the requirements and examples for every field available in the Course Directory.</p>
<p><strong><a href="/Content/Help/Valid Regions For Bulk Upload.xls" >Valid Regions for Bulk Upload</a></strong> - this Excel spreadsheet contains all the valid regions you can attach opportunities to. Please enter the entire string, including the ‘|’ character.</p>
<p><strong><a href="/Help/BulkUpload">Latest Bulk Upload Template and beginner information</a></strong> - a short guide on getting started with bulk upload, including the template for the format of the CSV file.</p>
<p><strong><a target="_blank" href="/Content/Help/LDCS_v3_Vol1.pdf" >The Learning Directory Classification System (volume 1)</a></strong> - The nationally approved subject classification system for Learning Information Databases.</p>
<p><strong><a target="_blank" href="/Content/Help/LDCS_v3_Vol2.pdf" >The Learning Directory Classification System (volume 2)</a></strong></p>
<p><strong><a target="_blank" href="/Content/Help/Editing CSV files in Excel - Leading Zeros.pdf" >Troubleshooting</a></strong> - the problem of leading zeros deleted by the default cell formatting when editing CSV files in Excel</p>
<p><strong><a target="_blank" href="/Content/Help/Course Directory Provider Dashboard Reference Guide.pdf" >Provider Dashboard Reference Guide</a></strong> - explains all the information that appears on the Provider Dashboard.</p>
<p><strong><a href="/Help/TopTips">Top Tips</a></strong> - a checklist to help you get high quality data on the portal.</p>
<p><strong><a target="_blank" href="/Content/Help/Skills Funding Agency Course Directory FAQs.pdf">Skills Funding Agency Course Directory Provider Portal FAQs</a></strong> - Frequently asked questions for SFA funded providers.</p>
<p><strong><a target="_blank" href="/Content/Help/DfE Course Directory Provider Portal FAQs.pdf">DfE Course Directory Provider Portal FAQs</a></strong> - Frequently asked questions for DfE/EFA funded providers.</p>

<h3>Webinars</h3>
<p>The webinars are video files you can download to your computer to watch later.</p>
<p><strong><a href="/Content/Webinars/Using_the_Portal.mp4">Webinar 1</a></strong> - Using the Portal</p>
<p><strong><a href="/Content/Webinars/Using_the_Dashboard_and_Uploading_Courses.mp4">Webinar 2</a></strong> - Using the Dashboard and Uploading Courses</p>
<p><strong><a href="/Content/Webinars/Bulk_Upload_and_CSV_Template.mp4">Webinar 3</a></strong> - Bulk Upload and CSV template</p>
'
           , null
           , null
           , 'Initial content'
           , 127 /* All user contexts */
           , 0 /* Embedded */
           , 2 /* Live */
           , 1 /* English */
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
		   )

-- /Help/BulkUpload

INSERT INTO [dbo].[Content]
           ([Version]
           ,[Path]
           ,[Title]
           ,[Body]
           ,[Scripts]
           ,[Styles]
           ,[Summary]
           ,[UserContext]
           ,[Embed]
           ,[RecordStatusId]
           ,[LanguageId]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]
           ,[ModifiedByUserId]
           ,[ModifiedDateTimeUtc])
     VALUES
           ( 1
           , 'Help/BulkUpload'
           , 'Detailed Instructions for Bulk Upload'
           , '<div id="divBulkUploadHelp">
    <p>Bulk upload allows you to easily send all your provision in a single file. The Comma Separated Values (CSV) format used is simply a series of rows and columns, and can be exported from many information management systems, or simply prepared using spreadsheet programs such as Microsoft Excel. Preparing your first bulk upload will take some work, but once you''ve set it up, it is a simple way to keep all your information up to date.</p>
    <p>If you’re a provider only uploading provision run by you, please note that each time you upload, <strong><i>all existing provision will be deleted and replaced with what you’ve uploaded</i></strong>. If you’re uncertain, you can always back up your existing provision using the “Download CSV” function on the Bulk Upload page. This gives you file containing all your provision in bulk upload format that can be uploaded at any time.</p>
    <p><strong>Organisations</strong></p>
    <p>If you’re a provider who is a member of one or more Organisations, you can upload provision that you run for that Organisation or promote under that Organisation’s brand. The Opportunity fields OFFERED_BY and DISPLAY_NAME are used for this. OFFERED_BY should be set to the ID of who has the contract with the Agency for this opportunity, and DISPLAY_NAME should be set to what name you wish learners to see on the National Careers Service site. If they are different, you can choose to have both searchable if you wish using BOTH_SEARCHABLE. You can see all Organisation IDs on the Organisations screen.</p>
    <p>Note if you do not include any provision for a particular Organisation in an upload file, any existing provision you run for that Organisation will be preserved. Only if at least one opportunity for that Organisation is included will your provision be affected. This also applies if you only upload provision offered by yourself directly &ndash; any existing provision for Organisations will be preserved. If you need to delete all your provision for a Organisation or for yourself, please contact the service desk.</p>
    <p>If you’re an Organisation superuser, you can upload provision for your member providers in the same way they do (if they have allowed you to do so), except you must preface each bulk upload file with the Provider ID. <a target="_blank" href="/Content/SampleData/multi_provider_template.csv">Click here to download an example</a>. All member Provider IDs can be viewed on the Organisation home screen. You can put one, some or all of your providers in a single file. </p>
    <h2>Getting started</h2>
    <p><strong>Step 1: Read the Data Standards &amp; Help Guide</strong></p>
    <p>The <a target="_blank" href="/Content/Help/Course Directory Provider Portal Data Standards.pdf">Data Standards</a> list the name and format of every piece of data used in the Course Directory. Not all fields are mandatory, but the more you supply, the more useful your data will be to learners. The <a target="_blank" href="/Content/Help/Course Directory Provider Portal Help Guide 2015.pdf">Help Guide</a> contains instructions on how to use Bulk Upload. For fields such as Study Mode, you will need to supply a code rather than the value itself. For example, instead of "Full Time", your file will contain "SM1", A complete list of fields requiring codes and the codes themselves can be found in Appendix 2 of the <a target="_blank" href="/Content/Help/Course Directory Provider Portal Help Guide 2015.pdf">Help Guide</a>.</p>
    <p><strong>Step 2: Understand the file format and create your file</strong></p>
    <p>Bulk upload files are in Comma Separated Values (CSV) format with a defined column and section order. You can review a <a target="_blank" href="/Content/SampleData/blank_file.csv">blank example with headers only</a> and an <a target="_blank" href="/Content/SampleData/csv_template_with_values.csv">example with test data</a>. This page always has the latest versions. Previous versions may not work. Take careful note of the following:</p>
    <div>
        <ul>
            <li>Your file must have four sections in the expected order - Provider, then Venues, then Courses and then Opportunities. If you’re an Organisation superuser, you simply repeat this format prefacing each Provider with their ID. Click here for an example.</li>
            <li>Headers for each section should read exactly as they are in the template, including the * where it is shown.</li>
            <li>Each section should have every item of that type: Venues section has ALL venues, Courses section has ALL courses, and so on.</li>
            <li>Each Venue and Course should have a unique identifier, stored in the VENUE_ID* and COURSE_ID* columns in the relevant section. Neither field is in the Data Standards, and will be discarded after the file is loaded - they''re simply to allow successful processing of your file. This may be any identifier you choose. If you’ve already got a unique identifier in your own system, use this; if not, simple numbers will do.</li>
            <li>To link an OPPORTUNITY to a venue, please put the venue''s ID in the VENUE_ID/REGION_NAME column. Alternatively, you may put a town, county or region name in this column. Please see the Help Guide for more information.</li>
            <li>The column OFFERED_BY is used to indicate who has the contract with the Agency the opportunity is run under &ndash; this can be the provider themselves or an Organisation of which the Provider is a member. If this column is left blank, the opportunity is assumed to be offered under the Provider’s direct contract with the Agency.</li>
            <li>The column DISPLAY_NAME is used to indicate which name the learner will see for this Opportunity. This can be the Provider themselves or an Organisation of which the Provider is a member. If this ID is different to the one in OFFERED_BY, you can further choose whether you want both names searchable by the learners using BOTH_SEARCHABLE.</li>
            <li><strong>All columns except OFFERED_BY, DISPLAY_NAME and BOTH_SEARCHABLE need to be present in the file (even if they have no data in them) and in the same order as the example files.</strong></li>
        </ul>
    </div>
    <p><strong>Step 3: Save &amp; upload your file</strong></p>
    <p>Your file may be named anything, but must have the extension .csv. Once uploaded it will begin processing immediately. The time to process the file will depend on the size of the file, complexity of the information and the time of day.</p>
    <p><strong>Step 4: Review errors and re-upload if necessary</strong></p>
    <p>If your file contains errors, the entire file will be rejected and no data will be published. You will likely have errors the first time you try an upload, and will likely need several attempts to get it exactly right. The errors are reported on the Provider Portal. The error list is broken down into Provider, Venue, Course and Opportunity errors and where possible a line number is given. You will be sent an email on the same day as the upload if it contains errors. You will not be sent an email the same day if your file is processed successfully without error. You will receive an email overnight to say your data has been published successfully to the Provider Portal and learner websites.</p>

    <p><a target="_parent" href="/BulkUpload">Return to the main bulk upload page.</a></p>
</div>'
           , null
           , '#divBulkUploadHelp a {
    color: red;
    font-weight: bold;
}'
           , 'Initial content'
           , 127 /* All user contexts */
           , 0 /* Embedded */
           , 2 /* Live */
           , 1 /* English */
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
		   )

-- /Help/TermsAndConditions

INSERT INTO [dbo].[Content]
           ([Version]
           ,[Path]
           ,[Title]
           ,[Body]
           ,[Scripts]
           ,[Styles]
           ,[Summary]
           ,[UserContext]
           ,[Embed]
           ,[RecordStatusId]
           ,[LanguageId]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]
           ,[ModifiedByUserId]
           ,[ModifiedDateTimeUtc])
     VALUES
           ( 1
           , 'Help/TermsAndConditions'
           , 'Terms and Conditions'
           , '<p>The Chief Executive of Skills Funding (the Chief Executive) operates the National Careers Service Course Directory Provider Portal (the Service) which is the publicly funded service enabling learning providers funded by the Skills Funding Agency or the Education Funding Agency to upload information related to themselves and the courses they provide.</p>

<p>Providers will contribute information related to Courses for individuals aged 16-19 years and for those aged 19 years and over.  This information can then be shared and re-used across Government providing valuable information to help people make informed choices about their future.</p>

<p>When using the National Career Service Course Directory Provider Portal you agree to these Terms and Conditions. Please:</p>

<ul>
    <li>Respect any patents, copyrights and trademarks. All rights are owned by the Chief Executive.</li>
</ul>

<p>The Chief Executive in delivering the Service may:</p>

<ul>
    <li>Collect personal information provided to us via the Provider Portal. This personal information will be processed in accordance with our security policy.</li>
    <li>The Chief Executive of the Skills Funding is registered with the Information Commissioner`s Office. The notification can be viewed on the ICO Data Controller Register at:<br/>
        <a href="https://ico.org.uk/about-the-ico/what-we-do/register-of-data-controllers" target="_blank">https://ico.org.uk/about-the-ico/what-we-do/register-of-data-controllers</a>
    </li>
    <li>Take action against you where your use of the service is deemed inappropriate or does not align to these terms and conditions. As a result, we may suspend or permanently remove your access to the service and any data you have submitted.</li>
</ul>

<p>The Chief Executive accepts no liability for:</p>

<ul>
    <li>Loss of access to the Provider Portal due to routine or emergency maintenance on the system, or due to excessive demands for the Service. The Service is offered 7 days per week 24 hours per day on a ''best endeavours'' basis.</li>
    <li>Loss of data to include both data sent to us via the Provider Portal and other data held by or on behalf of you.</li>
    <li>Delay or failure in receipt of data provided by you or to you.</li>
    <li>Damages arising from your use of the Provider Portal.</li>
    <li>The content and behaviour of external internet sites accessed from the Provider Portal.</li>
</ul>

<p>As a Provider you:</p>

<ul>
	<li>accept responsibility for information you provide to the Provider Portal. The Agency reserves the right to remove any information it does not deem suitable for the Service. </li>
	<li>should not use the Provider Portal for any unlawful purposes or to transmit or communicate any material which is obscene, offensive, blasphemous, pornographic, unlawful, threatening, menacing, abusive, and harmful (particularly to minor), invades privacy, defamatory and libellous. </li>
	<li>must not transmit any material which is likely to cause harm to the Agency or anyone else`s computer systems. This includes but is not limited to viruses, malicious code, worms, content data or other data purposely designed to cause defects, errors, malfunctions or corruption to any computer system. </li>
	<li>must not impersonate or falsely state or misrepresent your association with any entity or person including without limitation; the Agency, the Course Directory Service Providers or other third parties. </li>
	<li>must not collect, store or share any personal data relating to any visitors of the Provider Portal.  </li>
</ul>

<p>If you need further help, please contact the Course Directory Support Team on <a href="tel:0844 811 5073">0844 811 5073</a> or <a href="mailto:support@coursedirectoryproviderportal.org.uk">support@coursedirectoryproviderportal.org.uk</a>,or the DfE Support Team on <a href="tel:0844 811 5028">0844 811 5028</a> or <a href="mailto:dfe.support@coursedirectoryproviderportal.org.uk">dfe.support@coursedirectoryproviderportal.org.uk</a>.</p>
'
           , null
           , null
           , 'Default content for new pages'
           , 127 /* All user contexts */
           , 0 /* Embedded */
           , 2 /* Live */
           , 1 /* English */
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
		   )

-- Help/Cookies
INSERT INTO [dbo].[Content]
           ([Version]
           ,[Path]
           ,[Title]
           ,[Body]
           ,[Scripts]
           ,[Styles]
           ,[Summary]
           ,[UserContext]
           ,[Embed]
           ,[RecordStatusId]
           ,[LanguageId]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]
           ,[ModifiedByUserId]
           ,[ModifiedDateTimeUtc])
     VALUES
           ( 1
           , 'Help/Cookies'
           , 'Cookie Information'
           , '<p>When we provide services, we want to make them easy, useful and reliable. Where services are delivered on the internet, this sometimes involves placing small amounts of information on your device, for example, computer or mobile phone. These include small files known as cookies. They cannot be used to identify you personally.</p>

<p>These pieces of information are used to improve services for you through, for example:</p>

<ul>
	<li>enabling a service to recognise your device so you don''t have to give the same information several times during one task </li>
	<li>recognising that you may already have given a username and password so you don''t need to do it for every web page requested </li>
	<li>measuring how many people are using services, so they can be made easier to use and there''s enough capacity to ensure they are fast</li>
</ul>

<p>You can manage these small files yourself and learn more about them through <a href="http://www.direct.gov.uk/managingcookies" target="_blank">Internet browser cookies - what they are and how to manage them</a> (external link).</p>

<h3>Use of cookies by the Chief Executive of Skills Funding</h3>

<p>The service is provided on behalf of the Chief Executive of Skills Funding. Below is further information on the cookie name, how the cookie information is used and the length of time a cookie is active.</p>
<table class="table table-bordered">
    <thead>
        <tr>
            <th scope="col" style="width:20%">Cookie</th>
            <th scope="col">Name</th>
            <th scope="col" style="width:40%">Purpose</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Essential Site Cookie</td>
            <td>
                ASP.NET_SessionId<br/><br/>
                Example value:<br/>
                iyzmef0vns0gaa3qwvk2mjyr
            </td>
            <td>This cookie allows us to store basic information about your current session. This cookie is deleted when you close your browser.</td>
        </tr>
        <tr>
            <td>Essential Site Cookie</td>
            <td>
                __RequestVerificationToken<br/><br/>
                Example value:<br/>
                pTj1LK-jIMaLX4uQHBA-<wbr/>aUE2gu30kku2Fa7maDe7wzAQVbEZRNGgnlqVB-<wbr/>1pMnbQgqRddm9yYxsvm3UvRBNvBgzNzHbo1qCwjR_<wbr/>UBDVbLvM1
            </td>
            <td>This cookie is used to prevent CSRF forgery attacks.  This cookie is deleted when you close your browser.</td>
        </tr>
        <tr>
            <td>Essential Site Cookie</td>
            <td>
                AspNet.ApplicationCookie<br/><br/>
                Example value:<br/> 
                ZIuX3ZSWJXpTgYXVbB5c6mVZ1mHlB2<wbr/>LdsaNEEVXaucs_vYPpFq<wbr/>8iT83SxDOjV4R5dto6oH<wbr/>v53W7LfVpoABL2kRs54w<wbr/>cPVPgi1aKZqnkJrXriwC<wbr/>FyTlbuH8ODeTZiz5qmNq<wbr/>oaJMUYRBBGWG4MR-gyPb<wbr/>mT4bFd4RzQ9GBsZu4oCp<wbr/>nPWY_NteXQbP8CPDe6zI<wbr/>r3m96yjOajjExeDNcit_<wbr/>RrcVGmibafRMmr5ugmK-<wbr/>Cawg-Muy6Rna42ntyrlZ<wbr/>V_dwEfcasmMVYC094gqK<wbr/>qhsZUwjw4kxw3rKeN5E5<wbr/>OyNSaSJI_MVr6AANUS_H<wbr/>74kLg1SlL8sPHaqVbeS-<wbr/>8eV9rtcS2S8qZXpzsKdx<wbr/>CAp9qgeOlsRkKv995ISG<wbr/>IHYtNRS-wy5RM2z8hx_Q<wbr/>mZ4lSejQxTzPn04dBkgF<wbr/>AtKAgnseHDs3MwsGdTAe<wbr/>hKf_FCsZsh5pDFDr0uQn<wbr/>HNCUdkLlNysr44-2EW-E<wbr/>E7KIh-dbxYE4OFKZQ0JC<wbr/>t2OjUP-suifb5oPGkP31<wbr/>wtKlKTNlDSxsL6s
            </td>
            <td>This cookie allows us to keep users logged in during their session. This cookie is deleted when you close your browser.</td>
        </tr>
    </tbody>
</table>'
           , null
           , null
           , 'Initial content'
           , 127 /* All user contexts */
           , 0 /* Embedded */
           , 2 /* Live */
           , 1 /* English */
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
		   )

--/Help/TopTips

INSERT INTO [dbo].[Content]
           ([Version]
           ,[Path]
           ,[Title]
           ,[Body]
           ,[Scripts]
           ,[Styles]
           ,[Summary]
           ,[UserContext]
           ,[Embed]
           ,[RecordStatusId]
           ,[LanguageId]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]
           ,[ModifiedByUserId]
           ,[ModifiedDateTimeUtc])
     VALUES
           ( 1
           , 'Help/TopTips'
           , 'Top Tips'
           , '<h3>Top tips for good data on the Course Directory</h3>
<div>
    <ul>
        <li>New! Tracking codes can now be added automatically to all your URLs to allow easy tracking of NCS referrals to your site. Visit the Provider Details page to set them up.</li>
        <li>Send us your most recent update as soon as possible</li>
        <li>Update <em>ALL</em> courses that require an update</li>
        <li><strong>Course Titles:</strong> Include a relevant course title – simple, with qualification and one line detail.</li>
        <li><strong>Course Summaries:</strong> In order to achieve the highest quality score each course must have a summary of at least 200 characters in length, and each course must have a unique summary. The summary should provide an overview of the course content which will be useful for prospective students when making their learning choices.</li>
        <li><strong>Course URLs:</strong> To help prospective students find out more information about your courses, you should include a URL which provides more detail for each course. In order to achieve the maximum quality score, each URL should unique to the course.</li>
        <li><strong>Learning Aims:</strong> You can achieve a maximum score in this area by providing a Learning Aim for each course you upload to the Provider Portal. Using a Learning Aim not only provides accurate information to the user, but also simplifies the upload process as you have fewer fields to complete.</li>
        <li><strong>Start dates:</strong> You are encouraged to set a specific start date for each appropriate course on the portal. This will allow users to find a course which matches their availability. In order to ensure that out-of-date courses are not displayed on the Course Directory, your quality score is penalised for courses which do not have a start date which is in the future. If a course is delivered on a roll on / roll off basis, you can use the description field instead of a specific date.</li>
        <li><strong>Entry requirements:</strong> We encourage you to include entry requirements for each course so that students can assess whether they are suitably qualified take the course. In order to achieve the highest score in this section, each course must have a description of the entry requirements.</li>
        <li><strong>Last updated:</strong> You should update your provision when the start date for a course has passed, or the contents of the course changes. Your quality score will be affected if you have not updated your provision within the last three months.</li>
        <li><strong>Price:</strong> If the price depends on a number of variables, include pricing detail and variables in price description.</li>
    </ul>
</div>

<h3>Help and advice is available</h3>
<div>
    <p>
        If you need further help, please contact the Course Directory Support Team on <a href="tel:08448115073">0844 811 5073</a> or <a href="mailto:support@coursedirectoryproviderportal.org.uk">support@coursedirectoryproviderportal.org.uk</a>, or the DfE Support Team on <a href="tel:08448115028">0844 811 5028</a> or <a href="mailto:dfe.support@coursedirectoryproviderportal.org.uk">dfe.support@coursedirectoryproviderportal.org.uk</a>.
    </p>
</div>'
           , null
           , null
           , 'Top tips'
           , 127 /* All user contexts */
           , 0 /* Embedded */
           , 2 /* Live */
           , 1 /* English */
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
		   )

-- 404

INSERT INTO [dbo].[Content]
           ([Version]
           ,[Path]
           ,[Title]
           ,[Body]
           ,[Scripts]
           ,[Styles]
           ,[Summary]
           ,[UserContext]
           ,[Embed]
           ,[RecordStatusId]
           ,[LanguageId]
           ,[CreatedByUserId]
           ,[CreatedDateTimeUtc]
           ,[ModifiedByUserId]
           ,[ModifiedDateTimeUtc])
     VALUES
           ( 1
           , '404-NotFound'
           , 'The page you were looking for could not be found'
           , '<p>The page you are looking for does not seem to exist.</p>
<p>Here are some links to help you get where you are going:</p>
<ul>
<li><a href="/">Home</a></li>
<li><a href="/Account/Login">Log in</a></li>
<li><a href="/Help">Help</a></li>
</ul>'
           , null
           , null
           , 'Not found page'
           , 127 /* All user contexts */
           , 0 /* Embedded */
           , 2 /* Live */
           , 1 /* English */
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
           , '24314672-f766-47f1-98cb-ad9fc49f6e9d'
           , GetUtcDate()
		   )
END
GO
