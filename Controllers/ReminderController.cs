using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NailDesignerAPI.Services;

namespace NailDesignerAPI.Controllers {
    [ApiController]
    [Route( "api/[controller]" )]
    [Authorize]
    public class ReminderController : ControllerBase {
        private readonly AppointmentReminderService _reminderService;

        public ReminderController( AppointmentReminderService reminderService ) {
            _reminderService = reminderService;
        }

        [HttpPost( "send-reminders" )]
        public async Task<IActionResult> SendReminders() {
            await _reminderService.TriggerRemindersAsync();
            return Ok( new { message = "Lembretes enviados com sucesso!" } );
        }

        [HttpPost( "send-birthdays" )]
        public async Task<IActionResult> SendBirthdays() {
            await _reminderService.TriggerBirthdayMessagesAsync();
            return Ok( new { message = "Mensagens de aniversário enviadas com sucesso!" } );
        }
    }
}