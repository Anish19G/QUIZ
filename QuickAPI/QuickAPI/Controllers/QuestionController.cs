using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickAPI.Models;

namespace QuickAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly QuizDbContext _context;

        public QuestionController(QuizDbContext context)
        {
            _context = context;
        }

        //GET: api/controller
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
        {
            var random5Qns = await (_context.Questions
                .Select(x => new
                {
                    QnId = x.QnId,
                    QnInWords = x.QnInWords,
                    ImageName = x.ImageName,
                    Options = new string[] { x.Option1, x.Option2, x.Option3, x.Option4 }
                })
                .OrderBy(y => Guid.NewGuid()).Take(5)).ToListAsync();

            return Ok(random5Qns);
        }

        //GEt: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return question;
        }

        //Put: api/Question/5
        //To protect from overposting attacks
        [HttpPut("{id}")]
        public async Task<ActionResult> PutQuestion(int id, Question question)
        {
            if(id != question.QnId)
            {
                return BadRequest();
            }

            _context.Entry(question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
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

        //POst: api/Question/GetAnswer
        //To protect from overposting attacks
        [HttpPost]
        [Route("GetAnswers")]
        public async Task<ActionResult<Question>> RetrieveAnswers(int[] qnIds)
        {
            var answers = await (_context.Questions
                .Where(x => qnIds.Contains(x.QnId))
                .Select(y => new
                {
                    QnId = y.QnId,
                    QnInWords = y.QnInWords,
                    ImageName = y.ImageName,
                    Options = new string[] { y.Option1, y.Option2, y.Option3, y.Option4 },
                    Answer = y.Answer
                })).ToListAsync();
            return Ok(answers);
        }

        //Delete: api/Question/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestions(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if(question == null)
            {
                return NotFound();
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.QnId == id);
        }


    }
}