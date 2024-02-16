using Microsoft.AspNetCore.Mvc;
using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Shared.RequestJsons;
using BeltmanSoftwareDesign.Shared.ResponseJsons;
namespace BeltmanSoftwareDesign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProjectController : BaseControllerBase
    {
        public ProjectController(IProjectService projectService) 
        {
            ProjectService = projectService;
        }

        public IProjectService ProjectService { get; }

        [HttpPost]
        public ProjectCreateResponse Create(ProjectCreateRequest request) 
            => ProjectService.Create(request, IpAddress, Headers);

        [HttpPost]
        public ProjectReadResponse Read(ProjectReadRequest request) 
            => ProjectService.Read(request, IpAddress, Headers);

        [HttpPost]
        public ProjectUpdateResponse Update(ProjectUpdateRequest request) 
            => ProjectService.Update(request, IpAddress, Headers);

        [HttpPost]
        public ProjectDeleteResponse Delete(ProjectDeleteRequest request) 
            => ProjectService.Delete(request, IpAddress, Headers);

        [HttpPost]
        public ProjectListResponse List(ProjectListRequest request) 
            => ProjectService.List(request, IpAddress, Headers);
    }
}
