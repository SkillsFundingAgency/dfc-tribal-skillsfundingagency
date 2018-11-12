<%@ Page Title="Course Search" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="CourseSearch.aspx.cs" Inherits="IMS.NCS.CourseSearchService.TestHarness.CourseSearch" EnableViewState="false" %>
<%@ Register TagPrefix="suc" TagName="SearchControl" Src="~/CourseSearchCriteriaCtrl.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <script type="text/javascript">
        function navigatePage(nextPage) {

            document.getElementById("action").value = "pagenav";
            document.getElementById("getPage").value = nextPage;
            document.getElementById("form1").submit();
        }

        function compareSelected() {

            document.getElementById("action").value = "compare";
            document.getElementById("form1").submit();
        }
    </script>
    
    <form id="form1" runat="server" method="get" action="CourseSearch.aspx">
    
    <h2>COURSE SEARCH</h2>
    
    <div id="content">
        <suc:SearchControl runat="server" id="SearchControl">
        </suc:SearchControl>


        <asp:Label ID="ResultsOverviewLabel" runat="server" CssClass="coursesearchlabel"></asp:Label>
        <div id="divResults" class="results" runat="server" visible="false">
            
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
            
            <div class="divCompare">
                <label for="cmdCompare">
                    <asp:button ID="cmdCompare" runat="server" CssClass="compare" Text="Compare" OnClientClick="javascript:compareSelected()" />
                    <span class="wrappable">Select one or more courses to compare</span>
                </label>
             </div>
            <asp:Repeater runat="server" ID="CourseResultsRepeater">
            <ItemTemplate>
                <p></p>
                <table class="resultstable" title="Course Results Table">
                    <th colspan="2" scope="rowgroup">
                        <b>COURSE</b>
                    </th>
                    <tr title="Course name">
                        <td class="coursename">
                            <input type="checkbox" class="chk" name="CourseChk" value="<%# DataBinder.Eval(Container.DataItem, "courseid") %>" />
                            <a href="CourseDetail.aspx?CourseId=<%# DataBinder.Eval(Container.DataItem, "courseid") %><%= Request.QueryString["APIKey"] != null ? "&APIKey=" + Request.QueryString["APIKey"] : "" %>"><%# DataBinder.Eval(Container.DataItem, "coursename") %></a>
                        </td>
                        <td></td>
                    </tr>
                    <tr title="Course identifier">
                        <td class="titlecell">
                            <b>Course Id:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "courseid") %>
                        </td>
                    </tr>
                    <tr title="Provider name">
                        <td class="titlecell">
                            <b>Provider name:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "providername") %>
                        </td>
                    </tr>
                    <tr title="Provider 24+ Loans">
                        <td class="titlecell">
                            <b>Provider 24+ Loans:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "tfplusloans") %>
                        </td>
                    </tr>
                    <tr title="Provider DfE 16-19 Funded">
                        <td class="titlecell">
                            <b>Provider DfE 16-19 Funded:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "ProviderDfEFunded") %>
                        </td>
                    </tr>
                    <tr title="FE Choices Learner Destination">
                        <td class="titlecell">
                            <b>FE Choices Learner Destination:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "FEChoices_LearnerDestination") %>
                        </td>
                    </tr>
                    <tr title="FE Choices Learner Satisfaction">
                        <td class="titlecell">
                            <b>FE Choices Learner Satisfaction:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "FEChoices_LearnerSatisfaction") %>
                        </td>
                    </tr>
                    <tr title="FE Choices Employer Satisfaction">
                        <td class="titlecell">
                            <b>FE Choices Employer Satisfaction:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "FEChoices_EmployerSatisfaction") %>
                        </td>
                    </tr>
                    <tr title="Qualification type">
                        <td class="titlecell">
                            <b>Qualification type:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "qualificationtype") %>
                        </td>
                    </tr>
                    <tr title="Qualification level">
                        <td class="titlecell">
                            <b>Qualification level:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "qualificationlevel") %>
                        </td>
                    </tr>
                    <tr title="Number of opportunities">
                        <td class="titlecell">
                            <b>Number of opportunities:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "numberofopportunities") %>
                        </td>
                    </tr>
                    <tr title="Course summary">
                        <td class="titlecell">
                            <b>Course summary:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "coursesummary") %>
                        </td>
                    </tr>
                    <tr title="LDCS 1">
                        <td class="titlecell">
                            <b>LDCS 1:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "ldcs1") %>
                        </td>
                    </tr>
                    <tr title="LDCS 1 description">
                        <td class="titlecell">
                            <b>LDCS 1 description:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "ldcs1description") %>
                        </td>
                    </tr>
                    <tr title="LDCS 2">
                       <td class="titlecell">
                            <b>LDCS 2:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "ldcs2") %>
                        </td>
                    </tr>
                    <tr title="LDCS 2 description">
                        <td class="titlecell">
                            <b>LDCS 2 description:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "ldcs2description") %>
                        </td>
                    </tr>
                    <tr title="LDCS 3">
                        <td class="titlecell">
                            <b>LDCS 3:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "ldcs3") %>
                        </td>
                    </tr>
                    <tr title="LDCS 3 description">
                        <td class="titlecell">
                            <b>LDCS 3 description:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "ldcs3description") %>
                        </td>
                    </tr>
                    <tr title="LDCS 4">
                        <td class="titlecell">
                            <b>LDCS 4:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "ldcs4") %>
                        </td>
                    </tr>
                    <tr title="LDCS 4 description">
                        <td class="titlecell">
                            <b>LDCS 4 description:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "ldcs4description") %>
                        </td>
                    </tr>
                    <tr title="LDCS 5">
                       <td class="titlecell">
                            <b>LDCS 5:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "ldcs5") %>
                        </td>
                    </tr>
                    <tr title="LDCS 5 description">
                        <td class="titlecell">
                            <b>LDCS 5 description:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "ldcs5description") %>
                        </td>
                    </tr>
                </table>
                <table class="resultstable" title="Opportunity Results Table">
                    <th colspan="2" scope="rowgroup">
                        <b>OPPORTUNITY</b>
                    </th>
                        <tr title="Opportunity identifier">
                            <td class="titlecell">
                                <b>Opportunity Id</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "opportunityid")%>
                            </td>
                        </tr>
                        <tr title="Study mode">
                           <td class="titlecell">
                                <b>Study mode:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "studymode")%>
                            </td>
                        </tr>
                       <tr title="Attendance mode">
                            <td class="titlecell">
                                <b>Attendance mode:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "attendancemode")%>
                            </td>
                       </tr>
                       <tr title="Attendance pattern">
                            <td class="titlecell">
                                <b>Attendance pattern:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "attendancepattern")%>
                            </td>
                       </tr>
                       <tr title="Start date">
                            <td class="titlecell">
                                <b>Start date:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "startdate")%>
                            </td>
                       </tr>
                       <tr title="Start date description">
                            <td class="titlecell">
                                <b>Start date description:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "startdatedescription")%>
                            </td>
                       </tr>
                       <tr title="Duration value">
                            <td class="titlecell">
                                <b>Duration value:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "durationvalue")%>
                            </td>
                       </tr>
                       <tr title="Duration unit">
                            <td class="titlecell">
                                <b>Duration unit:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "durationunit")%>
                            </td>
                       </tr>
                       <tr title="Duration description">
                            <td class="titlecell">
                                <b>Duration description:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "durationdescription")%>
                            </td>
                       </tr>
                       <tr title="Region name">
                            <td class="titlecell">
                                <b>Region name:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "regionname")%>
                            </td>
                       </tr>
                       <tr title="Course DfE 16-19 Funded">
                            <td class="titlecell">
                                <b>Course DfE 16-19 Funded:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "CourseDfEFunded")%>
                            </td>
                       </tr>
                       <tr title="Venue">
                            <td class="titlecell">
                                <b>Venue:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "venue")%>
                            </td>
                       </tr>
                       <tr title="Distance in miles">
                            <td class="titlecell">
                                <b>Distance (miles):</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "distance")%>
                            </td>
                       </tr>
                       <tr title="Address line 1">
                            <td class="titlecell">
                                <b>Address line 1:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "addressline1")%>
                            </td>
                       </tr>
                       <tr title="Address line 2">
                            <td class="titlecell">
                                <b>Address line 2:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "addressline2")%>
                            </td>
                       </tr>
                       <tr title="Town">
                            <td class="titlecell">
                                <b>Town:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "town")%>
                            </td>
                       </tr>
                       <tr title="County">
                            <td class="titlecell">
                                <b>County:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "county")%>
                            </td>
                       </tr>
                       <tr title="Postcode">
                            <td class="titlecell">
                                <b>Postcode:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "postcode")%>
                            </td>
                       </tr>
                       <tr>
                            <td class="titlecell">
                                <b>Latitude:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "latitude") %>
                            </td>
                        </tr>
                        <tr>
                            <td class="titlecell">
                                <b>Longitude:</b>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "longitude")%>
                            </td>
                        </tr>
                </table>
            </ItemTemplate>
        </asp:Repeater>

        <p></p>
        <h3>Matching LDCS Codes</h3>

        <asp:Repeater runat="server" ID="MatchingLDCSCodesRepeater">
            <ItemTemplate>
                <p></p>
                <table class="resultsTable" title="Matching LDCS Codes Table">
                    <tr title="LDCS Code">
                        <td>
                            <b>LDCS Code:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "code") %>
                        </td>
                    </tr>
                    <tr title="LDCS description">
                        <td>
                            <b>LDCS description:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "description") %>
                        </td>
                    </tr>
                    <tr title="LDCS course count">
                        <td>
                            <b>LDCS course count:</b>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "coursecount") %>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:Repeater>
        </div>   
    </div>
    </form>
</asp:Content>
