using AutoMapper;
using BKConnect.BKConnectBE.Common;
using BKConnectBE.Common;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.NotificationManagement;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;
using BKConnectBE.Repository.FriendRequests;
using BKConnectBE.Repository.Notifications;
using BKConnectBE.Repository.Relationships;
using BKConnectBE.Repository.Rooms;
using BKConnectBE.Repository.Users;

namespace BKConnectBE.Service.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IGenericRepository<FriendRequest> _genericRepositoryForFriendRequest;
        private readonly IGenericRepository<Notification> _genericRepositoryForNotification;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository,
            IUserRepository userRepository,
            IFriendRequestRepository friendRequestRepository,
            IRelationshipRepository relationshipRepository,
            IGenericRepository<FriendRequest> genericRepositoryForFriendRequest,
            IGenericRepository<Notification> genericRepositoryForNotification,
            IRoomRepository roomRepository,
            IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _friendRequestRepository = friendRequestRepository;
            _relationshipRepository = relationshipRepository;
            _roomRepository = roomRepository;
            _genericRepositoryForFriendRequest = genericRepositoryForFriendRequest;
            _genericRepositoryForNotification = genericRepositoryForNotification;
            _mapper = mapper;
        }

        public async Task<List<ReceiveNotificationDto>> GetListOfNotificationsByUserIdAsync(string userId)
        {
            var notifications = await _notificationRepository.GetListOfNotificationsByUserIdAsync(userId);
            var notificationDtos = new List<ReceiveNotificationDto>();
            foreach (var notification in notifications)
            {
                var notificationDto = _mapper.Map<ReceiveNotificationDto>(notification);

                if (notification.Type == NotificationType.IsInRoom.ToString())
                {
                    if(notification.RoomId == null)
                    {
                        throw new Exception(MsgNo.ERROR_ROOM_NOT_FOUND);
                    }

                    var room = await _roomRepository.GetInformationOfRoom((long)notification.RoomId);

                    notificationDto.RoomMessage = new()
                    {
                        RoomName = room.Name,
                        RoomType = room.RoomType
                    };
                }

                notificationDtos.Add(notificationDto);
            }
            return notificationDtos;
        }

        public async Task UpdateNotificationsStatusOfUserAsync(string userId)
        {
            await _notificationRepository.UpdateNotificationsStatusOfUserAsync(userId);
            await _genericRepositoryForNotification.SaveChangeAsync();
        }

        public async Task<ReceiveNotificationDto> AddSendFriendRequestNotification(string senderId, string receiverId)
        {
            if (!await _friendRequestRepository.CheckCanSendFriendRequest(senderId, receiverId))
            {
                throw new Exception(MsgNo.ERROR_CREATE_FRIEND_REQUEST);
            }

            await _friendRequestRepository.CreateFriendRequest(senderId, receiverId);

            return await AddNotification(senderId, receiverId, NotificationType.IsSendFriendRequest.ToString(), null);
        }

        public async Task<ReceiveNotificationDto> AddAcceptedFriendRequestNotification(string senderId, string receiverId)
        {
            var friendRequest = await _friendRequestRepository.GetFriendRequestByUser(receiverId, senderId)
                ?? throw new Exception(MsgNo.ERROR_INTERNAL_SERVICE);

            if (friendRequest.ReceiverId != senderId)
            {
                throw new Exception(MsgNo.ERROR_RESPONSE_FRIEND_REQUEST);
            }

            await _genericRepositoryForFriendRequest.RemoveByIdAsync(friendRequest.Id);
            await _notificationRepository.RemoveFriendRequestNotificationAsync(senderId, receiverId);
            await _roomRepository.CreateNewPrivateRoom(senderId, receiverId, Constants.FRIEND_ACCEPTED_NOTIFICATION);
            await _relationshipRepository.CreateNewRelationship(senderId, receiverId);

            return await AddNotification(senderId, receiverId, NotificationType.IsAcceptFriendRequest.ToString(), null);
        }

        public async Task<ReceiveNotificationDto> AddRoomNotification(string senderId, string receiverId, string type, long roomId)
        {
            var notification = await AddNotification(senderId, receiverId, type, roomId);
            var room = await _roomRepository.GetInformationOfRoom(roomId);
            
            notification.RoomMessage = new NotifyRoomMessage();
            notification.RoomMessage.RoomName = room.Name;
            notification.RoomMessage.RoomType = room.RoomType;

            return notification;

        }

        private async Task<ReceiveNotificationDto> AddNotification(string senderId, string receiverId, string type, long? roomId)
        {
            var sender = await _userRepository.GetByIdAsync(senderId)
                ?? throw new Exception(MsgNo.ERROR_USER_NOT_FOUND);

            var notification = new Notification()
            {
                Avatar = sender.Avatar,
                ReceiverId = receiverId,
                Type = type,
                SendTime = DateTime.Now,
                IsRead = false,
                Content = senderId
            };
            
            if (type == NotificationType.IsInRoom.ToString() || type == NotificationType.IsOutRoom.ToString())
            {
                notification.RoomId = roomId;
            }

            await _genericRepositoryForNotification.AddAsync(notification);
            await _genericRepositoryForNotification.SaveChangeAsync();

            var notificationDto = _mapper.Map<ReceiveNotificationDto>(notification);
            notificationDto.Id = notification.Id;

            return notificationDto;
        }
    }
}