using System.Net.WebSockets;
using AutoMapper;
using BKConnectBE.Model.Dtos.MessageManagement;
using BKConnectBE.Repository.Messages;

namespace BKConnectBE.Service.Messages
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        public MessageService(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }
        
        public async Task<List<MessageDto>> GetAllMessagesInRoomAsync(string userId, long roomId)
        {
            var listMessages = await _messageRepository.GetAllMessagesInRoomAsync(roomId);
            var listMessagesDto = _mapper.Map<List<MessageDto>>(listMessages);
            foreach(var msgDto in listMessagesDto)
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