cd C:\My Documents\SFA_Phase2\Source\CourseSearchService\Ims.Schemas.Alse.CourseSearch.Contract
svcutil "Web Service specification WSDL\1.2\*.wsdl" "Web Service specification WSDL\1.2\xsd\*.xsd" /collectionType:System.Collections.Generic.List`1 /out:CourseSearchContract.cs /config:CourseSearchContract.config /n:*,Ims.Schemas.Alse.CourseSearch.Contract /syncOnly