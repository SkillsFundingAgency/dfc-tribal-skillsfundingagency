<%@ Page Title="Course Detail" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="CourseDetail.aspx.cs" Inherits="IMS.NCS.CourseSearchService.TestHarness.CourseDetail" EnableViewState="false" %>
<%@ Register TagPrefix="suc" TagName="SearchControl" Src="~/CourseSearchCriteriaCtrl.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <form id="form1" runat="server" method="get" action="CourseDetail.aspx">
    
        <h2>COURSE SEARCH</h2>
    
        <div id="content">
            <suc:SearchControl runat="server" id="SearchControl">
                </suc:SearchControl>
    
            <div id="divResults" class="results" runat="server">
                <asp:Label ID="ResultsOverviewLabel" runat="server"></asp:Label>
                <p></p>
                <asp:Label ID="NumberOfCourses" runat="server"></asp:Label>
                <p></p>
                <asp:Repeater runat="server" ID="CourseRepeater" 
                    onitemdatabound="CourseRepeater_ItemDataBound" EnableViewState="false">
                    <ItemTemplate>
                        <b>COURSE - <%# DataBinder.Eval(Container.DataItem, "providercoursetitle")%></b>
                        <p></p>
                        <table class="resultstable">
                            <tr>
                                <td class="titlecell">
                                    <b>NDLPP Course Id:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "courseid") %>
                                </td>
                            </tr>
                            <tr>
                                <td class="titlecell">
                                    <b>Provider course title:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "providercoursetitle") %>
                                </td>
                            </tr>
                            <tr>
                                <td class="titlecell">
                                    <b>Summary:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "summary") %>
                                </td>
                            </tr>
                            <tr>
                                <td class="titlecell">
                                    <b>URL:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "url") %>
                                </td>
                            </tr>
                            <tr>
                                <td class="titlecell">
                                    <b>Booking URL:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "bookingurl") %>
                                </td>
                            </tr>
                            <tr>
                                <td class="titlecell">
                                    <b>Entry requirements:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "entryrequirements") %>
                                </td>
                            </tr>
                            <tr>
                                <td class="titlecell">
                                    <b>Assessment method:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "assessmentmethod") %>
                                </td>
                            </tr>
                            <tr>
                               <td class="titlecell">
                                    <b>Equipment required:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "equipmentrequired") %>
                                </td>
                            </tr>
                            <tr>
                                <td class="titlecell">
                                    <b>Tariff required:</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "tariffrequired") %>
                                </td>
                            </tr>
                            <tr>
                                <td class="titlecell">
                                    <b>Learning Aim Ref ( foreign key ):</b>
                                </td>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "learningaimref") %>
                                </td>
                            </tr>
                        </table>

                        <h3 class="showHide">Referred LAD data made available in the API <em>&#9660;</em></h3>
                        <fieldset>
                            <table class="resultstable">
                                <tr>
                                    <td class="titlecell">
                                        <b>Awarding organisation name:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "awardingorganisationname") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>Level 2 Entitlement Category Description:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "level2entitlementcategorydescription")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>Level 3 Entitlement Category Description:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "level3entitlementcategorydescription")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>Sector lead body description:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "sectorleadbodydescription")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>Accreditation start date:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "accreditationstartdate")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>Accreditation end date:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "accreditationenddate")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>Certification end date:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "certificationenddate")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>Credit value:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "creditvalue")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>QCA Guided Learning Hours:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "qcaguidedlearninghours")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>Independent living skills:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "independentlivingskills")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>Skills for Life flag:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "skillsforlifeflag")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>Skills for Life Type description:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "skillsforlifetypedescription")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>ER App Status:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "erappstatus")%>
                                    </td>
                                </tr>
                                <tr>
                                   <td class="titlecell">
                                        <b>ER TTG Status:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "erttgstatus")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>Adult LR Status:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "adultlrstatus")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>Other Funding Non Funding Status:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "otherfundingnonfundingstatus")%>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>

                        <h3 class="showHide">(Conditionally) Derived data or entered by provider and exposed in the API <em>&#9660;</em></h3>
                        <fieldset>
                            <table class="resultstable">
                                <tr>
                                    <td class="titlecell">
                                        <b>Data type:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "datatype") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>Qualification reference authority:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "qualificationreferenceauthority")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>Qualification reference:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "qualificationreference")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>Qualification title:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "qualificationtitle")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>Qaulification type:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "qualificationtype")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>Qualification level:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "qualificationlevel")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>LDCS category code - applicability 1:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "ldcscategorycodeapplicability1")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="titlecell">
                                        <b>LDCS category code - applicability 2:</b>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "ldcscategorycodeapplicability2")%>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>

                        <h3 class="showHide">Opportunity <em>&#9660;</em></h3>
                        <fieldset>
                            <asp:Repeater runat="server" ID="OpportunityRepeater">
                            <ItemTemplate>
                                <table class="resultstable">
                                    <tr>
                                        <td class="titlecell">
                                            <b>Opportunity Id:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "opportunityid") %>
                                    </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Provider opportunity Id:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "provideropportunityid") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Price:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "price")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Price description:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "pricedescription")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Duration value:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "durationvalue")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Duration unit:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "durationunit")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Duration description:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "durationdescription")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Start date description:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "startdatedescription")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Start date:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "startdate")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>End date:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "enddate")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Study mode:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "studymode")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Attendance mode:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "attendancemode")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Attendance pattern:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "attendancepattern")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Language of instruction:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "languageofinstruction")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Language of assessment:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "languageofassessment")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Places available:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "placesavailable")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Enquire To:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "enquireto")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Apply to:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "applyto")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Apply from date:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "applyfromdate")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Apply until date:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "applyuntildate")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Apply until desc:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "applyuntildescription")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>URL:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "url")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Timetable:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "timetable")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>A10 field:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "a10field")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Application accepted throughout year:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "applicationacceptedthroughoutyear")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Venue Id:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "venueid")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titlecell">
                                            <b>Region name:</b>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "regionname")%>
                                        </td>
                                    </tr>
                                    </table>
                            </ItemTemplate>
                        </asp:Repeater>
                        </fieldset>

                        <h3 class="showHide">Provider <em>&#9660;</em></h3>
                        <fieldset>
                            <asp:Repeater runat="server" ID="ProviderRepeater" EnableViewState="false">
                                <ItemTemplate>
                                    <table class="resultstable">
                                        <tr>
                                            <td class="titlecell">
                                                <b>Provider name:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "providername") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>UKPRN:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "ukprn") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>UPIN:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "upin") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>Address line 1:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "addressline1") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>Address line 2:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "addressline2") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>Town:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "town") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>County:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "county") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>Postcode:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "postcode") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>Email:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "email") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>Website:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "website") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>Phone:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "phone") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>Fax:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "fax") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>24+ Loans:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "tfplusloans") %>
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
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </fieldset>

                        <h3 class="showHide">Venue <em>&#9660;</em></h3>
                        <fieldset>
                            <asp:Repeater runat="server" ID="VenueRepeater" EnableViewState="false">
                                <ItemTemplate>
                                    <table class="resultstable">
                                        <tr>
                                            <td colspan="2" class="coursename">
                                                <b>VENUE - <%# DataBinder.Eval(Container.DataItem, "venuename") %></b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>Venue Id:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "venueid") %>
                                            </td>
                                        </tr>
                                        <tr>
                                           <td class="titlecell">
                                                <b>Venue name:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "venuename") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>Address line 1:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "addressline1") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>Address line 2:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "addressline2") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>Town:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "town") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>County:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "county") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>Postcode:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "postcode") %>
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
                                        <tr>
                                            <td class="titlecell">
                                                <b>Email:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "Email") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>Website:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "website") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>Phone:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "phone") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>Fax:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "fax") %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="titlecell">
                                                <b>Facilities:</b>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "facilities") %>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </fieldset>
                        <p></p>
                        <p></p>
                    </ItemTemplate>
                </asp:Repeater>
            </div>   
        </div>
    </form>
</asp:Content>
