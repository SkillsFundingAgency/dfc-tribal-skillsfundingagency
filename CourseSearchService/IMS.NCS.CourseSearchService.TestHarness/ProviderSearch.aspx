<%@ Page Title="Provider Search" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ProviderSearch.aspx.cs" Inherits="IMS.NCS.CourseSearchService.TestHarness.ProviderSearch" EnableViewState="false"%>
<%@ Register TagPrefix="puc" TagName="SearchControl" Src="~/ProviderSearchCriteriaCtrl.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <form id="form1" runat="server" method="get" action="ProviderSearch.aspx">
    
        <h2>PROVIDER SEARCH</h2>
    
        <div id="content">
             <puc:SearchControl runat="server" id="SearchControl">
                </puc:SearchControl>

            <div id="divResults" class="results" runat="server">
                <asp:Label ID="ResultsOverviewLabel" runat="server"></asp:Label>

                <asp:Repeater runat="server" ID="RepeaterContainer">
                    <ItemTemplate>
                        <p></p>
                        <table class="resultstable">
                             <th colspan="2" scope="rowgroup">
                                <b>PROVIDER</b>
                            </th>
                            <tr title="Provider name">
                                <td class="coursename">
                                    <a href="ProviderDetail.aspx?providerid=<%# DataBinder.Eval(Container.DataItem, "providerid") %>&APIKey=<%= Request.QueryString["APIKey"] %>"><%# DataBinder.Eval(Container.DataItem, "providername") %></a>
                                </td>
                                <td></td>
                            </tr>
                            <tr title="Provider identifier">
                                <td class="titlecell">
                                    <b>ID:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "providerid") %>
                                </td>
                            </tr>
                            <tr title="Address line 1">
                                <td class="titlecell">
                                    <b>Address Line 1:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "addressline1") %>
                                </td>
                            </tr>
                            <tr title="Address line 2">
                                <td class="titlecell">
                                    <b>Address Line 2:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "addressline2") %>
                                </td>
                            </tr>
                            <tr title="Town">
                                <td class="titlecell">
                                    <b>Town:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "town") %>
                                </td>
                            </tr>
                            <tr title="County">
                                <td class="titlecell">
                                    <b>County:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "county") %>
                                </td>
                            </tr>
                            <tr title="Postcode">
                                <td class="titlecell">
                                    <b>Postcode:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "postcode") %>
                                </td>
                            </tr>
                            <tr title="Email">
                                <td class="titlecell">
                                    <b>Email:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "email") %>
                                </td>
                            </tr>
                            <tr title="Website">
                                <td class="titlecell">
                                    <b>Website:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "website") %>
                                </td>
                            </tr>
                            <tr title="Phone">
                                <td class="titlecell">
                                    <b>Phone:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "phone") %>
                                </td>
                            </tr>
                            <tr title="Fax">
                                <td class="titlecell">
                                    <b>Fax:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "fax") %>
                                </td>
                            </tr>
                            <tr title="24+ Loans">
                                <td class="titlecell">
                                    <b>24+ Loans:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "tfplusloans") %>
                                </td>
                            </tr>
                            <tr title="DFE Funded Provider">
                                <td class="titlecell">
                                    <b>DfE Funded Provider:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "dfe1619funded") %>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </form>
</asp:Content>
