<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CourseSearchCriteriaCtrl.ascx.cs" Inherits="IMS.NCS.CourseSearchService.TestHarness.CourseSearchCriteriaCtrl" ClassName="CourseSearchCriteriaCtrl" EnableViewState="false" %>
<div id="search">
    
    <asp:PlaceHolder ID="phAPIKey" runat="server">
        <h3>API Key</h3>
        <fieldset>
            <ol>
                <li>
                    <label for="APIKey">API Key:</label>
                    <input type="text" class="inputText" name="APIKey" id="APIKey" value="" />
                </li>
            </ol>
        </fieldset>
    </asp:PlaceHolder>

    <h3>Subject</h3>
    <fieldset>
        <ol>
            <li>
                <label for="Subject">Subject:</label>
                <input type="text" class="inputText" name="Subject" id="Subject" value=""/>
                <input type="hidden" name="action" id="action" value="search"/>
            </li>
            <li>
                <label for="LDCSCategoryCodes">LDCS category code(s):</label>
                <input type="text" name="LDCSCategoryCodes" id="LDCSCategoryCodes" value=""/>
            </li>
            <li>
                <label for="DfEFundedOnly">Course Funding Type:</label>
                <select name="DfEFundedOnly" id="DfEFundedOnly">
                    <option value="" selected="selected">All Courses</option>
                    <option value="Y">16-19 Only</option>
                    <option value="N">Post 19 Only</option>
                </select>
            </li>
        </ol>
    </fieldset>
            
    <h3>Region</h3>
    <fieldset>
        <ol>
            <li>
                <label for="LocationPostCode">Location/Postcode:</label>
                <input type="text" name="LocationPostCode" value="" id="LocationPostCode"/>
            </li>
            <li>
                <label for="MaxDistance">Max. distance (miles):</label>
                
                <input type="text" name="MaxDistance" id="MaxDistance" value=""/>
<%--                <select name="MaxDistance" id="MaxDistance">
                    <option value="" selected="selected">Please select</option>
                    <option value="5">5 miles</option>
                    <option value="10">10 miles</option>
                    <option value="20">20 miles</option>
                    <option value="30">30 miles</option>
                    <option value="50">50 miles</option>
                    <option value="75">75 miles</option>
                    <option value="100">100 miles</option>
                    <option value="150">150 miles</option>
                </select>--%>
            </li>
            <li>
                <label for="SortBy">Sort by:</label>
                <select name="SortBy" id="SortBy">
                    <option value="" selected="selected">Please select</option>
                    <option value="D">Distance</option>
                    <option value="S">Start date</option>
                    <option value="A">Relevance</option>
                </select>
            </li>
            <li>
                <label for="RecordsPerPage">Records Per Page:</label>
                <input type="text" name="RecordsPerPage" value="10" id="RecordsPerPage"/>
            </li>
        </ol>
    </fieldset>
            
    <h3 class="showHide">Provider <em>&#9660;</em></h3>
    <fieldset>
        <ol>
            <li>
                <label for="ProviderID">Provider ID:</label>
                <input type="text" name="ProviderID" value="" id="ProviderID"/>
            </li>
            <li>
                <label for="providerText">Provider Text:</label>
                <input type="text" name="ProviderText" value="" id="ProviderText"/>
            </li>
        </ol>
    </fieldset>
            
    <h3 class="showHide">Qualification <em>&#9660;</em></h3>
    <fieldset>
        <ol>
            <li>
                <h4>Qualification types:</h4>
                <div class="checkboxlist">    
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT1" value="QT1" /> No Qualification</label>
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT2" value="QT2" /> Certificate of Attendance</label>
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT3" value="QT3" /> Functional Skill</label>
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT4" value="QT4" /> Basic/Key Skill</label>
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT5" value="QT5" /> Course Provider certificate (this must include an assessed element)</label>
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT6" value="QT6" /> External awarded qualification - non-accredited</label>
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT7" value="QT7" /> Other regulated/accredited qualification</label>
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT8" value="QT8" /> GCSE or equivalent</label>
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT9" value="QT9" /> 14-19 Diploma and relevant components</label>
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT10" value="QT10" /> Apprenticeship</label>
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT11" value="QT11" /> NVQ and relevant components</label>
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT12" value="QT12" /> International Baccalaureate Diploma</label>
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT13" value="QT13" /> GCE A/AS Level or equivalent</label>
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT14" value="QT14" /> Access to Higher Education</label>
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT15" value="QT15" /> HNC/HND/Higher Education Awards</label>
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT16" value="QT16" /> Foundation Degree</label>
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT17" value="QT17" /> Undergraduate Qualification</label>
                    <label><input type="checkbox" name="QualificationTypes" id="QualificationTypes_QT18" value="QT18" /> Postgraduate Qualification</label>
                </div>
            </li>
            <li>
                <h4>Qualification level:</h4>
                <div class="checkboxlist">
                    <label><input type="checkbox" name="QualificationLevels" id="QualificationLevels_LVNA" value="LVNA" /> Not applicable</label>
                    <label><input type="checkbox" name="QualificationLevels" id="QualificationLevels_LV0" value="LV0" /> Entry Level</label>
                    <label><input type="checkbox" name="QualificationLevels" id="QualificationLevels_LV1" value="LV1" /> Level 1</label>
                    <label><input type="checkbox" name="QualificationLevels" id="QualificationLevels_LV2" value="LV2" /> Level 2</label>
                    <label><input type="checkbox" name="QualificationLevels" id="QualificationLevels_LV3" value="LV3" /> Level 3</label>
                    <label><input type="checkbox" name="QualificationLevels" id="QualificationLevels_LV4" value="LV4" /> Level 4</label>
                    <label><input type="checkbox" name="QualificationLevels" id="QualificationLevels_LV5" value="LV5" /> Level 5</label>
                    <label><input type="checkbox" name="QualificationLevels" id="QualificationLevels_LV6" value="LV6" /> Level 6</label>
                    <label><input type="checkbox" name="QualificationLevels" id="QualificationLevels_LV7" value="LV7" /> Level 7</label>
                    <label><input type="checkbox" name="QualificationLevels" id="QualificationLevels_LV8" value="LV8" /> Level 8</label>
                    <label><input type="checkbox" name="QualificationLevels" id="QualificationLevels_LV9" value="LV9" /> Level 9</label> 
                </div>
            </li>
        </ol>
    </fieldset>
            
    <h3 class="showHide">Study mode and attendance <em>&#9660;</em></h3>
    <fieldset>
        <ol>
        <li>
            <label for="EarliestStartDate">Earliest start date (yyyy-mm-dd)</label>
            <input type="text" name="EarliestStartDate" value="" id="EarliestStartDate" />
        </li>
        <li>
            <h4>Study mode:</h4>
            <div class="checkboxlist">
                    <label><input type="checkbox" name="StudyModes" id="StudyModes_SM1" value="SM1" /> Full time</label>
                    <label><input type="checkbox" name="StudyModes" id="StudyModes_SM2" value="SM2" /> Part time</label>
                    <label><input type="checkbox" name="StudyModes" id="StudyModes_SM3" value="SM3" /> Part of a full-time programme (14-19 requirement)</label>
                    <label><input type="checkbox" name="StudyModes" id="StudyModes_SM4" value="SM4" /> Flexible</label>
                    <label><input type="checkbox" name="StudyModes" id="StudyModes_SM5" value="SM5" /> Not known</label>
            </div> 
        </li>
        <li>
            <h4>Attendance mode:</h4>
            <div class="checkboxlist">
                <label><input type="checkbox" name="AttendanceModes" id="AttendanceModes_AM1" value="AM1" /> Location / campus</label>
                <label><input type="checkbox" name="AttendanceModes" id="AttendanceModes_AM2" value="AM2" /> Face-to-face (non-campus)</label>
                <label><input type="checkbox" name="AttendanceModes" id="AttendanceModes_AM3" value="AM3" /> Work-based</label>
                <label><input type="checkbox" name="AttendanceModes" id="AttendanceModes_AM4" value="AM4" /> Mixed mode</label>
                <label><input type="checkbox" name="AttendanceModes" id="AttendanceModes_AM5" value="AM5" /> Distance with attendance</label>
                <label><input type="checkbox" name="AttendanceModes" id="AttendanceModes_AM6" value="AM6" /> Distance without attendance</label>
                <label><input type="checkbox" name="AttendanceModes" id="AttendanceModes_AM7" value="AM7" /> Online with attendance</label>
                <label><input type="checkbox" name="AttendanceModes" id="AttendanceModes_AM8" value="AM8" /> Online without attendance</label>
                <label><input type="checkbox" name="AttendanceModes" id="AttendanceModes_AM9" value="AM9" /> Not known</label>
            </div>
        </li>
        <li>
            <h4>Attendance pattern:</h4>
            <div class="checkboxlist">
                <label><input type="checkbox" name="AttendancePatterns" id="AttendancePatterns_AP1" value="AP1" /> Daytime / working hours</label>
                <label><input type="checkbox" name="AttendancePatterns" id="AttendancePatterns_AP2" value="AP2" /> Day/Block release</label>
                <label><input type="checkbox" name="AttendancePatterns" id="AttendancePatterns_AP3" value="AP3" /> Evening</label>
                <label><input type="checkbox" name="AttendancePatterns" id="AttendancePatterns_AP4" value="AP4" /> Twilight</label>
                <label><input type="checkbox" name="AttendancePatterns" id="AttendancePatterns_AP5" value="AP5" /> Weekend</label>
                <label><input type="checkbox" name="AttendancePatterns" id="AttendancePatterns_AP6" value="AP6" /> Customized</label>
                <label><input type="checkbox" name="AttendancePatterns" id="AttendancePatterns_AP7" value="AP7" /> Not known</label>
            </div>
        </li>
    </ol>
    </fieldset>
            
    <h3 class="showHide">Flags <em>&#9660;</em></h3>
    <fieldset>
        <ol>
        <li>
            <h4>Include flags:</h4>
            <div class="checkboxlist">
                <label><input type="checkbox" name="IncFlexibleStartDateFlag" id="IncFlexibleStartDateFlag"/> Inc Flexible start date flag</label>
                <label><input type="checkbox" name="IncIfOpportunityApplicationClosedFlag" id="IncIfOpportunityApplicationClosedFlag"/> Inc if opportunity application closed flag</label>
                <label><input type="checkbox" name="IncTTGFlag" id="IncTTGFlag"/> Inc TTG flag</label>
                <label><input type="checkbox" name="IncTQSFlag" id="IncTQSFlag"/> Inc TQS flag</label>
                <label><input type="checkbox" name="IncIESFlag" id="IncIESFlag"/> Inc IES flag</label>
            </div>
        </li>
        <li>
            <h4>A10 flags:</h4>
            <div class="checkboxlist">
                <label><input type="checkbox" name="A10Flags" id="A10Flags_10" value="10" /> 10 Adult Safeguarded Learning (ASL)</label>
                <label><input type="checkbox" name="A10Flags" id="A10Flags_21" value="21" /> 21 16-18 Learner Responsive</label>
                <label><input type="checkbox" name="A10Flags" id="A10Flags_22" value="22" /> 22 Adult Learner Responsive</label>
                <label><input type="checkbox" name="A10Flags" id="A10Flags_45" value="45" /> 45 Employer Responsive</label>
                <label><input type="checkbox" name="A10Flags" id="A10Flags_46" value="46" /> 46 Employer Responsive main aim which is part of an apprenticeship programme</label>
                <label><input type="checkbox" name="A10Flags" id="A10Flags_70" value="70" /> 70 ESF funded (co-financed by the SFA)</label>
                <label><input type="checkbox" name="A10Flags" id="A10Flags_80" value="80" /> 80 Other LSC funding (valid only for learning aims starting before 1 August 2010)</label>
                <label><input type="checkbox" name="A10Flags" id="A10Flags_81" value="81" /> 81 Other SFA funding model - In</label>
                <label><input type="checkbox" name="A10Flags" id="A10Flags_82" value="82" /> 82 Other YPLA funding model</label>
                <label><input type="checkbox" name="A10Flags" id="A10Flags_99" value="99" /> 99 No SFA or YPLA funding for this learning aim</label>
                <label><input type="checkbox" name="A10Flags" id="A10Flags_NA" value="NA" /> Not applicable</label>
            </div>
        </li>
        <li>
            <h4>Living skills:</h4>
            <div class="checkboxlist">
                <label><input type="checkbox" name="IndLivingSkillsFlag" id="IndLivingSkillsFlag" value="Y" /> Ind. Living skills flag</label>
                <label><input type="checkbox" name="SkillsForLifeFlag" id="SkillsForLifeFlag" value="Y" /> Skills for life flag</label>
            </div>
        </li>
    </ol>
    </fieldset>
            
    <h3 class="showHide">Status <em>&#9660;</em></h3>
    <fieldset>
        <ol>
        <li>
            <h4>ER App status:</h4>
            <div class="checkboxlist">
                <label><input type="checkbox" name="ERAppStatus" id="ERAppStatus_Valid" value="Valid"/> Valid</label>
                <label><input type="checkbox" name="ERAppStatus" id="ERAppStatus_Invalid" value="Invalid"/> Invalid</label>
                <label><input type="checkbox" name="ERAppStatus" id="ERAppStatus_NotNewStarts" value="NotNewStarts"/> Not New Starts</label>
            </div>
        </li>
        <li>
            <h4>ER TTG status:</h4>
            <div class="checkboxlist">
                <label><input type="checkbox" name="ERTTGStatus" id="ERTTGStatus_Valid" value="Valid"/> Valid</label>
                <label><input type="checkbox" name="ERTTGStatus" id="ERTTGStatus_Invalid" value="Invalid"/> Invalid</label>
                <label><input type="checkbox" name="ERTTGStatus" id="ERTTGStatus_NotNewStarts" value="NotNewStarts"/> Not New Starts</label>
            </div>
        </li>
        <li>
            <h4>Adult LR status:</h4>
            <div class="checkboxlist">
                <label><input type="checkbox" name="AdultLRStatus" id="AdultLRStatus_Valid" value="Valid"/> Valid</label>
                <label><input type="checkbox" name="AdultLRStatus" id="AdultLRStatus_Invalid" value="Invalid"/> Invalid</label>
                <label><input type="checkbox" name="AdultLRStatus" id="AdultLRStatus_NoteNewStarts" value="NotNewStarts"/> Not New Starts</label>
            </div>
        </li>
        <li>
            <h4>Other funding status:</h4>
            <div class="checkboxlist">
                <label><input type="checkbox" name="OtherFundingStatus" id="OtherFundingStatus_Valid" value="Valid"/> Valid</label>
                <label><input type="checkbox" name="OtherFundingStatus" id="OtherFundingStatus_Invalid" value="Invalid"/> Invalid</label>
                <label><input type="checkbox" name="OtherFundingStatus" id="OtherFundingStatus_NotNewStarts" value="NotNewStarts"/> Not New Starts</label>
            </div>
        </li>
    </ol>
    </fieldset>
            
    <div class="button">
        <input type="submit" value="Submit" class="submit" />
    </div>
</div>