<%@ Page Title="Dashboard Detail" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="DashboardDetail.aspx.cs" Inherits="IMS.NCS.CourseSearchService.TestHarness.DashboardDetail" EnableViewState="false"%>
<%@ Register TagPrefix="duc" TagName="SearchControl" Src="~/DashboardSearchCriteriaCtrl.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <form id="form1" runat="server" method="get" action="DashboardDetail.aspx">
        <div id="content">
            <duc:SearchControl runat="server" id="SearchControl">
                </duc:SearchControl>
    
            <div id="divResults" class="results" runat="server">
                <asp:Label ID="ResultsOverviewLabel" runat="server"></asp:Label>
                <p></p>
                <div id="divJobData">
                    <asp:Label ID="JobTitle" runat="server"></asp:Label>
                    <p></p>
                    <table class="resultstable">
                        <tr>
                            <td>Job Id:</td>
                            <td><asp:Label ID="JobId" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Job Start date/time:</td>
                            <td><asp:Label ID="StartDate" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Job End date/time:</td>
                            <td><asp:Label ID="EndDate" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Time Elapsed (ms):</td>
                            <td><asp:Label ID="TimeElapsed" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </div>
                <p></p>

                <h3 class="showHide">STEPS <em>&#9660;</em></h3>
                <fieldset>
                    <asp:GridView ID="gridSteps" width="100%" runat="server" AutoGenerateColumns="false" BorderColor="#000000" RowStyle-BorderColor="#000000" GridLines="Both">
                        <FooterStyle CssClass="gridViewFooterStyle" />
                        <RowStyle CssClass="gridViewRowStyle" />    
                        <SelectedRowStyle CssClass="gridViewSelectedRowStyle" />
                        <PagerStyle CssClass="gridViewPagerStyle" />
                        <AlternatingRowStyle CssClass="gridViewAlternatingRowStyle" />
                        <HeaderStyle CssClass="gridViewHeaderStyle" />
                        <Columns>
                            <asp:BoundField DataField="StepName" HeaderText="Step" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="ProcessStart" HeaderText="Start date/time" />
                            <asp:BoundField DataField="ProcessEnd" HeaderText="End date/time" />
                            <asp:BoundField DataField="ElapsedTime" HeaderText="Elapsed Time (ms)" />
                        </Columns>
                    </asp:GridView>
                </fieldset>

            </div>
        </div>
    </form>

</asp:Content>
