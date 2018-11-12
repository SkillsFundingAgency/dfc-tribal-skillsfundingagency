<%@ Page Title="Provider Detail" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ProviderDetail.aspx.cs" Inherits="IMS.NCS.CourseSearchService.TestHarness.ProviderDetail" %>
<%@ Register TagPrefix="puc" TagName="SearchControl" Src="~/ProviderSearchCriteriaCtrl.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <form id="form1" runat="server" method="get" action="ProviderDetail.aspx">
    
        <h2>PROVIDER DETAIL</h2>
    
        <div id="content">
            <puc:SearchControl runat="server" id="SearchControl">
                </puc:SearchControl>

            <div id="divResults" class="results" runat="server">
                <asp:Label ID="ResultsOverviewLabel" runat="server"></asp:Label>

                <asp:Table runat="server" id="ProviderDetailTable" class="resultstable"></asp:Table>
            </div>
        </div>
    </form>
</asp:Content>
