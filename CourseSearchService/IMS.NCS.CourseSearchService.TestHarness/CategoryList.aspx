<%@ Page Title="Category List Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="CategoryList.aspx.cs" Inherits="IMS.NCS.CourseSearchService.TestHarness.CategoryList" EnableViewState="false"%>
<%@ Register TagPrefix="cuc" TagName="SearchControl" Src="~/CategoryListCriteriaCtrl.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script type="text/javascript">
        function hideShow(LinkObject) 
        {
            var ULTag = LinkObject.nextSibling;
            if (ULTag.style.display == 'block')
                ULTag.style.display = 'none';
            else
                ULTag.style.display = 'block';
        }
    </script>



    <form id="form1" runat="server" method="get" action="CategoryList.aspx">
    
        <h2>Category List</h2>
    
        <div id="content">
             <cuc:SearchControl runat="server" id="SearchControl" />
            <div id="results">
                
                <asp:Label ID="ResultsOverviewLabel" runat="server"></asp:Label>

                <asp:TreeView ID="TreeView1" runat="server" ExpandDepth="0" EnableViewState="false">
                </asp:TreeView>

            </div>
        </div>

    </form>

</asp:Content>
