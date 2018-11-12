<%@ Page Title="Dashboard Search" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="DashboardSearch.aspx.cs" Inherits="IMS.NCS.CourseSearchService.TestHarness.DashboardSearch" EnableViewState="false"%>
<%@ Register TagPrefix="duc" TagName="SearchControl" Src="~/DashboardSearchCriteriaCtrl.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <script type="text/javascript">
        
        function navigatePage(nextPage) {

            document.getElementById("action").value = "pagenav";
            document.getElementById("getPage").value = nextPage;
            document.getElementById("form1").submit();
        }

        function removeOldFiles() {

            document.getElementById("action").value = "remove";
            document.getElementById("form1").submit();
        }
    </script>

    <form id="form1" runat="server" method="get" action="DashboardSearch.aspx">
    
        <h2>DATA IMPORT DASHBOARD</h2>
    
        <div id="content">
            <duc:SearchControl runat="server" id="SearchControl">
            </duc:SearchControl>

            <asp:Label ID="ResultsOverviewLabel" runat="server" CssClass="coursesearchlabel"></asp:Label>
            <div id="divResults" class="results" runat="server" visible="true">
                <div id="divSearchResults" runat="server" visible="false">
                    <table width="100%" border="0">
                        <tr>
                            <td>
                                <asp:Button ID="cmdFirst" runat="server" Text="First" Visible="false" />
                                <asp:Button ID="cmdPrevious" runat="server" Text=" << " Visible="false"/>
                                <asp:Button ID="cmdNext" runat="server" Text=" >> " Visible="false"/>
                                <asp:Button ID="cmdLast" runat="server" Text="Last" Visible="false" />
                                <input type="hidden" id="getPage" name="getPage"/>
                            </td>
                        </tr>
                    </table>

                    <div class="divJobs">
                        <asp:Label ID="labelTitle" runat="server">JOBS</asp:Label>
                    </div>

                    <asp:GridView ID="gridJobs" width="100%" runat="server" AutoGenerateColumns="false" BorderColor="#000000" RowStyle-BorderColor="#000000" GridLines="Both">
                        <FooterStyle CssClass="gridViewFooterStyle" />
                        <RowStyle CssClass="gridViewRowStyle" />    
                        <SelectedRowStyle CssClass="gridViewSelectedRowStyle" />
                        <PagerStyle CssClass="gridViewPagerStyle" />
                        <AlternatingRowStyle CssClass="gridViewAlternatingRowStyle" />
                        <HeaderStyle CssClass="gridViewHeaderStyle" />
                        <Columns>
                            <asp:HyperLinkField HeaderText="Id"
                                                DataTextField="JobId" 
                                                DataNavigateUrlFormatString="DashboardDetail.aspx?jobid={0}"
                                                DataNavigateUrlFields="JobId" />
                            <asp:BoundField DataField="ProcessStart" HeaderText="Start date/time" />
                            <asp:BoundField DataField="ProcessEnd" HeaderText="End date/time" />
                            <asp:BoundField DataField="ElapsedTime" HeaderText="Elapsed Time (ms)" />
                            <asp:BoundField DataField="CurrentStep" HeaderText="Current Step" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="divJobMaintenance">
                    <asp:Label ID="labelMaintenance" runat="server">MAINTENANCE</asp:Label>
                    <br /><br />
                    <asp:Label ID="labelMaintenanceDesc" runat="server">To delete import files which are over 7 days old from the staging file shares click 'Remove Old Files' button.</asp:Label>
                    <div>
                        <asp:Button ID="buttonRemoveOldFiles" runat="server" Text="Remove Old Files" 
                            Visible="true" CssClass="remove" OnClientClick="javascript:removeOldFiles()" />
                    </div>
                </div>
            </div>
            
        </div>
    </form>
</asp:Content>
