using System;
using System.Configuration;
using System.ServiceModel;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using IMS.NCS.CourseSearchService.BusinessServices;
using Ims.Schemas.Alse.CourseSearch.Contract;
using IMS.NCS.CourseSearchService.Exceptions;

namespace IMS.NCS.CourseSearchService.ServiceImplementation
{
    /// <summary>
    /// CourseSeachService implementation.
    /// </summary>
    [ServiceBehavior(Namespace = "http://schemas.imservices.org.uk/alse/coursesearchservice/1.2", InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [ExceptionShielding("WCF Exception Policy")]
    public class CourseSearchService : ServiceInterface
    {
        #region Variables

        private static Boolean minMaxThreadsSet;
        private readonly ICourseService _courseService;
        private readonly IProviderService _providerService;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Unity Constructor.
        /// </summary>
        /// <param name="courseService">ICourseService object.</param>
        /// <param name="providerService">IProviderService object.</param>
        public CourseSearchService(ICourseService courseService, IProviderService providerService)
        {
            if (!minMaxThreadsSet)
            {
                try
                {
                    // Get current min values 
                    Int32 minWorker;
                    Int32 minCompletion;
                    ThreadPool.GetMinThreads(out minWorker, out minCompletion);

                    // Get current max values
                    Int32 maxWorker;
                    Int32 maxCompletion;
                    ThreadPool.GetMaxThreads(out maxWorker, out maxCompletion);

                    // Set new min values
                    Int32 minThreadsPerCPUCore;
                    Int32.TryParse(ConfigurationManager.AppSettings["MinThreadsPerCPUCore"], out minThreadsPerCPUCore);
                    if (minThreadsPerCPUCore > 0)
                    {
                        ThreadPool.SetMinThreads(minWorker, minThreadsPerCPUCore);
                    }

                    // Set new max values
                    Int32 maxThreadsPerCPUCore;
                    Int32.TryParse(ConfigurationManager.AppSettings["MaxThreadsPerCPUCore"], out maxThreadsPerCPUCore);
                    if (minThreadsPerCPUCore > 0)
                    {
                        ThreadPool.SetMaxThreads(maxWorker, maxThreadsPerCPUCore);
                    }

                    minMaxThreadsSet = true;
                }
                catch {}
            }

            _courseService = courseService;
            _providerService = providerService;
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Retrieves all Categories.
        /// </summary>
        /// <param name="request">SubjectBrowseInput request.</param>
        /// <exception cref="CourseSearchServiceException">The exception details of the error that has occurred.</exception>
        /// <returns>SubjectBrowseOutput object populated with Category details.</returns>
        public SubjectBrowseOutput GetCategories(SubjectBrowseInput request)
        {
            SubjectBrowseOutput sbo;

            try
            {
                sbo = _courseService.GetCategories(request);
            }
            catch (Exception ex)
            {
                CourseSearchServiceException exc = new CourseSearchServiceException("An error has occured in the CourseSearchService", ex);
                throw exc;
            }

            return sbo;
        }

        /// <summary>
        /// Retrieves Provider details matching search criteria contained within the request.
        /// </summary>
        /// <param name="request">ProviderSearchInput request object containing search criteria.</param>
        /// <exception cref="CourseSearchServiceException">The exception details of the error that has occurred.</exception>
        /// <returns>ProviderSearchOutput object populated with matching Provider details.</returns>
        public ProviderSearchOutput ProviderSearch(ProviderSearchInput request)
        {
            ProviderSearchOutput pso;

            try
            {
                pso = _providerService.GetProviders(request);
            }
            catch (Exception ex)
            {
                CourseSearchServiceException exc = new CourseSearchServiceException("An error has occured in the CourseSearchService", ex);
                throw exc;
            }

            return pso;
        }

        /// <summary>
        /// Retrieves Courses matching the search criteria in the CourseListInput request.
        /// </summary>
        /// <param name="request">CourseListInput request object containing search criteria.</param>
        /// <exception cref="CourseSearchServiceException">The exception details of the error that has occurred.</exception>
        /// <returns>CourseListOutput object containing matching Course details.</returns>
        public CourseListOutput CourseList(CourseListInput request)
        {
            CourseListOutput clo;

            try
            {
                clo = _courseService.GetCourseList(request);
            }
            catch (Exception ex)
            {
                CourseSearchServiceException exc = new CourseSearchServiceException("An error has occured in the CourseSearchService", ex);
                throw exc;
            }

            return clo;
        }

        /// <summary>
        /// Retrieves Course details for the list of Course ids provided in the CourseDetailInput request.
        /// </summary>
        /// <param name="request">CourseDetailInput request object containing Course ids to search for.</param>
        /// <exception cref="CourseSearchServiceException">The exception details of the error that has occurred.</exception>
        /// <returns>CourseDetailOutput object containing matching Course details.</returns>
        public CourseDetailOutput CourseDetail(CourseDetailInput request)
        {
            CourseDetailOutput cdo;

            try
            {
                cdo = _courseService.GetCourseDetails(request);
            }
            catch (Exception ex)
            {
                CourseSearchServiceException exc = new CourseSearchServiceException("An error has occured in the CourseSearchService", ex);
                throw exc;
            }

            return cdo;
        }

        /// <summary>
        /// Retrieves Provider details for the Provider Id provided in the request.
        /// </summary>
        /// <param name="request">ProviderDetailsInput request containing ProviderId.</param>
        /// <exception cref="CourseSearchServiceException">The exception details of the error that has occurred.</exception>
        /// <returns>ProviderDetailsOutput object populated with Provider details for the requested Provider Id.</returns>
        public ProviderDetailsOutput ProviderDetails(ProviderDetailsInput request)
        {
            ProviderDetailsOutput pdo;

            try
            {
                pdo = _providerService.GetProviderDetails(request);

                if (pdo == null)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                CourseSearchServiceException exc = new CourseSearchServiceException("An error has occured in the CourseSearchService", ex);
                throw exc;
            }

            return pdo;
        }

        #endregion Public Methods
    }
}