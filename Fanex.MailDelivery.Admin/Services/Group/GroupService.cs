using EasyNetQ;
using Fanex.MailDelivery.Admin.Configurations;
using Fanex.MailDelivery.Admin.Logging;
using Fanex.MailDelivery.Admin.Messaging.ManageGroup;
using Fanex.MailDelivery.Services.Messaging;

namespace Fanex.MailDelivery.Admin.Services.Group
{
    public class GroupService : IGroupService
    {
        private readonly IBus _bus;

        public GroupService()
        {
            _bus = RabbitHutch.CreateBus(AppSettings.MailAdminConfig.RabbitMQConnectionString, serviceRegister =>
            {
                serviceRegister.Register<IEasyNetQLogger>(_ => new MailAdminLogger());
            });
        }

        public void Dispose()
        {
            _bus.Dispose();
        }

        public GetGroupListResponse GetGroupList(GetGroupListRequest request)
        {
            return _bus.Request<GetGroupListRequest, GetGroupListResponse>(request);
        }

        public ResultResponse CreateGroup(CreateGroupRequest request)
        {
            return _bus.Request<CreateGroupRequest, ResultResponse>(request);
        }

        public ResultResponse UpdateGroup(UpdateGroupRequest request)
        {
            return _bus.Request<UpdateGroupRequest, ResultResponse>(request);
        }

        public ResultResponse DeleteGroup(DeleteGroupRequest request)
        {
            return _bus.Request<DeleteGroupRequest, ResultResponse>(request);
        }

        //public GetGroupListResponse GetGroupList(GetGroupListRequest request)
        //{
        //    IList<GroupModel> groupModels = _adminRepository.GetGroupList(new GetGroupListCriteria
        //    {
        //        MailGroupName = request.Name
        //    });

        //    IList<GroupDto> groupDtos = groupModels.Select(n => n.ToGroupDto()).ToList();

        //    return new GetGroupListResponse { GroupDtos = groupDtos };
        //}

        //public ResultResponse CreateGroup(CreateGroupRequest request)
        //{
        //    return _adminRepository.CreateGroup(new CreateGroupCriteria
        //    {
        //        MailGroupName = request.Name,
        //        Description = request.Description
        //    });
        //}

        //public ResultResponse UpdateGroup(UpdateGroupRequest request)
        //{
        //    return _adminRepository.UpdateGroup(new UpdateGroupCriteria
        //    {
        //        MailGroupId = request.Id,
        //        MailGroupName = request.Name,
        //        Description = request.Description
        //    });
        //}

        //public ResultResponse DeleteGroup(DeleteGroupRequest request)
        //{
        //    return _adminRepository.DeleteGroup(new DeleteGroupCriteria
        //    {
        //        MailGroupId = request.Id
        //    });
        //}


    }
}