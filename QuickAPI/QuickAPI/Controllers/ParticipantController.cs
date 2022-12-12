using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickAPI.Models;

namespace QuickAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParticipantController : ControllerBase
    {
        private readonly QuizDbContext _context;

        public ParticipantController(QuizDbContext context)
        {
            _context = context;
        }

        //Get: api/participant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Participant>>> GetParticipants()
        {
            return await _context.Participants.ToListAsync();
        }

        //Get: api/Participant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Participant>> GetParticipant(int id)
        {
            var participant = await _context.Participants.FindAsync(id); 
            if(participant == null)
            {
                return NotFound();
            }
            return participant;
        }

        //Put: api/Participant/5
        //To protect from overposting 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticipant(int id, ParticipantResult _participantResult)
        {
            if(id != _participantResult.ParticipantId)
            {
                return BadRequest();
            }
             
            //get all current details of the recored , then update with quiz results
            Participant participant = _context.Participants.Find(id);
            participant.Score = _participantResult.Score;
            participant.Timetaken = _participantResult.Timetaken;

            _context.Entry(participant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        //POST: api/Participant
        //To protect from overposting attacks
        [HttpPost]
        public async Task<ActionResult<Participant>> PostParticipant(Participant participant)
        {
            var temp = _context.Participants
                .Where(x => x.Name == participant.Name && x.Email == participant.Email).FirstOrDefault();
            if (temp == null)
            {
                _context.Participants.Add(participant);
                await _context.SaveChangesAsync();
            }
            else
                participant = temp;
            return Ok(participant);
        }

        //DELELTE; Api/Participant
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipant(int id)
        {
            var participant = await _context.Participants.FindAsync(id);
            if(participant == null)
            {
                return NotFound();
            }
            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ParticipantExists(int id)
        {
            return _context.Participants.Any(e => e.ParticipantId == id);
        }
    }
}