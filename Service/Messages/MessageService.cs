using AutoMapper;
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

        public async Task<ReceiveMessageDto> AddMessageAsync(SendMessageDto messageDto, string userId)
        {
            Message sendMsg = _mapper.Map<Message>(messageDto);
            sendMsg.SenderId = userId;
            await _genericRepositoryForMessage.AddAsync(sendMsg);
            await _genericRepositoryForMessage.SaveChangeAsync();
            Message newMsg = await _messageRepository.GetMessageByIdAsync(sendMsg.Id);
            return _mapper.Map<ReceiveMessageDto>(newMsg);
        }

        public async Task<List<ReceiveMessageDto>> GetAllMessagesInRoomAsync(string userId, long roomId)
        {
            var listMessages = await _messageRepository.GetAllMessagesInRoomAsync(roomId);
            var listMessagesDto = new List<ReceiveMessageDto>();
            foreach (var msg in listMessages)
            {
                var msgDto = _mapper.Map<ReceiveMessageDto>(msg);
                msgDto = await RenameUser(msgDto, userId, msg.RootMessage.SenderId);
                listMessagesDto.Add(msgDto);
            }

            return listMessagesDto;
        }

        public async Task<ReceiveMessageDto> RenameUser(ReceiveMessageDto receiveMsg, string userId, string rootSenderId)
        {
            if (receiveMsg.SenderId == userId)
            {
                receiveMsg.SenderName = "bạn";
            }
            else
            {
                receiveMsg.SenderName = await _userRepository.GetUsernameById(receiveMsg.SenderId);
            }

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
            return receiveMsg;
        }
        public async Task<string> GetRootMessageSenderId(long? messageId)
        {
            return await _messageRepository.GetRootMessageSenderIdAsync(messageId);
        }

    }
}