using AutoMapper;
using BKConnect.BKConnectBE.Common;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.NotificationManagement;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;
using BKConnectBE.Repository.Files;
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
        private readonly IFileRepository _fileRepository;
        private readonly IGenericRepository<FriendRequest> _genericRepositoryForFriendRequest;
        private readonly IGenericRepository<Notification> _genericRepositoryForNotification;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository,
            IUserRepository userRepository,
            IFriendRequestRepository friendRequestRepository,
            IRelationshipRepository relationshipRepository,
            IFileRepository fileRepository,
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
            _fileRepository = fileRepository;
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
                notificationDto.SenderName = await _userRepository.GetUsernameById(notificationDto.SenderId);

                if (notification.Type == NotificationType.IsOutRoom.ToString())
                {
                    if (notification.RoomId == null)
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
                else if (notification.Type == NotificationType.IsPostFile.ToString())
                {
                    if (notification.RoomId == null)
                    {
                        throw new Exception(MsgNo.ERROR_ROOM_NOT_FOUND);
                    }

                    var room = await _roomRepository.GetInformationOfRoom((long)notification.RoomId);
                    if (room.RoomType != RoomType.ClassRoom.ToString())
                    {
                        throw new Exception(MsgNo.ERROR_INTERNAL_SERVICE);
                    }

                    if (room.UsersOfRoom.Any(u => u.UserId == userId))
                    {
                        var file = await _fileRepository.GetFileByIdAsync(notification.FileId ?? 0)
                            ?? throw new Exception(MsgNo.ERROR_FILE_NOT_FOUND);
                        if (file.UserId == userId)
                        {
                            continue;
                        }
                        notificationDto.PostFile = new()
                        {
                            RoomId = room.Id,
                            RoomName = room.Name,
                            FileId = file.Id,
                            FileName = file.Path
                        };
                    }
                    else
                    {
                        continue;
                    }
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
            await _roomRepository.CreateNewPrivateRoom(senderId, receiverId, SystemMessageType.IsBecomeFriend.ToString());
            await _relationshipRepository.CreateNewRelationship(senderId, receiverId);

            return await AddNotification(senderId, receiverId, NotificationType.IsAcceptFriendRequest.ToString(), null);
        }

        public async Task<ReceiveNotificationDto> AddRoomNotification(string senderId, string receiverId, string type, long roomId)
        {
            var notification = await AddNotification(senderId, receiverId, type, roomId);
            var room = await _roomRepository.GetInformationOfRoom(roomId);

            notification.RoomMessage = new NotifyRoomMessage
            {
                RoomId = room.Id,
                RoomName = room.Name,
                RoomType = room.RoomType
            };

            return notification;

        }

        public async Task<ReceiveNotificationDto> AddPostFileNotification(long fileId)
        {
            var file = await _fileRepository.GetFileByIdAsync(fileId)
                ?? throw new Exception(MsgNo.ERROR_FILE_NOT_FOUND);
            var notification = await AddNotification(file.UserId, null, NotificationType.IsPostFile.ToString(), file.RoomId, fileId);

            notification.PostFile = new NotifyPostFile
            {
                RoomId = file.Room.Id,
                RoomName = file.Room.Name,
                FileId = file.Id,
                FileName = file.Path
            };

            return notification;
        }

        private async Task<ReceiveNotificationDto> AddNotification(string senderId, string receiverId, string type, long? roomId, long? fileId = null)
        {
            var sender = await _userRepository.GetByIdAsync(senderId)
                ?? throw new Exception(MsgNo.ERROR_USER_NOT_FOUND);

            var notification = new Notification()
            {
                Avatar = sender.Avatar,
                ReceiverId = receiverId,
                Type = type,
                SendTime = DateTime.UtcNow.AddHours(7),
                IsRead = false,
                SenderId = senderId
            };

            if (type == NotificationType.IsInRoom.ToString() || type == NotificationType.IsOutRoom.ToString())
            {
                notification.RoomId = roomId;
            }
            else if (type == NotificationType.IsPostFile.ToString())
            {
                notification.FileId = fileId;
                notification.RoomId = roomId;
            }

            await _genericRepositoryForNotification.AddAsync(notification);
            await _genericRepositoryForNotification.SaveChangeAsync();

            var notificationDto = _mapper.Map<ReceiveNotificationDto>(notification);
            notificationDto.Id = notification.Id;
            notificationDto.SenderName = sender.Name;
            notificationDto.SendTime = notificationDto.SendTime.AddHours(-7);

            return notificationDto;
        }
    }
}