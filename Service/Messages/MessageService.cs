using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using AutoMapper;
using BKConnect.BKConnectBE.Common;
using BKConnectBE.Common;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.MessageManagement;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;
using BKConnectBE.Repository.Messages;
using BKConnectBE.Repository.Users;

namespace BKConnectBE.Service.Messages
{
    public class MessageService : IMessageService
    {
        private readonly IGenericRepository<Message> _genericRepositoryForMessage;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public MessageService(IGenericRepository<Message> genericRepositoryForMessage,
            IMessageRepository messageRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _genericRepositoryForMessage = genericRepositoryForMessage;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ReceiveMessageDto> AddMessageAsync(SendMessageDto messageDto, string userId, string? affectedId = null)
        {
            Message sendMsg = _mapper.Map<Message>(messageDto);
            sendMsg.SenderId = userId;
            sendMsg.SendTime = DateTime.UtcNow.AddHours(7);

            var list = new List<string> {
                SystemMessageType.IsInRoom.ToString(),
                SystemMessageType.IsOutRoom.ToString()
            };

            if (sendMsg.TypeOfMessage == MessageType.System.ToString()
                && list.Contains(sendMsg.Content) && affectedId != null)
            {
                sendMsg.AffectedId = affectedId;
            }

            await _genericRepositoryForMessage.AddAsync(sendMsg);
            await _genericRepositoryForMessage.SaveChangeAsync();
            Message newMsg = await _messageRepository.GetMessageByIdAsync(sendMsg.Id);
            var newMsgDto = _mapper.Map<ReceiveMessageDto>(newMsg);
            newMsgDto.SendTime = newMsgDto.SendTime.AddHours(-7);
            return newMsgDto;
        }

        public async Task<List<ReceiveMessageDto>> GetAllMessagesInRoomAsync(string userId, long roomId)
        {
            var listMessages = await _messageRepository.GetAllMessagesInRoomAsync(roomId);
            var listMessagesDto = new List<ReceiveMessageDto>();
            foreach (var msg in listMessages)
            {
                var msgDto = _mapper.Map<ReceiveMessageDto>(msg);
                if (msg.TypeOfMessage == MessageType.System.ToString())
                {
                    msgDto = await ChangeSystemMessage(msgDto, userId, msg.AffectedId, msg.Content);
                }
                else
                {
                    msgDto = await ChangeMessage(msgDto, userId, msg.RootMessage?.SenderId);
                }
                listMessagesDto.Add(msgDto);
            }

            return listMessagesDto;
        }
        public async Task<ReceiveMessageDto> ChangeMessage(ReceiveMessageDto receiveMsg, string userId, string rootSenderId)
        {
            if (receiveMsg.SenderId == null)
            {
                return receiveMsg;
            }

            if (receiveMsg.SenderId == userId)
            {
                receiveMsg.SenderName = "Bạn";
            }
            else
            {
                receiveMsg.SenderName = await _userRepository.GetUsernameById(receiveMsg.SenderId);
            }

            if (rootSenderId != null)
            {
                if (rootSenderId == userId && receiveMsg.SenderId == userId)
                {
                    receiveMsg.RootSender = "chính mình";
                }
                else if (rootSenderId == userId)
                {
                    receiveMsg.RootSender = "bạn";
                }
                else if (receiveMsg.SenderId == rootSenderId)
                {
                    receiveMsg.RootSender = await _userRepository.GetUserGenderById(rootSenderId) ? "anh ấy" : "cô ấy";
                }
                else
                {
                    receiveMsg.RootSender = await _userRepository.GetUsernameById(rootSenderId);
                }
            }

            if (receiveMsg.TypeOfMessage == MessageType.Image.ToString())
            {
                receiveMsg.LastMessage = receiveMsg.SenderName + " đã gửi một ảnh đính kèm";
            }
            else if (receiveMsg.TypeOfMessage == MessageType.File.ToString())
            {
                receiveMsg.LastMessage = receiveMsg.SenderName + " đã gửi một file đính kèm";
            }
            else
            {
                receiveMsg.LastMessage = receiveMsg.SenderName + ": " + receiveMsg.Content;
            }

            return receiveMsg;
        }

        public async Task<string> GetRootMessageSenderId(long? messageId)
        {
            return await _messageRepository.GetRootMessageSenderIdAsync(messageId);
        }

        public async Task<List<FileDto>> GetAllNoneTextMessagesInRoomAsync(long roomId, string messageType, string userId)
        {
            var list = await _messageRepository.GetAllNoneTextMessagesInRoomAsync(roomId, messageType, userId);
            return _mapper.Map<List<FileDto>>(list);
        }

        public async Task<ReceiveMessageDto> ChangeSystemMessage(ReceiveMessageDto receiveMsg, string userId, string? receiverId, string type)
        {
            if (receiveMsg.SenderId == userId)
            {
                receiveMsg.SenderName = "Bạn";
            }
            else if (receiveMsg.SenderId != null)
            {
                receiveMsg.SenderName = await _userRepository.GetUsernameById(receiveMsg.SenderId);
            }

            if (receiveMsg.TypeOfMessage != MessageType.System.ToString())
            {
                throw new Exception(MsgNo.ERROR_UNHADLED_ACTION);
            }

            receiveMsg.Content = await ChangeContentSystemMessage(userId, receiveMsg.SenderName, receiverId, type);
            receiveMsg.LastMessage = receiveMsg.Content;

            return receiveMsg;
        }

        public async Task<string> ChangeContentSystemMessage(long messageId, string userId)
        {
            var msg = await _messageRepository.GetMessageByIdAsync(messageId)
                ?? throw new Exception(MsgNo.ERROR_UNHADLED_ACTION);

            if (msg.TypeOfMessage != MessageType.System.ToString())
            {
                throw new Exception(MsgNo.ERROR_UNHADLED_ACTION);
            }

            return await ChangeContentSystemMessage(userId, msg.Sender?.Name, msg.AffectedId, msg.Content);
        }

        private async Task<string> ChangeContentSystemMessage(string userId, string? msgSenderName, string? receiverId, string type)
        {

            if (type == SystemMessageType.IsBecomeFriend.ToString())
            {
                return Constants.FRIEND_ACCEPTED_NOTIFICATION;
            }

            if (type == SystemMessageType.IsCreateGroupRoom.ToString())
            {
                return msgSenderName + " đã tạo nhóm này";
            }

            if (type == SystemMessageType.IsLeaveRoom.ToString())
            {
                return msgSenderName + " đã rời khỏi nhóm";
            }

            if (receiverId == userId)
            {
                if (type == SystemMessageType.IsInRoom.ToString())
                {
                    return msgSenderName + " đã thêm bạn vào nhóm";
                }
                if (type == SystemMessageType.IsOutRoom.ToString())
                {
                    return msgSenderName + " đã xoá bạn ra khỏi nhóm";
                }
            }

            if (receiverId == null)
            {
                throw new Exception(MsgNo.ERROR_UNHADLED_ACTION);
            }

            var receiverName = await _userRepository.GetUsernameById(receiverId);
            if (type == SystemMessageType.IsInRoom.ToString())
            {
                return msgSenderName + " đã thêm " + receiverName + " vào nhóm";

            }
            if (type == SystemMessageType.IsOutRoom.ToString())
            {
                return msgSenderName + " đã xoá " + receiverName + " ra khỏi nhóm";
            }

            return "";
        }
    }
}