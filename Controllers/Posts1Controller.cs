﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reddit.Dtos;
using Reddit.Models;
using Reddit.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Reddit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Posts1Controller : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPostsRepository _postsRepository;

        public Posts1Controller(ApplicationDbContext context, IPostsRepository postsRepository)
        {
            _context = context;
            _postsRepository = postsRepository;
        }


        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<PagedList<Post>>> GetPosts([StringLength(15)]string? sortTearm, bool? isAscending, int page = 1, int pageSize = 3, string? searchTerm = null)
        {

            //   return await _context.Posts.Skip(3).Take(10).ToListAsync();
            //   return await PagedList<Post>.CreateAsync(_context.Posts, page, pageSize);
            return await _postsRepository.GetPosts(page, pageSize, searchTerm, sortTearm, isAscending);
        }

        // GET: api/Posts1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id); // .Include(p => p.Comments)

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // PUT: api/Posts1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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

        // POST: api/Posts1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(PostDto postDto)
        {
           
            var post = postDto.CreatePost();

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }

        // DELETE: api/Posts1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
