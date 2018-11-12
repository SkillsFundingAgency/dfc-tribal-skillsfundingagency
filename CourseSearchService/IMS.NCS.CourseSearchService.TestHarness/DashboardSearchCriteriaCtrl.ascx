<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardSearchCriteriaCtrl.ascx.cs" Inherits="IMS.NCS.CourseSearchService.TestHarness.DashboardSearchCriteriaCtrl" ClassName="DashboardSearchCriteriaCtrl" EnableViewState="false"%>
<div id="search">
    <h3>JOB DETAILS</h3>
    <fieldset>
        <ol>
            <li>
                <label for="StartDate">Start date (dd/mm/yyyy):</label>
                <input type="text" class="inputText" name="StartDate" id="StartDate" value=""/>
                <input type="hidden" name="action" id="action" value="search"/>
            </li>
            <li>
                <label for="EndDate">EndDate (dd/mm/yyyy):</label>
                <input type="text" name="EndDate" id="EndDate" value=""/>
            </li>
            <li>
                <label for="EndDate">Job status:</label>
                <div class="checkboxlist">    
                    <label><input type="checkbox" name="JobStatus" id="JobStatus_InProgress" value="InProgress" /> In Progress Jobs</label>
                    <label><input type="checkbox" name="JobStatus" id="JobStatus_Complete" value="Complete" /> Completed Jobs</label>
                    <label><input type="checkbox" name="JobStatus" id="JobStatus_Failed" value="Failed" /> Failed Jobs</label>
                </div>
            </li>
            <li>
                <label for="SortBy">Sort by:</label>
                <select name="SortBy" id="SortBy">
                    <option value="" selected="selected">Please select</option>
                    <option value="StartDate">Start date</option>
                    <option value="EndDate">End date</option>
                    <option value="Status">Job status</option>
                </select>
            </li>
            <li>
                <label for="RecordsPerPage">Records Per Page:</label>
                <input type="text" name="RecordsPerPage" value="10" id="RecordsPerPage"/>
            </li>
        </ol>
    </fieldset>
        
    <div class="button">
        <input type="submit" value="Submit" class="submit" />
    </div>
</div>