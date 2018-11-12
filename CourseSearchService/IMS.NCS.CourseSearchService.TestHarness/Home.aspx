<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Home.aspx.cs" Inherits="IMS.NCS.CourseSearchService.TestHarness.Home" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
<title>Home</title>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>SERVICES AVAILABLE</h2>

<h4>This application is to test the NCS Course Search web service.</h4>
<p></p>
<h4>Available services are...</h4>
<p></p>
<ul>
    <li><a href="ProviderSearch.aspx">Provider search</a></li>
    <li><a href="CategoryList.aspx">Category list</a></li>
    <li><a href="CourseSearch.aspx">Course search</a></li>
    <li>Course details</li>
</ul>
</asp:Content>
