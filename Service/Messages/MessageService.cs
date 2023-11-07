using AutoMapper;
using BKConnectBE.Model.Dtos.MessageManagement;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;
using BKConnectBE.Repository.Messages;

namespace BKConnectBE.Service.Messages
{
    public class MessageService : IMessageService
    {
        private readonly IGenericRepository<Message> _genericRepositoryForMessage;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        public MessageService(IGenericRepository<Message> genericRepositoryForMessage, IMessageRepository messageRepository, IMapper mapper)
        {
            _genericRepositoryForMessage = genericRepositoryForMessage;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task<ReceiveMessageDto> AddMessageAsync(SendMessageDto messageDto)
        {
            Message sendMsg = _mapper.Map<Message>(messageDto);
            await _genericRepositoryForMessage.AddAsync(sendMsg);
            await _genericRepositoryForMessage.SaveChangeAsync();
            Message newMsg = await _messageRepository.GetMessageByIdAsync(sendMsg.Id);
            return _mapper.Map<ReceiveMessageDto>(newMsg);
        }

        public async Task<List<ReceiveMessageDto>> GetAllMessagesInRoomAsync(string userId, long roomId)
        {
            var listMessages = await _messageRepository.GetAllMessagesInRoomAsync(roomId);
            var listMessagesDto = _mapper.Map<List<ReceiveMessageDto>>(listMessages);
            foreach (var msgDto in listMessagesDto)
            {
                if (msgDto.SenderId == userId)
                {
                    msgDto.SenderName = "Bạn";
                }
            }

            return listMessagesDto;
        }
    }
}